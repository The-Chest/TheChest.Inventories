using TheChest.Tests.Common.Extensions;

using TheChest.Tests.Common.Attributes;

namespace TheChest.Inventories.Tests.Containers.StackInventory
{
    public partial class StackInventoryTests<T>
    {
        [Test]
        [IgnoreIfValueType]
        public void CanReplace_NullItems_ThrowsArgumentNullException()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var item = this.itemFactory.CreateDefault();
            var inventory = this.inventoryFactory.FullContainer(size, stackSize, item);

            var randomIndex = this.random.Next(0, size);

            Assert.That(
                () => inventory.CanReplace(null!, randomIndex),
                Throws.ArgumentNullException.With.Property("ParamName").EqualTo("items")
            );
        }

        [TestCase(-1)]
        [TestCase(MAX_SIZE_TEST)]
        public void CanReplace_InvalidIndex_ThrowsArgumentOutOfRangeException(int index)
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var items = this.itemFactory.CreateMany(stackSize);
            var inventory = this.inventoryFactory.EmptyContainer(size, stackSize);

            Assert.That(
                () => inventory.CanReplace(items, index),
                Throws.TypeOf<ArgumentOutOfRangeException>().With.Property("ParamName").EqualTo("index")
            );
        }

        [Test]
        public void CanReplace_ItemsExceedStackSize_ThrowsArgumentOutOfRangeException()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var item = this.itemFactory.CreateDefault();
            var inventory = this.inventoryFactory.FullContainer(size, stackSize, item);

            var items = this.itemFactory.CreateMany(stackSize + 1);
            var index = this.random.Next(0, size);
            Assert.That(() =>
                inventory.Replace(items, index),
                Throws.TypeOf<ArgumentOutOfRangeException>()
                    .With.Property("ParamName").EqualTo("items")
                    .And.Message.StartsWith("The max stack size is smaller than the number of items to replace.")
            );
        }

        [Test]
        [IgnoreIfValueType]
        public void CanReplace_ItemsContainingNull_ReturnsFalse()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var item = this.itemFactory.CreateDefault();
            var inventory = this.inventoryFactory.FullContainer(size, stackSize, item);

            var index = this.random.Next(0, size);
            var items = this.itemFactory
                .CreateManyRandom(stackSize)
                .Append(default)
                .ToArray();
            items.Shuffle();

            var canReplace = inventory.CanReplace(items!, index);

            Assert.That(canReplace, Is.False);
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
    }
}
