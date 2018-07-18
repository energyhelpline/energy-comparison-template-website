using System.Collections.Generic;
using System.Linq;

namespace BareboneUi.Common
{
    public static class NamedRecordEnumerableExtensions
    {
        public static IEnumerable<KeyPair> AsKeyPairs<T>(this IEnumerable<T> enumerable) where T : INamedRecord
        {
            return enumerable.Select(x => new KeyPair { Id = x.Id, Name = x.Name });
        }
    }
}