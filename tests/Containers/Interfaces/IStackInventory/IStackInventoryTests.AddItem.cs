namespace TheChest.Inventories.Tests.Containers.Interfaces
{
    public partial class IStackInventoryTests<T>
    {
        [Test]
        public void AddItem_EmptyInventory_ReturnsTrue()
        {
            var item = this.itemFactory.CreateDefault();
            var inventory = this.inventoryFactory.EmptyContainer();

            var result = inventory.Add(item);

            Assert.That(result, Is.True);
        }

        [Test]
        public void AddItem_FullInventory_ThrowsInvalidOperationException()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var item = this.itemFactory.CreateDefault();
            var inventory = this.inventoryFactory.FullContainer(size, stackSize, this.itemFactory.CreateRandom());

            Assert.That(() => inventory.Add(item), Throws.InvalidOperationException);
        }

        [Test]
        public void AddItem_FullInventory_ThrowsInvalidOperationException_SecondCase()
        {
            var item = this.itemFactory.CreateDefault();
            var inventory = this.inventoryFactory.FullContainer(10, 10, this.itemFactory.CreateRandom());

            Assert.That(() => inventory.Add(item), Throws.InvalidOperationException);
        }
    }
}
