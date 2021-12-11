using Microsoft.AspNetCore.Mvc;
using System;
using Bank.API.Extensions;
using Bank.API.Application.Payloads.Requests;

namespace Bank.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransactionsController : ControllerBase
    {
        [HttpPost]
        public ActionResult EvaluateTransaction(PaymentApprovalRequest request)
        {
            // TODO: Change this to ILogger to look a bit more professional
            Console.WriteLine($"--> Evaluating transaction for: {request.CardNumber}");

            var r = new Random();
            return r.NextBoolean() 
                ? Ok("Approved") 
                : BadRequest("Rejected");
        }
    }
}
