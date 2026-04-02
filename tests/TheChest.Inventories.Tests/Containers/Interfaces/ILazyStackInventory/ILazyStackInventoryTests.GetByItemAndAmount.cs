namespace TheChest.Inventories.Tests.Containers.Interfaces
{
    public partial class ILazyStackInventoryTests<T>
    {
        [Test]
        public void Get_ByItemAndAmount_ExistingItemsAndAmountBiggerThanInventorySize_ReturnsMaxAvailableAmount()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var inventory = this.inventoryFactory.EmptyContainer(size, stackSize);

            var expectedAmount = size * stackSize;

            var item = this.itemFactory.CreateDefault();
            var amount = size * stackSize * 2;
            var result = inventory.Get(item, amount);

            Assert.That(result, Has.Length.EqualTo(expectedAmount).And.All.EqualTo(item));
        }

        [Test]
        public void Get_ByItemAndAmount_ExistingItems_ReturnsCorrectAmountFromMultipleSlots()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var item = this.itemFactory.CreateDefault();
            var inventory = this.inventoryFactory.FullContainer(size, stackSize, item);

            var amount = this.random.Next(2, size);
            var result = inventory.Get(item, amount);

            Assert.That(result, Has.Length.EqualTo(amount).And.All.EqualTo(item));
        }

        [Test]
        public void Get_ByItemAndAmount_NotFoundItem_ReturnsEmptyArray()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var randomItem = this.itemFactory.CreateRandom();
            var inventory = this.inventoryFactory.FullContainer(size, stackSize, randomItem);

            var amount = this.random.Next(2, size);
            var item = this.itemFactory.CreateDefault();
            var result = inventory.Get(item, amount);

            Assert.That(result, Is.Empty);
        }
    }
}
