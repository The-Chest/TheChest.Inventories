namespace TheChest.Inventories.Tests.Containers.Interfaces
{
    public partial class IStackInventoryTests<T>
    {
        [Test]
        public void CanMove_SameOriginAndTarget_ReturnsFalse()
        {
            var inventorySize = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var originIndex = this.random.Next(0, inventorySize - 1);
            var inventory = this.inventoryFactory.EmptyContainer(inventorySize, stackSize);

            var canMove = inventory.CanMove(originIndex, originIndex);

            Assert.That(canMove, Is.False);
        }

        [Test]
        public void CanMove_EmptyOrigin_TargetWithItems_ReturnsTrue()
        {
            var inventorySize = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var slotItem = this.itemFactory.CreateDefault();

            var originIndex = this.random.Next(MIN_SIZE_TEST, inventorySize - 1);
            var targetIndex = this.random.Next(0, originIndex - 1);

            var inventory = this.inventoryFactory.FullContainer(inventorySize, stackSize, slotItem);
            inventory.GetAll(originIndex);

            var canMove = inventory.CanMove(originIndex, targetIndex);

            Assert.That(canMove, Is.True);
        }

        [Test]
        public void CanMove_OriginWithItems_EmptyTarget_ReturnsTrue()
        {
            var inventorySize = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var slotItem = this.itemFactory.CreateDefault();

            var originIndex = this.random.Next(MIN_SIZE_TEST, inventorySize - 1);
            var targetIndex = this.random.Next(0, originIndex - 1);

            var inventory = this.inventoryFactory.FullContainer(inventorySize, stackSize, slotItem);
            inventory.GetAll(targetIndex);

            var canMove = inventory.CanMove(originIndex, targetIndex);

            Assert.That(canMove, Is.True);
        }

        [Test]
        public void CanMove_OriginAndTargetWithSameItems_ReturnsTrue()
        {
            var inventorySize = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var slotItem = this.itemFactory.CreateDefault();

            var originIndex = this.random.Next(MIN_SIZE_TEST, inventorySize - 1);
            var targetIndex = this.random.Next(0, originIndex - 1);

            var inventory = this.inventoryFactory.FullContainer(inventorySize, stackSize, slotItem);
            inventory.Get(originIndex, random.Next(1, stackSize - 2));

            var canMove = inventory.CanMove(originIndex, targetIndex);

            Assert.That(canMove, Is.True);
        }

        [Test]
        public void CanMove_OriginAndTargetWithDifferentItems_ReturnsTrue()
        {
            var inventorySize = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var slotItems = this.itemFactory.CreateMany(inventorySize / 2);
            var randomItems = this.itemFactory.CreateManyRandom(inventorySize / 2);
            var inventoryItems = slotItems.Concat(randomItems).ToArray();

            var inventory = this.inventoryFactory.ShuffledItemsContainer(inventorySize, stackSize, inventoryItems);

            var originIndex = this.random.Next(MIN_SIZE_TEST, inventorySize - 1);
            var targetIndex = this.random.Next(0, originIndex - 1);

            var canMove = inventory.CanMove(originIndex, targetIndex);

            Assert.That(canMove, Is.True);
        }
    }
}
