using TheChest.Inventories.Slots.Interfaces;
using TheChest.Tests.Common.Extensions.Containers;
using TheChest.Tests.Common.Extensions.Slots;
using TheChest.Tests.Common.Extensions;

namespace TheChest.Inventories.Tests.Containers.Inventory
{
    public partial class InventoryTests<T>
    {
        [Test]
        public void TryAddItems_EmptyItemsArray_ThrowsArgumentException()
        {
            var inventory = this.inventoryFactory.EmptyContainer(this.GenerateRandomSize());

            Assert.That(
                () => inventory.TryAdd(Array.Empty<T>()),
                Throws.ArgumentException.With.Property("ParamName").EqualTo("items")
            );
        }

        [Test]
        public void TryAddItems_ArrayContainingNullItems_ThrowsArgumentNullException()
        {
            var size = this.GenerateRandomSize();
            var inventory = this.inventoryFactory.EmptyContainer(size);
            var items = this.itemFactory
                .CreateManyRandom(this.random.Next(2, size))
                .Append(default!)
                .ToArray();
            items.Shuffle();

            Assert.That(
                () => inventory.TryAdd(items),
                Throws.ArgumentNullException.With.Property("ParamName").EqualTo("items")
            );
        }

        [Test]
        public void TryAddItems_ItemsBiggerThanInventorySize_ReturnsFalse()
        {
            var size = this.GenerateRandomSize();
            var inventory = this.inventoryFactory.EmptyContainer(size);
            var items = this.itemFactory.CreateMany(size + 1);

            var result = inventory.TryAdd(items);

            Assert.That(result, Is.False);
        }

        [Test]
        public void TryAddItems_NotEnoughAvailableSlots_CallsOnAddEventForAddedItems()
        {
            var size = this.GenerateRandomSize();
            var items = this.itemFactory.CreateMany(size - 1);
            var inventory = this.inventoryFactory.ShuffledItemsContainer(size, items);
            var addItems = this.itemFactory.CreateManyRandom(2);

            var called = false;
            inventory.OnAdd += (_, _) => called = true;

            inventory.TryAdd(addItems);

            Assert.That(called, Is.True);
        }

        [Test]
        public void TryAddItems_NotEnoughAvailableSlots_AddsOnlyUntilNoSlotsAvailable()
        {
            var size = this.GenerateRandomSize();
            var existingItems = this.itemFactory.CreateMany(size - 1);
            var inventory = this.inventoryFactory.ShuffledItemsContainer(size, existingItems);
            var addItems = this.itemFactory.CreateManyRandom(2);
            var slotsBefore = inventory.GetSlots().Select(slot => slot.GetContent()).ToArray();

            inventory.TryAdd(addItems);

            var filledAfter = inventory.GetSlots().Count(slot => slot.IsFull);

            Assert.That(filledAfter, Is.EqualTo(slotsBefore.Count(x => x is not null) + 1));
        }

        [Test]
        public void TryAddItems_EnoughAvailableSlots_AddsAllItems()
        {
            var size = this.GenerateRandomSize();
            var inventory = this.inventoryFactory.EmptyContainer(size);
            var addSize = this.random.Next(1, size);
            var items = this.itemFactory.CreateMany(addSize);

            inventory.TryAdd(items);

            Assert.That(
                inventory.GetSlots().Take(addSize).Select(slot => slot.GetContent()),
                Is.EqualTo(items)
            );
        }

        [Test]
        public void TryAddItems_EnoughAvailableSlots_ReturnsTrue()
        {
            var size = this.GenerateRandomSize();
            var inventory = this.inventoryFactory.EmptyContainer(size);

            var result = inventory.TryAdd(this.itemFactory.CreateMany(1));

            Assert.That(result, Is.True);
        }

        [Test]
        public void TryAddItems_EnoughAvailableSlots_CallsOnAddEvent()
        {
            var size = this.GenerateRandomSize();
            var inventory = this.inventoryFactory.EmptyContainer(size);
            var addSize = this.random.Next(1, size);
            var items = this.itemFactory.CreateMany(addSize);

            var called = false;
            inventory.OnAdd += (sender, args) =>
            {
                Assert.That(sender, Is.EqualTo(inventory));
                called = args.Data.Select(x => x.Item).SequenceEqual(items);
            };

            inventory.TryAdd(items);

            Assert.That(called, Is.True);
        }
    }
}
