using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Aurora.Insurance.Blazor.Services.Clients
{
    public class IdentityClient
    {
        private readonly HttpClient _httpClient;

        public IdentityClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task Login(string userName,
            string password)
        {
            var keyValues = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("userName", userName),
                new KeyValuePair<string, string>("password", password)
            };
            var formContent=new FormUrlEncodedContent(keyValues);
            await _httpClient.PostAsync("api/login", formContent);
        }
    }
}
