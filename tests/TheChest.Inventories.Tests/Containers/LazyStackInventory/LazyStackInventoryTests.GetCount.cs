using TheChest.Tests.Common.Attributes;
﻿namespace TheChest.Inventories.Tests.Containers.LazyStackInventory
{
    public partial class LazyStackInventoryTests<T>
    {
        [Test]
        [IgnoreIfValueTypeAttribute]
        public void GetCount_NullItem_ThrowsArgumentNullException()
        {
            var inventory = this.inventoryFactory.EmptyContainer();

            Assert.Throws<ArgumentNullException>(() => inventory.GetCount(default!));
        }
    }
}
