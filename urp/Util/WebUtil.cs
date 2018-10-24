using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;
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
        


        //get请求获取网络图片
        public async Task<BitmapImage> GetImage(Uri uri)
        {
            var response = await httpClient.GetAsync(uri);
            var buffer = response.Content.ReadAsBufferAsync().GetResults();
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
            
            var response = await httpClient.PostAsync(new Uri(uri),new HttpFormUrlEncodedContent(paramList) );
            var result = await response.Content.ReadAsStringAsync();
            return result;
        }
    }
}


