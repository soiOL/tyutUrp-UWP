using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using HtmlAgilityPack;
using Newtonsoft.Json;
using urp.contentPage;
using urp.Util;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace urp
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainView : Page
    {
        public MainView()
        {
            this.InitializeComponent();
        }

        //页面打开时
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            contentFrame.Navigate(typeof(MainViewPage));
            MainItem.IsSelected = true;
        }
        
        private void MainNavigation_OnItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            if (args.InvokedItem.Equals("主页"))
            {
                contentFrame.Navigate(typeof(MainViewPage));
            }
            if (args.InvokedItem.Equals("教务公告"))
            {
                contentFrame.Navigate(typeof(NotifyPage));
            }
            if (args.InvokedItem.Equals("个人信息"))
            {
                contentFrame.Navigate(typeof(XinxiPage));
            }
            if (args.InvokedItem.Equals("成绩"))
            {
                contentFrame.Navigate(typeof(ChengjiPage));
            }
            if (args.InvokedItem.Equals("学分绩点"))
            {
                contentFrame.Navigate(typeof(JidianPage));
            }
            if (args.InvokedItem.Equals("课表"))
            {
                contentFrame.Navigate(typeof(KebiaoPage));
            }
            if (args.InvokedItem.Equals("学生卡账单"))
            {
                contentFrame.Navigate(typeof(EcardPage));
            }

            if (args.IsSettingsInvoked)
            {
                contentFrame.Navigate(typeof(AboutPage));
            }
        }
    }
}
