using TheChest.Tests.Common.Attributes;
using TheChest.Tests.Common.Extensions.Containers;

namespace TheChest.Inventories.Tests.Containers.StackInventory
{
    public partial class StackInventoryTests<T>
    {
        [Test]
        [IgnoreIfReferenceType]
        public void AddItemsAt_ValueType_NullItems_ThrowsArgumentNullException()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var inventory = this.inventoryFactory.EmptyContainer(size, stackSize);

            Assert.That(() => inventory.AddAt(null!, 0), Throws.ArgumentNullException);
        }

        [Test]
        [IgnoreIfReferenceType]
        public void AddItemsAt_ValueType_ItemsContainingDefault_AddsItems()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var inventory = this.inventoryFactory.EmptyContainer(size, stackSize);
            var items = new T[] { default!, default! };

            inventory.AddAt(items, 0);

            Assert.That(inventory.GetItems(0), Is.EqualTo(items));
        }

        [Test]
        [IgnoreIfReferenceType]
        public void GetAmount_ValueType_DefaultItem_ReturnsEmptyItems()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var inventory = this.inventoryFactory.EmptyContainer(size, stackSize);

            Assert.That(inventory.Get((T)default!, stackSize), Is.Empty);
        }

        [Test]
        [IgnoreIfReferenceType]
        public void TryAddItemsAt_ValueType_NullItems_ThrowsArgumentNullException()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var inventory = this.inventoryFactory.EmptyContainer(size, stackSize);

            Assert.That(() => inventory.TryAddAt(null!, 0), Throws.ArgumentNullException);
        }

        [Test]
        [IgnoreIfReferenceType]
        public void CanAddItem_ValueType_DefaultItem_ReturnsTrue()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var inventory = this.inventoryFactory.EmptyContainer(size, stackSize);

            Assert.That(inventory.CanAdd((T)default!), Is.True);
        }

        [Test]
        [IgnoreIfReferenceType]
        public void CanAddItems_ValueType_NullItems_ThrowsArgumentNullException()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var inventory = this.inventoryFactory.EmptyContainer(size, stackSize);

            Assert.That(() => inventory.CanAdd(null!), Throws.ArgumentNullException);
        }

        [Test]
        [IgnoreIfReferenceType]
        public void CanAddItems_ValueType_ItemsContainingDefault_ReturnsTrue()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var inventory = this.inventoryFactory.EmptyContainer(size, stackSize);
            var items = new T[] { default!, default! };

            Assert.That(inventory.CanAdd(items), Is.True);
        }

        [Test]
        [IgnoreIfReferenceType]
        public void GetCount_ValueType_DefaultItem_ReturnsZero()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var inventory = this.inventoryFactory.EmptyContainer(size, stackSize);

            Assert.That(inventory.GetCount(default!), Is.Zero);
        }

        [Test]
        [IgnoreIfReferenceType]
        public void TryAddItems_ValueType_NullItems_ThrowsArgumentNullException()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var inventory = this.inventoryFactory.EmptyContainer(size, stackSize);

            Assert.That(() => inventory.TryAdd(null!), Throws.ArgumentNullException);
        }

        [Test]
        [IgnoreIfReferenceType]
        public void TryAddItems_ValueType_ItemsContainingDefault_ReturnsTrue()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var inventory = this.inventoryFactory.EmptyContainer(size, stackSize);
            var items = new T[] { default!, default! };

            Assert.That(inventory.TryAdd(items), Is.True);
        }

        [Test]
        [IgnoreIfReferenceType]
        public void CanAddItemsAt_ValueType_NullItems_ThrowsArgumentNullException()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var inventory = this.inventoryFactory.EmptyContainer(size, stackSize);

            Assert.That(() => inventory.CanAddAt(null!, 0), Throws.ArgumentNullException);
        }

        [Test]
        [IgnoreIfReferenceType]
        public void CanAddItemsAt_ValueType_ItemsContainingDefault_ReturnsTrue()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var inventory = this.inventoryFactory.EmptyContainer(size, stackSize);
            var items = new T[] { default!, default! };

            Assert.That(inventory.CanAddAt(items, 0), Is.True);
        }

        [Test]
        [IgnoreIfReferenceType]
        public void AddItem_ValueType_DefaultItem_AddsItem()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var inventory = this.inventoryFactory.EmptyContainer(size, stackSize);

            inventory.Add((T)default!);

            Assert.That(inventory.GetCount(default!), Is.EqualTo(1));
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

        [Test]
        [IgnoreIfReferenceType]
        public void AddItemAt_ValueType_DefaultItem_ReturnsTrue()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var inventory = this.inventoryFactory.EmptyContainer(size, stackSize);

            Assert.That(inventory.AddAt((T)default!, 0), Is.True);
        }

        [Test]
        [IgnoreIfReferenceType]
        public void CanAddItemAt_ValueType_DefaultItem_ReturnsTrue()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var inventory = this.inventoryFactory.EmptyContainer(size, stackSize);

            Assert.That(inventory.CanAddAt((T)default!, 0), Is.True);
        }

        [Test]
        [IgnoreIfReferenceType]
        public void GetAllByItem_ValueType_DefaultItem_ReturnsEmptyItems()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var inventory = this.inventoryFactory.EmptyContainer(size, stackSize);

            Assert.That(inventory.GetAll((T)default!), Is.Empty);
        }

        [Test]
        [IgnoreIfReferenceType]
        public void GetItems_ValueType_DefaultItem_ReturnsEmptyItems()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var inventory = this.inventoryFactory.EmptyContainer(size, stackSize);

            Assert.That(inventory.Get((T)default!, 1), Is.Empty);
        }

        [Test]
        [IgnoreIfReferenceType]
        public void CanReplace_ValueType_NullItems_ThrowsArgumentNullException()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var inventory = this.inventoryFactory.EmptyContainer(size, stackSize);

            Assert.That(() => inventory.CanReplace(null!, 0), Throws.ArgumentNullException);
        }

        [Test]
        [IgnoreIfReferenceType]
        public void CanReplace_ValueType_ItemsContainingDefault_ReturnsTrue()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var inventory = this.inventoryFactory.EmptyContainer(size, stackSize);
            var items = new T[] { default!, default! };

            Assert.That(inventory.CanReplace(items, 0), Is.True);
        }

        [Test]
        [IgnoreIfReferenceType]
        public void GetItem_ValueType_DefaultItem_ThrowsInvalidOperationException()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var inventory = this.inventoryFactory.EmptyContainer(size, stackSize);

            Assert.That(() => inventory.Get((T)default!), Throws.InvalidOperationException);
        }

        [Test]
        [IgnoreIfReferenceType]
        public void Replace_ValueType_NullItems_ThrowsArgumentNullException()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var inventory = this.inventoryFactory.EmptyContainer(size, stackSize);

            Assert.That(() => inventory.Replace(null!, 0), Throws.ArgumentNullException);
        }

        [Test]
        [IgnoreIfReferenceType]
        public void AddItems_ValueType_NullItems_ThrowsArgumentNullException()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var inventory = this.inventoryFactory.EmptyContainer(size, stackSize);

            Assert.That(() => inventory.Add(null!), Throws.ArgumentNullException);
        }

        [Test]
        [IgnoreIfReferenceType]
        public void AddItems_ValueType_ItemsContainingDefault_AddsItems()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var inventory = this.inventoryFactory.EmptyContainer(size, stackSize);
            var items = new T[] { default!, default! };

            inventory.Add(items);

            Assert.That(inventory.GetCount(default!), Is.EqualTo(items.Length));
        }
    }
}
