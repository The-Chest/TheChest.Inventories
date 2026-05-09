using TheChest.Inventories.Tests.Containers.Extensions;
﻿
namespace TheChest.Inventories.Tests.Containers.LazyStackInventory
{
    public partial class LazyStackInventoryTests<T>
    {
        [Test]
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

        [Test]
        public void CanAdd_InventoryPartiallyFilledWithDifferentItem_ReturnsTrue()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var inventoryItems = this.itemFactory.CreateManyRandom(MIN_SIZE_TEST / 2);
            var inventory = this.inventoryFactory.ShuffledItemsContainer(size, stackSize, inventoryItems);

            var item = this.itemFactory.CreateDefault();
            var amount = this.random.Next(1, stackSize);
            var canAdd = inventory.CanAdd(item, amount);

            Assert.That(canAdd, Is.True);
        }

        [Test]
        public void CanAdd_InventoryFilledWithDifferentItems_ReturnsFalse()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var inventoryItems = this.itemFactory.CreateManyRandom(size);
            var inventory = this.inventoryFactory.ShuffledItemsContainer(size, stackSize, inventoryItems);

            var item = this.itemFactory.CreateDefault();
            var amount = this.random.Next(1, stackSize);
            var canAdd = inventory.CanAdd(item, amount);

            Assert.That(canAdd, Is.False);
        }

        [Test]
        public void CanAdd_InventoryContainingSameItemAndEnoughSpace_ReturnsTrue()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var item = this.itemFactory.CreateDefault();
            var inventoryItems = this.itemFactory.CreateManyRandom(size - 1).ToList();
            inventoryItems.Add(item);
            var inventory = this.inventoryFactory.ShuffledItemsContainer(size, stackSize, inventoryItems.ToArray());

            var amount = this.random.Next(1, stackSize);
            inventory.Get(item, amount);

            var canAdd = inventory.CanAdd(item, amount);

            Assert.That(canAdd, Is.True);
        }

        [Test]
        public void CanAdd_InventoryWithSameItemAndNoEnoughSpace_ReturnsFalse()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var inventory = this.inventoryFactory.FullContainer(size, stackSize, this.itemFactory.CreateDefault());
            inventory.Remove(stackSize, this.random);

            var item = this.itemFactory.CreateDefault();
            var addAmount = stackSize + this.random.Next(1, stackSize);
            var canAdd = inventory.CanAdd(item, addAmount);

            Assert.That(canAdd, Is.False);
        }
    }
}
