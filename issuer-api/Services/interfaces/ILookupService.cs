using System.Threading.Tasks;
using Issuer.Models.Api;

namespace Issuer.Services
{
    public interface ILookupService
    {
        Task<LookupEntity> GetLookupsAsync();
    }
}
