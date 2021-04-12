using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Issuer.Models;

namespace Issuer.HttpClients
{
    public interface IImmunizationClient
    {
        Task<JObject> GetImmunizationRecordAsync(string url);
    }
}
