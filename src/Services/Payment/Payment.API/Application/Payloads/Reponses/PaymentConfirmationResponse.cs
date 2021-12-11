﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Payment.API.Application.Payloads.Reponses
{
    public class PaymentConfirmationResponse
    {
        public string TransactionId { get; set; }

        public string CardNumber { get; set; }

        public bool Approved { get; set; }

        public decimal Amount { get; set; }        

        public DateTime TransactionDate { get; set; }
    }
}
