namespace FATWA_GENERAL.Helper
{
    public static class ObjectExtensions
    {
        public static bool IsDefault<T>(this T source)
        {
            return source.GenericEquals(default);
        }
        public static bool GenericEquals<T>(this T source, T other)
        {
            return EqualityComparer<T>.Default.Equals(source, other);
        }
    }
}
