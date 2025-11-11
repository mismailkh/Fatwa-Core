namespace FATWA_GENERAL.Helper
{
    public static class EnumerableExtensions
    {
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> source)
        {
            return source == null || !source.FastAny();
            //return source == null || !source.Any();
        }
        internal static bool FastAny<TSource>(this IEnumerable<TSource> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (source is TSource[] array)
            {
                return array.Length > 0;
            }

            if (source is ICollection<TSource> collection)
            {
                return collection.Count > 0;
            }

            using (var enumerator = source.GetEnumerator())
            {
                return enumerator.MoveNext();
            }
        }
    }
}
