﻿using NodeCore.Core.SyncServer;
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

namespace ClientWindow
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {//开始
           NodeCore.Core.SystemManager.Instance.StartNode();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {//停止

        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {//申请加入
           NodeCore.Core.SystemManager.Instance.RegisterNode();
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {//申请退出

        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {//设置

        }
    }
}
