using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Notifications;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Newtonsoft.Json;
using urp.Struct;
using urp.Util;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace urp
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class JidianPage : Page
    {
        private Frame root = Window.Current.Content as Frame;
        private ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
        public JidianPage()
        {
            this.InitializeComponent();
        }
        //页面打开时
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            await GetJidian();
        }
        private async Task GetJidian()
        {
            WebUtil webUtil = new WebUtil();
            StuUser user = new StuUser();
            user.userName = (string) localSettings.Values["userName"];
            user.passWord = (string) localSettings.Values["passWord"];
            user.checkCode = " ";
            String json = JsonConvert.SerializeObject(user);
            List<KeyValuePair<string, string>> paramList = new List<KeyValuePair<string, string>>();
            paramList.Add(new KeyValuePair<string, string>("param", json));
            String result = await webUtil.PostString(UrpApi.BASEURL + UrpApi.GETJIDIAN,paramList);
            if (result.Equals("session"))
            {
                root.Navigate(typeof(MainPage));
            }
            else if (result.Equals("wrong"))
            {
                Notification.Show("获取信息失败");
            }
            else
            {
                
                }
        }

    }
}
