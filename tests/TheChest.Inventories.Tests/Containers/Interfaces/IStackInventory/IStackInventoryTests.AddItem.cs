namespace TheChest.Inventories.Tests.Containers.Interfaces
{
    public partial class IStackInventoryTests<T>
    {
        [Test]
        public void AddItem_EmptyInventory_ReturnsTrue()
        {
            var item = this.itemFactory.CreateDefault();
            var inventory = this.inventoryFactory.EmptyContainer();

            var result = inventory.Add(item);

            Assert.That(result, Is.True);
        }

        [Test]
        public void AddItem_FullInventory_DoesNotAddToSlot()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var item = this.itemFactory.CreateDefault();
            var inventory = this.inventoryFactory.FullContainer(size, stackSize, this.itemFactory.CreateRandom());

            inventory.Add(item);

            Assert.That(inventory.GetCount(item), Is.Zero);
        }

        [Test]
        public void AddItem_FullInventory_ReturnsFalse()
        {
            var item = this.itemFactory.CreateDefault();
            var inventory = this.inventoryFactory.FullContainer(10, 10, this.itemFactory.CreateRandom());

            var result = inventory.Add(item);

            Assert.That(result, Is.False);
        }
    }
}
