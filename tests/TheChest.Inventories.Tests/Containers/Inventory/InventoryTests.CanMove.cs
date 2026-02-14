namespace TheChest.Inventories.Tests.Containers
{
    public partial class InventoryTests<T>
    {
        [TestCase(-1)]
        [TestCase(100)]
        public void CanMove_InvalidOrigin_ReturnsFalse(int origin)
        {
            var size = this.random.Next(3, 10);
            var item = this.itemFactory.CreateDefault();
            var inventory = this.containerFactory.FullContainer(size, item);

            var canMove = inventory.CanMove(origin, 0);

            Assert.That(canMove, Is.False);
        }

        [TestCase(-1)]
        [TestCase(100)]
        public void CanMove_InvalidTarget_ReturnsFalse(int target)
        {
            var size = this.random.Next(3, 10);
            var item = this.itemFactory.CreateDefault();
            var inventory = this.containerFactory.FullContainer(size, item);

            var canMove = inventory.CanMove(0, target);

            Assert.That(canMove, Is.False);
        }

        [Test]
        public void CanMove_BothSlotsWithItems_ReturnsTrue()
        {
            var size = this.random.Next(2, 20);
            var items = this.itemFactory.CreateManyRandom(size);
            var inventory = this.containerFactory.ShuffledItemsContainer(size, items);

            var canMove = inventory.CanMove(0, 1);

            Assert.That(canMove, Is.True);
        }

        [Test]
        public void CanMove_EmptyTarget_ReturnsTrue()
        {
            var inventory = this.containerFactory.EmptyContainer(2);

            var item = this.itemFactory.CreateRandom();
            inventory.Add(item);

            var canMove = inventory.CanMove(0, 1);

            Assert.That(canMove, Is.True);
        }

        [Test]
        public void CanMove_EmptyOriginWithItemInTarget_ReturnsTrue()
        {
            var inventory = this.containerFactory.EmptyContainer(2);

            var item = this.itemFactory.CreateRandom();
            inventory.AddAt(item, 1);

            var canMove = inventory.CanMove(0, 1);

            Assert.That(canMove, Is.True);
        }

        [Test]
        public void CanMove_EmptyOriginAndTarget_ReturnsFalse()
        {
            var inventory = this.containerFactory.EmptyContainer(2);

            var canMove = inventory.CanMove(0, 1);

            Assert.That(canMove, Is.False);
        }
    }
}
