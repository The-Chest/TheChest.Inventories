using System;
using System.Reflection;
using TheChest.Tests.Common.Attributes.Reflection;
using TheChest.Tests.Common.Extensions;

namespace TheChest.Tests.Common.DependencyInjection
{
    /// <summary>
    /// Custom Dependency Injection container for tests, responsible for registering and resolving services and factories.
    /// </summary>
    public sealed class DIContainer
    {
        private readonly DIRegistry registry;

        /// <summary>
        /// Initializes a new instance of the <see cref="DIContainer"/> class.
        /// </summary>
        public DIContainer() 
        {
            this.registry = new DIRegistry();
        }

        /// <summary>
        /// Registers a service and its implementation.
        /// </summary>
        /// <typeparam name="TService">The service type.</typeparam>
        /// <typeparam name="TImpl">The implementation type.</typeparam>
        public DIContainer Register<TService, TImpl>() where TImpl : TService
        {
            this.registry.Register(typeof(TService), typeof(TImpl));
            return this;
        }

        /// <summary>
        /// Registers a service and its implementation by type.
        /// </summary>
        /// <param name="serviceType">The service type.</param>
        /// <param name="implementationType">The implementation type.</param>
        public DIContainer Register(Type serviceType, Type implementationType)
        {
            this.registry.Register(serviceType, implementationType);
            return this;
        }

        /// <summary>
        /// Registers a service using a custom factory.
        /// </summary>
        /// <typeparam name="TService">The service type.</typeparam>
        /// <param name="factory">The factory function.</param>
        public DIContainer Register<TService>(Func<DIContainer, TService> factory)
        {
            this.registry.Register(factory);
            return this;
        }

        /// <summary>
        /// Registers a concrete instance of a service.
        /// </summary>
        /// <typeparam name="TService">The service type.</typeparam>
        /// <param name="instance">The service instance.</param>
        public DIContainer Register<TService>(TService instance)
        {
            this.registry.Register(instance);
            return this;
        }

        /// <summary>
        /// Checks if a service is registered.
        /// </summary>
        /// <typeparam name="TService">The service type.</typeparam>
        public bool IsRegistered<TService>()
        {
            return this.registry.IsRegistered<TService>();
        }

        /// <summary>
        /// Resolves an instance of the registered service.
        /// </summary>
        /// <typeparam name="T">The service type.</typeparam>
        public T Resolve<T>() => (T)this.Resolve(typeof(T));

        /// <summary>
        /// Resolves an instance of the registered service by type.
        /// </summary>
        /// <param name="serviceType">The service type.</param>
        public object Resolve(Type serviceType)
        {
            if (!this.registry.TryGetRegistration(serviceType, out var reg))
                throw new InvalidOperationException($"Service {serviceType} is not registered");

            if (reg == null)
                throw new InvalidOperationException("Registration is null");

            if (reg.Factory != null)
                return reg.Factory(this);

            var implType = reg.ImplementationType ?? serviceType;
            var constructor = implType.GetSmallerConstructor() 
                ?? throw new InvalidOperationException($"No public constructor found for {implType.FullName}");

            var args = constructor.GetParameters();
            var instance = default(object);

            if (args.Length == 0)
                instance = Activator.CreateInstance(implType)!;

            var parameters = new object[args.Length];
            for (var i = 0; i < args.Length; i++)
            {
                parameters[i] = this.Resolve(args[i].ParameterType);
            }

            instance ??= constructor.Invoke(parameters)!;

            //TODO: separate in a "FactoryResolver" or something like that
            if ((implType.Name.Contains("Factory")) && (implType.Namespace?.EndsWith("Factories") ?? false))
            {
                var method = typeof(ReflectionExceptionHandleProxy<>)
                    .MakeGenericType(serviceType)
                    .GetMethod("Create", BindingFlags.Public | BindingFlags.Static);
                return method!.Invoke(constructor, new object[] { instance })!;
            }
            
            return instance;
        }
    }
}
