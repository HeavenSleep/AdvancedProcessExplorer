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
using System.Windows.Navigation;
using System.Windows.Shapes;
using MahApps.Metro.Controls;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Collections;

namespace MaJyx_Advanced_Proccess
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public static ObservableCollection<ProcessInfo> processList { get; set; }
        private Processes pManager;
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;
            pManager = new Processes();
            processList = pManager.Items;
        }

        private void dataGrid_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            if (dataGrid.SelectedItems.Count > 0)
            {
                killP.Visibility = Visibility.Visible;
                dumpP.Visibility = Visibility.Visible;
                openP.Visibility = Visibility.Visible;
            }
            else
            {
                killP.Visibility = Visibility.Hidden;
                dumpP.Visibility = Visibility.Hidden;
                openP.Visibility = Visibility.Hidden;
            }
        }

        private void killP_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid.SelectedItems.Count > 0)
            {
                for (int i = 0; i < dataGrid.SelectedItems.Count; ++i )
                {
                    ((ProcessInfo)dataGrid.SelectedItems[i]).kill();
                }
                pManager.refresh();
            }
        }

        private void dumpP_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid.SelectedItems.Count > 0)
            {
                for (int i = 0; i < dataGrid.SelectedItems.Count; ++i)
                {
                    ((ProcessInfo)dataGrid.SelectedItems[i]).dump();
                }
            }
        }

        private void openP_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid.SelectedItems.Count > 0)
            {
                for (int i = 0; i < dataGrid.SelectedItems.Count; ++i)
                {
                    ((ProcessInfo)dataGrid.SelectedItems[i]).open();
                }
            }
        }

        private void MetroWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void startP_Click(object sender, RoutedEventArgs e)
        {
            new Start().Show();
        }

        private void dataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
