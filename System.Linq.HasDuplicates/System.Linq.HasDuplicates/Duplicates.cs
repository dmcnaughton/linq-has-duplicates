using System;
using System.Collections.Generic;
using System.Text;

namespace System.Linq.HasDuplicates
{
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Returns true if two or more elements within the <paramref name="source"/> are the same.
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static bool HasDuplicates<TSource>(this IEnumerable<TSource> source) => HasDuplicates(source, null);

        /// <summary>
        /// Returns true if two or more elements within the <paramref name="source"/> are the same.
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <param name="comparer"></param>
        /// <returns></returns>
        public static bool HasDuplicates<TSource>(this IEnumerable<TSource> source, IEqualityComparer<TSource> comparer)
        {
            // Null objects don't have duplicates.
            if (source == null)
            {
                return false;
            }

            var set = new Set<TSource>(comparer);

            foreach (var element in source)
            {
                if (!set.Add(element))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Returns back an enumerable list of elements that were duplicates, as well as the count for each element instance in the source.
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static IEnumerable<KeyValuePair<TSource, int>> Duplicates<TSource>(this IEnumerable<TSource> source) => Duplicates(source, null);

        /// <summary>
        /// Returns back an enumerable list of elements that were duplicates, as well as the count for each element instance in the source.
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <param name="comparer"></param>
        /// <returns></returns>
        public static IEnumerable<KeyValuePair<TSource, int>> Duplicates<TSource>(this IEnumerable<TSource> source,
            IEqualityComparer<TSource> comparer)
        {
            // Null objects don't have duplicates, but we don't like throwing null value exceptions. Maybe if we null check this method it won't throw an exception.
            // Narrator(Morgan Freeman): It does throw an exception.
            if (source == null)
            {
                return Enumerable.Empty<KeyValuePair<TSource, int>>();
            }

            var set = new Set<TSource>(comparer);

            foreach (var element in source)
            {
                set.Add(element);
            }

            return set.ToEnumerableWithCount();
        }
    }
}
