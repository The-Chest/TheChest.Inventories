namespace TheChest.Inventories.Tests.Containers.Interfaces
{
    public partial class IInventoryTests<T>
    {
        [Test]
        public void GetItemByIndex_ValidIndexFullSlot_ReturnsItem()
        {
            var size = this.random.Next(10, 20);
            var item = this.itemFactory.CreateDefault();
            var inventory = this.inventoryFactory.FullContainer(size, item);

            var randomIndex = this.random.Next(0, size);
            var result = inventory.Get(randomIndex);

            Assert.That(result, Is.EqualTo(item));
        }

        [Test]
        public void GetItemByIndex_ValidIndexEmptySlot_ReturnsNull()
        {
            var size = this.random.Next(10, 20);
            var inventory = this.inventoryFactory.EmptyContainer(size);

            var randomIndex = this.random.Next(0, size);
            var result = inventory.Get(randomIndex);

            Assert.That(result, Is.Null);
        }
    }
}
