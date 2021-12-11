using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Payment.API.Application.Payloads.Reponses;
using Payment.API.Application.Payloads.Requests;
using Payment.API.Application.Services;
using Payment.API.Infrastructure.Services;
using Payment.Domain.AggregatesModel.TransactionAggregate;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Payment.API.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class PaymentController : ControllerBase
    {
        // https://stackoverflow.com/questions/21554977/should-services-always-return-dtos-or-can-they-also-return-domain-models
        private readonly IIdentityService _identityService;
        private readonly IValidator<PaymentAttemptRequest> _paymentRequestValidator;
        private readonly IPaymentService _paymentService;
        private readonly IMapper _mapper;

        public PaymentController(IPaymentService paymentService, 
            IMapper mapper,
            IValidator<PaymentAttemptRequest> paymentRequestValidator,
            IIdentityService identityService)
        {
            _paymentService = paymentService;
            _mapper = mapper;
            _paymentRequestValidator = paymentRequestValidator;
            _identityService = identityService;
        }
             
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<PaymentDetailsResponse>), (int)HttpStatusCode.OK)]        
        public async Task<ActionResult> GetPaymentsAsync()
        {
            var userId = _identityService.GetUserId();
            var transactions = await _paymentService.GetTransactionsAsync(userId);

            var result = _mapper.Map<IEnumerable<PaymentConfirmationResponse>>(transactions);
            return Ok(result);
        }
    
        [HttpGet]
        [Route("{paymentId}")]
        [ProducesResponseType(typeof(IEnumerable<PaymentDetailsResponse>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> GetPaymentByIdAsync(string paymentId)
        {
            if(string.IsNullOrEmpty(paymentId) || !Guid.TryParse(paymentId, out Guid paymentGuid))
            {
                return BadRequest("PaymentId format is invalid");
            }

            var userId = _identityService.GetUserId();
            var transaction = await _paymentService.FindTransactionAsync(paymentGuid);

            // Checking if the user requesting the payment information, is the same one from the request
            if(string.Compare(transaction?.UserId, userId, StringComparison.Ordinal) != 0)
            {
                return NotFound("Payment not found for the logged user");
            }

            var result = _mapper.Map<PaymentConfirmationResponse>(transaction);
            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(typeof(PaymentConfirmationResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(InvalidPaymentAttemptResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> ProcessPaymentAsync(PaymentAttemptRequest paymentRequest)
        {
            var validation = _paymentRequestValidator.Validate(paymentRequest);
            if (!validation.IsValid)
            {
                var response = new InvalidPaymentAttemptResponse { Errors = validation.Errors.Select(x => x.ErrorMessage).ToArray() };
                return BadRequest(response);
            }

            var userId = _identityService.GetUserId();
            var transaction = _mapper.Map<Transaction>(paymentRequest);

            var transactionResult = await _paymentService.ProcessTransactionAsync(transaction, userId);

            var result = _mapper.Map<PaymentConfirmationResponse>(transactionResult);
            return Ok(result);
        }

    }
}
