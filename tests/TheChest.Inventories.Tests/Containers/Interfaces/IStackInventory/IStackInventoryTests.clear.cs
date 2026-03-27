namespace TheChest.Inventories.Tests.Containers.Interfaces
{
    public partial class IStackInventoryTests<T>
    {
        [Test]
        public void Clear_EmptyInventory_ReturnsEmptyArray()
        {
            var inventory = this.inventoryFactory.EmptyContainer();

            var result = inventory.Clear();

            Assert.That(result, Is.Empty);
        }

        [Test]
        public void Clear_EmptyInventory_DoesNotCallOnGetEvent()
        {
            var inventory = this.inventoryFactory.EmptyContainer();
            inventory.OnGet += (sender, args) => Assert.Fail("OnGet event should not be called for empty inventory");
            inventory.Clear();
        }

        [Test]
        public void Clear_InventoryWithItems_ReturnsAllItems()
        {
            var inventorySize = 20;
            var stackSize = 10;
            var items = this.itemFactory.CreateManyRandom(inventorySize * stackSize);
            var inventory = this.inventoryFactory.ShuffledItemsContainer(inventorySize, stackSize, items);

            var result = inventory.Clear();

            Assert.That(result, Is.EquivalentTo(items));
        }

        [Test]
        public void Clear_InventoryWithItems_RemoveItemsFromInventory()
        {
            var inventorySize = 20;
            var stackSize = 10;
            var items = this.itemFactory.CreateManyRandom(inventorySize * stackSize);
            var inventory = this.inventoryFactory.ShuffledItemsContainer(inventorySize, stackSize, items);

            inventory.Clear();

            Assert.That(inventory.IsEmpty, Is.True);
        }

        [Test]
        public void Clear_InventoryWithItems_CallsOnGetEvent()
        {
            var inventorySize = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var stackSize = this.random.Next(10, 20);
            var item = this.itemFactory.CreateRandom();
            var inventory = this.inventoryFactory.FullContainer(inventorySize, stackSize, item);

            var raised = false;
            inventory.OnGet += (sender, args) =>
            {
                Assert.That(args.Data, Has.Count.EqualTo(inventorySize));
                Assert.That(args.Data.SelectMany(x => x.Items), Has.All.EqualTo(item));
                raised = true;
            };

            inventory.Clear();

            Assert.That(raised, Is.True);
        }

        [Test]
        public void Clear_FullInventory_ReturnsAllItems()
        {
            var inventorySize = 20;
            var stackSize = 10;
            var item = this.itemFactory.CreateRandom();
            var inventory = this.inventoryFactory.FullContainer(inventorySize, stackSize, item);

            var expectedArraySize = inventorySize * stackSize;
            var expectedArray = new T[expectedArraySize];
            Array.Fill(expectedArray, item);

            var result = inventory.Clear();

            Assert.That(result, Is.EquivalentTo(expectedArray));
        }

        [Test]
        public void Clear_FullInventory_RemoveItemsFromInventory()
        {
            var inventorySize = 20;
            var stackSize = 10;
            var item = this.itemFactory.CreateRandom();
            var inventory = this.inventoryFactory.FullContainer(inventorySize, stackSize, item);

            inventory.Clear();

            Assert.That(inventory.IsEmpty, Is.True);
        }
    }
}
