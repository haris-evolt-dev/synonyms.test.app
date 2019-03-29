using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Synonyms.Test.App
{
    public static class HttpClientHelper
    {
        private static readonly HttpClient _httpClient = new HttpClient();

        public static async Task<T> Get<T>(string queryString)
        {
            var baseUrl = new Uri(SettingsHelper.BaseUrl);
            var url = new Uri(baseUrl, queryString);

            using (var result = await _httpClient.GetAsync(url))
            {
                string content = await result.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<T>(content);
            }
        }

        public static async Task<T> Post<T>(string queryString, object postData)
        {
            var baseUrl = new Uri(SettingsHelper.BaseUrl);
            var url = new Uri(baseUrl, queryString);

            var data = JsonConvert.SerializeObject(postData);

            using (var content = new StringContent(data, Encoding.UTF8, "application/json"))
            {
                var result = await _httpClient.PostAsync(url, content);
                if (result.StatusCode == HttpStatusCode.OK)
                {
                    string resultContent = await result.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<T>(resultContent);
                }
                else
                {
                    // Do something with the contents, like write the statuscode and
                    // contents to a log file
                    string resultContent = await result.Content.ReadAsStringAsync();
                    throw new Exception(resultContent);
                    // ... write to log
                }
            }
        }

        public static async Task<T> Put<T>(string queryString, object postData)
        {
            var baseUrl = new Uri(SettingsHelper.BaseUrl);
            var url = new Uri(baseUrl, queryString);

            var data = JsonConvert.SerializeObject(postData);

            using (var content = new StringContent(data, Encoding.UTF8, "application/json"))
            {
                var result = await _httpClient.PutAsync(url, content);
                if (result.StatusCode == HttpStatusCode.OK)
                {
                    string resultContent = await result.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<T>(resultContent);
                }
                else
                {
                    // Do something with the contents, like write the statuscode and
                    // contents to a log file
                    string resultContent = await result.Content.ReadAsStringAsync();
                    throw new Exception(resultContent);
                    // ... write to log
                }
            }
        }
    }
}
