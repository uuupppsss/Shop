
using System.Net.Http;
using System.Net.Http.Headers;

namespace WpfClientShop.Model
{
    public class Client
    {
        private static HttpClient _httpClient;
        public static HttpClient HttpClient
        {
            get
            {
                if (_httpClient == null)
                    _httpClient = new HttpClient
                    {
                        BaseAddress = new Uri("http://localhost:5226/api/")
                    };
                return _httpClient;
            }
        }

        public static void SetToken(string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
        public static void ResetHeaders()
        {
            _httpClient.DefaultRequestHeaders.Clear();
        }
    }
}
