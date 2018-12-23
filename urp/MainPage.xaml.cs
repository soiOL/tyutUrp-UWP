using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using HtmlAgilityPack;
using Microsoft.Toolkit.Extensions;
using Microsoft.Toolkit.Uwp.UI.Extensions;
using Newtonsoft.Json;
using urp.contentPage;
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
            await Login();
        }

        //页面打开时
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter != null)
            {
                if (e.Parameter.ToString().Equals("1"))
                {
                    LoginNotification.Show("登陆过期，请重新登陆", 3000);
                }
                else if(e.Parameter.ToString().Equals("0"))
                {
                    await getCheckCode();
                }
            }
            EcardPage.IsLogin = false;
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
            if (localSettings.Values["URL"] != null && localSettings.Values["isDefaultUrl"] != null)
            {
                if (!(bool) localSettings.Values["isDefaultUrl"])
                {
                    UrpApi2.URL = (string)localSettings.Values["URL"];
                    UrpApi2.STUURL = (string)localSettings.Values["STUURL"];
                    ChangedButton.IsChecked = true;
                    UrpBox.IsEnabled = true;
                    StuBox.IsEnabled = true;
                }
                UrpBox.Text = (string)localSettings.Values["URL"];
                StuBox.Text = (string)localSettings.Values["STUURL"];
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
                Setting.IsEnabled = false;
                CheckCodeRing.IsActive = true;
                BitmapImage bitmapImage = await webUtil.GetImage(new Uri(UrpApi2.URL+UrpApi2.URL_YZM));
                CheckCodeImage.Source = bitmapImage;
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                LoginNotification.Show("获取验证码失败，请重试",3000);
            }
            finally
            {
                Setting.IsEnabled = true;
                CheckCodeRing.IsActive = false;
            }
        }

        //打开弹窗
        private async void Setting_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            await ContentDialog.ShowAsync();
        }

        //点击弹窗的保存按钮
        private async void ContentDialog_OnPrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            string urpString = UrpApi2.DEFAULTURL;
            string stuString = UrpApi2.DEFAULTSTUURL;
            if (DefaultButton.IsChecked == true)
            {
                UrpApi2.URL = UrpApi2.DEFAULTURL;
                UrpApi2.STUURL = UrpApi2.DEFAULTSTUURL;
                localSettings.Values["isDefaultUrl"] = true;
            }
            else
            {
                urpString = UrpBox.Text;
                if (!urpString.EndsWith("/"))
                {
                    urpString = urpString + "/";
                }
                stuString = StuBox.Text;
                if (!stuString.EndsWith("/"))
                {
                    stuString = stuString + "/";
                }
                UrpApi2.URL = urpString;
                UrpApi2.STUURL = stuString;
                localSettings.Values["URL"] = urpString;
                localSettings.Values["STUURL"] = stuString;
                localSettings.Values["isDefaultUrl"] = false;
            }

            await getCheckCode();

        }

        
        //选择默认链接按钮
        private void DefaultButton_OnClick(object sender, RoutedEventArgs e)
        {
            UrpBox.IsEnabled = false;
            StuBox.IsEnabled = false;
        }

        //选择自定义链接按钮
        private void ChangedButton_OnClick(object sender, RoutedEventArgs e)
        {
            UrpBox.IsEnabled = true;
            StuBox.IsEnabled = true;
        }

        //回车登录
        private async void Page_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Enter)
            {
                await Login();
            }
        }

        //登录事件
        private async Task Login()
        {
            StuUser user = new StuUser();
            user.userName = UserNameBox.Text;
            user.passWord = PassWordBox.Password;
            user.checkCode = CheckCode.Text;
            CheckCodeImage.IsTapEnabled = false;
            if (string.IsNullOrEmpty(user.userName) || !user.userName.IsNumeric())
            {
                LoginNotification.Show("请输入正确的用户名", 2000);
                return;
            }

            if (string.IsNullOrEmpty(user.passWord))
            {
                LoginNotification.Show("请输入密码", 2000);
                return;
            }

            if (string.IsNullOrEmpty(user.checkCode))
            {
                LoginNotification.Show("请输入验证码", 2000);
                return;
            }
            LoginRing.IsActive = true;
            LoginButton.IsEnabled = false;
            Setting.IsEnabled = false;
            try
            {
                List<KeyValuePair<string, string>> paramList = new List<KeyValuePair<string, string>>();
                paramList.Add(new KeyValuePair<string, string>("zjh", user.userName));
                paramList.Add(new KeyValuePair<string, string>("mm", user.passWord));
                paramList.Add(new KeyValuePair<string, string>("v_yzm", user.checkCode));
                string result = await webUtil.PostString(UrpApi2.URL + UrpApi2.URL_LOGIN, paramList);
                var doc = new HtmlDocument();
                doc.LoadHtml(result);
                result = doc.DocumentNode.SelectSingleNode("//title").InnerText;
                if (result.Contains("学分制综合教务"))
                {
                    localSettings.Values["userName"] = user.userName;
                    localSettings.Values["passWord"] = user.passWord;
                    if (localSettings.Values[user.userName] == null)
                    {
                        localSettings.Values[user.userName] = "123456";
                    }
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
                    result = doc.DocumentNode.SelectSingleNode("//td[@class='errorTop']").InnerText;
                    if (result.Contains("验证码"))
                    {
                        LoginNotification.Show("验证码错误", 3000);
                    }
                    else if (result.Contains("证件号"))
                    {
                        LoginNotification.Show("用户名错误", 3000);
                    }
                    else if (result.Contains("密码"))
                    {
                        LoginNotification.Show("密码错误", 3000);
                    }
                    else
                    {
                        LoginNotification.Show("数据库忙，请重试", 3000);
                    }
                    await getCheckCode();
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                LoginNotification.Show("网络错误", 3000);
            }
            finally
            {
                LoginRing.IsActive = false;
                LoginButton.IsEnabled = true;
                Setting.IsEnabled = true;
                CheckCodeImage.IsTapEnabled = true;
            }
        }
    }
}
