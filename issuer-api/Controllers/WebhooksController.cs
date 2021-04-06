using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

using Prime.Models.Api;
using Prime.Services;

namespace Prime.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class WebhooksController : ControllerBase
    {
        private readonly IVerifiableCredentialService _verifiableCredentialsService;

        public WebhooksController(IVerifiableCredentialService verifiableCredentialService)
        {
            _verifiableCredentialsService = verifiableCredentialService;
        }

        // POST: api/webhooks/1234-5678/topic/connections
        /// <summary>
        /// Handle webhook events sent from the issuing agent.
        /// </summary>
        /// <param name="apiKey"></param>
        /// <param name="topic"></param>
        /// <param name="data"></param>
        [HttpPost("{apiKey}/topic/{topic}", Name = nameof(Webhook))]
        [ProducesResponseType(typeof(ApiBadRequestResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> Webhook(string apiKey, string topic, [FromBody] JObject data)
        {
            if (apiKey != PrimeEnvironment.VerifiableCredentialApi.WebhookKey)
            {
                return Forbid();
            }
            await _verifiableCredentialsService.WebhookAsync(data, topic);
            return NoContent();
        }
    }
}
