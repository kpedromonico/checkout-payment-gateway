using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PaymentGateway.HttpAggregator.Payloads.IdentityService.Requests;
using PaymentGateway.HttpAggregator.Payloads.IdentityService.Responses;
using PaymentGateway.HttpAggregator.Services;
using System.Net;
using System.Threading.Tasks;

namespace PaymentGateway.HttpAggregator.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class IdentityController : Controller
    {
        private readonly IIdentityService _identityService;

        public IdentityController(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        [HttpPost("register")]
        [ProducesResponseType(typeof(AccountResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(AccountResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Register(AccountRegisterRequest payload)
        {
            var httpMessageResult = await _identityService.Register(payload);
            var model = JsonConvert.DeserializeObject<AccountResponse>(httpMessageResult);

            return model.ErrorMessages == null
                ? Ok(model)
                : BadRequest(model);
        }

        [HttpPost("login")]
        [ProducesResponseType(typeof(AccountResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(AccountResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Login(AccountLoginRequest payload)
        {
            var httpMessageResult = await _identityService.Login(payload);
            var model = JsonConvert.DeserializeObject<AccountResponse>(httpMessageResult);

            return model.ErrorMessages == null
                ? Ok(model)
                : BadRequest(model);
        }

    }
}
