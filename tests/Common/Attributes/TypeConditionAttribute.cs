using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;

namespace TheChest.Tests.Common.Attributes
{
    /// <summary>
    /// Base attribute for conditionally ignoring test methods based on the type provided.
    /// Derived attributes should implement logic to determine if a test should be ignored for a given type.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public abstract class TypeConditionAttribute : NUnitAttribute, IApplyToTest
    {
        protected abstract bool ShouldIgnore(Type type);

        protected virtual string Reason => "Condition not met.";

        public void ApplyToTest(Test test)
        {
            var fixtureType = test.TypeInfo?.Type;
            if (fixtureType == null)
                return;

            if (!fixtureType.IsGenericType)
                return;

            var firstArgument = fixtureType.GetGenericArguments()[0];

            if (this.ShouldIgnore(firstArgument))
            {
                test.RunState = RunState.Ignored;
            }
        }
    }
}
