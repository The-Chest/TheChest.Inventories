using TheChest.Inventories.Tests.Common.Extensions.Containers;

namespace TheChest.Inventories.Tests.Containers
{
    public partial class InventoryTests<T>
    {
        [Test]
        public void GetItem_NullItem_ThrowsArgumentNullException()
        {
            var inventory = this.inventoryFactory.EmptyContainer();
            Assert.That(() => inventory.Get(item: default!), Throws.ArgumentNullException);
        }

        [Test]
        public void GetItem_EmptyInventory_DoesNotCallOnGetEvent()
        {
            var inventory = this.inventoryFactory.EmptyContainer();

            inventory.OnGet += (sender, args) => Assert.Fail("Get(T item) should not be called if no item is found");

            inventory.Get(this.itemFactory.CreateRandom());
        }

        [Test]
        public void GetItem_ExistingItems_RemovesTheFirstFoundItemFromSlot()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var item = this.itemFactory.CreateDefault();
            var inventory = this.inventoryFactory.FullContainer(size, item);

            inventory.Get(item);

            Assert.That(inventory.GetItem<T>(0), Is.Null);
        }

        [Test]
        public void GetItem_ExistingItems_CallsOnGetEvent()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var items = this.itemFactory.CreateMany(size / 2);
            var sameItems = this.itemFactory.CreateManyRandom(size / 2);
            var inventory = this.inventoryFactory.ShuffledItemsContainer(size, items.Concat(sameItems).ToArray());

            var raised = false;
            var randomItem = sameItems[0];
            inventory.OnGet += (sender, args) =>
            {
                Assert.That(args.Data, Has.Count.EqualTo(1));
                Assert.That(args.Data.Select(x => x.Item), Has.All.EqualTo(randomItem));
                raised = true;
            };

            inventory.Get(sameItems[0]);

            Assert.That(raised, Is.True, "OnGet event was not raised");
        }
    }
}
