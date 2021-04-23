using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyPrismDemo.Extensions
{
    public static class CollectionExtensions
    {
        public static ICollection<T> Add<T>(this ICollection<T> collection, T addItem)
        {
            collection.Add(addItem);
            return collection;
        }
    }
}
