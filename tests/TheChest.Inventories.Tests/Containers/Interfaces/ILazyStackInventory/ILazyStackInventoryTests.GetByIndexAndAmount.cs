namespace TheChest.Inventories.Tests.Containers.Interfaces
{
    public partial class ILazyStackInventoryTests<T>
    {
        [Test]
        public void Get_ByIndexAndAmount_ValidIndexEmptySlot_ReturnsEmptyArray()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var inventory = this.inventoryFactory.EmptyContainer(size, stackSize);

            var index = this.random.Next(0, size);
            var amount = this.random.Next(1, stackSize);
            var result = inventory.Get(index, amount);

            Assert.That(result, Is.Empty);
        }

        [Test]
        public void Get_ByIndexAndAmount_ValidIndexAndAmount_ReturnsCorrectAmount()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var item = this.itemFactory.CreateDefault();
            var inventory = this.inventoryFactory.FullContainer(size, stackSize, item);

            var index = this.random.Next(0, size);
            var amount = this.random.Next(1, stackSize);
            var result = inventory.Get(index, amount);

            Assert.That(result, Has.Length.EqualTo(amount));
            Assert.That(result, Has.All.EqualTo(item));
        }

        [Test]
        public void Get_ByIndexAndAmount_ValidIndexAndAmountBiggerThanSlotSize_ReturnsMaxAvailableAmount()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var item = this.itemFactory.CreateDefault();
            var inventory = this.inventoryFactory.FullContainer(size, stackSize, item);

            var index = this.random.Next(0, size);
            var amount = this.random.Next(stackSize + 1, stackSize + 10);
            var result = inventory.Get(index, amount);

            Assert.That(result, Has.Length.EqualTo(size));
            Assert.That(result, Has.All.EqualTo(item));
        }
    }
}
