namespace TheChest.Inventories.Tests.Containers.Interfaces
{
    public partial class IInventoryTests<T>
    {
        [Test]
        [TheChest.Tests.Common.Attributes.IgnoreIfValueTypeAttribute]
        public void GetItem_NotFoundItem_ReturnsNull()
        {
            var size = this.random.Next(10, 20);
            var items = this.itemFactory.CreateMany(size / 2);
            var inventory = this.inventoryFactory.ShuffledItemsContainer(size, items);

            var searchItem = this.itemFactory.CreateRandom();
            var result = inventory.Get(searchItem);

            Assert.That(result, Is.Null);
        }

        [Test]
        public void GetItem_ExistingItems_ReturnsFirstFoundItem()
        {
            var size = this.random.Next(10, 20);
            var item = this.itemFactory.CreateDefault();
            var inventory = this.inventoryFactory.FullContainer(size, item);

            var result = inventory.Get(item);

            Assert.That(result, Is.Not.Null.And.EqualTo(item));
        }
    }
}
