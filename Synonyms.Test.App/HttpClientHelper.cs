using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Synonyms.Test.App
{
    public static class HttpClientHelper
    {
        private static readonly HttpClient _httpClient = new HttpClient();

        private static void PrepareData(string queryString, object postData, out Uri url, out string data)
        {
            var baseUrl = new Uri(SettingsHelper.BaseUrl);
            url = new Uri(baseUrl, queryString);

            if (postData != null)
                data = JsonConvert.SerializeObject(postData);
            else
                data = null;
        }

        private static async Task<T> ParseResponse<T>(HttpResponseMessage result)
        {
            if (result.StatusCode == HttpStatusCode.OK)
            {
                string resultContent = await result.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<T>(resultContent);
            }
            else
            {
                string resultContent = await result.Content.ReadAsStringAsync();
                var error = JsonConvert.DeserializeObject<Models.ErrorDetails>(resultContent);
                throw new Exception(error.Message);
            }
        }

        public static async Task<T> Get<T>(string queryString)
        {
            PrepareData(queryString, null, out Uri url, out string data);

            using (var result = await _httpClient.GetAsync(url))
            {
                string content = await result.Content.ReadAsStringAsync();
                return await ParseResponse<T>(result);
            }
        }

        public static async Task<T> Post<T>(string queryString, object postData)
        {
            PrepareData(queryString, postData, out Uri url, out string data);

            using (var content = new StringContent(data, Encoding.UTF8, "application/json"))
            {
                var result = await _httpClient.PostAsync(url, content);
                return await ParseResponse<T>(result);
            }
        }

        public static async Task<T> Put<T>(string queryString, object postData)
        {
            PrepareData(queryString, postData, out Uri url, out string data);

            using (var content = new StringContent(data, Encoding.UTF8, "application/json"))
            {
                var result = await _httpClient.PutAsync(url, content);
                return await ParseResponse<T>(result);
            }
        }
    }
}
