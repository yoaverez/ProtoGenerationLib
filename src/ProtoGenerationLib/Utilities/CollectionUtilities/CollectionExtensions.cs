using System;
using System.Collections.Generic;
using System.Linq;

namespace ProtoGenerationLib.Utilities.CollectionUtilities
{
    /// <summary>
    /// Extension methods for collection types.
    /// </summary>
    public static class CollectionExtensions
    {
        /// <summary>
        /// Calculate the hash code of the collection by combining its
        /// elements hash codes.
        /// </summary>
        /// <typeparam name="T">The collection type.</typeparam>
        /// <param name="collection">The collection whose hash code to calculate.</param>
        /// <returns>A hash code that represents the combination of this collection elements.</returns>
        public static int CalcHashCode<T>(this IEnumerable<T> collection)
        {
            if (collection == null) return 0;

            // Overflow is fine
            unchecked
            {
                int hash = 17;
                foreach (var item in collection)
                {
                    hash = hash * 31 + (item?.GetHashCode() ?? 0);
                }
                return hash;
            }
        }

        /// <summary>
        /// Convert the given <paramref name="collection"/> to <see cref="HashSet{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of the <paramref name="collection"/> elements.</typeparam>
        /// <param name="collection">The collection to convert to <see cref="HashSet{T}"/>.</param>
        /// <returns><see cref="HashSet{T}"/> the containing all the unique elements from the given <paramref name="collection"/>.</returns>
        public static HashSet<T> ToHashSet<T>(this IEnumerable<T> collection)
        {
            return new HashSet<T>(collection);
        }

        /// <summary>
        /// Add the given <paramref name="collection"/> to the <paramref name="set"/>.
        /// </summary>
        /// <typeparam name="T">The type of the elements in the collections.</typeparam>
        /// <param name="set">The set to add the <paramref name="collection"/> to.</param>
        /// <param name="collection">The collection to add to the <paramref name="set"/>.</param>
        public static void AddRange<T>(this ISet<T> set, IEnumerable<T> collection)
        {
            foreach (var item in collection)
            {
                set.Add(item);
            }
        }

        /// <summary>
        /// Add the given <paramref name="collection"/> to the <paramref name="dictionary"/>.
        /// </summary>
        /// <typeparam name="TKey">The type of the keys in the collections.</typeparam>
        /// <typeparam name="TValue">The type of the values in the collections.</typeparam>
        /// <param name="dictionary">The dictionary to add the <paramref name="collection"/> to.</param>
        /// <param name="collection">The collection to add to the <paramref name="dictionary"/>.</param>
        public static void AddRange<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, IEnumerable<KeyValuePair<TKey, TValue>> collection)
        {
            foreach (var item in collection)
            {
                dictionary[item.Key] = item.Value;
            }
        }

        /// <summary>
        /// Checks if two sequences are equivalent i.e have the same items
        /// but not necessarily in the same order.
        /// </summary>
        /// <typeparam name="T">The type of the collections items.</typeparam>
        /// <param name="collection1">The first collection.</param>
        /// <param name="collection2">The second collection.</param>
        /// <returns>
        /// <see langword="true"/> if both collections have the same items
        /// (not necessarily in the same order) otherwise <see langword="false"/>.
        /// </returns>
        public static bool SequenceEquivalence<T>(this IEnumerable<T> collection1, IEnumerable<T> collection2)
        {
            if (collection1 is null && collection2 is null)
                return true;

            if (collection1 is null || collection2 is null)
                return false;

            if (ReferenceEquals(collection1, collection2))
                return true;

            if (collection1.Count() != collection2.Count())
                return false;

            var dictionary1 = collection1.ToDictionary(item => EqualityComparer<T>.Default.GetHashCode(item), item => item);
            var dictionary2 = collection2.ToDictionary(item => EqualityComparer<T>.Default.GetHashCode(item), item => item);

            foreach (var hashcode in dictionary1.Keys)
            {
                if (!dictionary2.ContainsKey(hashcode))
                {
                    return false;
                }
                else
                {
                    // If the items with the same hash code does not equals, return false.
                    if (!EqualityComparer<T>.Default.Equals(dictionary1[hashcode], dictionary2[hashcode]))
                    {
                        return false;
                    }
                }
            }
            return true;
        }
    }
}
