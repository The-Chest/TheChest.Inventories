using TheChest.Tests.Common.Attributes;
﻿namespace TheChest.Inventories.Tests.Containers.Interfaces
{
    public partial class IInventoryTests<T>
    {
        [Test]
        public void GetItemByIndex_ValidIndexFullSlot_ReturnsItem()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var item = this.itemFactory.CreateDefault();
            var inventory = this.inventoryFactory.FullContainer(size, item);

            var randomIndex = this.random.Next(0, size);
            var result = inventory.Get(randomIndex);

            Assert.That(result, Is.EqualTo(item));
        }

        [Test]
        [IgnoreIfValueTypeAttribute]
        public void GetItemByIndex_ValidIndexEmptySlot_ReturnsNull()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var inventory = this.inventoryFactory.EmptyContainer(size);

            var randomIndex = this.random.Next(0, size);
            var result = inventory.Get(randomIndex);

            Assert.That(result, Is.Null);
        }

        [Test]
        [IgnoreIfReferenceTypeAttribute]
        public void GetItemByIndex_ValidIndexEmptySlotValueType_ReturnsDefault()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var inventory = this.inventoryFactory.EmptyContainer(size);

            var randomIndex = this.random.Next(0, size);
            var result = inventory.Get(randomIndex);

            Assert.That(result, Is.EqualTo(default(T)));
        }
    }
}
