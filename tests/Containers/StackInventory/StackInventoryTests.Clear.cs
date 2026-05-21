namespace TheChest.Inventories.Tests.Containers.StackInventory
{
    public partial class StackInventoryTests<T>
    {
        [Test]
        public void Clear_EmptyInventory_DoesNotCallOnGetEvent()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var inventory = this.inventoryFactory.EmptyContainer(size, stackSize);

            inventory.OnGet += (sender, args) => Assert.Fail("OnGet event should not be called for empty inventory");

            inventory.Clear();
        }

        [Test]
        public void Clear_InventoryWithItems_RemoveItemsFromInventory()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var items = this.itemFactory.CreateManyRandom(size * stackSize);
            var inventory = this.inventoryFactory.ShuffledItemsContainer(size, stackSize, items);

            inventory.Clear();

            Assert.That(inventory.IsEmpty, Is.True);
        }

        [Test]
        public void Clear_InventoryWithItems_CallsOnGetEvent()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var item = this.itemFactory.CreateRandom();
            var inventory = this.inventoryFactory.FullContainer(size, stackSize, item);

            var raised = false;
            inventory.OnGet += (sender, args) =>
            {
                Assert.That(args.Data, Has.Count.EqualTo(size));
                Assert.That(args.Data.SelectMany(x => x.Items), Has.All.EqualTo(item));
                raised = true;
            };

            inventory.Clear();

            Assert.That(raised, Is.True);
        }

        [Test]
        public void Clear_FullInventory_RemoveItemsFromInventory()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var item = this.itemFactory.CreateRandom();
            var inventory = this.inventoryFactory.FullContainer(size, stackSize, item);

            inventory.Clear();

            Assert.That(inventory.IsEmpty, Is.True);
        }


        [Test]
        public void Clear_EmptyInventory_ReturnsEmptyArray()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var inventory = this.inventoryFactory.EmptyContainer(size, stackSize);

            var result = inventory.Clear();

            Assert.That(result, Is.Empty);
        }

        [Test]
        public void Clear_InventoryWithItems_ReturnsAllItems()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var items = this.itemFactory.CreateManyRandom(size * stackSize);
            var inventory = this.inventoryFactory.ShuffledItemsContainer(size, stackSize, items);

            var result = inventory.Clear();

            Assert.That(result, Is.EquivalentTo(items));
        }

        [Test]
        public void Clear_FullInventory_ReturnsAllItems()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var item = this.itemFactory.CreateRandom();
            var inventory = this.inventoryFactory.FullContainer(size, stackSize, item);

            var expectedArraySize = size * stackSize;
            var expectedArray = new T[expectedArraySize];
            Array.Fill(expectedArray, item);

            var result = inventory.Clear();

            Assert.That(result, Is.EquivalentTo(expectedArray));
        }
    }
}
