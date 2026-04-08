namespace TheChest.Inventories.Tests.Slots.Interfaces
{
    public partial class IInventoryStackSlotTests<T>
    {
        [Test]
        public void GetAmount_SlotWithEnoughItems_ReturnsWithAmount()
        {
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var items = this.itemFactory.CreateMany(stackSize);
            var slot = this.slotFactory.FullSlot(items);

            var amount = this.random.Next(1, stackSize);
            var result = slot.Get(amount);

            Assert.That(result, Is.EquivalentTo(items[0..amount]));
        }

        [Test]
        public void GetAmount_SlotWithNotEnoughItems_ReturnsItemsFromSlot()
        {
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var halfStack = stackSize / 2;
            var items = this.itemFactory.CreateMany(halfStack);
            var slot = this.slotFactory.WithItems(items, stackSize);

            var amount = this.random.Next(halfStack + 1, stackSize);
            var result = slot.Get(amount);

            Assert.That(result, Is.EquivalentTo(items));
        }

        [Test]
        public void GetAmount_SlotWithLessItemsThanRequested_ReturnsAllItemsFromSlot()
        {
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var items = this.itemFactory.CreateMany(stackSize);
            var slot = this.slotFactory.WithItems(items, stackSize);

            var result = slot.Get(stackSize * 2);

            Assert.That(result, Is.EquivalentTo(items));
        }

        [Test]
        public void GetAmount_SlotWithLessItemsThanRequested_DecreasesAmountToZero()
        {
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var itemSize = this.random.Next(1, stackSize / 2);
            var items = this.itemFactory.CreateMany(itemSize);
            var slot = this.slotFactory.WithItems(items, stackSize);

            var amount = this.random.Next(itemSize, stackSize);
            slot.Get(amount);
            
            Assert.That(slot.Amount, Is.EqualTo(0));
        }

        [Test]
        public void GetAmount_EmptySlot_ReturnsEmptyArray()
        {
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var slot = this.slotFactory.EmptySlot(stackSize);

            var amount = this.random.Next(1, stackSize / 2);
            var result = slot.Get(amount);

            Assert.That(result, Is.EquivalentTo(Array.Empty<T>()));
        }

        [Test]
        public void GetAmount_FullSlot_DecreasesAmount()
        {
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var items = this.itemFactory.CreateMany(stackSize);
            var slot = this.slotFactory.FullSlot(items);

            var amount = this.random.Next(1, stackSize);
            slot.Get(amount);
            
            Assert.That(slot.Amount, Is.EqualTo(stackSize - amount));
        }
    }
}
