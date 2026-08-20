using TheChest.Tests.Common.Extensions.Containers;

using TheChest.Tests.Common.Attributes;

namespace TheChest.Inventories.Tests.Containers.StackInventory
{
    public partial class StackInventoryTests<T>
    {
        [Test]
        [IgnoreIfValueType]
        public void TryReplace_NullItems_ThrowsArgumentNullException()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var inventory = this.inventoryFactory.EmptyContainer(size, stackSize);

            Assert.That(
                () => inventory.TryReplace(null!, 0, out _),
                Throws.ArgumentNullException.With.Property("ParamName").EqualTo("items")
            );
        }

        [TestCase(-1)]
        [TestCase(MAX_SIZE_TEST)]
        public void TryReplace_InvalidIndex_ThrowsArgumentOutOfRangeException(int index)
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var items = this.itemFactory.CreateMany(stackSize);
            var inventory = this.inventoryFactory.EmptyContainer(size, stackSize);

            Assert.That(
                () => inventory.TryReplace(items, index, out _),
                Throws.TypeOf<ArgumentOutOfRangeException>().With.Property("ParamName").EqualTo("index")
            );
        }

        [Test]
        public void TryReplace_EmptyItems_ReturnsFalse()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var inventory = this.inventoryFactory.EmptyContainer(size, stackSize);
            var index = this.random.Next(0, size);

            var result = inventory.TryReplace(Array.Empty<T>(), index, out _);

            Assert.That(result, Is.False);
        }

        [Test]
        [IgnoreIfValueType]
        public void TryReplace_EmptyItems_SetsOldItemsToDefault()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var inventory = this.inventoryFactory.EmptyContainer(size, stackSize);
            var index = this.random.Next(0, size);

            inventory.TryReplace(Array.Empty<T>(), index, out var oldItems);

            Assert.That(oldItems, Is.Null);
        }

        [Test]
        public void TryReplace_EmptyItems_DoesntCallOnReplaceEvent()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var inventory = this.inventoryFactory.EmptyContainer(size, stackSize);
            var index = this.random.Next(0, size);
            var onReplaceCalled = false;

            inventory.OnReplace += (_, _) => onReplaceCalled = true;
            inventory.TryReplace(Array.Empty<T>(), index, out _);

            Assert.That(onReplaceCalled, Is.False);
        }

        [Test]
        [IgnoreIfValueType]
        public void TryReplace_ItemsContainingNull_ReturnsFalse()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var inventory = this.inventoryFactory.EmptyContainer(size, stackSize);
            var index = this.random.Next(0, size);
            var items = this.itemFactory.CreateMany(stackSize - 1).Append(default).ToArray();

            var result = inventory.TryReplace(items!, index, out _);

            Assert.That(result, Is.False);
        }

        [Test]
        [IgnoreIfValueType]
        public void TryReplace_ItemsContainingNull_DoesntReplaceItemsInSlot()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var initialItem = this.itemFactory.CreateDefault();
            var inventory = this.inventoryFactory.FullContainer(size, stackSize, initialItem);
            var index = this.random.Next(0, size);
            var items = this.itemFactory.CreateMany(stackSize - 1).Append(default).ToArray();

            inventory.TryReplace(items!, index, out _);

            Assert.That(inventory.GetItems(index), Has.Length.EqualTo(stackSize).And.All.EqualTo(initialItem));
        }

        [Test]
        [IgnoreIfValueType]
        public void TryReplace_ItemsContainingNull_DoesntCallOnReplaceEvent()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var inventory = this.inventoryFactory.EmptyContainer(size, stackSize);
            var index = this.random.Next(0, size);
            var items = this.itemFactory.CreateMany(stackSize - 1).Append(default).ToArray();
            var onReplaceCalled = false;

            inventory.OnReplace += (_, _) => onReplaceCalled = true;
            inventory.TryReplace(items!, index, out _);

            Assert.That(onReplaceCalled, Is.False);
        }

        [Test]
        public void TryReplace_ItemsExceedStackSize_ReturnsFalse()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var inventory = this.inventoryFactory.EmptyContainer(size, stackSize);
            var index = this.random.Next(0, size);
            var items = this.itemFactory.CreateMany(stackSize + 1);

            var result = inventory.TryReplace(items, index, out _);

            Assert.That(result, Is.False);
        }

        [Test]
        public void TryReplace_ItemsExceedStackSize_DoesntReplaceItemsInSlot()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var initialItem = this.itemFactory.CreateDefault();
            var inventory = this.inventoryFactory.FullContainer(size, stackSize, initialItem);
            var index = this.random.Next(0, size);
            var items = this.itemFactory.CreateMany(stackSize + 1);

            inventory.TryReplace(items, index, out _);

            Assert.That(inventory.GetItems(index), Has.Length.EqualTo(stackSize).And.All.EqualTo(initialItem));
        }

        [Test]
        public void TryReplace_ItemsExceedStackSize_DoesntCallOnReplaceEvent()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var inventory = this.inventoryFactory.EmptyContainer(size, stackSize);
            var index = this.random.Next(0, size);
            var items = this.itemFactory.CreateMany(stackSize + 1);
            var onReplaceCalled = false;

            inventory.OnReplace += (_, _) => onReplaceCalled = true;
            inventory.TryReplace(items, index, out _);

            Assert.That(onReplaceCalled, Is.False);
        }

        [Test]
        public void TryReplace_SlotWithItems_ReplacesItemsInSlot()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var initialItem = this.itemFactory.CreateDefault();
            var inventory = this.inventoryFactory.FullContainer(size, stackSize, initialItem);
            var index = this.random.Next(0, size);
            var newItems = this.itemFactory.CreateManyRandom(stackSize);

            inventory.TryReplace(newItems, index, out _);

            Assert.That(inventory.GetItems(index), Is.EqualTo(newItems));
        }

        [Test]
        public void TryReplace_SlotWithItems_SetsOldItemsToPreviousItems()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var initialItem = this.itemFactory.CreateDefault();
            var inventory = this.inventoryFactory.FullContainer(size, stackSize, initialItem);
            var index = this.random.Next(0, size);
            var newItems = this.itemFactory.CreateManyRandom(stackSize);

            inventory.TryReplace(newItems, index, out var oldItems);

            Assert.That(oldItems, Has.Length.EqualTo(stackSize).And.All.EqualTo(initialItem));
        }

        [Test]
        public void TryReplace_SlotWithItems_CallsOnReplaceEvent()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var initialItem = this.itemFactory.CreateDefault();
            var inventory = this.inventoryFactory.FullContainer(size, stackSize, initialItem);
            var index = this.random.Next(0, size);
            var newItems = this.itemFactory.CreateManyRandom(stackSize);
            var calledWithExpectedData = false;

            inventory.OnReplace += (sender, args) =>
            {
                var data = args.Data.Single();
                calledWithExpectedData =
                    sender == inventory &&
                    data.Index == index &&
                    data.OldItems.Length == stackSize &&
                    data.OldItems.All(item => item!.Equals(initialItem)) &&
                    data.NewItems.SequenceEqual(newItems);
            };
            inventory.TryReplace(newItems, index, out _);

            Assert.That(calledWithExpectedData, Is.True);
        }

        [Test]
        public void TryReplace_SlotWithItems_ReturnsTrue()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var initialItem = this.itemFactory.CreateDefault();
            var inventory = this.inventoryFactory.FullContainer(size, stackSize, initialItem);
            var index = this.random.Next(0, size);
            var newItems = this.itemFactory.CreateManyRandom(stackSize);

            var result = inventory.TryReplace(newItems, index, out _);

            Assert.That(result, Is.True);
        }

        [Test]
        public void TryReplace_EmptySlot_ReplacesItemsInSlot()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var inventory = this.inventoryFactory.EmptyContainer(size, stackSize);
            var index = this.random.Next(0, size);
            var newItems = this.itemFactory.CreateManyRandom(stackSize);

            inventory.TryReplace(newItems, index, out _);

            Assert.That(inventory.GetItems(index), Is.EqualTo(newItems));
        }

        [Test]
        public void TryReplace_EmptySlot_SetsOldItemsToEmptyItems()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var inventory = this.inventoryFactory.EmptyContainer(size, stackSize);
            var index = this.random.Next(0, size);
            var newItems = this.itemFactory.CreateManyRandom(stackSize);

            inventory.TryReplace(newItems, index, out var oldItems);

            Assert.That(oldItems, Is.Empty);
        }

        [Test]
        public void TryReplace_EmptySlot_CallsOnReplaceEvent()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var inventory = this.inventoryFactory.EmptyContainer(size, stackSize);
            var index = this.random.Next(0, size);
            var newItems = this.itemFactory.CreateManyRandom(stackSize);
            var calledWithExpectedData = false;

            inventory.OnReplace += (sender, args) =>
            {
                var data = args.Data.Single();
                calledWithExpectedData =
                    sender == inventory &&
                    data.Index == index &&
                    data.OldItems.Length == 0 &&
                    data.NewItems.SequenceEqual(newItems);
            };
            inventory.TryReplace(newItems, index, out _);

            Assert.That(calledWithExpectedData, Is.True);
        }

        [Test]
        public void TryReplace_EmptySlot_ReturnsTrue()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var inventory = this.inventoryFactory.EmptyContainer(size, stackSize);
            var index = this.random.Next(0, size);
            var newItems = this.itemFactory.CreateManyRandom(stackSize);

            var result = inventory.TryReplace(newItems, index, out _);

            Assert.That(result, Is.True);
        }

        [Test]
        [IgnoreIfReferenceType]
        public void TryReplace_ValueType_NullItems_ThrowsArgumentNullException()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var inventory = this.inventoryFactory.EmptyContainer(size, stackSize);

            Assert.That(() => inventory.TryReplace(null!, 0, out _), Throws.ArgumentNullException);
        }

        [Test]
        [IgnoreIfReferenceType]
        public void TryReplace_ValueType_EmptyItems_SetsOldItemsToDefault()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var inventory = this.inventoryFactory.EmptyContainer(size, stackSize);
            var index = this.random.Next(0, size);

            inventory.TryReplace(Array.Empty<T>(), index, out var oldItems);

            Assert.That(oldItems, Is.Null);
        }

        [Test]
        [IgnoreIfReferenceType]
        public void TryReplace_ValueType_ItemsContainingDefault_ReturnsTrue()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var inventory = this.inventoryFactory.EmptyContainer(size, stackSize);
            var items = new T[] { default!, default! };

            Assert.That(inventory.TryReplace(items, 0, out _), Is.True);
        }

        [Test]
        [IgnoreIfReferenceType]
        public void TryReplace_ValueType_ItemsContainingDefault_ReplacesItemsInSlot()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var inventory = this.inventoryFactory.EmptyContainer(size, stackSize);
            var items = new T[] { default!, default! };

            inventory.TryReplace(items, 0, out _);

            Assert.That(inventory.GetItems(0), Is.EqualTo(items));
        }

        [Test]
        [IgnoreIfReferenceType]
        public void TryReplace_ValueType_ItemsContainingDefault_CallsOnReplaceEvent()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var inventory = this.inventoryFactory.EmptyContainer(size, stackSize);
            var items = new T[] { default!, default! };
            var called = false;
            inventory.OnReplace += (_, _) => called = true;

            inventory.TryReplace(items, 0, out _);

            Assert.That(called, Is.True);
        }
    }
}
