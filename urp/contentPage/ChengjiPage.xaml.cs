using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.RegularExpressions;
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
using HtmlAgilityPack;
using Newtonsoft.Json;
using urp.Struct;
using urp.Util;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace urp
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class ChengjiPage : Page
    {
        private Frame root = Window.Current.Content as Frame;
        private bool isFirst = true;
        private bool isFirst2 = true;
        private UrpUtil urpUtil;
        public ChengjiPage()
        {
            this.InitializeComponent();
            urpUtil = new UrpUtil();
        }

        private async void Pivot1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
            Ring.IsActive = true;
            try
            {
                switch (Pivot1.SelectedIndex)
                {
                    case 0:
                        if (isFirst)
                        {
                            string jigehtml = await urpUtil.getGoodScore();
                            if (jigehtml.Contains("SESSION"))
                            {
                                root.Navigate(typeof(MainPage), 1);
                            }
                            else
                            {

                                List<AllScore> allScores = new List<AllScore>();
                                urpUtil.getGoodScoreList(allScores, jigehtml);
                                if (allScores.Count > 0)
                                {
                                    this.itemcollectionSource.Source = allScores;
                                    OutListView1.ItemsSource = itemcollectionSource.View.CollectionGroups;
                                    InListView1.ItemsSource = itemcollectionSource.View;
                                }
                                else
                                {
                                    SemanticZoom1.Visibility = Visibility.Collapsed;
                                    NoImage1.Source = new BitmapImage(new Uri("ms-appx:///Assets/null.png"));
                                    NoTextBlock1.Text = "新同学！还没有成绩呢，去别处看看吧！";
                                }
                                isFirst = false;    
                            }
                        }

                        break;
                    case 1:
                        if (isFirst2)
                        {
                            string bujigehtml = await urpUtil.getBadScore();
                            if (bujigehtml.Contains("SESSION"))
                                root.Navigate(typeof(MainPage), 1);
                            else
                            {
                                List<AllScore> allScores = new List<AllScore>();
                                urpUtil.getBadScoreList(allScores, bujigehtml);
                                if (allScores.Count > 0)
                                {
                                    this.itemcollectionSource.Source = allScores;
                                    OutListView2.ItemsSource = itemcollectionSource.View.CollectionGroups;
                                    InListView2.ItemsSource = itemcollectionSource.View;
                                }
                                else
                                {
                                    SemanticZoom2.Visibility = Visibility.Collapsed;
                                    NoImage2.Source = new BitmapImage(new Uri("ms-appx:///Assets/nobad.png"));
                                    NoTextBlock2.Text = "没有挂科呢！奖励一朵小红花！";
                                }
                                isFirst2 = false;
                            }
                        }

                        break;
                }
            }
            catch (COMException)
            {
                root.Navigate(typeof(MainPage), 1);
            }
            catch (Exception exception)
            {
                Notification.Show("获取信息失败，请重试", 3000);
                Console.WriteLine(exception);
            }
            finally
            {
                Ring.IsActive = false;
            }
        }

    }
}
