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
        public ChengjiPage()
        {
            this.InitializeComponent();
        }

        private async void Pivot1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            WebUtil webUtil = new WebUtil();
            switch (Pivot1.SelectedIndex)
            {
                case 0:
                    string jigehtml = await webUtil.GetString(UrpApi.BASEURL + UrpApi.GETCHENGJI);
                    if(jigehtml.Contains("session"))
                        root.Navigate(typeof(MainPage));
                    else if (jigehtml.Contains("wrong")) ;
                    else
                    {
                        WebView1.NavigateToString(jigehtml);
                    }
                    break;
                case 1:
                    jigehtml = await webUtil.GetString(UrpApi.BASEURL + UrpApi.GETNOCHENGJI);
                    if (jigehtml.Contains("session"))
                        root.Navigate(typeof(MainPage));
                    else if (jigehtml.Contains("wrong")) ;
                    else
                    {
                        WebView2.NavigateToString(jigehtml);
                    }
                    break;
            }
        }

    }
}
