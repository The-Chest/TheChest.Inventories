using System;

namespace TheChest.Tests.Common.Attributes
{
    /// <summary>
    /// Base attribute for conditionally ignoring test methods based on the type provided.
    /// Derived attributes should implement logic to determine if a test should be ignored for a given type.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public abstract class TypeConditionAttribute : Attribute
    {
        /// <summary>
        /// The reason why the test was ignored.
        /// </summary>
        protected abstract string Reason { get; }

        /// <summary>
        /// Determines whether the test should be ignored for the specified type.
        /// </summary>
        /// <param name="type">The type to evaluate.</param>
        /// <returns>True if the test should be ignored; otherwise, false.</returns>
        protected abstract bool ShouldIgnore(Type type);
    }
}
