using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PaymentGateway.HttpAggregator.Payloads.BankService.Requests;
using PaymentGateway.HttpAggregator.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace PaymentGateway.HttpAggregator.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BankController : ControllerBase
    {
        private readonly IBankService _bankService;

        public BankController(IBankService bankService)
        {
            _bankService = bankService;
        }

        [HttpPost]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> SubmitTransaction(PaymentApprovalRequest payload)
        {
            if(payload != null)
            {
                var result = await _bankService.EvaluateTransaction(payload);

                return result == "Approved" ? Ok(result) : BadRequest(result);
            }

            return BadRequest("Empty payload");
        }
    }
}
