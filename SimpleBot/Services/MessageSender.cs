using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace AskIT.Services
{
    public class MessageSender
    {
        private readonly IConfiguration _configuration;

        public MessageSender(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendMessageAsync(JObject message, string senderId)
        {
            string apiUrl = $"https://graph.facebook.com/v22.0/{senderId}/messages";
            string apiToken = _configuration["MetaDeveloper:ApiToken"];

            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiToken);
                var content = new StringContent(message.ToString(), Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync(apiUrl, content);
                var respBody = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Error sending message: {response.StatusCode}");
                    Console.WriteLine($"Response body: {respBody}");
                }
            }
        }
    }
}