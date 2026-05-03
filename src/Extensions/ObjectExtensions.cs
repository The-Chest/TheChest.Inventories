namespace TheChest.Inventories.Extensions
{
    /// <summary>
    /// Provides helper methods for object null checks.
    /// </summary>
    internal static class ObjectExtensions
    {
        /// <summary>
        /// Determines whether a reference is null.
        /// </summary>
        /// <typeparam name="T">The object type.</typeparam>
        /// <param name="obj">The object instance to evaluate.</param>
        /// <returns>True if the reference is null; otherwise false.</returns>
        internal static bool IsNull<T>(this T obj)
        {
            if (typeof(T).IsValueType)
                return false;//Has no nullable in value types, so it can't be null

            return (object)obj is null;
        }
    }
}
