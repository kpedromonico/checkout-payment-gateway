using PaymentGateway.HttpAggregator.Payloads.PaymentService.Responses;
using System.Threading.Tasks;

namespace PaymentGateway.HttpAggregator.Services
{
    public interface IPaymentService
    {
        Task<string> GetPaymentsAsync(string jwt);

        Task<string> GetPaymentByIdAsync(string jwt, string paymentId);

        Task<string> ProcessPaymentAsync(string jwt, PaymentAttemptRequest paymentRequest);
    }
}
