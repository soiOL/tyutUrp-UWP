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
            Ring.IsActive = true;
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
                Notification.Show("获取信息失败",3000);
            }
            else
            {
                var jidianStruct = JsonConvert.DeserializeObject<JidianStruct>(result);
                List<JidianInfo> strList = new List<JidianInfo>();
                strList.Add(new JidianInfo(){values = "学号：" + jidianStruct.xh});
                strList.Add(new JidianInfo() { values = "姓名：" + jidianStruct.xm});
                strList.Add(new JidianInfo() { values = "班级：" + jidianStruct.bjh});
                strList.Add(new JidianInfo() { values = "要求总学分：" + jidianStruct.zxf});
                strList.Add(new JidianInfo() { values = "已修课程学分：" + jidianStruct.yxzxf});
                strList.Add(new JidianInfo() { values = "已修自主实践学分：" + jidianStruct.yxzzsjxf});
                strList.Add(new JidianInfo() { values = "曾不及格学分：" + jidianStruct.cbjgxf});
                strList.Add(new JidianInfo() { values = "尚不及格学分：" + jidianStruct.sbjgxf});
                strList.Add(new JidianInfo() { values = "GPA：" + jidianStruct.pjxfjd});
                strList.Add(new JidianInfo() { values = "GPA班级排名：" + jidianStruct.gpabjpm});
                strList.Add(new JidianInfo() { values = "GPA专业排名：" + jidianStruct.gpazypm });
                strList.Add(new JidianInfo() { values = "GPA大类排名：" + jidianStruct.gpadlpm });
                strList.Add(new JidianInfo() { values = "加权学分成绩：" + jidianStruct.jqxfcj });
                strList.Add(new JidianInfo() { values = "加权班级排名：" + jidianStruct.jqbjpm });
                strList.Add(new JidianInfo() { values = "加权专业排名：" + jidianStruct.jqzypm });
                strList.Add(new JidianInfo() { values = "平均成绩：" + jidianStruct.pjcj });
                strList.Add(new JidianInfo() { values = "平均成绩班级排名：" + jidianStruct.pjcjbjpm });
                strList.Add(new JidianInfo() { values = "平均成绩专业排名：" + jidianStruct.pjcjzypm });
                strList.Add(new JidianInfo() { values = "统计时间：" + jidianStruct.tjsj });
                GridView.ItemsSource = strList;
            }

            Ring.IsActive = false;
        }

    }

    class JidianInfo
    {
        public string values { get; set; }
    }
}
