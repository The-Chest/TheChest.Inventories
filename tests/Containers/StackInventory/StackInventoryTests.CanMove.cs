namespace TheChest.Inventories.Tests.Containers.StackInventory
{
    public partial class StackInventoryTests<T>
    {
        [TestCase(-1)]
        [TestCase(MAX_SIZE_TEST)]
        public void CanMove_InvalidOriginIndex_ThrowsArgumentOutOfRangeException(int origin)
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var inventory = this.inventoryFactory.EmptyContainer(size, stackSize);

            Assert.That(
                () => inventory.CanMove(origin, 0), 
                Throws.TypeOf<ArgumentOutOfRangeException>().With.Property("ParamName").EqualTo("origin")
            );
        }

        [Test]
        public void CanMove_OriginEqualToSize_ThrowsArgumentOutOfRangeException()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var inventory = this.inventoryFactory.EmptyContainer(size, stackSize);

            Assert.That(
                () => inventory.CanMove(size, 0), 
                Throws.TypeOf<ArgumentOutOfRangeException>().With.Property("ParamName").EqualTo("origin")
            );
        }

        [TestCase(-1)]
        [TestCase(MAX_SIZE_TEST)]
        public void CanMove_InvalidTargetIndex_ThrowsArgumentOutOfRangeException(int target)
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var inventory = this.inventoryFactory.EmptyContainer(size, stackSize);

            Assert.That(
                () => inventory.CanMove(0, target), 
                Throws.TypeOf<ArgumentOutOfRangeException>().With.Property("ParamName").EqualTo("target")
            );
        }

        [Test]
        public void CanMove_TargetEqualToSize_ThrowsArgumentOutOfRangeException()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var inventory = this.inventoryFactory.EmptyContainer(size, stackSize);

            Assert.That(
                () => inventory.CanMove(0, size), 
                Throws.TypeOf<ArgumentOutOfRangeException>().With.Property("ParamName").EqualTo("target")
            );
        }

        [Test]
        public void CanMove_SameOriginAndTarget_ReturnsFalse()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var originIndex = this.random.Next(0, size - 1);
            var inventory = this.inventoryFactory.EmptyContainer(size, stackSize);

            var canMove = inventory.CanMove(originIndex, originIndex);

            Assert.That(canMove, Is.False);
        }

        [Test]
        public void CanMove_EmptyOrigin_TargetWithItems_ReturnsTrue()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var slotItem = this.itemFactory.CreateDefault();

            var originIndex = this.random.Next(size / 2, size - 1);
            var targetIndex = this.random.Next(0, originIndex - 1);

            var inventory = this.inventoryFactory.FullContainer(size, stackSize, slotItem);
            inventory.GetAll(originIndex);

            var canMove = inventory.CanMove(originIndex, targetIndex);

            Assert.That(canMove, Is.True);
        }

        [Test]
        public void CanMove_OriginWithItems_EmptyTarget_ReturnsTrue()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var slotItem = this.itemFactory.CreateDefault();

            var originIndex = this.random.Next(size / 2, size - 1);
            var targetIndex = this.random.Next(0, originIndex - 1);

            var inventory = this.inventoryFactory.FullContainer(size, stackSize, slotItem);
            inventory.GetAll(targetIndex);

            var canMove = inventory.CanMove(originIndex, targetIndex);

            Assert.That(canMove, Is.True);
        }

        [Test]
        public void CanMove_OriginAndTargetWithSameItems_ReturnsTrue()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var slotItem = this.itemFactory.CreateDefault();

            var originIndex = this.random.Next(size / 2, size - 1);
            var targetIndex = this.random.Next(0, originIndex - 1);

            var inventory = this.inventoryFactory.FullContainer(size, stackSize, slotItem);
            inventory.Get(originIndex, random.Next(1, stackSize - 2));

            var canMove = inventory.CanMove(originIndex, targetIndex);

            Assert.That(canMove, Is.True);
        }

        [Test]
        public void CanMove_OriginAndTargetWithDifferentItems_ReturnsTrue()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var slotItems = this.itemFactory.CreateMany(size / 2);
            var randomItems = this.itemFactory.CreateManyRandom(size / 2);
            var inventoryItems = slotItems.Concat(randomItems).ToArray();

            var inventory = this.inventoryFactory.ShuffledItemsContainer(size, stackSize, inventoryItems);

            var originIndex = this.random.Next(size / 2, size - 1);
            var targetIndex = this.random.Next(0, originIndex - 1);

            var canMove = inventory.CanMove(originIndex, targetIndex);

            Assert.That(canMove, Is.True);
        }
    }
}
