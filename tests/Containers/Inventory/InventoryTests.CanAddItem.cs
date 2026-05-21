namespace TheChest.Inventories.Tests.Containers.Inventory
{
    public partial class InventoryTests<T>
    {
        [Test]
        public void CanAddItem_NullItem_ThrowsArgumentNullException()
        {
            var size = this.GenerateRandomSize();
            var inventory = this.inventoryFactory.EmptyContainer(size);
            Assert.That(
                () => inventory.CanAdd(item: default!),
                Throws.ArgumentNullException.With.Property("ParamName").EqualTo("item")
            );
        }

        [Test]
        public void CanAddItem_FullInventory_ReturnsFalse()
        {
            var size = this.GenerateRandomSize();
            var item = this.itemFactory.CreateDefault();
            var inventory = this.inventoryFactory.FullContainer(size, item);

            var canAdd = inventory.CanAdd(item);

            Assert.That(canAdd, Is.False);
        }

        [Test]
        public void CanAddItem_EmptyInventory_ReturnsTrue()
        {
            var size = this.GenerateRandomSize();
            var inventory = this.inventoryFactory.EmptyContainer(size);
            var item = this.itemFactory.CreateDefault();

            var canAdd = inventory.CanAdd(item);

            Assert.That(canAdd, Is.True);
        }

        [Test]
        public void CanAddItem_PartiallyFullInventory_ReturnsTrue()
        {
            var size = this.GenerateRandomSize();
            var item = this.itemFactory.CreateDefault();
            var inventory = this.inventoryFactory.FullContainer(size, item);

            var randomIndex = this.random.Next(0, size);
            inventory.Get(randomIndex);

            var canAdd = inventory.CanAdd(item);
            Assert.That(canAdd, Is.True);
        }
    }
}
