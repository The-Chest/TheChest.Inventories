using TheChest.Tests.Common.Extensions;

namespace TheChest.Inventories.Tests.Containers
{
    public partial class InventoryTests<T>
    {
        [Test]
        public void GetAll_NullItem_ThrowsArgumentNullException()
        {
            var inventory = this.containerFactory.EmptyContainer();
            Assert.That(() => inventory.GetAll(item: default!), Throws.ArgumentNullException);
        }

        [Test]
        public void GetAll_EmptyInventory_DoesNotCallOnGetEvent()
        {
            var inventory = this.containerFactory.EmptyContainer();

            inventory.OnGet += (sender, args) => Assert.Fail("OnGet should not be called if no item is found");

            inventory.GetAll(this.itemFactory.CreateRandom());
        }

        [Test]
        public void GetAll_WithItems_RemovesFromFoundSlots()
        {
            var size = this.random.Next(10, 20);
            var items = this.itemFactory.CreateMany(size);
            var inventory = this.containerFactory.ShuffledItemsContainer(size, items);

            var index = this.random.Next(0, size);
            var randomItem = items[index];
            inventory.GetAll(randomItem);

            Assert.That(inventory.GetSlots<T>()![index].IsEmpty, Is.True);
        }

        [Test]
        public void GetAll_NotFoundItem_DoesNotRemoveFromAnySlots()
        {
            var size = this.random.Next(10, 20);
            var items = this.itemFactory.CreateMany(size);
            var inventory = this.containerFactory.ShuffledItemsContainer(size, items);
            var slots = inventory.GetSlots<T>()?.ToArray();

            var randomItem = this.itemFactory.CreateRandom();
            inventory.GetAll(randomItem);

            Assert.That(inventory.GetSlots<T>(), Is.EqualTo(slots));
        }

        [Test]
        public void GetAll_WithItems_CallsOnGetEvent()
        {
            var size = this.random.Next(10, 20);
            var items = this.itemFactory.CreateMany(size / 2);
            var sameItems = this.itemFactory.CreateManyRandom(size / 2);
            var inventory = this.containerFactory.ShuffledItemsContainer(size, items.Concat(sameItems).ToArray());

            var raised = false;
            inventory.OnGet += (sender, args) =>
            {
                Assert.That(args.Data, Has.Count.EqualTo(sameItems.Length));
                Assert.That(args.Data.Select(x => x.Item), Is.EqualTo(sameItems));
                raised = true;
            };
            inventory.GetAll(sameItems[0]);

            Assert.That(raised, Is.True, "OnGet event was not raised");
        }
    }
}
