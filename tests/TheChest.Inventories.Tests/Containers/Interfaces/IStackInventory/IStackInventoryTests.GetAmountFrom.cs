namespace TheChest.Inventories.Tests.Containers.Interfaces
{
    public partial class IStackInventoryTests<T>
    {
        [Test]
        public void GetAmountFrom_EmptySlot_ReturnsEmptyArray()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var inventory = this.inventoryFactory.EmptyContainer(size, stackSize);
            
            var index = this.random.Next(0, size);
            var amount = this.random.Next(1, stackSize);
            var item = inventory.Get(index, amount);
            
            Assert.That(item, Is.Empty);
        }

        [Test]
        public void GetAmountFrom_SlotWithItems_ReturnsItems()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var slotItem = this.itemFactory.CreateRandom();
            var inventory = this.inventoryFactory.FullContainer(size, stackSize, slotItem);

            var amount = this.random.Next(1, stackSize);
            var index = this.random.Next(0, size);
            var items = inventory.Get(index, amount);

            Assert.That(items, Is.Not.Empty);
            Assert.That(items, Has.Length.EqualTo(amount));
            Assert.That(items, Has.All.EqualTo(slotItem));
        }

        [Test]
        public void GetAmountFrom_AmountBiggerThanItemsInsideSlot_ReturnsTheMaximumAmountPossible()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var slotItem = this.itemFactory.CreateRandom();
            var inventory = this.inventoryFactory.FullContainer(size, stackSize, slotItem);

            var amount = this.random.Next(stackSize + 1, stackSize * 2);
            var index = this.random.Next(0, size);
            var items = inventory.Get(index, amount);

            Assert.That(items.Count, Is.EqualTo(stackSize));
        }
    }
}
