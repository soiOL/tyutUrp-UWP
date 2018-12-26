using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using System.Threading.Tasks;
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
using HtmlAgilityPack;
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
        public MainView()
        {
            this.InitializeComponent();
        }

        //页面打开时
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            contentFrame.Navigate(typeof(MainViewPage));
        }
        
        
        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            if (splitView.IsPaneOpen)
            {
                splitView.IsPaneOpen = false;
            }
            else
            {
                splitView.IsPaneOpen = true;
            }
        }

        private void MainItem_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            contentFrame.Navigate(typeof(MainViewPage));
            splitView.IsPaneOpen = false;
        }

        private void GonggaoItem_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            contentFrame.Navigate(typeof(NotifyPage));
            splitView.IsPaneOpen = false;
        }

        private void XinxiItem_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            contentFrame.Navigate(typeof(XinxiPage));
            splitView.IsPaneOpen = false;
        }

        private void ChengjiItem_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            contentFrame.Navigate(typeof(ChengjiPage));
            splitView.IsPaneOpen = false;
        }

        private void JidianItem_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            contentFrame.Navigate(typeof(JidianPage));
            splitView.IsPaneOpen = false;
        }

        private void EcardItem_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            contentFrame.Navigate(typeof(EcardPage));
            splitView.IsPaneOpen = false;
        }

        private void PingjiaoItem_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            contentFrame.Navigate(typeof(PingguPage));
            splitView.IsPaneOpen = false;
        }

        private void UIElement_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            contentFrame.Navigate(typeof(AboutPage));
            splitView.IsPaneOpen = false;
        }

        private void KebiaoItem_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            contentFrame.Navigate(typeof(KebiaoPage));
            splitView.IsPaneOpen = false;
        }
    }
}
