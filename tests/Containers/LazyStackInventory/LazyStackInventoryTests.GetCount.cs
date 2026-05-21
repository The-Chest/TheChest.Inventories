namespace TheChest.Inventories.Tests.Containers.LazyStackInventory
{
    public partial class LazyStackInventoryTests<T>
    {
        [Test]
        public void GetCount_NullItem_ThrowsArgumentNullException()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var inventory = this.inventoryFactory.EmptyContainer(size, stackSize);

            Assert.That(
                () => inventory.GetCount(default!), 
                Throws.ArgumentNullException.With.Property("ParamName").EqualTo("item")
            );
        }

        [Test]
        public void GetCount_NotFoundItem_ReturnsZero()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var wrongItem = this.itemFactory.CreateRandom();
            var inventory = this.inventoryFactory.ShuffledItemsContainer(size, stackSize, wrongItem);

            var item = this.itemFactory.CreateDefault();
            var count = inventory.GetCount(item);

            Assert.That(count, Is.Zero);
        }

        [Test]
        public void GetCount_ExistingItem_ReturnsCorrectCount()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var item = this.itemFactory.CreateDefault();
            var inventory = this.inventoryFactory.FullContainer(size, stackSize, item);

            var count = size * stackSize;
            var result = inventory.GetCount(item);

            Assert.That(result, Is.EqualTo(count));
        }
    }
}
