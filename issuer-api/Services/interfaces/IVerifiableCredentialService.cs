using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

using Prime.Models;

namespace Prime.Services
{
    public interface IVerifiableCredentialService
    {
        Task<bool> CreateConnectionAsync(Patient patient);
        Task<bool> WebhookAsync(JObject data, string topic);
        Task<bool> RevokeCredentialsAsync(int patientId);
    }
}
