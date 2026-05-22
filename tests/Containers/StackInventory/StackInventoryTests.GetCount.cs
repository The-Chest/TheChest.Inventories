namespace TheChest.Inventories.Tests.Containers.StackInventory
{
    public partial class StackInventoryTests<T>
    {
        [Test]
        public void GetCount_InvalidItem_ThrowsArgumentNullException()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var inventory = this.inventoryFactory.EmptyContainer(size, stackSize);

            Assert.That(
                () => inventory.GetCount(default!), 
                Throws.ArgumentNullException.With.Property("ParamName").EqualTo("item")
            );
        }


        [Test]
        public void GetCount_EmptyInventory_ReturnsZero()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var inventory = this.inventoryFactory.EmptyContainer(size, stackSize);
            var item = this.itemFactory.CreateDefault();

            var count = inventory.GetCount(item);

            Assert.That(count, Is.Zero);
        }

        [Test]
        public void GetCount_InventoryWithItems_ReturnsItemCount()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var item = this.itemFactory.CreateDefault();
            var inventoryItems = this.itemFactory.CreateManyRandom(size - 2)
                .Append(item)
                .Append(item)
                .ToList();

            var inventory = this.inventoryFactory.ShuffledItemsContainer(size, stackSize, inventoryItems.ToArray());

            var count = inventory.GetCount(item);

            Assert.That(count, Is.EqualTo(stackSize * 2));
        }
    }
}
