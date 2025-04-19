using System.Collections.Generic;

namespace ProtoGenerator.Utilities.CollectionUtilities
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
        public static void AddRange<T>(this HashSet<T> set, IEnumerable<T> collection)
        {
            foreach (var item in collection)
            {
                set.Add(item);
            }
        }
    }
}
