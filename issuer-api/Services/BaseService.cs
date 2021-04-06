using Microsoft.AspNetCore.Http;
using Prime.Models;

namespace Prime.Services
{
    public abstract class BaseService
    {
        protected readonly ApiDbContext _context;

        protected readonly IHttpContextAccessor _httpContext;

        protected BaseService(ApiDbContext context, IHttpContextAccessor httpContext)
        {
            _context = context;
            _httpContext = httpContext;
        }
    }
}
