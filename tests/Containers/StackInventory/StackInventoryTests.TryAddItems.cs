using System.Linq;
using TheChest.Core.Slots.Interfaces;
using TheChest.Tests.Common.Extensions.Containers;
using TheChest.Tests.Common.Extensions.Slots;

namespace TheChest.Inventories.Tests.Containers.StackInventory
{
    public partial class StackInventoryTests<T>
    {
        [Test]
        public void TryAddItems_NullItems_ThrowsArgumentNullException()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var inventory = this.inventoryFactory.EmptyContainer(size, stackSize);

            Assert.That(
                () => inventory.TryAdd(null!),
                Throws.ArgumentNullException.With.Property("ParamName").EqualTo("items")
            );
        }

        [Test]
        public void TryAddItems_EmptyArray_ReturnsTrue()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var inventory = this.inventoryFactory.EmptyContainer(size, stackSize);

            var result = inventory.TryAdd(Array.Empty<T>());

            Assert.That(result, Is.True);
        }

        [Test]
        public void TryAddItems_ItemsContainsNull_ThrowsArgumentNullException()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var inventory = this.inventoryFactory.EmptyContainer(size, stackSize);

            var validItem = this.itemFactory.CreateDefault();
            var items = new T[] { validItem, default!, validItem };

            Assert.That(
                () => inventory.TryAdd(items),
                Throws.ArgumentNullException
                    .With.Property("ParamName").EqualTo("items").And
                    .Message.Contains("One of the items to add is null")
            );
        }

        [Test]
        public void TryAddItems_ItemsAreNotAllEqual_ThrowsArgumentException()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var inventory = this.inventoryFactory.EmptyContainer(size, stackSize);

            var items = new[] { this.itemFactory.CreateDefault(), this.itemFactory.CreateRandom() };

            Assert.That(
                () => inventory.TryAdd(items),
                Throws.ArgumentException
                    .With.Property("ParamName").EqualTo("items").And
                    .Message.Contains("Cannot add an array of items with different types")
            );
        }

        [Test]
        public void TryAddItems_NotEnoughSpace_ReturnsFalse()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var item = this.itemFactory.CreateDefault();
            var inventory = this.inventoryFactory.EmptyContainer(size, stackSize);

            var items = Enumerable.Repeat(item, size * stackSize + 1).ToArray();
            var result = inventory.TryAdd(items);

            Assert.That(result, Is.False);
        }

        [Test]
        public void TryAddItems_NotEnoughSpace_DoesNotAddItems()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var item = this.itemFactory.CreateDefault();
            var inventory = this.inventoryFactory.EmptyContainer(size, stackSize);

            var items = Enumerable.Repeat(item, size * stackSize + 1).ToArray();
            inventory.TryAdd(items);

            Assert.That(inventory.GetSlots(), Has.All.Matches<IStackSlot<T>>(x => x.IsEmpty));
        }

        [Test]
        public void TryAddItems_NotEnoughSpace_DoesNotCallOnAddEvent()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var item = this.itemFactory.CreateDefault();
            var inventory = this.inventoryFactory.EmptyContainer(size, stackSize);

            var items = Enumerable.Repeat(item, size * stackSize + 1).ToArray();
            inventory.OnAdd += (_, _) => Assert.Fail("OnAdd event should not be called when TryAdd returns false");

            inventory.TryAdd(items);
        }

        [Test]
        public void TryAddItems_ItemsFit_ReturnsTrue()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var inventory = this.inventoryFactory.EmptyContainer(size, stackSize);

            var items = this.itemFactory.CreateMany(stackSize);
            var result = inventory.TryAdd(items);

            Assert.That(result, Is.True);
        }
    }
}
