using TheChest.Tests.Common.Attributes;
﻿namespace TheChest.Inventories.Tests.Containers.Interfaces
{
    public partial class IStackInventoryTests<T>
    {
        [Test]
        [IgnoreIfValueTypeAttribute]
        public void GetItem_EmptyInventory_ReturnsNull()
        {
            var item = this.itemFactory.CreateDefault();
            var inventory = this.inventoryFactory.EmptyContainer();

            var foundItem = inventory.Get(item);
            
            Assert.That(foundItem, Is.Null);
        }

        [Test]
        [IgnoreIfReferenceTypeAttribute]
        public void GetItem_EmptyInventoryValueType_ReturnsDefault()
        {
            var item = this.itemFactory.CreateDefault();
            var inventory = this.inventoryFactory.EmptyContainer();

            var foundItem = inventory.Get(item);

            Assert.That(foundItem, Is.EqualTo(default(T)));
        }

        [Test]
        public void GetItem_InventoryWithItems_ReturnsItem()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var slotItem = this.itemFactory.CreateRandom();
            var inventory = this.inventoryFactory.FullContainer(size, stackSize, slotItem);

            var item = inventory.Get(slotItem);

            Assert.That(item, Is.EqualTo(slotItem));
        }

        [Test]
        [IgnoreIfValueTypeAttribute]
        public void GetItem_InventoryWithDifferentItems_ReturnsNull()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var slotItem = this.itemFactory.CreateRandom();
            var inventory = this.inventoryFactory.FullContainer(size, stackSize, slotItem);

            var item = this.itemFactory.CreateDefault();
            var result = inventory.Get(item);

            Assert.That(result, Is.Null);
        }

        [Test]
        [IgnoreIfReferenceTypeAttribute]
        public void GetItem_InventoryWithDifferentItemsValueType_ReturnsDefault()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var slotItem = this.itemFactory.CreateRandom();
            var inventory = this.inventoryFactory.FullContainer(size, stackSize, slotItem);

            var item = this.itemFactory.CreateDefault();
            var result = inventory.Get(item);

            Assert.That(result, Is.EqualTo(default(T)));
        }
    }
}
