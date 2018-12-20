using System;
using System.Collections;
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
using Microsoft.Toolkit.Uwp.UI.Controls;
using urp.Util;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace urp.contentPage
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class NotifyPage : Page
    {
        private UrpUtil urpUtil;
        private Dictionary<string, string> gonggaoD;
        public NotifyPage()
        {
            this.InitializeComponent();
            urpUtil = new UrpUtil();
        }

        //页面打开时
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            List<Head> headList = new List<Head>();
            gonggaoD = await urpUtil.GetNotifyList();
            foreach (var kv in gonggaoD)
            {
                headList.Add(new Head(){value = kv.Key});
            }

            ListView.ItemsSource = headList;
        }

        private async void Expander_OnExpanded(object sender, EventArgs e)
        {
            Expander expander = (Expander) sender;
            string link = gonggaoD[expander.Header.ToString()];
            string content = await urpUtil.GetNotifyContent(UrpApi2.URL_JWC + link);
            TextBlock contentText = new TextBlock();
            contentText.Text = content;
            contentText.Margin = new Thickness(20.0,0,20.0,0);
            contentText.TextWrapping = TextWrapping.Wrap;
            expander.Content = contentText;
        }
    }

    class Head
    {
        public string value { get; set; }
    }
}
