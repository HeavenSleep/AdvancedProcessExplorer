using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.IO;
using MahApps.Metro.Controls;
using Microsoft.Win32;
using System.Diagnostics;
using System.Windows.Interactivity;

namespace MaJyx_Advanced_Proccess
{
    /// <summary>
    /// Logique d'interaction pour Start.xaml
    /// </summary>
    public partial class Start : MetroWindow
    {
        private bool browsed;
        public Start()
        {
            InitializeComponent();
        }

        private void program_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(program.Text))
            {
                try
                {
                    string processname = program.Text;
                    if (browsed == true)
                    {
                        Process.Start(processname);
                    }
                    else if (processname.ToLower() == "%appdata%")
                    {
                        Process.Start(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData));
                    }
                    else if (processname.ToLower() == "%temp%")
                    {
                        Process.Start(Path.GetTempPath());
                    }
                    else if (processname.ToLower() == "system32")
                    {
                        Process.Start("C:\\Windows\\System32");
                    }
                    else if (processname.ToLower() == "temp")
                    {
                        Process.Start("C:\\Windows\\Temp");
                    }
                    else if (processname.ToLower() == "appdata")
                    {
                        Process.Start("C:\\Users\\" + Environment.UserName + "\\AppData");
                    }
                    else
                    {
                        if (processname.Contains("\\") | processname.Contains("www.") | processname.Contains("http://") | processname.Contains("/") | processname.Contains(".fr") | processname.Contains(".com") | processname.Contains(".net") | processname.Contains(".org") | processname.Contains(".uk") | processname.Contains(".de"))
                        {
                            if (processname.Contains("http://"))
                            {
                                processname.Replace("http://", null);
                                Process.Start(processname);
                            }
                            else if (processname.Contains("www."))
                            {
                                Process.Start(processname);
                            }
                            else
                            {
                                Process.Start("www." + processname);
                            }
                        }
                        else if (processname.Contains(".exe"))
                        {
                            Process.Start("C:\\Windows\\System32\\" + processname);
                        }
                        else
                        {
                            Process.Start("C:\\Windows\\System32\\" + processname + ".exe");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            this.Close();
        }

        private void program_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (!string.IsNullOrEmpty(program.Text))
                {
                    try
                    {
                        string processname = program.Text;
                        if (browsed == true)
                        {
                            Process.Start(processname);
                        }
                        else if (processname.ToLower() == "%appdata%")
                        {
                            Process.Start(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData));
                        }
                        else if (processname.ToLower() == "%temp%")
                        {
                            Process.Start(Path.GetTempPath());
                        }
                        else if (processname.ToLower() == "system32")
                        {
                            Process.Start("C:\\Windows\\System32");
                        }
                        else if (processname.ToLower() == "temp")
                        {
                            Process.Start("C:\\Windows\\Temp");
                        }
                        else if (processname.ToLower() == "appdata")
                        {
                            Process.Start("C:\\Users\\" + Environment.UserName + "\\AppData");
                        }
                        else
                        {
                            if (processname.Contains("\\") | processname.Contains("www.") | processname.Contains("http://") | processname.Contains("/") | processname.Contains(".fr") | processname.Contains(".com") | processname.Contains(".net") | processname.Contains(".org") | processname.Contains(".uk") | processname.Contains(".de"))
                            {
                                if (processname.Contains("http://"))
                                {
                                    processname.Replace("http://", null);
                                    Process.Start(processname);
                                }
                                else if (processname.Contains("www."))
                                {
                                    Process.Start(processname);
                                }
                                else
                                {
                                    Process.Start("www." + processname);
                                }
                            }
                            else if (processname.Contains(".exe"))
                            {
                                Process.Start("C:\\Windows\\System32\\" + processname);
                            }
                            else
                            {
                                Process.Start("C:\\Windows\\System32\\" + processname + ".exe");
                            }
                        }
                        this.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            OpenFileDialog browse = new OpenFileDialog();
            if ((bool)browse.ShowDialog())
            {
                program.Text = browse.FileName;
                browsed = true;
            }
        }
    }
}
