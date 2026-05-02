using TheChest.Tests.Common.Extensions.Containers;
using TheChest.Tests.Common.Extensions.Slots;
using TheChest.Tests.Common.Extensions;

namespace TheChest.Inventories.Tests.Containers.Inventory
{
    public partial class InventoryTests<T>
    {
        [Test]
        public void AddItems_NoItems_ReturnsEmptyArray()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var inventory = this.inventoryFactory.EmptyContainer(size);

            var items = this.itemFactory.CreateMany(0);

            Assert.That(
                () => inventory.Add(items),
                Throws.ArgumentException.With.Property("ParamName").EqualTo("items")
            );
        }

        [Test]
        public void AddItems_EmptyItemsArray_ThrowsArgumentException()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var inventory = this.inventoryFactory.EmptyContainer(size);

            Assert.That(
                () => inventory.Add(Array.Empty<T>()),
                Throws.ArgumentException.With.Property("ParamName").EqualTo("items")
            );
        }

        [Test]
        public void AddItems_ArrayContainingNullItems_ThrowsArgumentNullException()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var inventory = this.inventoryFactory.EmptyContainer(size);

            var items = this.itemFactory
                .CreateManyRandom(this.random.Next(2, size))
                .Append(default!)
                .ToArray();
            items.Shuffle();

            Assert.That(
                () => inventory.Add(items),
                Throws.ArgumentNullException.With.Message.EqualTo("One of the items to add is null (Parameter 'items')")
            );
        }

        [Test]
        public void AddItems_ArrayWithOnlyNullItems_ThrowsArgumentNullException()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var inventory = this.inventoryFactory.EmptyContainer(size);

            Assert.That(
                () => inventory.Add(new T[10]), 
                Throws.ArgumentNullException.With.Message.EqualTo("One of the items to add is null (Parameter 'items')")
            );
        }

        [Test]
        public void AddItems_NotAvailabeSlotsForAllItems_ThrowsInvalidOperationException()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var emptySlotsSize = this.random.Next(1, size);
            var itemAmount = size - emptySlotsSize;
            var items = this.itemFactory.CreateMany(itemAmount);
            var inventory = this.inventoryFactory.ShuffledItemsContainer(size, items);

            var addSize = emptySlotsSize + 1;
            var manyAdded = this.itemFactory.CreateManyRandom(addSize);
            Assert.That(
                () => inventory.Add(manyAdded),
                Throws.InvalidOperationException.
                    With.Message.EqualTo("There are not enough free slots to add all the items.")
            );
        }

        [Test]
        public void AddItems_FullInventory_ThrowsInvalidOperationException()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var item = this.itemFactory.CreateDefault();
            var inventory = this.inventoryFactory.FullContainer(size, item);

            var items = this.itemFactory.CreateMany(size);
            Assert.That(
                () => inventory.Add(items), 
                Throws.InvalidOperationException.With.Message.EqualTo("There are not enough free slots to add all the items.")
            );
        }

        [Test]
        public void AddItems_EmptySlots_AddsAllItems()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var inventory = this.inventoryFactory.EmptyContainer(size);

            var randomSize = this.random.Next(1, size);
            var items = this.itemFactory.CreateMany(randomSize);
            inventory.Add(items);

            Assert.That(
                inventory.GetSlots()?.Take(randomSize).Select(x => x.GetContent()), 
                Is.EqualTo(items)
            );
        }

        [Test]
        public void AddItems_EmptySlots_CallsOnAddForAllItems()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var inventory = this.inventoryFactory.EmptyContainer(size);

            var randomSize = this.random.Next(1, size);
            var items = this.itemFactory.CreateMany(randomSize);
            inventory.OnAdd += (sender, args) =>
            {
                Assert.Multiple(() =>
                {
                    Assert.That(sender, Is.EqualTo(inventory));
                    Assert.That(args.Data.Select(x => x.Item), Is.EqualTo(items));
                    Assert.That(args.Data, Has.Count.EqualTo(randomSize));
                });
            };
            inventory.Add(items);
        }
    }
}
