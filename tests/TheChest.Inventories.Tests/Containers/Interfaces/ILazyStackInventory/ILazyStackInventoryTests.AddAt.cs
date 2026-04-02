namespace TheChest.Inventories.Tests.Containers.Interfaces
{
    public partial class ILazyStackInventoryTests<T>
    {
        [Test]
        public void AddAt_AllItemsSuccessfullyAdded_ReturnsZero()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var inventory = this.inventoryFactory.EmptyContainer(size, stackSize);
            var item = this.itemFactory.CreateDefault();

            var notAddedCount = inventory.AddAt(item, 0, stackSize);
            
            Assert.That(notAddedCount, Is.Zero);
        }

        [Test]
        public void AddAt_SlotWithDifferentItem_ReturnsNotAddedAmount()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var randomItem = this.itemFactory.CreateManyRandom(size);
            var inventory = this.inventoryFactory.ShuffledItemsContainer(size, stackSize, randomItem);

            var item = this.itemFactory.CreateDefault();
            var randomIndex = this.random.Next(0, size);
            var result = inventory.AddAt(item, randomIndex, stackSize);

            Assert.That(result, Is.EqualTo(stackSize));
        }

        [Test]
        public void AddAt_AmountBiggerThanSlotSize_ReturnsAmount()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var inventory = this.inventoryFactory.EmptyContainer(size, stackSize);

            var item = this.itemFactory.CreateDefault();
            var randomIndex = this.random.Next(0, size);
            var amount = this.random.Next(1, 10) + stackSize;
            var notAddedCount = inventory.AddAt(item, randomIndex, amount);

            Assert.That(notAddedCount, Is.EqualTo(amount));
        }
    }
}
