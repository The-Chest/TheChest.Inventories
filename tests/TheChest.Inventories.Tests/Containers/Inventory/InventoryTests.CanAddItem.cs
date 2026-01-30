namespace TheChest.Inventories.Tests.Containers
{
    public partial class InventoryTests<T>
    {
        [Test]
        public void CanAddItem_NullItem_ThrowsArgumentNullException()
        {
            var inventory = this.containerFactory.EmptyContainer();
            Assert.That(() => inventory.CanAdd(item: default!), Throws.ArgumentNullException);
        }

        [Test]
        public void CanAddItem_EmptyInventory_ReturnsTrue()
        {
            var inventory = this.containerFactory.EmptyContainer();
            var item = this.itemFactory.CreateDefault();

            var canAdd = inventory.CanAdd(item);

            Assert.That(canAdd, Is.True);
        }

        [Test]
        public void CanAddItem_PartiallyFullInventory_ReturnsTrue()
        {
            var size = this.random.Next(10, 20);
            var item = this.itemFactory.CreateDefault();
            var inventory = this.containerFactory.FullContainer(size, item);

            var randomIndex = this.random.Next(0, size);
            inventory.Get(randomIndex);

            var canAdd = inventory.CanAdd(item);
            Assert.That(canAdd, Is.True);
        }

        [Test]
        public void CanAddItem_FullInventory_ReturnsFalse()
        {
            var size = this.random.Next(10, 20);
            var item = this.itemFactory.CreateDefault();
            var inventory = this.containerFactory.FullContainer(size, item);

            var canAdd = inventory.CanAdd(item);

            Assert.That(canAdd, Is.False);
        }
    }
}
