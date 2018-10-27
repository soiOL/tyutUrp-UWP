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
using Windows.UI.Xaml.Navigation;
using Microsoft.Toolkit.Uwp.UI.Controls;
using Newtonsoft.Json;
using urp.Struct;
using urp.Util;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace urp
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class KebiaoPage : Page
    {
        private Frame root = Window.Current.Content as Frame;
        private List<string> monList;
        private List<string> tueList;
        private List<string> wedList;
        private List<string> thuList;
        private List<string> friList;
        private List<string> satList;
        private List<string> sunList;
        private List<KebiaoStruct> kebiaoList;
        public KebiaoPage()
        {
            this.InitializeComponent();
        }

        //页面打开时
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            Ring.IsActive = true;
            try
            {
                WebUtil webUtil = new WebUtil();
                string result = await webUtil.GetString(UrpApi2.URL + UrpApi2.URL_KB);
                if (result.Contains("学生选课结果"))
                {
                    WebView.Navigate(new Uri(UrpApi2.URL + UrpApi2.URL_KB));
                }
                else
                {
                    root.Navigate(typeof(MainPage), 1);
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                Notification.Show("获取信息失败", 3000);
            }
            finally
            {
                Ring.IsActive = false;
            }
            
            //await getKebiao();

        }

        private async Task getKebiao()
        {
            WebUtil webUtil = new WebUtil();
            try
            {
                string result = await webUtil.GetString(UrpApi2.URL + UrpApi2.URL_KB);
                if (result.Equals("session"))
                {
                    root.Navigate(typeof(MainPage),1);
                }
                else if (result.Equals("wrong"))
                {
                    Notification.Show("获取信息失败，请重试", 3000);
                }
                else
                {
                    var jsonStruct = JsonConvert.DeserializeObject<Dictionary<String, List<string>>>(result);
                    kebiaoList = new List<KebiaoStruct>();
                    monList = jsonStruct["星期一"];
                    tueList = jsonStruct["星期二"];
                    wedList = jsonStruct["星期三"];
                    thuList = jsonStruct["星期四"];
                    friList = jsonStruct["星期五"];
                    satList = jsonStruct["星期六"];
                    sunList = jsonStruct["星期日"];
                    int count = 0;
                    foreach (var mon in monList)
                    {
                        kebiaoList.Add(new KebiaoStruct()
                        {
                            Count = "第"+(count+1)+"小节",
                            Mon = monList[count],
                            Tue = tueList[count],
                            Wed = wedList[count],
                            Thu = thuList[count],
                            Fri = friList[count],
                            Sat = satList[count],
                            Sun = sunList[count++]
                        });
                    }

                    //KebiaoGrid.ItemsSource = kebiaoList;
                }
            }
            catch (Exception e)
            {
                Notification.Show("获取信息失败，请重试", 3000);
                Console.WriteLine(e);
            }
        }
    }
}
