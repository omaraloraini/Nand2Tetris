using System;
using System.Collections.Generic;

namespace VirtualMachine
{
    internal static class EnumerableSplit
    {
        public static IEnumerable<IList<T>> Split<T>(
            this IEnumerable<T> source, T item) where T : IEquatable<T>
        {
            return Split(source, t => t.Equals(item));
        }
        
        public static IEnumerable<IList<T>> Split<T>(
            this IEnumerable<T> source, Func<T, bool> predicate)
        {
            var batch = new List<T>();

            foreach (var item in source)
            {
                if (batch.Count > 0 && predicate(item))
                {
                    yield return batch;
                    batch.Clear();
                }
                
                batch.Add(item);
            }

            if (batch.Count > 0) yield return batch;
        }
    }
}