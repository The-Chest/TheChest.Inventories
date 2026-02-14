namespace TheChest.Inventories.Tests.Containers.LazyStackInventory
{
    public partial class LazyStackInventoryTests<T>
    {
        [TestCase(-1)]
        [TestCase(100)]
        public void CanMove_InvalidOriginIndex_ThrowsArgumentOutOfRangeException(int origin)
        {
            var inventory = this.containerFactory.EmptyContainer();
            Assert.That(
                () => inventory.CanMove(origin, 0),
                Throws.TypeOf<ArgumentOutOfRangeException>()
            );
        }
        [TestCase(-1)]
        [TestCase(100)]
        public void CanMove_InvalidTargetIndex_ThrowsArgumentOutOfRangeException(int target)
        {
            var inventory = this.containerFactory.EmptyContainer();
            Assert.That(
                () => inventory.CanMove(0, target),
                Throws.TypeOf<ArgumentOutOfRangeException>()
            );
        }

        [Test]
        public void CanMove_SameOriginAndTarget_ReturnsFalse()
        {
            var inventorySize = this.random.Next(10, 20);
            var stackSize = this.random.Next(1, 20);
            var originIndex = this.random.Next(0, inventorySize - 1);
            var inventory = this.containerFactory.EmptyContainer(inventorySize, stackSize);

            var canMove = inventory.CanMove(originIndex, originIndex);

            Assert.That(canMove, Is.False);
        }

        [Test]
        public void CanMove_EmptyOrigin_TargetWithItems_ReturnsTrue()
        {
            var inventorySize = this.random.Next(10, 20);
            var stackSize = this.random.Next(1, 20);
            var slotItem = this.itemFactory.CreateDefault();

            var originIndex = this.random.Next(5, inventorySize - 1);
            var targetIndex = this.random.Next(0, originIndex - 1);

            var inventory = this.containerFactory.FullContainer(inventorySize, stackSize, slotItem);
            inventory.GetAll(originIndex);

            var canMove = inventory.CanMove(originIndex, targetIndex);

            Assert.That(canMove, Is.True);
        }

        [Test]
        public void CanMove_OriginWithItems_EmptyTarget_ReturnsTrue()
        {
            var inventorySize = this.random.Next(10, 20);
            var stackSize = this.random.Next(1, 20);
            var slotItem = this.itemFactory.CreateDefault();

            var originIndex = this.random.Next(5, inventorySize - 1);
            var targetIndex = this.random.Next(0, originIndex - 1);

            var inventory = this.containerFactory.FullContainer(inventorySize, stackSize, slotItem);
            inventory.GetAll(targetIndex);

            var canMove = inventory.CanMove(originIndex, targetIndex);

            Assert.That(canMove, Is.True);
        }

        [Test]
        public void Move_OriginAndTargetWithSameItems_ReturnsTrue()
        {
            var inventorySize = this.random.Next(10, 20);
            var stackSize = this.random.Next(5, 20);
            var slotItem = this.itemFactory.CreateDefault();

            var originIndex = this.random.Next(5, inventorySize - 1);
            var targetIndex = this.random.Next(0, originIndex - 1);

            var inventory = this.containerFactory.FullContainer(inventorySize, stackSize, slotItem);
            inventory.Get(originIndex, random.Next(1, stackSize - 2));

            var canMove = inventory.CanMove(originIndex, targetIndex);

            Assert.That(canMove, Is.True);
        }

        [Test]
        public void Move_OriginAndTargetWithDifferentItems_ReturnsTrue()
        {
            var inventorySize = this.random.Next(10, 20);
            var stackSize = this.random.Next(1, 20);
            var slotItems = this.itemFactory.CreateMany(inventorySize / 2);
            var randomItems = this.itemFactory.CreateManyRandom(inventorySize / 2);
            var inventoryItems = slotItems.Concat(randomItems).ToArray();

            var inventory = this.containerFactory.ShuffledItemsContainer(inventorySize, stackSize, inventoryItems);

            var originIndex = this.random.Next(5, inventorySize - 1);
            var targetIndex = this.random.Next(0, originIndex - 1);

            var canMove = inventory.CanMove(originIndex, targetIndex);

            Assert.That(canMove, Is.True);
        }
    }
}
