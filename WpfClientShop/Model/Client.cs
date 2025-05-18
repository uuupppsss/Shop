
using System.Net.Http;

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

        //private static HubConnection _userHubConnection;
        //public static HubConnection UserHubConnection
        //{
        //    get
        //    {
        //        if (_userHubConnection == null)
        //        {
        //            return _userHubConnection= new HubConnectionBuilder()
        //        .WithUrl("http://localhost:5175/gamehub")
        //        .Build();
        //        }
        //        return _userHubConnection;
        //    }
        //}

        //private static HubConnection _adminHubConnection;
        //public static HubConnection AdminHubConnection
        //{
        //    get
        //    {
        //        if (_adminHubConnection == null)
        //            _adminHubConnection = new HubConnectionBuilder()
        //        .WithUrl("http://localhost:5216/")
        //        .Build();
        //        return _adminHubConnection;
        //    }
        //}
    }
}
