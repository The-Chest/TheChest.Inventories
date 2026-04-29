using NUnit.Framework.Internal;

namespace TheChest.Inventories.Tests.Containers.Interfaces
{
    public partial class IStackInventoryTests<T>
    {
        [Test]
        public void CanAddItemAt_EmptyInventory_ReturnsTrue()
        {
            var inventory = this.inventoryFactory.EmptyContainer();

            var item = this.itemFactory.CreateDefault();

            var canAdd = inventory.CanAddAt(item, 0);

            Assert.That(canAdd, Is.True);
        }

        [Test]
        public void CanAddItemAt_SlotWithSameItemsAndEnoughSpace_ReturnsTrue()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var stackSize = this.random.Next(5, 10);
            var item = this.itemFactory.CreateDefault();
            var inventory = this.inventoryFactory.FullContainer(size, stackSize, item);
            var randomIndex = this.random.Next(0, size);
            inventory.Get(randomIndex, stackSize - 1);

            var canAdd = inventory.CanAddAt(item, randomIndex);
            
            Assert.That(canAdd, Is.True);
        }

        [Test]
        public void CanAddItemAt_SlotWithDifferentItemsAndEnoughSpace_ReturnsFalse()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var stackSize = this.random.Next(5, 10);
            var item = this.itemFactory.CreateDefault();
            var inventory = this.inventoryFactory.FullContainer(size, stackSize, item);
            var randomIndex = this.random.Next(0, size);
            inventory.Get(randomIndex, stackSize - 1);

            var randomItem = this.itemFactory.CreateRandom();
            var canAdd = inventory.CanAddAt(randomItem, randomIndex);

            Assert.That(canAdd, Is.False);
        }

        [Test]
        public void CanAddItemAt_FullInventory_ReturnsFalse()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var stackSize = this.random.Next(5, 10);
            var item = this.itemFactory.CreateDefault();
            var inventory = this.inventoryFactory.FullContainer(size, stackSize, item);

            var randomIndex = this.random.Next(0, size);
            inventory.AddAt(item, randomIndex);

            var canAdd = inventory.CanAddAt(item, randomIndex);

            Assert.That(canAdd, Is.False);
        }
    }
}
