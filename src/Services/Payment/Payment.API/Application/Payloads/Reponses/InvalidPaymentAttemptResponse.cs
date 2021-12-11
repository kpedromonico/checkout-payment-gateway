using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Payment.API.Application.Payloads.Reponses
{
    public class InvalidPaymentAttemptResponse
    {
        public string[] Errors { get; set; }
    }
}
