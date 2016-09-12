using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace MaJyx_Advanced_Proccess
{
    class Processes : DispatcherObject
    {
        public ObservableCollection<ProcessInfo> Items { get; set; }

        public List<string> Errors { 
            get{
                return getProcessErrors();
            }
        }

        private List<string> managerError;
        private Thread sync;

        public Processes()
        {
            this.Items = new ObservableCollection<ProcessInfo>();
            this.managerError = new List<string>();
            this.sync = new Thread(Syncronization) { IsBackground = true };
            this.sync.Start();
        }

        private void Syncronization()
        {
            while (true)
            {
                this.refresh();
                Thread.Sleep(2000);
            }
        }

        public void refresh()
        {
            ObservableCollection<ProcessInfo> tmpList = new ObservableCollection<ProcessInfo>();
            foreach (Process p in Process.GetProcesses(Environment.MachineName))
            {
                try
                {
                    tmpList.Add(new ProcessInfo() { ID = p.Id, Location = p.MainModule.FileName, Name = p.ProcessName, Title = p.MainWindowTitle, User = GetProcessOwner(p.Id), Version = p.MainModule.FileVersionInfo.FileVersion });
                }
                catch (Exception ex)
                {
                    managerError.Add(ex.ToString());
                }
            }
            if (this.Dispatcher.CheckAccess())
            {
                this.Items.Clear();
                foreach (ProcessInfo p in tmpList)
                {
                    this.Items.Add(p);
                }
            }
            else
            {
                this.Dispatcher.Invoke(new Action(() => {
                    this.Items.Clear();
                    foreach (ProcessInfo p in tmpList)
                    {
                        this.Items.Add(p);
                    }
                }));
            }
            
        }

        private List<string> getProcessErrors()
        {
            List<string> tmpErrors = new List<string>();
            foreach (ProcessInfo p in Items)
            {
                tmpErrors.AddRange(p.getErrors());
            }
            tmpErrors.AddRange(managerError);

            return tmpErrors;
        }

        private string GetProcessOwner(int processId)
        {
            string query = "Select * From Win32_Process Where ProcessID = " + processId;
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(query);
            ManagementObjectCollection processList = searcher.Get();

            foreach (ManagementObject obj in processList)
            {
                string[] argList = new string[] { string.Empty, string.Empty };
                int returnVal = Convert.ToInt32(obj.InvokeMethod("GetOwner", argList));
                if (returnVal == 0)
                {
                    // return DOMAIN\user
                    return argList[1] + "\\" + argList[0];
                }
            }

            return "NO OWNER";
        }
    }
}
