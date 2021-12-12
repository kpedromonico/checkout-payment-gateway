using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using PaymentGateway.HttpAggregator.Payloads.PaymentService.Responses;
using PaymentGateway.HttpAggregator.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace PaymentGateway.HttpAggregator.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<PaymentDetailsResponse>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetPaymentsAsync()
        {
            var jwt = GetJwt();

            var httpMessageResult = await _paymentService.GetPaymentsAsync(jwt);
            var payments = JsonConvert.DeserializeObject<IEnumerable<PaymentDetailsResponse>>(httpMessageResult);

            return Ok(payments);
        }

        [HttpGet]
        [Route("{paymentId}")]
        [ProducesResponseType(typeof(IEnumerable<PaymentDetailsResponse>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> GetPaymentByIdAsync(string paymentId)
        {
            var jwt = GetJwt();

            var httpMessageResult = await _paymentService.GetPaymentByIdAsync(jwt, paymentId);
            var paymentDetails = JsonConvert.DeserializeObject<PaymentDetailsResponse>(httpMessageResult);

            if (paymentDetails?.CardNumber == null)
                return NotFound(paymentDetails);

            return Ok(paymentDetails);
        }

        [HttpPost]
        [ProducesResponseType(typeof(PaymentConfirmationResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(InvalidPaymentAttemptResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> ProcessPaymentAsync(PaymentAttemptRequest paymentRequest)
        {
            var jwt = GetJwt();

            var httpMessageResult = await _paymentService.ProcessPaymentAsync(jwt, paymentRequest);

            var errors = JsonConvert.DeserializeObject<InvalidPaymentAttemptResponse>(httpMessageResult);
            var paymentDetails = JsonConvert.DeserializeObject<PaymentConfirmationResponse>(httpMessageResult);

            if (errors?.Errors != null)
                return BadRequest(errors);

            return Ok(paymentDetails);
        }

        private string GetJwt()
        {
            return Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", string.Empty);
        }
    }


}
