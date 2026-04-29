namespace TheChest.Inventories.Tests.Containers.Interfaces
{
    public partial class IStackInventoryTests<T>
    {
        [Test]
        public void GetAllFrom_EmptySlot_ReturnsEmptyArray()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var inventory = this.inventoryFactory.EmptyContainer(size, stackSize);

            var index = this.random.Next(0, size - 1);
            var items = inventory.GetAll(index);

            Assert.That(items, Is.Empty);
        }

        [Test]
        public void GetAllFrom_SlotWithItems_ReturnItems()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var slotItem = this.itemFactory.CreateRandom();
            var inventory = this.inventoryFactory.FullContainer(size, stackSize, slotItem);

            var index = this.random.Next(0, size);
            var items = inventory.GetAll(index);

            Assert.Multiple(() =>
            {
                Assert.That(items, Has.Length.EqualTo(stackSize));
                Assert.That(items, Has.All.EqualTo(slotItem));
            });
        }
    }
}
