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
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using Microsoft.Toolkit.Uwp.UI.Controls;
using urp.Util;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace urp.contentPage
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainViewPage : Page
    {
        private static readonly int SUCCESS = 1;
        private static readonly int TIMEOUT = 0;
        private static readonly int FAIL = -1;
        private Frame root = Window.Current.Content as Frame;

        public MainViewPage()
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
            var urpUtil = new UrpUtil();
            var map = new Dictionary<string, string>();
            int result = await urpUtil.GetUserInfo(map);
            if (result == SUCCESS)
            {
                string name = "姓名：" + map["姓名:"];
                string stunumber = "学号：" + map["学号:"];
                string xisuo = "学院：" + map["系所:"];
                string banji = "班级：" + map["班级:"];
                NameText.Text = name;
                StuNumberText.Text = stunumber;
                XiSuoText.Text = xisuo;
                BanJiText.Text = banji;
                var image = await urpUtil.GetUserImage();
                UserImage.ImageSource = image;
            }
            else
            {
                root.Navigate(typeof(MainPage),1);
            }

        }
    }
}
