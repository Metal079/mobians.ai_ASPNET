using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace webapi.Interfaces
{
    public interface IApiSelector
    {
        string ChooseApi(string generateType);
    }

    public class ApiSelector : IApiSelector
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _clientFactory;

        public ApiSelector(IConfiguration configuration, IHttpClientFactory clientFactory)
        {
            _configuration = configuration;
            _clientFactory = clientFactory;
        }

        public string ChooseApi(string generateType)
        {
            // Read the API_IPs array from the configuration
            var API_IPs = _configuration.GetSection("API_IPs").Get<string[]>();

            // Select the API IP based on the generateType
            if (generateType == "img2img") {
                return API_IPs[0];
            }

            // If the generateType is not "img2img", choose a random API IP
            var random = new Random();
            int randomIndex = random.Next(API_IPs.Length);

            if (IsApiHealthy(API_IPs[randomIndex]).Result)
            {
                return API_IPs[randomIndex];
            }
            else
            {
                // If the randomly selected API is not healthy, choose the other API
                randomIndex = (randomIndex + 1) % API_IPs.Length;
                return API_IPs[randomIndex];
            }
        }

        public async Task<bool> IsApiHealthy(string API_IP)
        {
            var client = _clientFactory.CreateClient();
            HttpResponseMessage response = await client.GetAsync($"{API_IP}api/test/alive");

            return response.IsSuccessStatusCode;
        }

    }

}
