using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;
using Issuer.Models;

namespace Issuer.HttpClients
{
    public class ImmunizationClient : IImmunizationClient
    {
        private readonly HttpClient _client;
        private readonly ILogger _logger;

        public ImmunizationClient(
            HttpClient client,
            ILogger<VerifiableCredentialClient> logger)
        {
            // Auth header and api-key are injected in Startup.cs
            _client = client;
            _logger = logger;
        }

        public async Task<JObject> GetImmunizationRecordAsync(string url)
        {
            HttpResponseMessage response = null;
            try
            {
                response = await _client.GetAsync($"{url}");
            }
            catch (Exception ex)
            {
                await LogError(response, ex);
                throw new ApiException("Error occurred when calling Immunization API. Try again later.", ex);
            }

            if (!response.IsSuccessStatusCode)
            {
                await LogError(response);
                throw new ApiException($"Error code {response.StatusCode} was provided when calling ImmunizationClient::GetImmunizationRecordAsync");
            }

            _logger.LogInformation("Create connection invitation response {@JObject}", JsonConvert.SerializeObject(response));

            return JObject.Parse(await response.Content.ReadAsStringAsync());
        }

        private async Task LogError(HttpResponseMessage response, Exception exception = null)
        {
            await LogError(null, response, exception);
        }

        private async Task LogError(HttpContent content, HttpResponseMessage response, Exception exception = null)
        {
            string secondaryMessage;
            if (exception != null)
            {
                secondaryMessage = $"Exception: {exception.Message}";
            }
            else if (response != null)
            {
                string responseMessage = await response.Content.ReadAsStringAsync();
                secondaryMessage = $"Response code: {(int)response.StatusCode}, response body:{responseMessage}";
            }
            else
            {
                secondaryMessage = "No additional message. Http response and exception were null.";
            }

            _logger.LogError(exception, secondaryMessage, new Object[] { content, response });
        }
    }
}

