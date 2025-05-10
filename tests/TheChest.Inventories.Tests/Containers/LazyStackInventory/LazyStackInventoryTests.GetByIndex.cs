namespace TheChest.Inventories.Tests.Containers.LazyStackInventory
{
    public partial class LazyStackInventoryTests<T>
    {
        [Test]
        public void Get_ByIndex_ValidIndex_ReturnsItem()
        {
            var expectedItem = this.itemFactory.CreateDefault();
            var inventory = this.containerFactory.FullContainer(20, 1, expectedItem);

            var result = inventory.Get(0);

            Assert.That(result, Is.EqualTo(expectedItem));
        }

        [Test]
        public void Get_ByIndex_EmptySlot_ReturnsNull()
        {
            var inventory = this.containerFactory.EmptyContainer(20);

            var result = inventory.Get(0);

            Assert.That(result, Is.Null);
        }

        [TestCase(-1)]
        [TestCase(1000)]
        public void Get_ByIndex_ShouldThrowArgumentOutOfRangeException_WhenIndexIsInvalid(int index)
        {
            var item = this.itemFactory.CreateDefault();
            var inventory = this.containerFactory.FullContainer(20, 1, item);

            Assert.Throws<ArgumentOutOfRangeException>(() => inventory.Get(index));
        }
    }
}
