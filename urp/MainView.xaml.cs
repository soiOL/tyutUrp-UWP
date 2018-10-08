using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
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
            WebUtil webUtil = new WebUtil();
            String result = await webUtil.GetString(UrpApi.BASEURL + UrpApi.GETUSERINFO);
            if (result.Equals("session"))
            {
                root.Navigate(typeof(MainPage));
            }
            else if (result.Equals("wrong"))
            {
                MainNavigation.Header = "你好，同学";
                TitleText.Text = "你好，同学";
            }
            else
            {
                var jsonStruct = JsonConvert.DeserializeObject<Dictionary<String,String>>(result);
                MainNavigation.Header = "你好，" + jsonStruct["姓名:"];
                TitleText.Text = "你好，" + jsonStruct["姓名:"];
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
    }
}
