using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;

namespace MaJyx_Advanced_Proccess
{
    public class ProcessInfo
    {

        public int ID { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string Location { get; set; }
        public string Version { get; set; }
        public string User { get; set; }

        private List<string> errorList;

        public ProcessInfo()
        {
            this.errorList = new List<string>();
        }

        public void dump()
        {
            DumpViewer view = new DumpViewer(this);
            view.Show();
        }

        public void kill()
        {
            try
            {
                Process p = Process.GetProcessById(this.ID);
                p.Kill();
            }
            catch (Exception ex)
            {
                errorList.Add(ex.ToString());
            }
        }

        public void start()
        {
            Process.Start(this.Location);
        }

        public void open()
        {
            Process.Start(Path.GetDirectoryName(this.Location));
        }

        public List<string> getErrors()
        {
            return errorList;
        }
    }
}
