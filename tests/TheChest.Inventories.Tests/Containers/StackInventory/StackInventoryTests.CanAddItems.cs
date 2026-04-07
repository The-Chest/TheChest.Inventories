using TheChest.Tests.Common.Attributes;
﻿namespace TheChest.Inventories.Tests.Containers.StackInventory
{
    public partial class StackInventoryTests<T>
    {
        [Test]
        [IgnoreIfValueTypeAttribute]
        public void CanAddItems_NullItem_ThrowsArgumentNullException()
        {
            var inventory = this.inventoryFactory.EmptyContainer();
            Assert.That(() => inventory.CanAdd(items: default!), Throws.ArgumentNullException);
        }

        [Test]
        [IgnoreIfValueTypeAttribute]
        public void CanAddItems_ArrayContainingNullItem_ThrowsArgumentNullException()
        {
            var inventory = this.inventoryFactory.EmptyContainer();
            var items = this.itemFactory.CreateMany(5).ToList();
            items.Add(default!);

            Assert.That(() => inventory.CanAdd(items.ToArray()), Throws.ArgumentNullException);
        }
    }
}
