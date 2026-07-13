using NUnit.Framework.Internal;
using TheChest.Tests.Common.Attributes;

namespace TheChest.Inventories.Tests.Containers.Inventory
{
    public partial class InventoryTests<T>
    {
        [Test]
        [IgnoreIfValueType]
        public void CanAddItems_NullItem_ThrowsArgumentNullException()
        {
            var size = this.GenerateRandomSize();

            var inventory = this.inventoryFactory.EmptyContainer(size);
            Assert.That(
                () => inventory.CanAdd(items: default!), 
                Throws.ArgumentNullException.With.Property("ParamName").EqualTo("items")
            );
        }

        [Test]
        [IgnoreIfReferenceType]
        public void CanAddItems_ArrayContainingDefaultValue_AvailableInventory_ReturnsTrue()
        {
            var size = this.GenerateRandomSize();
            var inventory = this.inventoryFactory.EmptyContainer(size);

            var randomSize = this.random.Next(1, size - 1);
            var items = this.itemFactory.CreateMany(randomSize).ToList();
            items.Add(default!);
            var canAdd = inventory.CanAdd(items.ToArray());

            Assert.That(canAdd, Is.True);
        }

        [Test]
        [IgnoreIfReferenceType]
        public void CanAddItems_ArrayContainingDefaultValue_FullInventory_ReturnsFalse()
        {
            var size = this.GenerateRandomSize();
            var inventory = this.inventoryFactory.FullContainer(size, default!);

            var randomSize = this.random.Next(1, size - 1);
            var items = this.itemFactory.CreateMany(randomSize).ToList();
            items.Add(default!);
            var canAdd = inventory.CanAdd(items.ToArray());

            Assert.That(canAdd, Is.False);
        }

        [Test]
        [IgnoreIfValueType]
        public void CanAddItems_ArrayContainingNullItem_ThrowsArgumentNullException()
        {
            var size = this.GenerateRandomSize();
            var inventory = this.inventoryFactory.EmptyContainer(size);

            var randomSize = this.random.Next(1, size - 1);
            var items = this.itemFactory.CreateMany(randomSize).ToList();
            items.Add(default!);

            Assert.That(
                () => inventory.CanAdd(items.ToArray()), 
                Throws.ArgumentNullException.With.Property("ParamName").EqualTo("items")
            );
        }

        [Test]
        public void CanAddItems_ItemsAmountBiggerThanInventorySize_ReturnsFalse()
        {
            var size = this.GenerateRandomSize();
            var inventory = this.inventoryFactory.EmptyContainer(size);

            var items = this.itemFactory.CreateMany(size + 1);
            var canAdd = inventory.CanAdd(items);

            Assert.That(canAdd, Is.False);
        }

        [Test]
        public void CanAddItems_PartiallyFullInventory_ReturnsTrue()
        {
            var size = this.GenerateRandomSize();
            var item = this.itemFactory.CreateDefault();
            var inventory = this.inventoryFactory.FullContainer(size, item);

            var randomAmount = this.random.Next(1, size);
            inventory.Get(item, randomAmount);

            var items = this.itemFactory.CreateMany(randomAmount);
            var canAdd = inventory.CanAdd(items);

            Assert.That(canAdd, Is.True);
        }

        [Test]
        public void CanAddItems_InsufficientSpace_ReturnsFalse()
        {
            var size = this.GenerateRandomSize();
            var item = this.itemFactory.CreateDefault();
            var inventory = this.inventoryFactory.FullContainer(size, item);
            var randomAmount = this.random.Next(5, size);
            inventory.Get(item, randomAmount - 1);

            var items = this.itemFactory.CreateMany(randomAmount);
            var canAdd = inventory.CanAdd(items);

            Assert.That(canAdd, Is.False);
        }

        [Test]
        public void CanAddItems_FullInventory_ReturnsFalse()
        {
            var size = this.GenerateRandomSize();
            var item = this.itemFactory.CreateDefault();
            var inventory = this.inventoryFactory.FullContainer(size, item);

            var items = this.itemFactory.CreateMany(size);
            var canAdd = inventory.CanAdd(items);

            Assert.That(canAdd, Is.False);
        }

        [Test]
        public void CanAddItems_EmptyInventory_ReturnsTrue()
        {
            var size = this.GenerateRandomSize();
            var inventory = this.inventoryFactory.EmptyContainer(size);

            var items = this.itemFactory.CreateMany(10);
            var canAdd = inventory.CanAdd(items);

            Assert.That(canAdd, Is.True);
        }

        [Test]
        public void CanAddItems_EmptyItemsArray_ReturnsTrue()
        {
            var size = this.GenerateRandomSize();
            var inventory = this.inventoryFactory.EmptyContainer(size);
            var items = Array.Empty<T>();

            var canAdd = inventory.CanAdd(items);

            Assert.That(canAdd, Is.False);
        }
    }
}
