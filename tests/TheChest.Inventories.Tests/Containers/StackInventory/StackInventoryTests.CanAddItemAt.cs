namespace TheChest.Inventories.Tests.Containers.StackInventory
{
    public partial class StackInventoryTests<T>
    {
        [Test]
        [TheChest.Tests.Common.Attributes.IgnoreIfValueTypeAttribute]
        public void CanAddItemAt_NullItem_ThrowsArgumentNullException()
        {
            var inventory = this.inventoryFactory.EmptyContainer();
            Assert.That(() => inventory.CanAddAt(item: default!, 0), Throws.ArgumentNullException);
        }

        [TestCase(-1)]
        [TestCase(MAX_SIZE_TEST + 1)]
        public void CanAddItemAt_InvalidSlotIndex_ThrowsArgumentOutOfRangeException(int index)
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var inventory = this.inventoryFactory.EmptyContainer(size);

            Assert.That(() => inventory.CanAddAt(this.itemFactory.CreateDefault(), index), Throws.Exception.TypeOf<ArgumentOutOfRangeException>());
        }
    }
}
