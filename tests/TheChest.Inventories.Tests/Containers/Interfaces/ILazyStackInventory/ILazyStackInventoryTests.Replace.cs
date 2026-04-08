namespace TheChest.Inventories.Tests.Containers.LazyStackInventory
{
    public partial class LazyStackInventoryTests<T>
    {
        [Test]
        public void Replace_SlotWithItems_ReturnsItemFromSlot()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var oldItem = this.itemFactory.CreateDefault();
            var inventory = this.inventoryFactory.FullContainer(size, stackSize, oldItem);

            var newItem = this.itemFactory.CreateRandom();
            var index = this.random.Next(0, size - 1);
            var oldItems = inventory.Replace(newItem, index, stackSize);

            Assert.That(oldItems, Has.Length.EqualTo(stackSize).And.All.EqualTo(oldItem));
        }

        [Test]
        public void Replace_EmptySlot_ReturnsEmptyArray()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var inventory = this.inventoryFactory.EmptyContainer(size, stackSize);

            var newItem = this.itemFactory.CreateRandom();
            var index = this.random.Next(0, size - 1);
            var oldItems = inventory.Replace(newItem, index, stackSize);

            Assert.That(oldItems, Is.Empty);
        }
    }
}
