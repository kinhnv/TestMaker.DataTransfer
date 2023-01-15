using DataTransfer.Api.Configurations;
using DataTransfer.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace DataTransfer.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserQuestionsController : ControllerBase
    {
        private readonly ILogger<UserQuestionsController> _logger;
        private readonly IOptions<GatewayConfiguration> _gatewayConfigurationOptions;

        public UserQuestionsController(ILogger<UserQuestionsController> logger, IOptions<GatewayConfiguration> gatewayConfigurationOptions)
        {
            _logger = logger;
            _gatewayConfigurationOptions = gatewayConfigurationOptions;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]UserQuestionPostBody body)
        {
            var httpClient = new HttpClient
            {
                BaseAddress = new Uri(_gatewayConfigurationOptions.Value.Url)
            };

            if (body.After != null)
            {
                var res = await httpClient.GetAsync($"api/Test/DataTransfer/UserQuestions?userId={body.After.UserId}&questionId={body.After.QuestionId}");

                if (res.IsSuccessStatusCode)
                {
                    var userQuestion = await res.Content.ReadFromJsonAsync<ServiceResult<UserQuestion>>();

                    if (userQuestion?.Code == 200 && userQuestion?.Data != null)
                    {
                        await httpClient.PostAsJsonAsync($"api/Event/DataTransfer/UserQuestions", userQuestion.Data);
                    }
                }
            }

            return Ok();
        }
    }
}