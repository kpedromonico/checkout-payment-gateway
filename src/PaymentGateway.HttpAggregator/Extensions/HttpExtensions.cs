using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace PaymentGateway.HttpAggregator.Extensions
{
    public static class HttpExtensions
    {
        public static StringContent ConvertToStringContent(this object request)
        {
            var serialized = JsonSerializer.Serialize(request);
            var str = serialized.Replace('\\', ' ');

            return new StringContent(str, Encoding.UTF8, "application/json" );
        }
    }
}
