namespace TheChest.Inventories.Tests.Containers.LazyStackInventory
{
    public partial class LazyStackInventoryTests<T>
    {
        [Test]
        public void Get_ByItemAndAmount_Nulltem_ThrowsArgumentNullException()
        {
            var inventory = this.containerFactory.EmptyContainer();

            Assert.Throws<ArgumentNullException>(() => inventory.Get(item: default!, amount: 1));
        }

        [TestCase(-1)]
        [TestCase(0)]
        public void Get_ByItemAndAmount_InvalidAmount_ThrowsArgumentOutOfRangeException(int amount)
        {
            var inventory = this.containerFactory.EmptyContainer();
            var item = this.itemFactory.CreateDefault();

            Assert.Throws<ArgumentOutOfRangeException>(() => inventory.Get(item, amount));
        }

        [Test]
        public void Get_ByItemAndAmount_ExistingItemsAndAmountBiggerThanInventorySize_ReturnsMaxAvailableAmount()
        {
            var size = this.random.Next(1, 20);
            var stackSize = this.random.Next(1, 20);
            var item = this.itemFactory.CreateDefault();
            var inventory = this.containerFactory.FullContainer(20, 5, item);

            var amount = size * stackSize * 2;
            var expectedAmount = size * stackSize;
            var result = inventory.Get(item, amount);

            Assert.That(result, Has.Length.EqualTo(expectedAmount).And.All.EqualTo(item));
        }

        [Test]
        public void Get_ByItemAndAmount_ExistingItems_ReturnsCorrectAmountFromMultipleSlots()
        {
            var item = this.itemFactory.CreateDefault();
            var inventory = this.containerFactory.FullContainer(20, 1, item);
            var amount = this.random.Next(2, 20);

            var result = inventory.Get(item, amount);

            Assert.That(result, Has.Length.EqualTo(amount).And.All.EqualTo(item));
        }

        [Test]
        public void Get_ByItemAndAmount_NotFoundItem_ReturnsEmptyArray()
        {
            var item = this.itemFactory.CreateDefault();
            var inventory = this.containerFactory.FullContainer(20, 1, item);
            var amount = this.random.Next(2, 20);

            var result = inventory.Get(item, amount);

            Assert.That(result, Is.Empty);
        }
    }
}
