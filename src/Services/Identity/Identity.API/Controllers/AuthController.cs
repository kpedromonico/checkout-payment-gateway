using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using Identity.API.Models;
using Identity.API.Payloads.v1.Requests;
using Identity.API.Payloads.v1.Responses;
using Identity.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Identity.API.Controllers
{
    [ApiController]
    [AllowAnonymous]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IIdentityService _identityService;
        private readonly IMapper _mapper;
        private readonly IValidator<AccountRegisterRequest> _registerValidator;
        private readonly IValidator<AccountLoginRequest> _loginValidator;

        public AuthController(IIdentityService identityService,
            IMapper mapper,
            IValidator<AccountRegisterRequest> registerValidator,
            IValidator<AccountLoginRequest> loginValidator)
        {
            _identityService = identityService;
            _mapper = mapper;
            _registerValidator = registerValidator;
            _loginValidator = loginValidator;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] AccountRegisterRequest payload)
        {
            var validationResult = _registerValidator.Validate(payload);
            if(!validationResult.IsValid)
            {
                return InvalidAccountValidation(validationResult);
            }

            var user = _mapper.Map<User>(payload);
            var registration = await _identityService.Register(user);

            return registration.Success
                ? Ok(new AccountResponse { Token = registration.Token })
                : BadRequest(new AccountResponse { ErrorMessages = registration.ErrorMessages });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] AccountLoginRequest payload)
        {
            var validationResult = _loginValidator.Validate(payload);
            if(!validationResult.IsValid)
            {
                return InvalidAccountValidation(validationResult);
            }

            var user = _mapper.Map<User>(payload);
            var registration = await _identityService.Login(user);

            return registration.Success
                ? Ok(new AccountResponse { Token = registration.Token })
                : BadRequest(new AccountResponse { ErrorMessages = registration.ErrorMessages });
        }

        private IActionResult InvalidAccountValidation(ValidationResult validationResult)
        {
            return BadRequest(new AccountResponse
            {
                ErrorMessages = validationResult.Errors.Select(p => p.ErrorMessage)
            });
        }
    }
}