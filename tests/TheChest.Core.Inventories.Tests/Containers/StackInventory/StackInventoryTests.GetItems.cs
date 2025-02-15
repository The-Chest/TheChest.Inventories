namespace TheChest.Core.Inventories.Tests.Containers
{
    public partial class StackInventoryTests<T>
    {
        [Test]
        public void GetItems_WhenAmountIsZeroOrSmaller_ThrowsArgumentOutOfRangeException()
        {
            var inventory = this.containerFactory.EmptyContainer();
            var item = this.itemFactory.CreateDefault();

            Assert.That(() => inventory.Get(item, 0), Throws.TypeOf(typeof(ArgumentOutOfRangeException)));
        }

        [Test]
        public void GetItems_InvalidItem_ThrowsArgumentNullException()
        {
            var inventory = this.containerFactory.EmptyContainer();

            Assert.That(() => inventory.Get(default(T)!, 1), Throws.ArgumentNullException);
        }

        [Test]
        public void GetItems_ItemNotFound_ReturnsEmptyArray()
        {
            var item = this.itemFactory.CreateDefault();
            var inventory = this.containerFactory.EmptyContainer();

            var items = inventory.Get(item, 1);

            Assert.That(items, Is.Empty);
        }

        [Test]
        public void GetItems_ItemsFound_ReturnsItems()
        {
            var amount = 10;
            var stackSize = this.random.Next(1, 20);
            var items = this.itemFactory.CreateManyRandom(amount);
            var inventoryItems = this.itemFactory.CreateManyRandom(10).ToList();
            inventoryItems.AddRange(items);

            var inventory = this.containerFactory.ShuffledItemsContainer(20, stackSize, inventoryItems.ToArray());

            var expectedItem = items[0];
            var result = inventory.Get(expectedItem, amount);

            Assert.That(result, Has.Length.EqualTo(amount));
            Assert.That(result, Has.All.EqualTo(expectedItem));
        }

        [Test]
        public void GetItems_AmoutBiggerThanItemsInInventory_ReturnsAllItemsFound()
        {
            var amount = 10;
            var stackSize = this.random.Next(1, 20);
            var items = this.itemFactory.CreateManyRandom(amount);
            var inventoryItems = this.itemFactory.CreateManyRandom(10).ToList();
            inventoryItems.AddRange(items);

            var inventory = this.containerFactory.ShuffledItemsContainer(20, stackSize, inventoryItems.ToArray());

            var expectedItem = items[0];
            var result = inventory.Get(expectedItem, 20);

            Assert.That(result, Has.Length.EqualTo(amount));
            Assert.That(result, Has.All.EqualTo(expectedItem));
        }

        [Test]
        public void GetItems_ItemsFound_RemovesItems()
        {
            var amount = 10;
            var stackSize = this.random.Next(1, 20);
            var items = this.itemFactory.CreateManyRandom(amount);
            var inventoryItems = this.itemFactory.CreateManyRandom(10).ToList();
            inventoryItems.AddRange(items);
            var inventory = this.containerFactory.ShuffledItemsContainer(20, stackSize, inventoryItems.ToArray());

            inventory.Get(items[0], amount);

            Assert.That(inventory.Slots.Any(x => x.Content?.Contains(items[0]) ?? false), Is.False);
        }
    }
}
