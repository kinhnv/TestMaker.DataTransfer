using DataTransfer.Api.Configurations;
using DataTransfer.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace DataTransfer.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class QuestionsController : ControllerBase
    {
        private readonly ILogger<QuestionsController> _logger;
        private readonly IOptions<GatewayConfiguration> _gatewayConfigurationOptions;

        public QuestionsController(ILogger<QuestionsController> logger, IOptions<GatewayConfiguration> gatewayConfigurationOptions)
        {
            _logger = logger;
            _gatewayConfigurationOptions = gatewayConfigurationOptions;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]PostBody<QuestionPostBody> body)
        {
            _logger.LogInformation(JsonConvert.SerializeObject(body));

            var httpClient = new HttpClient
            {
                BaseAddress = new Uri(_gatewayConfigurationOptions.Value.Url)
            };

            if (body.After != null)
            {
                var res = await httpClient.PostAsJsonAsync($"api/Event/DataTransfer/Questions", body.After);

                if (res.IsSuccessStatusCode)
                {
                    return Ok(JsonConvert.SerializeObject(new
                    {
                        message = "Update successful",
                        userId = body.After.QuestionId
                    }));
                }
            }
            else if (body.Before != null)
            {
                await httpClient.DeleteAsync($"api/Event/DataTransfer/UserQuestions?questionId={body.Before.QuestionId}");
                return Ok(JsonConvert.SerializeObject(new
                {
                    message = "Delete successful",
                    questionId = body.Before.QuestionId
                }));
            }

            return Ok(JsonConvert.SerializeObject(new
            {
                message = "Action error",
                questionId = body.After?.QuestionId ?? body.Before?.QuestionId
            }));
        }
    }
}