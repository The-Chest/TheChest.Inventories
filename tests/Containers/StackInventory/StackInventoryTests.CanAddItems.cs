using TheChest.Inventories.Tests.Containers.Extensions;
using TheChest.Tests.Common.Extensions;

using TheChest.Tests.Common.Attributes;

namespace TheChest.Inventories.Tests.Containers.StackInventory
{
    public partial class StackInventoryTests<T>
    {
        [Test]
        [IgnoreIfValueType]
        public void CanAddItems_NullItems_ThrowsArgumentNullException()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var inventory = this.inventoryFactory.EmptyContainer(size, stackSize);

            Assert.That(
                () => inventory.CanAdd(items: default!),
                Throws.ArgumentNullException.With.Property("ParamName").EqualTo("items")
            );
        }

        [Test]
        [IgnoreIfValueType]
        public void CanAddItems_ItemsContainingNull_ThrowsArgumentNullException()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var inventory = this.inventoryFactory.EmptyContainer(size, stackSize);

            var items = this.itemFactory.CreateMany(stackSize - 1)
                .Append(default!)
                .ToArray();
            items.Shuffle();

            Assert.That(
                () => inventory.CanAdd(items),
                Throws.ArgumentNullException.With.Property("ParamName").EqualTo("items")
            );
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
        public void CanAddItems_DifferentItemTypes_ThrowsArgumentException()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var inventory = this.inventoryFactory.EmptyContainer(size, stackSize);

            var items = this.itemFactory.CreateMany(stackSize - 1)
                .Append(this.itemFactory.CreateRandom())
                .ToArray();
            items.Shuffle();

            Assert.That(
                () => inventory.CanAdd(items.ToArray()),
                Throws.ArgumentException.With.Property("ParamName").EqualTo("items")
            );
        }

        [Test]
        public void CanAddItems_InsufficientSpace_ReturnsFalse()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var item = this.itemFactory.CreateDefault();
            var inventory = this.inventoryFactory.FullContainer(size, stackSize, item);
            var randomAmount = this.random.Next(2, size);
            inventory.Get(item, randomAmount - 1);

            var items = this.itemFactory.CreateMany(randomAmount);
            var canAdd = inventory.CanAdd(items);

            Assert.That(canAdd, Is.False);
        }

        [Test]
        public void CanAddItems_SufficientSpaceWithDifferentItem_ReturnsFalse()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var item = this.itemFactory.CreateDefault();
            var inventory = this.inventoryFactory.FullContainer(size, stackSize, item);

            var randomIndex = this.random.Next(0, size);
            inventory.Get(randomIndex, stackSize - 1);

            var randomAmount = this.random.Next(1, stackSize);
            var items = this.itemFactory.CreateMany(randomAmount);
            var canAdd = inventory.CanAdd(items);

            Assert.That(canAdd, Is.True);
        }

        [Test]
        public void CanAddItems_FullInventory_ReturnsFalse()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var item = this.itemFactory.CreateDefault();
            var inventory = this.inventoryFactory.FullContainer(size, stackSize, item);

            var items = this.itemFactory.CreateMany(size);
            var canAdd = inventory.CanAdd(items);

            Assert.That(canAdd, Is.False);
        }

        [Test]
        public void CanAddItems_EmptyItems_ReturnsTrue()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var inventory = this.inventoryFactory.EmptyContainer(size, stackSize);

            var canAdd = inventory.CanAdd(Array.Empty<T>());

            Assert.That(canAdd, Is.True);
        }

        [Test]
        public void CanAddItems_EmptyInventory_ReturnsTrue()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var inventory = this.inventoryFactory.EmptyContainer(size, stackSize);

            var items = this.itemFactory.CreateMany(stackSize);
            var canAdd = inventory.CanAdd(items);

            Assert.That(canAdd, Is.True);
        }

        [Test]
        public void CanAddItems_SufficientSpaceWithSameItem_ReturnsTrue()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var item = this.itemFactory.CreateDefault();
            var inventory = this.inventoryFactory.FullContainer(size, stackSize, item);

            var randomIndex = this.random.Next(0, size);
            inventory.Get(randomIndex, stackSize - 1);

            var items = this.itemFactory.CreateMany(stackSize - 1);
            var canAdd = inventory.CanAdd(items);

            Assert.That(canAdd, Is.True);
        }

        [Test]
        public void CanAddItems_SufficientSpaceWithSameItemInMultipleSlots_ReturnsTrue()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var item = this.itemFactory.CreateDefault();
            var inventory = this.inventoryFactory.FullContainer(size, stackSize, item);

            inventory.Remove(stackSize, this.random);

            var items = this.itemFactory.CreateMany(stackSize);
            var canAdd = inventory.CanAdd(items);

            Assert.That(canAdd, Is.True);
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
    }
}
