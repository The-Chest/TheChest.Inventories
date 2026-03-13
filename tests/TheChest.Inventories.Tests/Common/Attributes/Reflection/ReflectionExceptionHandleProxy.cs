using System;
using System.Reflection;

namespace TheChest.Tests.Common.Attributes.Reflection
{
    /// <summary>
    /// Proxy class for handling exceptions thrown by methods invoked via reflection.
    /// When used, it can unwrap and rethrow inner exceptions based on the presence of the <see cref="ReflectionExceptionHandleAttribute"/>.
    /// </summary>
    public sealed class ReflectionExceptionHandleProxy<T> : DispatchProxy
    {
        private T target;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReflectionExceptionHandleProxy{T}"/> class.
        /// </summary>
        public ReflectionExceptionHandleProxy() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ReflectionExceptionHandleProxy{T}"/> class with the specified target.
        /// </summary>
        /// <param name="target">The target object to proxy.</param>
        public ReflectionExceptionHandleProxy(T target)
        {
            this.target = Create(target);
        }

        /// <summary>
        /// Creates a proxy instance for the specified target.
        /// </summary>
        /// <param name="target">The target object to proxy.</param>
        /// <returns>A proxy instance implementing <typeparamref name="T"/>.</returns>
        public static T Create(T target)
        {
            object proxy = Create<T, ReflectionExceptionHandleProxy<T>>() 
                ?? throw new InvalidOperationException("Failed to create proxy instance.");
            
            ((ReflectionExceptionHandleProxy<T>)proxy).target = target;

            return (T)proxy;
        }

        /// <summary>
        /// Invokes the specified method on the proxied target, handling exceptions as configured.
        /// </summary>
        /// <param name="targetMethod">The method to invoke.</param>
        /// <param name="args">The arguments to pass to the method.</param>
        /// <returns>The result of the method invocation.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="targetMethod"/> is null.</exception>
        /// <exception cref="Exception">Rethrows exceptions according to the attribute and invocation context.</exception>
        protected override object Invoke(MethodInfo targetMethod, object[] args)
        {
            if (targetMethod == null)
                throw new ArgumentNullException(nameof(targetMethod));
            try
            {
                return targetMethod.Invoke(target, args);
            }
            catch (TargetInvocationException ex) when (ex.InnerException != null)
            {
                var inner = ex.InnerException;
                var attribute = targetMethod.GetCustomAttribute<ReflectionExceptionHandleAttribute>();
                
                if (inner.GetType() != typeof(TargetInvocationException))
                    throw inner;

                if (attribute != null && inner.InnerException != null)
                    throw inner.InnerException;

                throw inner;
            }
        }
    }
}
