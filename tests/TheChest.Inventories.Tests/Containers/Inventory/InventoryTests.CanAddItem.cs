namespace TheChest.Inventories.Tests.Containers.Inventory
{
    public partial class InventoryTests<T>
    {
        [Test]
        [TheChest.Tests.Common.Attributes.IgnoreIfValueTypeAttribute]
        public void CanAddItem_NullItem_ThrowsArgumentNullException()
        {
            var inventory = this.inventoryFactory.EmptyContainer();
            Assert.That(() => inventory.CanAdd(item: default!), Throws.ArgumentNullException);
        }
    }
}
