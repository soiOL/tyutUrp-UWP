using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using Windows.ApplicationModel.Core;
using Windows.Storage;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using urp.Struct;
using urp.Util;


// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace urp.contentPage
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class EcardPage : Page
    {
        private static readonly int SUCCESS = 1;
        private static readonly int TIMEOUT = 0;
        private static readonly int FAIL = -1;
        private UrpUtil urpUtil;
        private ApplicationDataContainer localSettings;
        private ObservableCollection<Value> MoneyList;
        private DateTime NowDateTime;
        private bool IsLoadingList = false;
        private bool IsAllLoaded = false;
        public static bool IsLogin = false;

        //线程锁
        private object o = new object();


        public EcardPage()
        {
            this.InitializeComponent();
            urpUtil = new UrpUtil();
            localSettings = ApplicationData.Current.LocalSettings;
            MoneyList = new ObservableCollection<Value>();
            NowDateTime = DateTime.Now;
            GridView.ItemsSource = MoneyList;
        }

        //页面打开时
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            int status = TIMEOUT;
            if (!IsLogin)
            {
                status = await urpUtil.LoginEcard();
            }
            else
            {
                status = SUCCESS;
            }
            if (status == SUCCESS)
            {
                IsLogin = true;
                GetInfo();
            }
            else if (status == FAIL)
            {
                Ring1.IsActive = false;
                Ring2.IsActive = false;
                Ring3.IsActive = false;
                Ring4.IsActive = false;
                await ContentDialog.ShowAsync();
            }
            else
            {
                Ring1.IsActive = false;
                Ring2.IsActive = false;
                Ring3.IsActive = false;
                Ring4.IsActive = false;
                Notification.Show("获取信息失败,请重试",3000);
            }
        }
        

        private void GetInfo()
        {
            Thread moneyThread = new Thread(GetMoneyDelegate);
            Thread listThread = new Thread(GetMoneyListDelegate);
            Thread thisThread = new Thread(GetThisMonthMoneyDelegate);
            Thread lastThread = new Thread(GetLastMonthMoneyDelegate);
            moneyThread.Start();
            listThread.Start();
            thisThread.Start();
            lastThread.Start();
        }

        private async void GetMoneyDelegate()
        {
            await CoreApplication.MainView.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                new DispatchedHandler(GetMoney));
        }
        private async void GetMoney()
        {
            //获取余额
            try
            {
                string money = await urpUtil.GetMoney();
                NowMoney.Text = money;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Notification.Show("获取余额失败,请重试",3000);
            }
            finally
            {
                Ring1.IsActive = false;
            }
            
        }

        private async void GetMoneyListDelegate()
        {
            await CoreApplication.MainView.Dispatcher.RunAsync(CoreDispatcherPriority.High,
                new DispatchedHandler(GetMoneyList));
        }
        private async void GetMoneyList()
        {
            //获取账单
            try
            {
                Ring4.IsActive = true;
                string endDate = string.Format("{0:yyyy-MM-dd}", NowDateTime);
                NowDateTime = NowDateTime.AddMonths(-1);
                string startDate = string.Format("{0:yyyy-MM-dd}", NowDateTime);
                NowDateTime = NowDateTime.AddDays(-1);
                List<Value> moneyList = await urpUtil.GetMoneyList(startDate, endDate);
                if (moneyList.Count == 0)
                {
                    IsAllLoaded = true;
                }
                else
                {
                    foreach (var moneyStruct in moneyList)
                    {
                        MoneyList.Add(moneyStruct);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Notification.Show("账单获取失败,请重试",3000);
            }
            finally
            {
                Ring4.IsActive = false;
                IsLoadingList = false;
            }
        }

        private async void GetThisMonthMoneyDelegate()
        {
            await CoreApplication.MainView.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                new DispatchedHandler(GetThisMonthMoney));
        }
        private async void GetThisMonthMoney()
        {
            //计算本月花费
            try
            {
                DateTime dateTime = DateTime.Now;
                string startDate = string.Format("{0:yyyy-MM}", dateTime) + "-01";
                string endDate = string.Format("{0:yyyy-MM-dd}", dateTime);
                List<Value> moneyList = await urpUtil.GetMoneyList(startDate, endDate);
                double allMoney = 0;
                foreach (var moneyStruct in moneyList)
                {
                    double money = ParseDouble(moneyStruct.ConsumeAmount);
                    if (money < 0)
                    {
                        allMoney += Math.Abs(money);
                    }
                }

                ThisMonthMoney.Text = allMoney.ToString();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Notification.Show("获取本月消费总额失败,请重试",3000);
            }
            finally
            {
                Ring2.IsActive = false;
            }
            
        }

        private async void GetLastMonthMoneyDelegate()
        {
            await CoreApplication.MainView.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                new DispatchedHandler(GetLastMonthMoney));
        }
        private async void GetLastMonthMoney()
        {
            //计算上月花费
            try
            {
                DateTime dateTime = DateTime.Now;
                string startDate = string.Format("{0:yyyy-MM}", dateTime.AddMonths(-1)) + "-01";
                string endDate = string.Format("{0:yyyy-MM-dd}", dateTime.AddDays(-dateTime.Day));
                List<Value> moneyList = await urpUtil.GetMoneyList(startDate, endDate);
                double allMoney = 0;
                foreach (var moneyStruct in moneyList)
                {
                    double money = ParseDouble(moneyStruct.ConsumeAmount);
                    if (money < 0)
                    {
                        allMoney += Math.Abs(money);
                    }
                }

                LastrMonthMoney.Text = allMoney.ToString();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Notification.Show("获取上月消费总额失败,请重试",3000);
            }
            finally
            {
                Ring3.IsActive = false;
            }

        }

        //获取金额转换为浮点数
        public double ParseDouble(string intStr, double defaultValue = 0)
        {
            double parseDouble;
            if (double.TryParse(intStr, out parseDouble))
                return parseDouble;
            return defaultValue;
        }

        //修改密码后重新获取数据
        private void ContentDialog_OnPrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            localSettings.Values[(string)localSettings.Values["userName"]] = PassWordBox.Password;
            GetInfo();
        }

        //下拉自动获取更多数据
        private void GridView_ContainerContentChanging(ListViewBase sender, ContainerContentChangingEventArgs args)
        {
            lock (o)
            {
                if (!IsAllLoaded && !IsLoadingList)
                {
                    if (GridView.Items != null && args.ItemIndex == GridView.Items.Count - 1)
                    {
                        IsLoadingList = true;
                        Thread getListThread = new Thread(GetMoneyListDelegate);
                        getListThread.Start();
                    }
                }
            }
        }
    }
}
