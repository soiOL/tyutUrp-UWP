using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
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
            WebUtil webUtil = new WebUtil();
            String result = await webUtil.GetString(UrpApi.BASEURL + UrpApi.GETUSERINFO);
            if (result.Equals("session"))
            {
                root.Navigate(typeof(MainPage));
            }
            else if (result.Equals("wrong"))
            {
                Notification.Show("获取信息失败",3000);
            }
            else
            {
                var jsonStruct = JsonConvert.DeserializeObject<Dictionary<String, String>>(result);
                List<InfoStruct> list = new List<InfoStruct>();
                foreach (var infoD in jsonStruct)
                {
                    string value = infoD.Key + "  " + infoD.Value;

                    if (!string.IsNullOrEmpty(value))
                    {
                        list.Add(new InfoStruct() { values = value });
                    }
                }
                list.RemoveAt(0);
                GridView.ItemsSource = list;
            }

            Ring.IsActive = false;
        }
    }

    class InfoStruct
    {
        public string values { get; set; }
    }
}
