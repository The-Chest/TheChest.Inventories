using TheChest.Inventories.Extensions;

namespace TheChest.Inventories.Tests.Extensions
{
    [TestFixture]
    public class ArrayExtensionsTests
    {
        [Test]
        public void ContainsNull_WhenArrayHasNull_ReturnsTrue()
        {
            string?[] values = { "a", null, "c" };

            var result = values.ContainsNull();

            Assert.That(result, Is.True);
        }

        [Test]
        public void ContainsNull_WhenArrayHasNoNull_ReturnsFalse()
        {
            string?[] values = { "a", "b" };

            var result = values.ContainsNull();

            Assert.That(result, Is.False);
        }

        [Test]
        public void GetAdjacentEqualCount_WhenValuesMatchUntilMaxCount_ReturnsLastMatchingIndex()
        {
            int[] values = { 2, 2, 2, 3, 3 };

            var result = values.GetAdjacentEqualCount(startIndex: 0, maxCount: 3);

            Assert.That(result, Is.EqualTo(2));
        }

        [Test]
        public void GetAdjacentEqualCount_WhenAdjacentValueDiffers_ReturnsStartIndex()
        {
            int[] values = { 1, 2, 2 };

            var result = values.GetAdjacentEqualCount(startIndex: 0, maxCount: 5);

            Assert.That(result, Is.EqualTo(0));
        }

        [Test]
        public void HasAllEqual_WhenAllValuesEqual_ReturnsTrue()
        {
            int[] values = { 8, 8, 8 };

            var result = values.HasAllEqual();

            Assert.That(result, Is.True);
        }

        [Test]
        public void HasAllEqual_WhenValueDiffers_ReturnsFalse()
        {
            int[] values = { 8, 9, 8 };

            var result = values.HasAllEqual();

            Assert.That(result, Is.False);
        }

        [Test]
        public void HasAllEqualAndNoNull_WhenAllEqualAndNotNull_ReturnsTrue()
        {
            string?[] values = { "x", "x", "x" };

            var result = values.HasAllEqualAndNoNull();

            Assert.That(result, Is.True);
        }

        [Test]
        public void HasAllEqualAndNoNull_WhenAnyNull_ReturnsFalse()
        {
            string?[] values = { "x", null, "x" };

            var result = values.HasAllEqualAndNoNull();

            Assert.That(result, Is.False);
        }
    }
}
