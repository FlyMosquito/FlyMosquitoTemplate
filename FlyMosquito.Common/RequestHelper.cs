#region using
using System.Text;
#endregion

namespace FlyMosquito.Common
{
    public class RequestHelper
    {
        private readonly HttpClient Client;

        /// <summary>
        /// 支持通过构造函数传入HttpClient，如果不传则使用默认的单例HttpClient。
        /// </summary>
        /// <param name="HttpClient">自定义的HttpClient实例</param>
        public RequestHelper(HttpClient HttpClient = null)
        {
            Client = HttpClient ?? new HttpClient();
            //{
            //    Timeout = TimeSpan.FromSeconds(3000)
            //};
        }

        /// <summary>
        /// 发送GET请求并返回响应内容
        /// </summary>
        /// <param name="StringUrl">请求的完整URL或相对URL（取决于 HttpClient 的 BaseAddress）</param>
        /// <param name="Headers">自定义请求头</param>
        /// <returns>响应内容的 JSON 字符串</returns>
        public async Task<string> GetAsync(string StringUrl, Dictionary<string, string> Headers = null)
        {
            try
            {
                using var Request = new HttpRequestMessage(HttpMethod.Get, StringUrl);
                AddHeaders(Request, Headers);
                using HttpResponseMessage HttpResponseMessage = await Client.SendAsync(Request);
                HttpResponseMessage.EnsureSuccessStatusCode();//确保响应成功 (2xx)，否则抛出异常
                string StringJsonResponse = await HttpResponseMessage.Content.ReadAsStringAsync();//读取响应内容
                return StringJsonResponse;
            }
            catch (HttpRequestException ex)
            {
                LoggerHelper.Error($"GET请求错误: {ex.Message}");
                throw new Exception("GET请求错误:", ex);
            }
            catch (TaskCanceledException ex)
            {
                LoggerHelper.Error($"请求超时: {ex.Message}");
                throw new Exception("请求超时", ex);
            }
            catch (Exception ex)
            {
                LoggerHelper.Error($"GET请求发生未知错误: {ex.Message}");
                throw new Exception("GET请求发生未知错误:", ex);
            }
        }

        /// <summary>
        /// 发送POST请求，内容为字符串形式的JSON，数据可选
        /// </summary>
        /// <param name="StringUrl">请求的完整URL或相对URL</param>
        /// <param name="Data">要序列化为JSON的对象，传入null则不发送请求体</param>
        /// <param name="Headers">自定义请求头</param>
        /// <returns>响应内容的 JSON 字符串</returns>
        public async Task<string> PostAsync(string StringUrl, object Data = null, Dictionary<string, string> Headers = null)
        {
            try
            {
                using var Request = new HttpRequestMessage(HttpMethod.Post, StringUrl);
                AddHeaders(Request, Headers);
                if (Data != null)
                {
                    var StringJsonData = JsonHelper.Deserialize<string>((byte[])Data);
                    Request.Content = new StringContent(StringJsonData, Encoding.UTF8, "application/json");
                }
                using HttpResponseMessage HttpResponseMessage = await Client.SendAsync(Request);
                HttpResponseMessage.EnsureSuccessStatusCode();//确保响应成功(2xx)，否则抛出异常
                string StringJsonResponse = await HttpResponseMessage.Content.ReadAsStringAsync();//读取响应内容
                return StringJsonResponse;
            }
            catch (HttpRequestException ex)
            {
                LoggerHelper.Error($"POST请求错误: {ex.Message}");
                throw new Exception("POST请求错误", ex);
            }
            catch (TaskCanceledException ex)
            {
                LoggerHelper.Error($"请求超时: {ex.Message}");
                throw new Exception("请求超时", ex);
            }
            catch (Exception ex)
            {
                LoggerHelper.Error($"POST请求发生未知错误: {ex.Message}");
                throw new Exception("POST请求发生未知错误:", ex);
            }
        }

        /// <summary>
        /// 发送DELETE请求并返回响应内容
        /// </summary>
        /// <param name="StringUrl">请求的完整 URL 或相对 URL</param>
        /// <param name="Headers">自定义请求头</param>
        /// <returns>响应内容的 JSON 字符串</returns>
        public async Task<string> DeleteAsync(string StringUrl, Dictionary<string, string> Headers = null)
        {
            try
            {
                using var Request = new HttpRequestMessage(HttpMethod.Delete, StringUrl);
                AddHeaders(Request, Headers);
                using HttpResponseMessage HttpResponseMessage = await Client.SendAsync(Request);
                HttpResponseMessage.EnsureSuccessStatusCode();//确保响应成功(2xx)，否则抛出异常
                string StringJsonResponse = await HttpResponseMessage.Content.ReadAsStringAsync();//读取响应内容
                return StringJsonResponse;
            }
            catch (HttpRequestException ex)
            {
                LoggerHelper.Error($"DELETE请求错误: {ex.Message}");
                throw new Exception("DELETE请求错误", ex);
            }
            catch (TaskCanceledException ex)
            {
                LoggerHelper.Error($"请求超时: {ex.Message}");
                throw new Exception("请求超时:", ex);
            }
            catch (Exception ex)
            {
                LoggerHelper.Error($"DELETE请求发生未知错误: {ex.Message}");
                throw new Exception("DELETE请求发生未知错误:", ex);
            }
        }

        /// <summary>
        /// 在每个请求中添加自定义请求头。
        /// </summary>
        /// <param name="request">HttpRequestMessage 对象</param>
        /// <param name="headers">自定义请求头</param>
        private void AddHeaders(HttpRequestMessage request, Dictionary<string, string> headers)
        {
            if (headers != null)
            {
                foreach (var header in headers)
                {
                    //确保头部不存在，避免异常
                    if (!request.Headers.Contains(header.Key))
                    {
                        request.Headers.Add(header.Key, header.Value);
                    }
                }
            }
        }
    }

    /// <summary>
    /// 扩展方法，用于输出请求信息到控制台。
    /// </summary>
    public static class HttpResponseMessageExtensions
    {
        public static void WriteRequestToConsole(this HttpResponseMessage response)
        {
            if (response?.RequestMessage == null)
            {
                return;
            }

            var request = response.RequestMessage;
            Console.Write($"{request?.Method} ");
            Console.Write($"{request?.RequestUri} ");
            Console.WriteLine($"HTTP/{request?.Version}");
        }
    }
}
