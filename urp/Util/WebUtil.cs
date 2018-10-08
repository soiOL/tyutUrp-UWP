using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;
using Windows.Web.Http;
using Windows.Web.Http.Filters;
using HttpClient = Windows.Web.Http.HttpClient;

namespace urp.Util
{
    class WebUtil
    {
        private ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
        private HttpClient httpClient = new HttpClient();

        //get请求获取字符串
        public async Task<string> GetString(string uri)
        {                                                                                                                     
            var response = await httpClient.GetStringAsync(new Uri(uri));
            return response;
        }

        ////get请求获取字符串,带cookie
        //public async Task<string> GetStringWithCookie(string uri)
        //{
        //    HttpCookie httpCookie = new HttpCookie("JSESSIONID", "urp.soiol.cn", "/");
        //    HttpRequestMessage httpRequest = new HttpRequestMessage(new HttpMethod("GET"), new Uri(uri));
        //    String cookie = localSettings.Values["Cookie"].ToString();
        //    httpCookie.Value = cookie;
        //    HttpBaseProtocolFilter filter = new HttpBaseProtocolFilter();
        //    filter.CookieManager.SetCookie(httpCookie, false);
        //    var response = await httpClient.SendRequestAsync(httpRequest);
        //    return response.Content.ToString();
        //}


        //get请求获取网络图片
        public async Task<BitmapImage> GetImage(Uri uri)
        {
            var response = await httpClient.GetAsync(uri);
            var buffer = response.Content.ReadAsBufferAsync().GetResults();
            //if (response.Headers.ToString().Contains("set-cookie"))
            //{
            //    String pattern1 = "Path=/; HttpOnly";
            //    string pattern2 = "JSESSIONID=";
            //    string cookie = response.Headers["set-cookie"].Replace(pattern1, "");
            //    cookie = cookie.Replace(pattern2, "");
            //    localSettings.Values["Cookie"] = cookie;
            //}
            BitmapImage img = new BitmapImage();
            using (IRandomAccessStream stream = new InMemoryRandomAccessStream())
            {
                await stream.WriteAsync(buffer);
                stream.Seek(0);
                await img.SetSourceAsync(stream);
                stream.Dispose();   
                return img;
            }
        }

        //post请求获取字符串
        public async Task<string> PostString(string uri, List<KeyValuePair<string, string>> paramList)
        {
            //HttpClient httpClient = new HttpClient();
            var response = await httpClient.PostAsync(new Uri(uri),new HttpFormUrlEncodedContent(paramList) );
            var result = await response.Content.ReadAsStringAsync();
            return result;
        }
    }
}


