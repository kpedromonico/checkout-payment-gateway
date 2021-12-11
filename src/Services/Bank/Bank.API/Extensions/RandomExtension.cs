using System;

namespace Bank.API.Extensions
{
    public static class RandomExtension
    {
        public static bool NextBoolean(this Random random)
        {
            return random.Next() > (Int32.MaxValue / 2);
        }
    }
}
