using System;

namespace TheChest.Tests.Common.DependencyInjection
{
    /// <summary>
    /// Represents a service registration for the DI container, including service type, implementation, and factory.
    /// </summary>
    public sealed class Registration
    {
        /// <summary>
        /// The registered service type.
        /// </summary>
        public Type ServiceType { get; }
        /// <summary>
        /// The registered implementation type.
        /// </summary>
        public Type ImplementationType { get; }
        /// <summary>
        /// Custom factory for service creation.
        /// </summary>
        public Func<DIContainer, object> Factory { get; }

        /// <summary>
        /// Initializes a registration with service type and implementation type.
        /// </summary>
        /// <param name="serviceType">The service type.</param>
        /// <param name="implType">The implementation type.</param>
        public Registration(Type serviceType, Type implType)
        {
            ServiceType = serviceType;
            ImplementationType = implType;
        }

        /// <summary>
        /// Initializes a registration with service type and custom factory.
        /// </summary>
        /// <param name="serviceType">The service type.</param>
        /// <param name="factory">The factory function.</param>
        public Registration(Type serviceType, Func<DIContainer, object> factory)
        {
            ServiceType = serviceType;
            Factory = factory;
        }
    }
}
