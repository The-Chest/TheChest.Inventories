using System;
using TheChest.Tests.Common.DependencyInjection;
using TheChest.Tests.Common.Items;
using TheChest.Tests.Common.Items.Interfaces;

namespace TheChest.Tests.Common
{
    /// <summary>
    /// Base class for tests that provides a dependency injection container and a <see cref="Random"/> instance. 
    /// </summary>
    /// <typeparam name="T">The item type used by the test factory.</typeparam>
    public abstract class BaseTest<T>
    {
        /// <summary>
        /// Container for dependency registrations used by the test.
        /// </summary>
        protected readonly DIContainer configurations;
        /// <summary>
        /// Random instance for generating test values.
        /// </summary>
        protected readonly Random random;

        /// <summary>
        /// Initializes a new instance of <see cref="BaseTest{T}"/>.
        /// </summary>
        /// <remarks>
        /// Executes the provided <paramref name="configure"/> action to register services in the container, and registers a default <see cref="IItemFactory{T}"/> when none is already registered.
        /// </remarks>
        /// <param name="configure">Action to configure the test <see cref="DIContainer"/>.</param>
        protected BaseTest(Action<DIContainer> configure)
        {
            this.configurations = new DIContainer();

            configure(this.configurations);

            if (!this.configurations.IsRegistered<IItemFactory<T>>())
            {
                this.configurations.Register<IItemFactory<T>, ItemFactory<T>>();
            }

            this.random = new Random();
        }
    }
}
