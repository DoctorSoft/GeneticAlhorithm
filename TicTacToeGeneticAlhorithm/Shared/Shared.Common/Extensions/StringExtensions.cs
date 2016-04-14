namespace Shared.Common.Extensions
{
    using System;

    public static class StringExtensions
    {
        public static string Reverse(this string originalString)
        {
            var arr = originalString.ToCharArray();
            Array.Reverse(arr);
            return new string(arr);
        }
    }
}
