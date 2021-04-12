using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Issuer.Models;
using Issuer.Models.Api;

namespace Issuer.Services
{
    public class LookupService : BaseService, ILookupService
    {
        public LookupService(
            ApiDbContext context,
            IHttpContextAccessor httpContext)
            : base(context, httpContext)
        { }

        public async Task<LookupEntity> GetLookupsAsync()
        {
            return new LookupEntity();
        }
    }
}
