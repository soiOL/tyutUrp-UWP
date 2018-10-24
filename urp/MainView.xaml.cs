using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
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
        private static readonly int SUCCESS = 1;
        private static readonly int TIMEOUT = 0;
        private static readonly int FAIL = -1;
        private Frame root = Window.Current.Content as Frame;
        public MainView()
        {
            this.InitializeComponent();
        }

        //页面打开时
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
             await GetTitleText();
        }

        private async Task GetTitleText()
        {
            var urpUtil = new UrpUtil();
            var map = new Dictionary<string, string>();
            int result = await urpUtil.GetUserInfo(map);
            if (result == SUCCESS)
            {
                MainNavigation.Header = "你好，" + map["姓名:"];
                TitleText.Text = "你好，" + map["姓名:"];
            }
            else if (result == FAIL)
            {
                MainNavigation.Header = "你好，同学";
                TitleText.Text = "你好，同学";
            }
            else
            {
                root.Navigate(typeof(MainPage));
            }
        }

        private void Gerenxinxi_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            contentFrame.Navigate(typeof(XinxiPage));
        }

        private void Chengji_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            contentFrame.Navigate(typeof(ChengjiPage));
        }

        private void Jidian_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            contentFrame.Navigate(typeof(JidianPage));
        }

        private void Kebiao_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            contentFrame.Navigate(typeof(KebiaoPage));
        }

        private async void Dengchu_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            var message = new MessageDialog("是否登出账号");
            message.Commands.Add(new UICommand("确定", cmd => { }, "退出"));
            message.Commands.Add(new UICommand("取消", cmd => { }));
            message.DefaultCommandIndex = 1;
            message.CancelCommandIndex = 1;
            IUICommand result = await message.ShowAsync();
            if (result.Id as string == "退出")
            {
                root.Navigate(typeof(MainPage));
            }
        }

        private void About_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            contentFrame.Navigate(typeof(AboutPage));
        }
    }
}
