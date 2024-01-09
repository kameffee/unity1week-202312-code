using System.Collections.Generic;
using System.Linq;

namespace Kameffee.Extensions
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source)
        {
            return source.OrderBy(x => System.Guid.NewGuid());
        }
    }
}