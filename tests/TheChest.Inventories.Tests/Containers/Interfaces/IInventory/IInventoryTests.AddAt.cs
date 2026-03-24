using TheChest.Tests.Common.Extensions;

namespace TheChest.Inventories.Tests.Containers.Interfaces
{
    public partial class IInventoryTests<T>
    {
        [Test]
        public void AddAt_EmptySlot_ReturnsTrue()
        {
            var size = this.random.Next(10, 20);
            var inventory = this.inventoryFactory.EmptyContainer(size);

            var randomIndex = this.random.Next(0, size);
            var item = this.itemFactory.CreateDefault();
            var result = inventory.AddAt(item, randomIndex);

            Assert.That(result, Is.True);
        }

        [Test]
        public void AddAt_FullSlot_ReturnsFalse()
        {
            var size = this.random.Next(10, 20);
            var oldItem = this.itemFactory.CreateDefault();
            var inventory = this.inventoryFactory.FullContainer(size, oldItem);

            var randomIndex = this.random.Next(0, size);
            var item = this.itemFactory.CreateDefault();
            var result = inventory.AddAt(item, randomIndex);

            Assert.That(result, Is.False);
        }
    }
}
