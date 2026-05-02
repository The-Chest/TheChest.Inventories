namespace TheChest.Inventories.Tests.Containers.Inventory
{
    public partial class InventoryTests<T>
    {
        [Test]
        public void CanAddItem_NullItem_ThrowsArgumentNullException()
        {
            var inventory = this.inventoryFactory.EmptyContainer();
            Assert.That(
                () => inventory.CanAdd(item: default!),
                Throws.ArgumentNullException.With.Message.EqualTo("Value cannot be null. (Parameter 'item')")
            );
        }
    }
}
