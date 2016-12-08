using QDownApp.Controls;
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

namespace QDownApp
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        TransportingWin transportingWin = new TransportingWin();
        DustbinWin dustbinWin = new DustbinWin();
        CompletedWin completedWin = new CompletedWin();
        public MainWindow()
        {
            InitializeComponent();
            MainGrid.Children.Add(transportingWin);
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
           if(ListMen.SelectedIndex==0)
           {
               MainGrid.Children.Clear();
               MainGrid.Children.Add(transportingWin);
           }
           if (ListMen.SelectedIndex == 1)
           {
               MainGrid.Children.Clear();
               MainGrid.Children.Add(dustbinWin);
           } 
            if (ListMen.SelectedIndex == 2)
           {
               MainGrid.Children.Clear();
               MainGrid.Children.Add(completedWin);
           }
        }
    }
}
