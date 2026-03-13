using System;
using System.Linq;
using System.Reflection;

namespace TheChest.Tests.Common.Extensions
{
    internal static class TypeExtensions
    {
        internal static ConstructorInfo GetSmallerConstructor(this Type implType)
        {
            return implType.GetConstructors(BindingFlags.Public | BindingFlags.Instance)
                .OrderByDescending(c => c.GetParameters().Length)
                .FirstOrDefault()
                ?? throw new InvalidOperationException("No public constructor found for " + implType.FullName);
        }

        internal static object SetRandomValue(this Type type)
        {
            var random = new Random();

            return type switch
            {
                var t when t == typeof(int) || t == typeof(short) || t == typeof(long)
                    => random.Next(1, 1000),

                var t when t == typeof(double) || t == typeof(float) || t == typeof(decimal)
                    => random.NextDouble() * 1000,

                var t when t == typeof(byte)
                    => (byte)random.Next(1, 255),

                var t when t == typeof(string)
                    => Guid.NewGuid().ToString(),

                var t when t == typeof(bool)
                    => random.Next(0, 2) == 1,

                var t => throw new NotImplementedException(
                    $"Random generation for type {t.Name} is not implemented.")
            };
        }
    }
}
