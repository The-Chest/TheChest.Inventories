using TheChest.Tests.Common.Attributes;
using TheChest.Tests.Common.Extensions.Containers;

namespace TheChest.Inventories.Tests.Containers.Inventory
{
    public partial class InventoryTests<T>
    {
        [Test]
        [IgnoreIfValueType]
        public void GetAll_NullItem_ThrowsArgumentNullException()
        {
            var size = this.GenerateRandomSize();
            var inventory = this.inventoryFactory.EmptyContainer(size);

            Assert.That(
                () => inventory.GetAll(item: default!), 
                Throws.ArgumentNullException.With.Property("ParamName").EqualTo("item")
            );
        }

        [Test]
        [IgnoreIfReferenceType]
        public void GetAll_DefaultValue_EmptyInventory_ReturnsEmptyArray()
        {
            var size = this.GenerateRandomSize();
            var inventory = this.inventoryFactory.EmptyContainer(size);

            var result = inventory.GetAll(default!);

            Assert.That(result, Is.Empty);
        }

        [Test]
        [IgnoreIfReferenceType]
        public void GetAll_DefaultValue_FullInventory_ReturnsArrayWithItems()
        {
            var size = this.GenerateRandomSize();
            var inventory = this.inventoryFactory.FullContainer(size, default!);

            var result = inventory.GetAll(default!);

            Assert.That(result, Has.Length.EqualTo(size));
        }

        [Test]
        public void GetAll_EmptyInventory_DoesNotCallOnGetEvent()
        {
            var size = this.GenerateRandomSize();
            var inventory = this.inventoryFactory.EmptyContainer(size);

            inventory.OnGet += (sender, args) => Assert.Fail("OnGet should not be called if no item is found");

            inventory.GetAll(this.itemFactory.CreateRandom());
        }

        [Test]
        public void GetAll_WithItems_RemovesFromFoundSlots()
        {
            var size = this.GenerateRandomSize();
            var items = this.itemFactory.CreateMany(size);
            var inventory = this.inventoryFactory.ShuffledItemsContainer(size, items);

            var index = this.random.Next(0, size);
            var randomItem = items[index];
            inventory.GetAll(randomItem);

            Assert.That(inventory.GetSlot(index).IsEmpty, Is.True);
        }

        [Test]
        public void GetAll_NotFoundItem_DoesNotRemoveFromAnySlots()
        {
            var size = this.GenerateRandomSize();
            var items = this.itemFactory.CreateMany(size);
            var inventory = this.inventoryFactory.ShuffledItemsContainer(size, items);
            var slots = inventory.GetSlots()?.ToArray();

            var randomItem = this.itemFactory.CreateRandom();
            inventory.GetAll(randomItem);

            Assert.That(inventory.GetSlots(), Is.EqualTo(slots));
        }

        [Test]
        public void GetAll_WithItems_CallsOnGetEvent()
        {
            var size = this.GenerateRandomSize();
            var items = this.itemFactory.CreateMany(size / 2);
            var sameItems = this.itemFactory.CreateManyRandom(size / 2);
            var inventory = this.inventoryFactory.ShuffledItemsContainer(size, items.Concat(sameItems).ToArray());

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

        [Test]
        public void GetAll_NotFoundItem_ReturnsEmptyArray()
        {
            var size = this.GenerateRandomSize();
            var items = this.itemFactory.CreateMany(size);
            var inventory = this.inventoryFactory.ShuffledItemsContainer(size, items);

            var randomItem = this.itemFactory.CreateRandom();
            var result = inventory.GetAll(randomItem);

            Assert.That(result, Is.Empty);
        }

        [Test]
        public void GetAll_FoundItems_ReturnsSearchingItem()
        {
            var size = this.GenerateRandomSize();
            var items = this.itemFactory.CreateMany(size / 2);
            var sameItems = this.itemFactory.CreateManyRandom(size / 2);
            var inventory = this.inventoryFactory.ShuffledItemsContainer(size, items.Concat(sameItems).ToArray());

            var randomItem = sameItems[0];
            var result = inventory.GetAll(randomItem);

            Assert.That(result, Is.EqualTo(sameItems));
        }
    }
}
