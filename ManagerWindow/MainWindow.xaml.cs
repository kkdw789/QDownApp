using ManagerCore.Core;
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

namespace ManagerWindow
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private Node SelectNode = new Node();
        public MainWindow()
        {
            InitializeComponent();
            ManagerCore.Core.SystemManager.Instance.OnReceiveMessage += Instance_OnReceiveMessage;  
            
        }

        //加载列表
        void Instance_OnReceiveMessage(Group group)
        {
            this.Dispatcher.Invoke(new Action(() =>
            {
                if (group != null)
                {
                    listNode.ItemsSource = group.Nodes;
                    listNode.UpdateLayout();
                }
            }));
        }
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {//开启服务
            SystemManager.Instance.StartService();

        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {//关闭服务
            SystemManager.Instance.StopService();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {//添加节点
            //SystemManager.Instance.AddNode();
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {//删除节点
            //SystemManager.Instance.DelNode();
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {//开启节点
            //SelectNode.Instance.Controller.TryStartNode();
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {//停止节点
            //SelectNode.Instance.Controller.TryStopNode();
        }

        private void Button_Click_6(object sender, RoutedEventArgs e)
        {//配置
            //SelectNode.Instance.Controller.SetterNode();
        }

        private void Button_Click_7(object sender, RoutedEventArgs e)
        {
            
        }

    }
}
