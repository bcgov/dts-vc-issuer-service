using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

using Issuer.Models;
using System.Collections.Generic;

namespace Issuer.Services
{
    public interface IVerifiableCredentialService
    {
        Task<int> IssueCredentialsAsync(Patient patient, List<Identifier> identifiers);
        Task<bool> WebhookAsync(JObject data, string topic);
        Task<bool> RevokeCredentialsAsync(int patientId);
    }
}
