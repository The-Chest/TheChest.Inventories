using System;
using System.Linq;
using System.Reflection;
using TheChest.Tests.Common.Items.Interfaces;
using TheChest.Tests.Common.Extensions;

namespace TheChest.Tests.Common.Items
{
    /// <summary>
    /// A generic factory for creating instances of type <typeparamref name="T"/> with default or random values.
    /// This factory supports value types, reference types, enums, and primitive types, automatically populating fields with appropriate random values based on their types.
    /// </summary>
    /// <typeparam name="T">The type of instance to create.</typeparam>
    public sealed class ItemFactory<T> : IItemFactory<T>
    {
        /// <summary>
        /// Creates a new instance of type <typeparamref name="T"/> with default values.
        /// </summary>
        /// <returns>A new instance of type <typeparamref name="T"/> initialized with default values.</returns>
        /// <exception cref="InvalidOperationException">When the instance cannot be created.</exception>
        public T CreateDefault()
        {
            var type = typeof(T);
            var instance = Activator.CreateInstance(type) ??
                throw new InvalidOperationException($"Could not create instance of type {type.FullName}");

            return (T)instance;
        }

        /// <summary>
        /// Creates a new instance of type <typeparamref name="T"/> with random values.
        /// For primitive types and enums, generates random values directly. For complex types, randomly populates all private instance fields.
        /// </summary>
        /// <returns>A new instance of type <typeparamref name="T"/> with random values assigned to fields.</returns>
        /// <exception cref="InvalidOperationException">When the instance cannot be created.</exception>
        /// <exception cref="NotImplementedException">When random generation for a field type is not supported.</exception>
        public T CreateRandom()
        {
            var type = typeof(T);
            var instance = Activator.CreateInstance(type) ??
                throw new InvalidOperationException($"Could not create instance of type {type.FullName}");

            var instanceType = instance.GetType();
            if (instanceType.IsEnum || instanceType.IsPrimitive)
            {
                if (instanceType.IsEnum)
                {
                    var values = ((T[])instanceType.GetEnumValues()).Skip(1).ToArray();
                    values.Shuffle();
                    return (T)values.GetValue(0)!;
                }
                return (T)instanceType.SetRandomValue();
            }

            var fields = instanceType.GetFields(BindingFlags.NonPublic | BindingFlags.Instance);
            foreach (var field in fields)
            {
                var value = field.FieldType.SetRandomValue();
                field.SetValue(instance, value);
            }
            return (T)instance;
        }

        /// <summary>
        /// Creates multiple instances of type <typeparamref name="T"/> with default values.
        /// </summary>
        /// <param name="amount">The number of instances to create.</param>
        /// <returns>An array of instances, each initialized with default values.</returns>
        public T[] CreateMany(int amount)
        {
            return Enumerable.Repeat(CreateDefault(), amount).ToArray();
        }

        /// <summary>
        /// Creates multiple instances of type <typeparamref name="T"/> with random values.
        /// </summary>
        /// <param name="amount">The number of instances to create.</param>
        /// <returns>An array of instances, each initialized with random values.</returns>
        public T[] CreateManyRandom(int amount)
        {
            return Enumerable.Repeat(CreateRandom(), amount).ToArray();
        }
    }
}
