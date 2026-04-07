using TheChest.Tests.Common.Extensions.Containers;

namespace TheChest.Inventories.Tests.Containers.LazyStackInventory
{
    public partial class LazyStackInventoryTests<T>
    {
        [Test]
        [TheChest.Tests.Common.Attributes.IgnoreIfValueTypeAttribute]
        public void GetAllByItem_NullItem_ThrowsArgumentNullException()
        {
            var inventory = this.inventoryFactory.EmptyContainer();

            Assert.Throws<ArgumentNullException>(() => inventory.GetAll(item: default!));
        }

        [Test]
        public void GetAllByItem_ExistingItem_CallsOnGetEvent()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var invalidRandomItems = this.itemFactory.CreateManyRandom(size - 5);
            var validItems = this.itemFactory.CreateMany(size - invalidRandomItems.Length);
            var items = invalidRandomItems.Concat(validItems).ToArray();
            var inventory = this.inventoryFactory.ShuffledItemsContainer(size, stackSize, items);

            var item = this.itemFactory.CreateDefault();
            var raised = false;
            inventory.OnGet += (sender, args) =>
            {
                Assert.That(args.Data, Has.Count.EqualTo(validItems.Length));
                Assert.Multiple(() =>
                {
                    Assert.That(args.Data.Select(x => x.Item), Has.All.EqualTo(item));
                    Assert.That(args.Data.Select(x => x.Amount), Has.All.InRange(1, stackSize));
                });
                raised = true;
            };

            inventory.GetAll(item);

            Assert.That(raised, Is.True, "OnGet event was not raised");
        }

        [Test]
        public void GetAllByItem_ExistingItem_RemovesAllMatchingItemsFromInventory()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var inventoryItem = this.itemFactory.CreateDefault();
            var inventoryRandomItem = this.itemFactory.CreateRandom();
            var inventory = this.inventoryFactory.ShuffledItemsContainer(size, stackSize, inventoryItem, inventoryRandomItem);

            var item = this.itemFactory.CreateDefault();
            inventory.GetAll(item);

            Assert.That(inventory.GetSlots(), Has.None.EqualTo(item));
        }

        [Test]
        public void GetAllByItem_DifferentItems_DoesNotCallOnGetEvent()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var invalidRandomItems = this.itemFactory.CreateManyRandom(size);
            var inventory = this.inventoryFactory.ShuffledItemsContainer(size, stackSize, invalidRandomItems);

            inventory.OnGet += (sender, args) => Assert.Fail("OnGet event should not be called for an empty slot.");
            
            var item = this.itemFactory.CreateDefault();
            inventory.GetAll(item);
        }
    }
}
