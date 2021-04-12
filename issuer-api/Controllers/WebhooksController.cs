using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

using Issuer.Models.Api;
using Issuer.Services;

namespace Issuer.Controllers
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

        // POST: api/webhooks/topic/connections
        /// <summary>
        /// Handle webhook events sent from the issuing agent.
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="data"></param>
        [HttpPost("topic/{topic}", Name = nameof(Webhook))]
        [ProducesResponseType(typeof(ApiBadRequestResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> Webhook(string topic, [FromBody] JObject data)
        {
            await _verifiableCredentialsService.WebhookAsync(data, topic);
            return NoContent();
        }
    }
}
