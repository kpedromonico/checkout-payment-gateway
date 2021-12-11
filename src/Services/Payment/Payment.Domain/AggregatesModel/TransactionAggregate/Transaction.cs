using Payment.Domain.SeedWork;
using System;

namespace Payment.Domain.AggregatesModel.TransactionAggregate
{
    public class Transaction : Entity, IAggregateRoot
    {
        public Guid _cardId;

        public Card Card { get; private set; }

        public string UserId { get; private set; }

        public decimal Amount { get; set; }

        public string Currency { get; set; }

        public bool Sucess { get; protected set; }

        public DateTime CreationDate { get; set; } = DateTime.UtcNow;

        public void SetTransactionOwner(string userId)
        {
            UserId = userId;

            // If the card doesn't exist yet, add a new owner to it
            if(string.IsNullOrEmpty(Card?.UserId))
            {
                Card.SetCardOwner(userId);
            }
        }

        public void ApproveTransaction()
        {
            Sucess = true;
        }

        public void RejectTransaction()
        {
            Sucess = false;
        }
    }
}