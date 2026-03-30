namespace TheChest.Inventories.Tests.Containers
{
    public partial class StackInventoryTests<T>
    {
        [TestCase(-1)]
        [TestCase(MAX_SIZE_TEST + 1)]
        public void CanMove_InvalidOriginIndex_ThrowsArgumentOutOfRangeException(int origin)
        {
            var inventorySize = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var inventory = this.inventoryFactory.EmptyContainer(inventorySize);
            Assert.That(() => inventory.CanMove(origin, 0), Throws.TypeOf<ArgumentOutOfRangeException>());
        }

        [TestCase(-1)]
        [TestCase(MAX_SIZE_TEST + 1)]
        public void CanMove_InvalidTargetIndex_ThrowsArgumentOutOfRangeException(int target)
        {
            var inventorySize = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var inventory = this.inventoryFactory.EmptyContainer(inventorySize);
            Assert.That(() => inventory.CanMove(0, target), Throws.TypeOf<ArgumentOutOfRangeException>());
        }
    }
}
