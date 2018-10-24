using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using urp.Util;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace urp
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class ChengjiPage : Page
    {
        private Frame root = Window.Current.Content as Frame;
        private bool isFirst = true;
        private bool isFirst2 = true;
        private UrpUtil urpUtil;
        public ChengjiPage()
        {
            this.InitializeComponent();
            urpUtil = new UrpUtil();
        }

        private async void Pivot1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
            Ring.IsActive = true;
            try
            {
                switch (Pivot1.SelectedIndex)
                {
                    case 0:
                        if (isFirst)
                        {
                            string jigehtml = await urpUtil.getGoodScore();
                            if (jigehtml.Equals("SESSION"))
                                root.Navigate(typeof(MainPage), 1);
                            else
                            {
                                WebView1.NavigateToString(jigehtml);
                                isFirst = false;
                            }
                        }
                        break;
                    case 1:
                        if (isFirst2)
                        {
                            string bujigehtml = await urpUtil.getBadScore();
                            if (bujigehtml.Equals("SESSION"))
                                root.Navigate(typeof(MainPage), 1);
                            else
                            {
                                WebView2.NavigateToString(bujigehtml);
                                isFirst2 = false;
                            }
                        }
                        break;
                }
            }
            catch (Exception exception)
            {
                Notification.Show("获取信息失败，请重试", 3000);
                Console.WriteLine(exception);
            }
            
            Ring.IsActive = false;
        }

    }
}
