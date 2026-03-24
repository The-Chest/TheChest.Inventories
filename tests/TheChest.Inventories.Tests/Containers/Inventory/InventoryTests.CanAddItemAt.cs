using NUnit.Framework.Internal;

namespace TheChest.Inventories.Tests.Containers
{
    public partial class InventoryTests<T>
    {
        [Test]
        public void CanAddAt_NullItem_ThrowsArgumentNullException()
        {
            var inventory = this.containerFactory.EmptyContainer();
            Assert.That(() => inventory.CanAddAt(item: default!, 0), Throws.ArgumentNullException);
        }

        [TestCase(-1)]
        [TestCase(22)]
        public void CanAddAt_InvalidSlotIndex_ThrowsArgumentOutOfRangeException(int index)
        {
            var inventory = this.containerFactory.EmptyContainer();

            Assert.That(
                () => inventory.CanAddAt(this.itemFactory.CreateDefault(), index),
                Throws.Exception.TypeOf<ArgumentOutOfRangeException>()
            );
        }
    }
}
