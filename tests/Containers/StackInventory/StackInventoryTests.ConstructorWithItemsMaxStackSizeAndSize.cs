using System.Linq;
using NUnit.Framework;
using System;
using TheChest.Tests.Common.Attributes;
using TheChest.Tests.Common.Extensions.Containers;
using TheChest.Tests.Common.Extensions.Slots;

namespace TheChest.Inventories.Tests.Containers.StackInventory
{
    public partial class StackInventoryTests<T>
    {
        [Test]
        [IgnoreIfValueType]
        public void ConstructorWithItemsMaxStackSizeAndSize_NullItems_ThrowsArgumentNullException()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();

            Assert.That(
                () => this.inventoryFactory.WithItems(null!, stackSize, size),
                Throws.ArgumentNullException.With.Property("ParamName").EqualTo("items")
            );
        }

        [Test]
        public void ConstructorWithItemsMaxStackSizeAndSize_NonPositiveMaxStackSize_ThrowsArgumentOutOfRangeException()
        {
            var (size, _) = this.GenerateRandomSizeAndStackSize();

            Assert.That(
                () => this.inventoryFactory.WithItems(Array.Empty<T>(), 0, size),
                Throws.TypeOf<ArgumentOutOfRangeException>().With.Property("ParamName").EqualTo("maxStackSize")
            );
        }

        [Test]
        public void ConstructorWithItemsMaxStackSizeAndSize_NonPositiveSize_ThrowsArgumentOutOfRangeException()
        {
            var (_, stackSize) = this.GenerateRandomSizeAndStackSize();

            Assert.That(
                () => this.inventoryFactory.WithItems(Array.Empty<T>(), stackSize, 0),
                Throws.TypeOf<ArgumentOutOfRangeException>().With.Property("ParamName").EqualTo("size")
            );
        }

        [Test]
        [IgnoreIfValueType]
        public void ConstructorWithItemsMaxStackSizeAndSize_ItemsContainingNull_ThrowsArgumentNullException()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var items = new[] { default(T)! };

            Assert.That(
                () => this.inventoryFactory.WithItems(items, stackSize, size),
                Throws.ArgumentNullException.With.Property("ParamName").EqualTo("items")
            );
        }

        [Test]
        public void ConstructorWithItemsMaxStackSizeAndSize_ItemStacksExceedSize_ThrowsArgumentException()
        {
            var (_, stackSize) = this.GenerateRandomSizeAndStackSize();
            var (firstItem, secondItem) = this.itemFactory.CreateRandomDistinctPair();
            var items = new[] { firstItem, secondItem };

            Assert.That(
                () => this.inventoryFactory.WithItems(items, stackSize, 1),
                Throws.ArgumentException.With.Property("ParamName").EqualTo("size")
            );
        }

        [Test]
        public void ConstructorWithItemsMaxStackSizeAndSize_ValidArguments_CreatesRequestedNumberOfSlots()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var items = this.itemFactory.CreateManyRandom(stackSize + 1);

            var inventory = this.inventoryFactory.WithItems(items, stackSize, size);

            Assert.That(inventory.Size, Is.EqualTo(size));
        }

        [Test]
        public void ConstructorWithItemsMaxStackSizeAndSize_ValidArguments_PreservesItems()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var items = this.itemFactory.CreateManyRandom(stackSize + 1);

            var inventory = this.inventoryFactory.WithItems(items, stackSize, size);

            Assert.That(inventory.GetSlots().Sum(slot => slot.GetContents()?.Length ?? 0), Is.EqualTo(items.Length));
        }

        [Test]
        public void ConstructorWithItemsMaxStackSizeAndSize_ValidArguments_AppliesMaxStackSizeToEverySlot()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var items = this.itemFactory.CreateManyRandom(stackSize + 1);

            var inventory = this.inventoryFactory.WithItems(items, stackSize, size);

            Assert.That(inventory.GetSlots(), Has.All.Property("MaxAmount").EqualTo(stackSize));
        }
    }
}
