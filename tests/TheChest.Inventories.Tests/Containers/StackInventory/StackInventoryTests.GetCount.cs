using TheChest.Tests.Common.Attributes;
﻿namespace TheChest.Inventories.Tests.Containers.StackInventory
{
    public partial class StackInventoryTests<T>
    {
        [Test]
        [IgnoreIfValueTypeAttribute]
        public void GetCount_InvalidItem_ThrowsArgumentNullException()
        {
            var inventory = this.inventoryFactory.EmptyContainer();

            Assert.That(() => inventory.GetCount(default!), Throws.ArgumentNullException);
        }
    }
}
