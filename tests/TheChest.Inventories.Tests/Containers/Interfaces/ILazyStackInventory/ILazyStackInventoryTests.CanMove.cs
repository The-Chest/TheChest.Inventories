using System.Drawing;

namespace TheChest.Inventories.Tests.Containers.Interfaces
{
    public partial class ILazyStackInventoryTests<T>
    {
        [Test]
        public void CanMove_SameOriginAndTarget_ReturnsFalse()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var inventory = this.inventoryFactory.EmptyContainer(size, stackSize);

            var originIndex = this.random.Next(0, size - 1);
            var canMove = inventory.CanMove(originIndex, originIndex);

            Assert.That(canMove, Is.False);
        }

        [Test]
        public void CanMove_EmptyOrigin_TargetWithItems_ReturnsTrue()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var slotItem = this.itemFactory.CreateDefault();

            var originIndex = this.random.Next(size / 2, size - 1);
            var inventory = this.inventoryFactory.FullContainer(size, stackSize, slotItem);
            inventory.GetAll(originIndex);

            var targetIndex = this.random.Next(0, originIndex - 1);
            var canMove = inventory.CanMove(originIndex, targetIndex);

            Assert.That(canMove, Is.True);
        }

        [Test]
        public void CanMove_OriginWithItems_EmptyTarget_ReturnsTrue()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var slotItem = this.itemFactory.CreateDefault();

            var originIndex = this.random.Next(size / 2, size - 1);
            var targetIndex = this.random.Next(0, originIndex - 1);

            var inventory = this.inventoryFactory.FullContainer(size, stackSize, slotItem);
            inventory.GetAll(targetIndex);

            var canMove = inventory.CanMove(originIndex, targetIndex);

            Assert.That(canMove, Is.True);
        }

        [Test]
        public void Move_OriginAndTargetWithSameItems_ReturnsTrue()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var slotItem = this.itemFactory.CreateDefault();

            var originIndex = this.random.Next(size / 2, size - 1);
            var targetIndex = this.random.Next(0, originIndex - 1);

            var inventory = this.inventoryFactory.FullContainer(size, stackSize, slotItem);
            inventory.Get(originIndex, random.Next(1, stackSize - 2));

            var canMove = inventory.CanMove(originIndex, targetIndex);

            Assert.That(canMove, Is.True);
        }

        [Test]
        public void Move_OriginAndTargetWithDifferentItems_ReturnsTrue()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
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
