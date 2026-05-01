using TheChest.Tests.Common.Attributes;

namespace TheChest.Inventories.Tests.Containers.Interfaces
{
    public partial class IInventoryTests<T>
    {
        [Test]
        [IgnoreIfValueType]
        public void AddItem_NullItem_ThrowsArgumentNullException()
        {
            var inventory = this.inventoryFactory.EmptyContainer();

            Assert.That(
                () => inventory.Add(item: default!),
                Throws.ArgumentNullException
                    .With.Message.EqualTo("Value cannot be null. (Parameter 'item')")
            );
        }

        [Test]
        public void AddItem_EmptyInventory_ReturnsTrue()
        {
            var inventory = this.inventoryFactory.EmptyContainer();

            var item = this.itemFactory.CreateDefault();
            var result = inventory.Add(item);

            Assert.That(result, Is.True);
        }
    }
}
