using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CSquared
{
    internal static class Extensions
    {
        public static IEnumerable<T> FromSingle<T>(this T item)
        {
            yield return item;
        }

        public static bool IsEmpty<T>(this IEnumerable<T> items)
        {
            if (items.GetType().IsAssignableFrom(typeof(string)))
            {
                return string.IsNullOrEmpty(items as string);
            }

            return items.FirstOrDefault() == null;
        }

        public static bool IsNull<T>(this T item)
        {
            return item == null;
        }

        public static IEnumerable<Tuple<T, J>> Pair<T, J>(this IEnumerable<T> items1, IEnumerable<J> items2)
        {
            return items1.Zip(items2, (i1, i2) => new Tuple<T, J>(i1, i2));
        }
    }
}
