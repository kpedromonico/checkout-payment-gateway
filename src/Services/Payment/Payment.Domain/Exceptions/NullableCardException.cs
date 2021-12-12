using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payment.Domain.Exceptions
{
    public class NullableCardException : Exception
    {
        public NullableCardException(string message) : base(message)
        {
        }
    }
}
