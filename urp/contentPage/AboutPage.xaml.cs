using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace urp.contentPage
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class AboutPage : Page
    {

        private Frame root = Window.Current.Content as Frame;
        public AboutPage()
        {
            this.InitializeComponent();
        }

        private async void LogOut_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            var message = new MessageDialog("是否登出账号");
            message.Commands.Add(new UICommand("确定", cmd => { }, "退出"));
            message.Commands.Add(new UICommand("取消", cmd => { }));
            message.DefaultCommandIndex = 1;
            message.CancelCommandIndex = 1;
            IUICommand result = await message.ShowAsync();
            if (result.Id as string == "退出" && root.CanGoBack)
            {
                root.GoBack();
            }
        }
    }
}
