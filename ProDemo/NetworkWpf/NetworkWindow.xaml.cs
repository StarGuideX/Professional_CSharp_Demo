using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace NetworkWpf
{
    /// <summary>
    /// NetworkWindow.xaml 的交互逻辑
    /// </summary>
    public partial class NetworkWindow : Window
    {
        private const string NorthwindUrl = "http://services.odata.org/Northwind/Northwind.svc/Regions";
        private const string IncorrectUrl = "http://services.odata.org/Northwind1/Northwind.svc/Regions";
        public NetworkWindow()
        {
            InitializeComponent();
        }
        #region HttpClient相关
        /// <summary>
        /// 单击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void HttpClientBtn_Click(object sender, RoutedEventArgs e)
        {
            await GetDataSimpleAsync();
        }

        /// <summary>
        /// HttpClient相关
        /// </summary>
        /// <returns></returns>
        private async Task GetDataSimpleAsync()
        {
            #region 正确的Url
            using (var client = new HttpClient())
            {
                //接受响应
                HttpResponseMessage response = await client.GetAsync(NorthwindUrl);
                //判断状态code
                if (response.IsSuccessStatusCode)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append($"Response Status Code:{(int)response.StatusCode}" + $"{response.ReasonPhrase}\r\n");
                    sb.Append(await response.Content.ReadAsStringAsync());
                    showText.Text = sb.ToString();
                }
            }
            #endregion
            #region 错误的url和异常示例
            //using (var client = new HttpClient())
            //{
            //    //接受响应
            //    HttpResponseMessage response = await client.GetAsync(IncorrectUrl);

            //    //如果StatusCode为False，则会抛出异常
            //    response.EnsureSuccessStatusCode();
            //    StringBuilder sb = new StringBuilder();
            //    sb.Append($"Response Status Code:{(int)response.StatusCode}" + $"{response.ReasonPhrase}\r\n");
            //    sb.Append(await response.Content.ReadAsStringAsync());
            //    showText.Text = sb.ToString();
            //}
            #endregion
        }
        #endregion
        #region HttpClient标题
        /// <summary>
        /// 单击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void GetDataWithHeadersBtn_Click(object sender, RoutedEventArgs e)
        {
            await GetDataWithHeaders();
        }
        /// <summary>
        /// HttpClient标题
        /// </summary>
        /// <returns></returns>
        private async Task GetDataWithHeaders()
        {
            try
            {
                using (var client = new HttpClient())
                {
                    //传递标题
                    client.DefaultRequestHeaders.Add("Accept", "application/json;odata=verbose");
                    StringBuilder sb = new StringBuilder();
                    //请求头
                    sb.Append(ShowHeaders("Request Headers:", client.DefaultRequestHeaders));

                    HttpResponseMessage response = await client.GetAsync(NorthwindUrl);
                    response.EnsureSuccessStatusCode();
                    //响应头
                    sb.Append(ShowHeaders("Response Headers:", response.Headers));

                    showText.Text = sb.ToString();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// HttpClient标题
        /// </summary>
        /// <param name="tittle"></param>
        /// <param name="headers"></param>
        /// <returns></returns>
        private string ShowHeaders(string tittle, HttpHeaders headers)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(tittle + "\r\n");
            foreach (var item in headers)
            {
                string value = string.Join(" ", item.Value);
                sb.Append($"header:{item.Key} Value:{value} \r\n");
            }
            sb.Append("\r\n");
            return sb.ToString();
        }
        #endregion
        #region Uri和Uribuilder
        private void UriBtn_Click(object sender, RoutedEventArgs e)
        {
            string uri = "https://item.jd.com/12179822.html";
            var page = new Uri(uri);
            StringBuilder sb = new StringBuilder();
            //请求头
            sb.Append($"host:{page.Host},type:{page.HostNameType}\r\n");
            sb.Append($"port:{page.Port}\r\n");
            sb.Append($"path:{page.AbsolutePath}\r\n");
            sb.Append($"query:{page.Query}\r\n");
            foreach (var item in page.Segments)
            {
                sb.Append($"segment:{item}\r\n");
            }
            showText.Text = sb.ToString();
        }
        private void UribuilderBtn_Click(object sender, RoutedEventArgs e)
        {
            var builder = new UriBuilder();
            builder.Host = "www.cninnovation.com";
            builder.Port = 80;
            builder.Path = "training/MVC";
            Uri uri = builder.Uri;
            showText.Text = uri.ToString();
        }
        #endregion


    }
}
