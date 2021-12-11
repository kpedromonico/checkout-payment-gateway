using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Payment.API.Infrastructure.Services
{
    public interface IIdentityService
    {
        string GetUserId();
    }
}
