namespace TheChest.Inventories.Tests.Containers.Interfaces
{
    public partial class IInventoryTests<T>
    {
        [Test]
        [TheChest.Tests.Common.Attributes.IgnoreIfValueTypeAttribute]
        public void Replace_EmptySlot_ReturnsNull()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var inventory = this.inventoryFactory.EmptyContainer(size);

            var randomIndex = this.random.Next(0, size);
            var item = this.itemFactory.CreateDefault();
            var result = inventory.Replace(item, randomIndex);

            Assert.That(result, Is.Null);
        }

        [Test]
        public void Replace_FullSlot_ReturnsOldItemFromSlot()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var initialItem = this.itemFactory.CreateDefault();
            var inventory = this.inventoryFactory.FullContainer(size, initialItem);

            var randomIndex = this.random.Next(0, size);
            var newItem = this.itemFactory.CreateRandom();
            var result = inventory.Replace(newItem, randomIndex);

            Assert.That(result, Is.EqualTo(initialItem));
        }
    }
}
