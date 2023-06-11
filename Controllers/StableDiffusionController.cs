using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using System.Text.Json;
using System.Xml.Linq;
using webapi.Interfaces;


namespace webapi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StableDiffusionController : ControllerBase
    {
        private readonly ILogger<StableDiffusionController> _sd;
        private readonly IHttpClientFactory _clientFactory;
        private readonly IApiSelector _apiSelector;

        public StableDiffusionController(IHttpClientFactory clientFactory, ILogger<StableDiffusionController> sd, IApiSelector apiSelector)
        {
            _sd = sd;
            _clientFactory = clientFactory;
            _apiSelector = apiSelector;
        }

        // POST: api/StableDiffusion/txt2img
        [HttpPost]
        [Route("txt2img")]
        public async Task<IActionResult> Txt2Img([FromBody] JsonElement data)
        {
            var client = _clientFactory.CreateClient();
            client.Timeout = TimeSpan.FromMinutes(5); // set the timeout to 5 minutes
            string API_IP = _apiSelector.ChooseApi("txt2img");

            var content = new StringContent(data.ToString(), Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PostAsync($"{API_IP}api/generate/txt2img", content);

            if (response.IsSuccessStatusCode)
            {
                var responseData = await response.Content.ReadAsStringAsync();
                return Ok(responseData);
            }

            return BadRequest();
        }

        // POST: api/StableDiffusion/img2img
        [HttpPost]
        [Route("img2img")]
        public async Task<IActionResult> Img2Img([FromBody] JsonElement data)
        {
            var client = _clientFactory.CreateClient();
            client.Timeout = TimeSpan.FromMinutes(5); // set the timeout to 5 minutes
            string API_IP = _apiSelector.ChooseApi("img2img");


            var content = new StringContent(data.ToString(), Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PostAsync($"{API_IP}api/generate/img2img", content);

            if (response.IsSuccessStatusCode)
            {
                var responseData = await response.Content.ReadAsStringAsync();
                return Ok(responseData);
            }

            return BadRequest();
        }
    }

}