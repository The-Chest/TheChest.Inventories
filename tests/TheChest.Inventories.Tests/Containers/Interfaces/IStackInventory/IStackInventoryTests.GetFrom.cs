using TheChest.Tests.Common.Attributes;
﻿namespace TheChest.Inventories.Tests.Containers.Interfaces
{
    public partial class IStackInventoryTests<T>
    {
        [Test]
        [IgnoreIfValueType]
        public void GetFrom_EmptySlot_ReturnsNull()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var inventory = this.inventoryFactory.EmptyContainer(size);
            
            var index = this.random.Next(0, size);
            var item = inventory.Get(index);
            
            Assert.That(item, Is.Null);
        }

        [Test]
        [IgnoreIfReferenceType]
        public void GetFrom_EmptySlotValueType_ReturnsDefault()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var inventory = this.inventoryFactory.EmptyContainer(size);

            var index = this.random.Next(0, size);
            var item = inventory.Get(index);

            Assert.That(item, Is.EqualTo(default(T)));
        }

        [Test]
        public void GetFrom_SlotWithItems_ReturnsItem()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var slotItem = this.itemFactory.CreateRandom();
            var inventory = this.inventoryFactory.FullContainer(size, stackSize, slotItem);

            var index = this.random.Next(0, size);
            var item = inventory.Get(index);

            Assert.That(item, Is.EqualTo(slotItem));
        }
    }
}
