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
        /// Creates multiple instances of type <typeparamref name="T"/> with default values.
        /// </summary>
        /// <param name="amount">The number of instances to create.</param>
        /// <returns>An array of instances, each initialized with default values.</returns>
        public T[] CreateMany(int amount)
        {
            return Enumerable.Repeat(CreateDefault(), amount).ToArray();
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
        /// Creates a new instance of type <typeparamref name="T"/> with random values, ensuring that the generated instance is different from the provided <paramref name="item"/>.
        /// </summary>
        /// <param name="item">The instance to compare against.</param>
        /// <returns>A new instance of type <typeparamref name="T"/> with random values, distinct from the provided <paramref name="item"/>.</returns>
        /// <exception cref="InvalidOperationException">When a distinct instance cannot be created after multiple attempts.</exception>
        public T CreateRandomDifferentFrom(T item)
        {
            for (var attempt = 0; attempt < 20; attempt++)
            {
                var second = this.CreateRandom();
                if (!object.Equals(item, second))
                    return second;
            }

            throw new InvalidOperationException($"Could not create distinct items for {typeof(T).FullName}.");
        }

        public (T item1, T item2) CreateRandomDistinctPair()
        {
            var item1 = this.CreateRandom();
            var item2 = this.CreateRandomDifferentFrom(item1);

            return (item1, item2);
        }

        /// <summary>
        /// Creates multiple instances of type <typeparamref name="T"/> with random values.
        /// </summary>
        /// <param name="amount">The number of instances to create.</param>
        /// <returns>An array of instances, each initialized with random values.</returns>
        public T[] CreateManyRandom(int amount)
        {
            var randomItem = this.CreateRandom();
            return Enumerable.Repeat(randomItem, amount).ToArray();
        }
        /// <summary>
        /// Creates multiple instances of type <typeparamref name="T"/> with random values, ensuring that each generated instance is different from the provided <paramref name="item"/>.
        /// </summary>
        /// <param name="item">The instance to compare against.</param>
        /// <param name="amount">The number of instances to create.</param>
        /// <returns>An array of instances, each initialized with random values and distinct from the provided <paramref name="item"/>.</returns>
        /// <exception cref="InvalidOperationException">When a distinct instance cannot be created after multiple attempts.</exception>
        public T[] CreateManyRandomDifferentFrom(T item, int amount)
        {
            var randomItem = this.CreateRandomDifferentFrom(item);
            return Enumerable.Repeat(randomItem, amount).ToArray();
        }

        public (T[] items1, T[] items2) CreateManyRandomDistinctPairs(int amount)
        {
            var items1 = this.CreateManyRandom(amount);
            var items2 = this.CreateManyRandomDifferentFrom(items1[0], amount);

            return (items1, items2);
        }
    }
}
