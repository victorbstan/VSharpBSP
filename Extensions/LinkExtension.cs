using System.Collections.Generic;

namespace VSharpBSP.Extensions
{
    public static class LinqExtensions
    {
        public static IEnumerable<T> SelectOdds<T>(this IEnumerable<T> enumerable)
        {
            bool odd = false;

            foreach (var item in enumerable)
            {
                if (odd)
                    yield return item;

                odd = !odd;
            }
        }
        
        public static IEnumerable<T> SelectEvens<T>(this IEnumerable<T> enumerable)
        {
            bool even = true;

            foreach (var item in enumerable)
            {
                if (even)
                    yield return item;

                even = !even;
            }
        }
    }
}