namespace TheChest.Inventories.Tests.Containers.Inventory
{
    public partial class InventoryTests<T>
    {
        [TestCase(0)]
        [TestCase(-1)]
        public void GetItems_InvalidAmount_ThrowsArgumentOutOfRangeException(int amount)
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var item = this.itemFactory.CreateDefault();
            var inventory = this.inventoryFactory.FullContainer(size, item);

            Assert.That(
                () => inventory.Get(item, amount),
                Throws.Exception.TypeOf<ArgumentOutOfRangeException>()
            );
        }

        [Test]
        public void GetItems_NullItem_ThrowsArgumentNullException()
        {
            var inventory = this.inventoryFactory.EmptyContainer();
            Assert.That(() => inventory.Get(item: default!, amount: 1), Throws.ArgumentNullException);
        }

        [Test]
        public void GetItems_EmptyInventory_DoesNotCallOnGetEvent()
        {
            var inventory = this.inventoryFactory.EmptyContainer();
            
            inventory.OnGet += (sender, args) => Assert.Fail("OnGet should not be called if no item is found");
            
            inventory.Get(this.itemFactory.CreateRandom(), 10);
        }

        [Test]
        public void GetItems_ExistingItemOnSlot_CallsOnGetEvent()
        {
            var inventorySize = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var expectedAmount = this.random.Next(1, inventorySize / 2);
            var items = this.itemFactory.CreateMany(expectedAmount);
            var inventory = this.inventoryFactory.ShuffledItemsContainer(inventorySize, items);

            var raised = false;
            inventory.OnGet += (sender, args) =>
            {
                Assert.That(args.Data, Has.Count.EqualTo(expectedAmount));
                Assert.That(args.Data.Select(x => x.Item), Is.EqualTo(items));
                raised = true;
            };
            inventory.Get(items[0], expectedAmount);

            Assert.That(raised, Is.True, "OnGet event was not raised");
        }
    }
}
