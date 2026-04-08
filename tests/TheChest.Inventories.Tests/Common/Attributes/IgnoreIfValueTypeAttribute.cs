using System;

namespace TheChest.Tests.Common.Attributes
{
    /// <summary>
    /// Attribute to ignore test methods when the provided type is a value type (struct, enum, etc).
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public sealed class IgnoreIfValueTypeAttribute : TypeConditionAttribute
    {
        /// <summary>
        /// Determines whether the test should be ignored based on whether the type is a value type.
        /// </summary>
        /// <param name="type">The type to be evaluated.</param>
        /// <returns>True if the type is a value type; otherwise, false.</returns>
        protected override bool ShouldIgnore(Type type) => type.IsValueType;

        /// <summary>
        /// The reason why the test was ignored.
        /// </summary>
        protected override string Reason => "Ignored because test does not apply to value types.";
    }
}
