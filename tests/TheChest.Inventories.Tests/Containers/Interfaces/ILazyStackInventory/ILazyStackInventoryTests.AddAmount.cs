namespace TheChest.Inventories.Tests.Containers.Interfaces
{
    public partial class ILazyStackInventoryTests<T>
    {
        [Test]
        public void Add_WithAmount_SuccessfullyAddedItems_ReturnsZero()
        {
            var inventory = this.inventoryFactory.EmptyContainer();
            var item = this.itemFactory.CreateDefault();
            var amount = this.random.Next(1, 5);

            var result = inventory.Add(item, amount);

            Assert.That(result, Is.Zero);
        }

        [Test]
        public void Add_WithAmount_FullInventory_ReturnsRemainingAmount()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var randomItem = this.itemFactory.CreateRandom();
            var inventory = this.inventoryFactory.FullContainer(size, stackSize, randomItem);

            var item = this.itemFactory.CreateDefault();
            var amount = this.random.Next(1, 5);
            var result = inventory.Add(item, amount);

            Assert.That(result, Is.EqualTo(amount));
        }

        [Test]
        public void Add_WithAmount_NotAllItemsBeAdded_ReturnsRemainingAmount()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var randomItem = this.itemFactory.CreateManyRandom(size - 1);
            var inventory = this.inventoryFactory.ShuffledItemsContainer(size, stackSize, randomItem);

            var item = this.itemFactory.CreateDefault();
            var amount = stackSize + this.random.Next(1, 5);
            var result = inventory.Add(item, amount);

            Assert.That(result, Is.EqualTo(amount - stackSize));
        }
    }
}
