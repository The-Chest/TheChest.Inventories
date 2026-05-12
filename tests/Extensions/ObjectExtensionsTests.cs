using TheChest.Inventories.Extensions;

namespace TheChest.Inventories.Tests.Extensions
{
    [TestFixture]
    public class ObjectExtensionsTests
    {
        [Test]
        public void IsNull_WhenReferenceIsNull_ReturnsTrue()
        {
            object? value = null;

            var result = value.IsNull();

            Assert.That(result, Is.True);
        }

        [Test]
        public void IsNull_WhenReferenceIsNotNull_ReturnsFalse()
        {
            object value = new object();

            var result = value.IsNull();

            Assert.That(result, Is.False);
        }

        [Test]
        public void IsNull_WhenValueTypeDefault_ReturnsFalse()
        {
            var value = default(int);

            var result = value.IsNull();

            Assert.That(result, Is.False);
        }
    }
}
