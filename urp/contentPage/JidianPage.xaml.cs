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
using Windows.UI.Xaml.Automation;
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
        private static readonly int SUCCESS = 1;
        private static readonly int TIMEOUT = 0;
        private static readonly int FAIL = -1;
        private UrpUtil urpUtil;
        private Frame root = Window.Current.Content as Frame;
        private ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
        public JidianPage()
        {
            this.InitializeComponent();
            urpUtil = new UrpUtil();
        }
        //页面打开时
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            await GetJidian();
        }
        private async Task GetJidian()
        {
            Ring.IsActive = true;
            List<JidianInfo> jidianList = new List<JidianInfo>();
            StuUser user = new StuUser();
            user.userName = (string)localSettings.Values["userName"];
            user.passWord = (string)localSettings.Values["passWord"];
            int status = await urpUtil.getJiDian(jidianList,user);
            if (status == SUCCESS)
            {
                GridView.ItemsSource = jidianList;
            }
            else
            {
                status = await urpUtil.loginStu(user);
                if (status == SUCCESS)
                {
                    await GetJidian();
                }
                else
                {
                    Notification.Show("获取信息失败，请重试", 3000);
                }
            }
           
            Ring.IsActive = false;
        }

    }

    class JidianInfo
    {
        public string values { get; set; }
    }
}
