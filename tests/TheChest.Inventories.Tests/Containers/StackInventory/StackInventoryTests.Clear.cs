namespace TheChest.Inventories.Tests.Containers.StackInventory
{
    public partial class StackInventoryTests<T>
    {
        [Test]
        public void Clear_EmptyInventory_DoesNotCallOnGetEvent()
        {
            var inventory = this.inventoryFactory.EmptyContainer();

            inventory.OnGet += (sender, args) => Assert.Fail("OnGet event should not be called for empty inventory");

            inventory.Clear();
        }

        [Test]
        public void Clear_InventoryWithItems_RemoveItemsFromInventory()
        {
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var items = this.itemFactory.CreateManyRandom(size * stackSize);
            var inventory = this.inventoryFactory.ShuffledItemsContainer(size, stackSize, items);

            inventory.Clear();

            Assert.That(inventory.IsEmpty, Is.True);
        }

        [Test]
        public void Clear_InventoryWithItems_CallsOnGetEvent()
        {
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
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
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var item = this.itemFactory.CreateRandom();
            var inventory = this.inventoryFactory.FullContainer(size, stackSize, item);

            inventory.Clear();

            Assert.That(inventory.IsEmpty, Is.True);
        }
    }
}
