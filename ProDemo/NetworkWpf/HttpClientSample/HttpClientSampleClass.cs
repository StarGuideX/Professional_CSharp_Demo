using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace NetworkWpf.HttpClientSample
{
    public class HttpClientSampleClass
    {
        private const string NorthwindUrl = "http://services.odata.org/Northwind/Northwind.svc/Regions";
        private const string IncorrectUrl = "http://services.odata.org/Northwind1/Northwind.svc/Regions";

        /// <summary>
        /// HttpClient相关
        /// </summary>
        /// <returns></returns>
        public async Task GetDataSimpleAsync()
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
                    Console.WriteLine(sb.ToString());
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

        /// <summary>
        /// HttpClient标题
        /// </summary>
        /// <returns></returns>
        public async Task GetDataWithHeaders()
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
                    Console.WriteLine(sb.ToString());
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
    }
}
