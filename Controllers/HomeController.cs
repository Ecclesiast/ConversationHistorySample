using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace CoreBot.Controllers
{
    [Route("api/token")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        protected IConfiguration Configuration;
        protected IHostingEnvironment HostingEnvironment;

        public HomeController(
            IHostingEnvironment hostingEnvironment,
            IConfiguration configuration)
        {
            HostingEnvironment = hostingEnvironment;
            Configuration = configuration;
        }

        [Route("generate")]
        [HttpPost]
        public async Task<ContentResult> Token()
        {
            return await PostAsync(Configuration["DirectLineSecret"], "generate");
        }

        /// <summary>
        /// Refreshes token.
        /// </summary>
        /// <returns>Generated token.</returns>
        [Route("refresh")]
        [HttpGet]
        public async Task<ContentResult> TokenRefresh(string conversationId)
        {
            return await ReconnectAsync(Configuration["DirectLineSecret"], conversationId);
        }


        private async Task<ContentResult> ReconnectAsync(string secretOrToken, string conversationId)
        {
            var client = new HttpClient();
            var content = new StringContent(" ", Encoding.UTF8, "application/json");
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization
                = new AuthenticationHeaderValue("Bearer", secretOrToken);

            var response = await client.GetAsync(
                $"https://directline.botframework.com/v3/directline/conversations/{conversationId}");
            var result = await response.Content.ReadAsStringAsync();
            return Content(result);
        }

        private async Task<ContentResult> PostAsync(string secretOrToken, string method)
        {
            var client = new HttpClient();
            var content = new StringContent(" ", Encoding.UTF8, "application/json");
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization
                = new AuthenticationHeaderValue("Bearer", secretOrToken);

            var response = await client.PostAsync(
                $"https://directline.botframework.com/v3/directline/tokens/{method}", content);
            var result = await response.Content.ReadAsStringAsync();
            return Content(result);
        }

        private async Task<ContentResult> GetConversationIdAsync(string secretOrToken)
        {
            var client = new HttpClient();
            var content = new StringContent(" ", Encoding.UTF8, "application/json");
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization
                = new AuthenticationHeaderValue("Bearer", secretOrToken);

            var response = await client.PostAsync(
                $"https://directline.botframework.com/v3/directline/conversations", content);
            var result = await response.Content.ReadAsStringAsync();
            return Content(result);
        }
    }
}