using TheChest.Tests.Common.Attributes;
﻿namespace TheChest.Inventories.Tests.Containers.LazyStackInventory
{
    public partial class LazyStackInventoryTests<T>
    {
        [Test]
        [IgnoreIfValueType]
        public void CanAdd_NullItem_ThrowsArgumentNullException()
        {
            var inventory = this.inventoryFactory.EmptyContainer();

            Assert.That(() => inventory.CanAdd(item: default!, amount: 1), Throws.ArgumentNullException);
        }

        [TestCase(0)]
        [TestCase(-1)]
        public void CanAdd_ZeroOrLessAmount_ThrowsArgumentOutOfRangeException(int amount)
        {
            var inventory = this.inventoryFactory.EmptyContainer();
            var item = this.itemFactory.CreateDefault();

            Assert.That(() => inventory.CanAdd(item, amount), Throws.Exception.TypeOf<ArgumentOutOfRangeException>());
        }
    }
}
