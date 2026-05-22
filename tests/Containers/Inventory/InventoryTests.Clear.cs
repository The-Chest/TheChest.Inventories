namespace TheChest.Inventories.Tests.Containers.Inventory
{
    public partial class InventoryTests<T>
    {
        [Test]
        public void Clear_EmptyInventory_DoesNotCallOnGetEvent()
        {
            var size = this.GenerateRandomSize();
            var inventory = this.inventoryFactory.EmptyContainer(size);
            
            inventory.OnGet += (sender, args) => Assert.Fail("OnGet should not be called if no item is found");
            
            inventory.Clear();
        }

        [Test]
        public void Clear_FullInventory_CallsOnGetEvent()
        {
            var size = this.GenerateRandomSize();
            var items = this.itemFactory.CreateManyRandom(size / 2)
                .Concat(this.itemFactory.CreateMany(size / 2))
                .ToArray();
            var inventory = this.inventoryFactory.ShuffledItemsContainer(size, items);

            var raised = false;
            inventory.OnGet += (sender, args) =>
            {
                Assert.That(args.Data, Has.Count.EqualTo(items.Length));
                Assert.That(args.Data.Select(x => x.Item), Is.EquivalentTo(items));
                raised = true;
            };
            inventory.Clear();

            Assert.That(raised, Is.True);
        }

        [Test]
        public void Clear_FullInventory_ReturnsEveryItemFromInventory()
        {
            var size = this.GenerateRandomSize();
            var items = this.itemFactory.CreateManyRandom(size / 2)
                .Concat(this.itemFactory.CreateMany(size / 2))
                .ToArray();
            var inventory = this.inventoryFactory.ShuffledItemsContainer(size, items);

            var result = inventory.Clear();

            Assert.That(result, Is.EquivalentTo(items));
        }

        [Test]
        public void Clear_EmptyInventory_ReturnsEmptyArray()
        {
            var size = this.GenerateRandomSize();
            var inventory = this.inventoryFactory.EmptyContainer(size);

            var result = inventory.Clear();

            Assert.That(result, Is.Empty);
        }
    }
}
