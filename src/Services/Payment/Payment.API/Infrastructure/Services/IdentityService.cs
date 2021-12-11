using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Payment.API.Infrastructure.Services
{
    public class IdentityService : IIdentityService
    {
        private IHttpContextAccessor _context;

        public IdentityService(IHttpContextAccessor httpContext)
        {
            _context = httpContext;
        }

        public string GetUserId()
        {
            return _context.HttpContext.User.FindFirst("id").Value;
        }        
    }
}
