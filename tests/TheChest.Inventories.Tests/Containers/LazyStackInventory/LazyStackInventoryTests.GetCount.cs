namespace TheChest.Inventories.Tests.Containers.LazyStackInventory
{
    public partial class LazyStackInventoryTests<T>
    {
        [Test]
        public void GetCount_NullItem_ThrowsArgumentNullException()
        {
            var inventory = this.inventoryFactory.EmptyContainer();

            Assert.Throws<ArgumentNullException>(() => inventory.GetCount(default!));
        }
    }
}
