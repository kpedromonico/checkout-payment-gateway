using Payment.Domain.SeedWork;
using System;

namespace Payment.Domain.AggregatesModel.TransactionAggregate
{
    public class Card : Entity
    {
        public string UserId { get; private set; }

        public string CardNumber { get; set; }

        public int ExpiryMonth { get; set; }

        public int ExpiryYear { get; set; }

        public int SecurityNumber { get; set; }

        public void SetCardOwner(string userId)
        {
            UserId = userId;
        }
    }
}