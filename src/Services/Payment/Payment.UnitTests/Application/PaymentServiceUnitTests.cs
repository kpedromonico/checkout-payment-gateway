using AutoFixture;
using FluentAssertions;
using Moq;
using Payment.API.Application.Services;
using Payment.API.Infrastructure.Services;
using Payment.Domain.AggregatesModel.TransactionAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Payment.UnitTests.Application
{
    [Trait("Category", "Application")]
    public class PaymentServiceUnitTests
    {
        private IPaymentService _sut;
        private Mock<ITransactionRepository> _transactionRepository;
        private Mock<IBankService> _bankService;

        private readonly Fixture _fixture = new Fixture();

        public PaymentServiceUnitTests()
        {
            _transactionRepository = new Mock<ITransactionRepository>();
            _bankService = new Mock<IBankService>();
            _sut = new PaymentService(_transactionRepository.Object, _bankService.Object);
        }

        [Fact]
        public async Task FindTransactionAsync_ReturnsAValidTransactionIfIdExists()
        {
            // arrange
            var transactionId = Guid.NewGuid();

            var transaction = _fixture.Build<Transaction>()
                .With(x => x.Id, transactionId)
                .Create();

            _transactionRepository.Setup(x => x.FindByIdAsync(transactionId))
                .ReturnsAsync(transaction);

            // Act
            var result = await _sut.FindTransactionAsync(transactionId);

            // Assert
            result.Should().BeSameAs(transaction);
            _transactionRepository.Verify(x => x.FindByIdAsync(transactionId), Times.Once);
        }

        [Fact]
        public async Task FindTransactionAsync_ReturnsEmptyIfTransaction_DoesNotExist()
        {
            // arrange
            var transactionId = Guid.NewGuid();            

            _transactionRepository.Setup(x => x.FindByIdAsync(transactionId))
                .ReturnsAsync(value: null);

            // Act
            var result = await _sut.FindTransactionAsync(transactionId);

            // Assert
            result.Should().BeNull();
            _transactionRepository.Verify(x => x.FindByIdAsync(transactionId), Times.Once);
        }

        [Fact]
        public async Task GetTransactionsAsync_ReturnListOfTransaction_ForLoggedUser()
        {
            // arrange
            var loggedUserId = Guid.NewGuid().ToString();
            var transactions = _fixture.Create<IEnumerable<Transaction>>();
            var card = _fixture.Create<Card>();

            foreach (var x in transactions)
            {
                x.SetCard(card);
                x.SetTransactionOwner(loggedUserId);
            }

            _transactionRepository.Setup(x => x.GetByUserIdAsync(loggedUserId))
                .ReturnsAsync(transactions);

            // Act
            var result = await _sut.GetTransactionsAsync(loggedUserId);

            // Assert
            result.ToList().Should().HaveCount(transactions.Count());
            result.ToList().Where(x => x.UserId != loggedUserId).Should().HaveCount(0);
            _transactionRepository.Verify(x => x.GetByUserIdAsync(loggedUserId), Times.Once);
        }

        [Fact]
        public async Task ProcessTransactionAsync_ShouldApproveTransaction_IfBankApprovesIt()
        {
            // arrange
            var transaction = _fixture.Create<Transaction>();
            var card = _fixture.Create<Card>();
            var loggedUserId = Guid.NewGuid().ToString();

            transaction.SetCard(card);

            _bankService.Setup(x => x.ProcessTransaction(transaction))
                .ReturnsAsync(true);

            _transactionRepository.Setup(x => x.Add(transaction));
            _transactionRepository.Setup(x => x.UnitOfWork.SaveChangesAsync(default));

            // act
            var result = await _sut.ProcessTransactionAsync(transaction, loggedUserId);

            // assert
            result.Currency.Should().Be(transaction.Currency);
            result.Sucess.Should().BeTrue();
            result.Amount.Should().Be(transaction.Amount);
            result.UserId.Should().Be(loggedUserId);
            result.Card.CardNumber.Should().Be(transaction.Card.CardNumber);
            result.Card.ExpiryMonth.Should().Be(transaction.Card.ExpiryMonth);
            result.Card.ExpiryYear.Should().Be(transaction.Card.ExpiryYear);
            _transactionRepository.Verify(x => x.Add(transaction), Times.Once);
            _transactionRepository.Verify(x => x.UnitOfWork.SaveChangesAsync(default), Times.Once);
        }

        [Fact]
        public async Task ProcessTransactionAsync_ShouldRejectTransaction_IfBankRejectsIt()
        {
            // arrange
            var transaction = _fixture.Create<Transaction>();
            var card = _fixture.Create<Card>();
            var loggedUserId = Guid.NewGuid().ToString();

            transaction.SetCard(card);

            _bankService.Setup(x => x.ProcessTransaction(transaction))
                .ReturnsAsync(false);

            _transactionRepository.Setup(x => x.Add(transaction));
            _transactionRepository.Setup(x => x.UnitOfWork.SaveChangesAsync(default));

            // act
            var result = await _sut.ProcessTransactionAsync(transaction, loggedUserId);

            // assert
            result.Currency.Should().Be(transaction.Currency);
            result.Sucess.Should().BeFalse();
            result.Amount.Should().Be(transaction.Amount);
            result.UserId.Should().Be(loggedUserId);
            result.Card.CardNumber.Should().Be(transaction.Card.CardNumber);
            result.Card.ExpiryMonth.Should().Be(transaction.Card.ExpiryMonth);
            result.Card.ExpiryYear.Should().Be(transaction.Card.ExpiryYear);
            _transactionRepository.Verify(x => x.Add(transaction), Times.Once);
            _transactionRepository.Verify(x => x.UnitOfWork.SaveChangesAsync(default), Times.Once);
        }

        [Fact]
        public async Task ProcessTransactionAsync_ShouldPutTransactionToBeProcessedLater_IfBankIsUnresponsive()
        {
            // arrange
            var transaction = _fixture.Create<Transaction>();
            var card = _fixture.Create<Card>();
            var loggedUserId = Guid.NewGuid().ToString();

            transaction.SetCard(card);

            _bankService.Setup(x => x.ProcessTransaction(transaction))
                .ReturnsAsync(value: null);

            _transactionRepository.Setup(x => x.Add(transaction));
            _transactionRepository.Setup(x => x.UnitOfWork.SaveChangesAsync(default));

            // act
            var result = await _sut.ProcessTransactionAsync(transaction, loggedUserId);

            // assert
            result.Currency.Should().Be(transaction.Currency);
            result.Sucess.Should().BeNull();
            result.Amount.Should().Be(transaction.Amount);
            result.UserId.Should().Be(loggedUserId);
            result.Card.CardNumber.Should().Be(transaction.Card.CardNumber);
            result.Card.ExpiryMonth.Should().Be(transaction.Card.ExpiryMonth);
            result.Card.ExpiryYear.Should().Be(transaction.Card.ExpiryYear);
            _transactionRepository.Verify(x => x.Add(transaction), Times.Once);
            _transactionRepository.Verify(x => x.UnitOfWork.SaveChangesAsync(default), Times.Once);
        }

    }
}
