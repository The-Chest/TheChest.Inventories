using TheChest.Tests.Common.Attributes;

namespace TheChest.Inventories.Tests.Containers.Inventory
{
    public partial class InventoryTests<T>
    {
        [Test]
        [IgnoreIfValueType]
        public void CanReplace_NullItem_ThrowsArgumentNullException()
        {
            var inventory = this.inventoryFactory.EmptyContainer();

            Assert.That(
                () => inventory.CanReplace(default!, 0),
                Throws.ArgumentNullException.With.Property("ParamName").EqualTo("item")
            );
        }

        [TestCase(-1)]
        [TestCase(MAX_SIZE_TEST + 1)]
        public void CanReplace_InvalidSlotIndex_ThrowsArgumentOutOfRangeException(int index)
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var inventory = this.inventoryFactory.EmptyContainer(size);

            Assert.That(
                () => inventory.CanReplace(this.itemFactory.CreateDefault(), index),
                Throws.Exception.TypeOf<ArgumentOutOfRangeException>().With.Property("ParamName").EqualTo("index")
            );
        }

        [Test]
        public void CanReplace_IndexEqualToSize_ThrowsArgumentOutOfRangeException()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var inventory = this.inventoryFactory.EmptyContainer(size);

            Assert.That(
                () => inventory.CanReplace(this.itemFactory.CreateDefault(), size),
                Throws.Exception.TypeOf<ArgumentOutOfRangeException>().With.Property("ParamName").EqualTo("index")
            );
        }

        [Test]
        public void CanReplace_EmptySelectedSlot_ReturnsTrue()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var inventory = this.inventoryFactory.EmptyContainer(size);
            var randomIndex = this.random.Next(0, size);

            var canReplace = inventory.CanReplace(this.itemFactory.CreateDefault(), randomIndex);

            Assert.That(canReplace, Is.True);
        }

        [Test]
        public void CanReplace_FullSelectedSlot_ReturnsTrue()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var inventory = this.inventoryFactory.FullContainer(size, this.itemFactory.CreateDefault());
            var randomIndex = this.random.Next(0, size);

            var canReplace = inventory.CanReplace(this.itemFactory.CreateRandom(), randomIndex);

            Assert.That(canReplace, Is.True);
        }

        [Test]
        public void CanReplace_FullSlot_ReturnsTrue()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var inventory = this.inventoryFactory.FullContainer(size, this.itemFactory.CreateDefault());
            var randomIndex = this.random.Next(0, size);

            var result = inventory.CanReplace(this.itemFactory.CreateRandom(), randomIndex);

            Assert.That(result, Is.True);
        }

        [Test]
        public void CanReplace_EmptySlot_ReturnsTrue()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var inventory = this.inventoryFactory.EmptyContainer(size);
            var randomIndex = this.random.Next(0, size);

            var result = inventory.CanReplace(this.itemFactory.CreateDefault(), randomIndex);

            Assert.That(result, Is.True);
        }
    }
}
