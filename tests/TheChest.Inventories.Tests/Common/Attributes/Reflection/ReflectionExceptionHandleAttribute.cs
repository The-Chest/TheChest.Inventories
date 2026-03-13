using System;

namespace TheChest.Tests.Common.Attributes.Reflection
{
    /// <summary>
    /// Attribute to indicate that exceptions thrown by a method invoked via reflection should be handled specially.
    /// When applied, the proxy will unwrap and rethrow the inner exception.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public sealed class ReflectionExceptionHandleAttribute : Attribute { }
}
