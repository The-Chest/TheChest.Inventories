namespace TheChest.Inventories.Tests.Containers.LazyStackInventory
{
    public partial class LazyStackInventoryTests<T>
    {
        [Test]
        public void Get_ByItem_NullItem_ThrowsArgumentNullException()
        {
            var inventory = this.containerFactory.EmptyContainer();
            Assert.Throws<ArgumentNullException>(() => inventory.Get(item: default!));
        }

        [Test]
        public void Get_ByItem_ExistingItem_ReturnsItem()
        {
            var items = this.itemFactory.CreateDefault();
            var inventory = this.containerFactory.ShuffledItemsContainer(10,5, items);
            var expectedItem = this.itemFactory.CreateDefault();

            var result = inventory.Get(expectedItem);

            Assert.That(result, Is.EqualTo(expectedItem));
        }

        [Test]
        public void Get_ByItem_ExistingItem_RemovesOneFromFirstFoundSlot()
        {
            var items = this.itemFactory.CreateDefault();
            var amount = 5;
            var inventory = this.containerFactory.FullContainer(10, amount, items);
            var expectedItem = this.itemFactory.CreateDefault();

            inventory.Get(expectedItem);

            Assert.That(inventory[0].StackAmount, Is.EqualTo(amount - 1));
        }

        [Test]
        public void Get_ByItem_NotFoundItem_ReturnsNull()
        {
            var items = this.itemFactory.CreateDefault();
            var inventory = this.containerFactory.ShuffledItemsContainer(10, 5, items);
            var item = this.itemFactory.CreateRandom();

            var result = inventory.Get(item);

            Assert.That(result, Is.Null);
        }
    }
}
