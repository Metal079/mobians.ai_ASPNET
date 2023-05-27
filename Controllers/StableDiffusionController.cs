using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using System.Text.Json;
using System.Xml.Linq;


namespace webapi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StableDiffusionController : ControllerBase
    {
        private readonly ILogger<StableDiffusionController> _sd;
        private readonly IHttpClientFactory _clientFactory;
        private readonly IConfiguration _configuration;

        public StableDiffusionController(IHttpClientFactory clientFactory, IConfiguration configuration, ILogger<StableDiffusionController> sd)
        {
            _sd = sd;
            _clientFactory = clientFactory;
            _configuration = configuration;
        }

        // POST: api/StableDiffusion/txt2img
        [HttpPost]
        [Route("txt2img")]
        public async Task<IActionResult> Txt2Img([FromBody] JsonElement data)
        {
            var client = _clientFactory.CreateClient();
            client.Timeout = TimeSpan.FromMinutes(5); // set the timeout to 5 minutes
            string API_IP = _configuration.GetValue<string>("API_IP");

            var content = new StringContent(data.ToString(), Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PostAsync($"{API_IP}api/generate/txt2img", content);

            if (response.IsSuccessStatusCode)
            {
                var responseData = await response.Content.ReadAsStringAsync();
                return Ok(responseData);
            }

            return BadRequest();
        }


        private object ProcessResponse(string response)
        {
            // Process the response data
            // This is where you'd implement the watermarking logic

            // Create a new object with some properties
            var product = new { property1 = "value1", property2 = "value2" };

            // You don't necessarily need to serialize the object here, 
            // because the Ok() method will do it for you
            return product;
        }

    }

}