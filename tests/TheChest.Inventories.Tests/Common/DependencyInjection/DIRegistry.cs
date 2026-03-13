using System;
using System.Collections.Generic;
using System.Linq;

namespace TheChest.Tests.Common.DependencyInjection
{
    /// <summary>
    /// Central registry of services and implementations for the custom DI container.
    /// </summary>
    public sealed class DIRegistry
    {
        private readonly Dictionary<Type, Registration> registrations;
        private readonly List<Registration> openGenericRegistrations;

        /// <summary>
        /// Initializes a new instance of the <see cref="DIRegistry"/> class.
        /// </summary>
        public DIRegistry()
        {
            this.registrations = new Dictionary<Type, Registration>();
            this.openGenericRegistrations = new List<Registration>();
        }

        /// <summary>
        /// Registers a service and its implementation.
        /// </summary>
        /// <typeparam name="TService">The service type.</typeparam>
        /// <typeparam name="TImpl">The implementation type.</typeparam>
        public void Register<TService, TImpl>() where TImpl : TService
        {
            this.Register(typeof(TService), typeof(TImpl));
        }

        /// <summary>
        /// Registers a service and its implementation by type.
        /// </summary>
        /// <param name="serviceType">The service type.</param>
        /// <param name="implementationType">The implementation type.</param>
        public void Register(Type serviceType, Type implementationType)
        {
            var registration = new Registration(serviceType, implementationType);

            if (serviceType.IsGenericTypeDefinition || implementationType.IsGenericTypeDefinition)
            {
                this.openGenericRegistrations.Add(registration);
            }
            else 
            {
                this.registrations[serviceType] = registration;
            }
        }

        /// <summary>
        /// Registers a service using a custom factory.
        /// </summary>
        /// <typeparam name="TService">The service type.</typeparam>
        /// <param name="factory">The factory function.</param>
        public void Register<TService>(Func<DIContainer, object> factory)
        {
            this.registrations[typeof(TService)] = new Registration(typeof(TService), factory);
        }

        /// <summary>
        /// Registers a concrete instance of a service.
        /// </summary>
        /// <typeparam name="TService">The service type.</typeparam>
        /// <param name="instance">The service instance.</param>
        public void Register<TService>(TService instance)
        {
            if (instance is null)
                throw new ArgumentNullException(nameof(instance));

            var serviceType = typeof(TService);
            this.registrations[serviceType] = new Registration(serviceType, c => instance);
        }

        /// <summary>
        /// Checks if a service is registered.
        /// </summary>
        /// <typeparam name="TService">The service type.</typeparam>
        public bool IsRegistered<TService>()
        {
            return this.TryGetRegistration(typeof(TService), out var _);
        }

        /// <summary>
        /// Tries to get the registration of a service by type.
        /// </summary>
        /// <param name="serviceType">The service type.</param>
        /// <param name="registration">The found registration, if any.</param>
        /// <returns>True if the registration was found; otherwise, false.</returns>
        public bool TryGetRegistration(Type serviceType, out Registration registration)
        {
            if (registrations.TryGetValue(serviceType, out registration))
                return true;

            if (serviceType.IsGenericType)
            {
                var genDef = serviceType.GetGenericTypeDefinition();
                var match = this.openGenericRegistrations.FirstOrDefault(r => r.ServiceType == genDef);
                if (match != null)
                {
                    if (match.ImplementationType == null)
                        throw new InvalidOperationException("Open generic registration has no implementation type");

                    var implType = match.ImplementationType.MakeGenericType(serviceType.GetGenericArguments());
                    registration = new Registration(serviceType, implType);
                    return true;
                }
            }

            registration = null;
            return false;
        }
    }
}
