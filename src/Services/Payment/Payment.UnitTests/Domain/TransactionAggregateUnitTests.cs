using AutoFixture;
using FluentAssertions;
using Payment.Domain.AggregatesModel.TransactionAggregate;
using Payment.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Payment.UnitTests.Domain
{    
    [Trait("Category", "Domain")]
    public class TransactionAggregateUnitTests
    {
        private readonly Fixture _fixture = new Fixture();

        [Fact]
        public void ApproveTransaction_Success()
        {
            // arrange
            var sut = _fixture.Create<Transaction>();

            // act
            sut.ApproveTransaction();

            // assert
            sut.Sucess.Should().Be(true);
        }

        [Fact]
        public void RejectTransaction_Sucess()
        {
            // arrange
            var sut = _fixture.Create<Transaction>();

            // act
            sut.RejectTransaction();

            // assert
            sut.Sucess.Should().Be(false);
        }

        [Fact]
        public void SetCard_ChangesSuccessfully_TheTransactionCard()
        {
            // arrange
            var sut = _fixture.Create<Transaction>();
            var card = _fixture.Create<Card>();

            // act
            sut.SetCard(card);

            // assert
            sut.Card.Should().BeSameAs(card);
        }

        [Fact]
        public void SetTransactionOwner_AppliesUserId_ForTheTransactionAndTheCardOwner()
        {
            // arrange
            var sut = _fixture.Create<Transaction>();
            var card = _fixture.Create<Card>();
            sut.SetCard(card);

            var userId = Guid.NewGuid().ToString();

            // act
            sut.SetTransactionOwner(userId);

            // assert
            sut.UserId.Should().Be(userId);
            sut.Card.UserId.Should().Be(userId);
        }


        [Fact]
        public void SetTransactionOwner_ThrowsError_IfTransactionHasNoCard()
        {
            // arrange
            var sut = _fixture.Create<Transaction>();

            var userId = Guid.NewGuid().ToString();

            // act & Assert
            Assert.Throws<NullableCardException>(
                () => sut.SetTransactionOwner(userId));
        }
    }
}
