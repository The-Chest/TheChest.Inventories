using TheChest.Tests.Common.Attributes;
﻿namespace TheChest.Inventories.Tests.Containers.Interfaces
{
    public partial class IInventoryTests<T>
    {
        [Test]
        [IgnoreIfValueTypeAttribute]
        public void GetItem_NotFoundItem_ReturnsNull()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var items = this.itemFactory.CreateMany(size / 2);
            var inventory = this.inventoryFactory.ShuffledItemsContainer(size, items);

            var searchItem = this.itemFactory.CreateRandom();
            var result = inventory.Get(searchItem);

            Assert.That(result, Is.Null);
        }

        [Test]
        [IgnoreIfReferenceTypeAttribute]
        public void GetItem_NotFoundItemValueType_ReturnsDefault()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var items = this.itemFactory.CreateMany(size / 2);
            var inventory = this.inventoryFactory.ShuffledItemsContainer(size, items);

            var searchItem = this.itemFactory.CreateRandom();
            var result = inventory.Get(searchItem);

            Assert.That(result, Is.EqualTo(default(T)));
        }

        [Test]
        public void GetItem_ExistingItems_ReturnsFirstFoundItem()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var item = this.itemFactory.CreateDefault();
            var inventory = this.inventoryFactory.FullContainer(size, item);

            var result = inventory.Get(item);

            Assert.That(result, Is.Not.Null.And.EqualTo(item));
        }
    }
}
