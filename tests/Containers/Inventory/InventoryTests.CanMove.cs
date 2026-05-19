namespace TheChest.Inventories.Tests.Containers.Inventory
{
    public partial class InventoryTests<T>
    {
        [TestCase(-1)]
        [TestCase(MAX_SIZE_TEST)]
        public void CanMove_InvalidOrigin_ThrowsArgumentOutOfRangeException(int origin)
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var item = this.itemFactory.CreateDefault();
            var inventory = this.inventoryFactory.FullContainer(size, item);

            Assert.That(
                () => inventory.CanMove(origin, 0),
                Throws.TypeOf<ArgumentOutOfRangeException>().With.Property("ParamName").EqualTo("origin")
            );
        }

        [Test]
        public void CanMove_OriginEqualToSize_ThrowsArgumentOutOfRangeException()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var item = this.itemFactory.CreateDefault();
            var inventory = this.inventoryFactory.FullContainer(size, item);

            Assert.That(
                () => inventory.CanMove(size, 0),
                Throws.TypeOf<ArgumentOutOfRangeException>().With.Property("ParamName").EqualTo("origin")
            );
        }

        [TestCase(-1)]
        [TestCase(MAX_SIZE_TEST)]
        public void CanMove_InvalidTarget_ThrowsArgumentOutOfRangeException(int target)
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var item = this.itemFactory.CreateDefault();
            var inventory = this.inventoryFactory.FullContainer(size, item);

            Assert.That(
                () => inventory.CanMove(0, target),
                Throws.TypeOf<ArgumentOutOfRangeException>().With.Property("ParamName").EqualTo("target")
            );
        }

        [Test]
        public void CanMove_TargetEqualToSize_ThrowsArgumentOutOfRangeException()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var item = this.itemFactory.CreateDefault();
            var inventory = this.inventoryFactory.FullContainer(size, item);

            Assert.That(
                () => inventory.CanMove(0, size),
                Throws.TypeOf<ArgumentOutOfRangeException>().With.Property("ParamName").EqualTo("target")
            );
        }

        [Test]
        public void CanMove_OriginEqualToTarget_ReturnsFalse()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var item = this.itemFactory.CreateDefault();
            var inventory = this.inventoryFactory.FullContainer(size, item);

            var randomIndex = this.random.Next(0, size);
            var canMove = inventory.CanMove(randomIndex, randomIndex);

            Assert.That(canMove, Is.False);
        }

        [Test]
        public void CanMove_BothSlotsWithItems_ReturnsTrue()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var inventory = this.inventoryFactory.EmptyContainer(size);

            var firstItem = this.itemFactory.CreateRandom();
            var secondItem = this.itemFactory.CreateRandom();
            inventory.AddAt(firstItem, 0);
            inventory.AddAt(secondItem, 1);

            var canMove = inventory.CanMove(0, 1);

            Assert.That(canMove, Is.True);
        }

        [Test]
        public void CanMove_OriginWithItem_EmptyTarget_ReturnsTrue()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var inventory = this.inventoryFactory.EmptyContainer(size);

            var item = this.itemFactory.CreateRandom();
            inventory.Add(item);

            var canMove = inventory.CanMove(0, 1);

            Assert.That(canMove, Is.True);
        }

        [Test]
        public void CanMove_EmptyOrigin_TargetWithItem_ReturnsTrue()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var inventory = this.inventoryFactory.EmptyContainer(size);

            var item = this.itemFactory.CreateRandom();
            inventory.AddAt(item, 1);

            var canMove = inventory.CanMove(0, 1);

            Assert.That(canMove, Is.True);
        }

        [Test]
        public void CanMove_EmptyOrigin_EmptyTarget_ReturnsFalse()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var inventory = this.inventoryFactory.EmptyContainer(size);

            var canMove = inventory.CanMove(0, 1);

            Assert.That(canMove, Is.False);
        }
    }
}
