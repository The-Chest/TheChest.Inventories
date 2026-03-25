namespace TheChest.Inventories.Tests.Containers.Interfaces
{
    public partial class IInventoryTests<T>
    {
        [Test]
        public void GetItems_ValidAmountNotFoundItem_ReturnsEmptyArray()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var item = this.itemFactory.CreateDefault();
            var inventory = this.inventoryFactory.FullContainer(size, item);

            var amount = this.random.Next(1, size);
            var result = inventory.Get(this.itemFactory.CreateRandom(), amount);

            Assert.That(result, Has.Length.EqualTo(0));
        }

        [Test]
        public void GetItems_ValidAmountFullInventory_ReturnsItemArrayWithAmountSize()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var item = this.itemFactory.CreateDefault();
            var inventory = this.inventoryFactory.FullContainer(size, item);

            var amount = this.random.Next(1, size);
            var result = inventory.Get(item, amount);

            Assert.That(result, Has.Length.EqualTo(amount));
        }

        [Test]
        public void GetItems_ValidAmountWithItems_ReturnsItemArrayWithMaxAvailable()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var expectedAmount = this.random.Next(1, size / 2);
            var items = this.itemFactory.CreateMany(expectedAmount);
            var inventory = this.inventoryFactory.ShuffledItemsContainer(size, items);

            var amount = this.random.Next(expectedAmount, size);
            var result = inventory.Get(items[0], amount);

            Assert.That(result, Has.Length.EqualTo(expectedAmount));
        }
    }
}
