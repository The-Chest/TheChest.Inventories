using TheChest.Tests.Common.Attributes;
﻿namespace TheChest.Inventories.Tests.Containers.Inventory
{
    public partial class InventoryTests<T>
    {
        [Test]
        [IgnoreIfValueTypeAttribute]
        public void CanAddItem_NullItem_ThrowsArgumentNullException()
        {
            var inventory = this.inventoryFactory.EmptyContainer();
            Assert.That(() => inventory.CanAdd(item: default!), Throws.ArgumentNullException);
        }
    }
}
