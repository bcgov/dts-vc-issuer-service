using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Prime.Models;
using Prime.Models.Api;

namespace Prime.Services
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
