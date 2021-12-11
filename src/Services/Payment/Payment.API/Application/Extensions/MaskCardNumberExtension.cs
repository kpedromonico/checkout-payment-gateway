using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Payment.API.Application.Extensions
{
    public static class MaskCardNumberExtension
    {
        public static string MaskCardNumber(this string cardNumber)
        {
            if (string.IsNullOrEmpty(cardNumber))
            {
                throw new ArgumentNullException(nameof(cardNumber));
            }

            var arr = new char[cardNumber.Length];

            for(int i = 0; i < cardNumber.Length; i++)
            {
                if(i < cardNumber.Length - 5)
                {
                    arr[i] = '*';
                }
                else
                {
                    arr[i] = cardNumber[i];
                }
            }

            return new string(arr);
        }
    }
}
