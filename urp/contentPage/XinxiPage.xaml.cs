using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using HtmlAgilityPack;
using Microsoft.Toolkit.Uwp.UI.Controls.TextToolbarSymbols;
using Newtonsoft.Json;
using urp.Struct;
using urp.Util;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace urp.contentPage
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class XinxiPage : Page
    {
        private static readonly int SUCCESS = 1;
        private static readonly int TIMEOUT = 0;
        private static readonly int FAIL = -1;
        private Frame root = Window.Current.Content as Frame;
        public XinxiPage()
        {
            this.InitializeComponent();
        }

        //页面打开时
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            await GetUserInfo();
        }

        private async Task GetUserInfo()
        {
            Ring.IsActive = true;
            var urpUtil = new UrpUtil();
            var map = new Dictionary<string, string>();
            int result = await urpUtil.GetUserInfo(map);
            if (result == SUCCESS)
            {
                List<InfoStruct> list = new List<InfoStruct>();
                foreach (var infoD in map)
                {
                    string info = " " + infoD.Key + "  " + infoD.Value;
                    list.Add(new InfoStruct() { values = info });
                }
                list.RemoveAt(0);
                GridView.ItemsSource = list;
            }
            else if (result == FAIL)
            {
                Notification.Show("获取信息失败，请重试", 3000);
            }
            else
            {
                root.Navigate(typeof(MainPage),1);
            }
            
            Ring.IsActive = false;
        }
    }

    class InfoStruct
    {
        public string values { get; set; }
    }
}
