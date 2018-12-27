using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using urp.Struct;
using urp.Util;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace urp.contentPage
{
    /// <summary
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class PingguPage : Page
    {
        private UrpUtil urpUtil;
        private List<PG> pgList;
        public PingguPage()
        {
            this.InitializeComponent();
            urpUtil = new UrpUtil();
        }
        //页面打开时
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            pgList = await urpUtil.getPlist();
            if (pgList.Count == 0)
            {
                TextBox1.IsEnabled = false;
                Button.IsEnabled = false;
                Notification.Show("无可评估项",4000);
            }
            else
            {
                Button.IsEnabled = true;
                TextBox1.IsEnabled = true;
            }
        }

        private async void Button_OnClick(object sender, RoutedEventArgs e)
        {
            Thread thread = new Thread(PingJiaoDelegate);
            thread.Start();
            ContentDialog.IsPrimaryButtonEnabled = false;
            await ContentDialog.ShowAsync();
        }

        private async void PingJiaoDelegate()
        {
            Thread.Sleep(2000);
            await CoreApplication.MainView.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                new DispatchedHandler(PingJiaoAll));
        }
        private async void PingJiaoAll()
        {
            if (pgList.Count > 0)
            {
                int count = pgList.Count;
                int index = 1;
                Button.IsEnabled = false;
                foreach (var pj in pgList)
                {
                    ContentDialog.Content = "正在评教：" + index + "/" + count;
                    bool isSuccess = await urpUtil.PingJiao(pj, TextBox1.Text);
                    if (!isSuccess)
                    {
                        Notification.Show("一评教失败", 2000);
                    }
                    index++;
                }
                pgList = await urpUtil.getPlist();
                Notification.Show("评教完成");
                ContentDialog.IsPrimaryButtonEnabled = true;
            }
            else
            {
                Notification.Show("无可评估项",3000);
            }
        }
    }
}
