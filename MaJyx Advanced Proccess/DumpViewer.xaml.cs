using System;
using System.Windows;
using System.Windows.Documents;
using MahApps.Metro.Controls;
using System.Runtime.InteropServices;
using System.IO;
using System.Threading;
using System.Diagnostics;

namespace MaJyx_Advanced_Proccess
{
    /// <summary>
    /// Logique d'interaction pour DumpViewer.xaml
    /// </summary>
    public partial class DumpViewer : MetroWindow
    {
        ProcessInfo _procces;
        Thread startDump;

        public DumpViewer(ProcessInfo p)
        {
            InitializeComponent();
            this._procces = p;
            this.Title += p.Name + " (ProcessID: " + p.ID + ";Location: " + p.Location + ")";

            dumpViewer.Document = new FlowDocument();

            startDump = new Thread(dump){ IsBackground = true };
            startDump.Start();
        }

        private void dump()
        {
            ProcessStartInfo p = new ProcessStartInfo();
            p.FileName = "dumper.exe";
            p.Arguments = this._procces.ID.ToString();
            p.WindowStyle = ProcessWindowStyle.Hidden;
            p.RedirectStandardOutput = true;
            p.UseShellExecute = false;
            p.CreateNoWindow = true;

            var dumper = Process.Start(p);
            while (!dumper.StandardOutput.EndOfStream)
            {
                string line = dumper.StandardOutput.ReadLine();
            }

            if(File.Exists("dump_" + this._procces.Name + "_" + this._procces.ID + ".txt")){

                if (this.dumpViewer.Dispatcher.CheckAccess())
                {
                    Paragraph paragraph = new Paragraph();

                    paragraph.Inlines.Add(File.ReadAllText("dump_" + this._procces.Name + "_" + this._procces.ID + ".txt"));

                    FlowDocument document = new FlowDocument(paragraph);
                    this.dumpViewer.Document = document; 
                }
                else
                {
                    this.dumpViewer.Dispatcher.BeginInvoke(new Action(() => {
                        try
                        {
                            Paragraph paragraph = new Paragraph();

                            paragraph.Inlines.Add(File.ReadAllText("dump_" + this._procces.Name + "_" + this._procces.ID + ".txt"));

                            FlowDocument document = new FlowDocument(paragraph);
                            dumpViewer.Document = document;
                        }
                        catch (OutOfMemoryException)
                        {
                            try
                            {
                                Paragraph paragraph = new Paragraph();

                                foreach (string line in File.ReadAllLines("dump_" + this._procces.Name + "_" + this._procces.ID + ".txt"))
                                {
                                    try
                                    {
                                        paragraph.Inlines.Add(line);
                                    }
                                    catch (OutOfMemoryException)
                                    {
                                        MessageBox.Show("Dump is to heavy, we are not able to fully load the file...");
                                    }
                                    
                                }                         

                                FlowDocument document = new FlowDocument(paragraph);
                                dumpViewer.Document = document;
                            }
                            catch (OutOfMemoryException)
                            {
                                Process.Start("dump_" + this._procces.Name + "_" + this._procces.ID + ".txt");
                                MessageBox.Show("Dump is to heavy, will be launched by default text reader application.");
                                this.Close();
                            }
                            
                        }
                    }));
                }
            }
        }
    }
}
