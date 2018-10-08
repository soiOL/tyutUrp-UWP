﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using Microsoft.Toolkit.Extensions;
using Newtonsoft.Json;
using urp.Struct;
using urp.Util;

// https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x804 上介绍了“空白页”项模板

namespace urp
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private Frame root = Window.Current.Content as Frame;
        private ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
        private WebUtil webUtil;
        public MainPage()
        {
            this.InitializeComponent();
            webUtil = new WebUtil();
        }

        //登录操作
        private async void LoginButton_OnClick(object sender, RoutedEventArgs e)
        {
            StuUser user = new StuUser();
            user.userName = UserNameBox.Text;
            user.passWord = PassWordBox.Password;
            user.checkCode = CheckCode.Text;
            if (string.IsNullOrEmpty(user.userName) || !user.userName.IsNumeric())
            {
                LoginNotification.Show("请输入用户名");
                return;
            }

            if (string.IsNullOrEmpty(user.passWord))
            {
                LoginNotification.Show("请输入密码");
                return;
            }
             
            if (string.IsNullOrEmpty(user.checkCode))
            {
                LoginNotification.Show("请输入验证码");
                return;
            }
            LoginRing.IsActive = true;
            LoginButton.IsEnabled = false;
            try
            {
                string LoginJson = JsonConvert.SerializeObject(user);
                //LoginJson = Secret.encryption(LoginJson);
                List<KeyValuePair<string, string>> paramList = new List<KeyValuePair<string, string>>();
                paramList.Add(new KeyValuePair<string, string>("param", LoginJson));
                String result = await webUtil.PostString(UrpApi.BASEURL+UrpApi.LOGIN, paramList);
                if (result.Equals("success"))
                {
                    localSettings.Values["userName"] = user.userName;
                    localSettings.Values["passWord"] = user.passWord;
                    if (IsSaveBox.IsChecked == true)
                    {
                        localSettings.Values["isSave"] = true;
                    }
                    else
                    {
                        localSettings.Values["isSave"] = false;
                    }
                    root.Navigate(typeof(MainView));
                }
                else
                {
                    if (result.Equals("checkError"))
                    {
                        LoginNotification.Show("验证码错误");
                    }
                    else if (result.Equals("userNameError"))
                    {
                        LoginNotification.Show("用户名错误");
                    }
                    else if (result.Equals("passWordError"))
                    {
                        LoginNotification.Show("密码错误");
                    }
                    else
                    {
                        LoginNotification.Show("服务端错误");
                    }
                    await getCheckCode();
                }
            }
            catch (System.Runtime.InteropServices.COMException exception)
            {
                LoginNotification.Show("无法连接网络");
            }
            finally
            {
                LoginRing.IsActive = false;
                LoginButton.IsEnabled = true;
                
            }
        }

        //页面打开时
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
           
            if (localSettings.Values["isSave"] != null)
            {
                bool isSave = (bool)localSettings.Values["isSave"];
                if (isSave)
                {
                    UserNameBox.Text = (string)localSettings.Values["userName"];
                    PassWordBox.Password = (string)localSettings.Values["passWord"];
                    IsSaveBox.IsChecked = true;
                }
                else
                {
                    UserNameBox.Text = (string)localSettings.Values["userName"];
                }
            }
            await getCheckCode();
        }

        //点击验证码图片更换验证码
        private async void CheckCodeImage_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            await getCheckCode();
        }


        //获取验证码
        private async Task getCheckCode()
        {
            try
            {
                CheckCodeRing.IsActive = true;
                BitmapImage bitmapImage = await webUtil.GetImage(new Uri(UrpApi.BASEURL+UrpApi.GETCHECKCODE));
                CheckCodeImage.Source = bitmapImage;
            }
            catch (System.Runtime.InteropServices.COMException exception)
            {
                LoginNotification.Show("无法连接网络");
            }
            finally
            {
                CheckCodeRing.IsActive = false;
            }
        }
    }
}
