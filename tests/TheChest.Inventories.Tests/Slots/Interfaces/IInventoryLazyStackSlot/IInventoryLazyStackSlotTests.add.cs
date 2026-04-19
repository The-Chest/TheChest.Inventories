namespace TheChest.Inventories.Tests.Slots.Interfaces
{
    public partial class IInventoryLazyStackSlotTests<T>
    {
        [Test]
        public void Add_FullSlot_ReturnsNotAddedAmount()
        {
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var item = this.itemFactory.CreateDefault();
            var slot = this.slotFactory.FullSlot(item, stackSize);

            var amount = this.random.Next(1, stackSize);
            var result = slot.Add(item, amount);

            Assert.That(result, Is.EqualTo(amount));
        }

        [Test]
        public void Add_NotEmptySlotAndDifferentItem_ReturnsNotAddedAmount()
        {
            var item = this.itemFactory.CreateDefault();
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var halfStackSize = stackSize / 2;
            var amount = this.random.Next(1, halfStackSize);
            var slot = this.slotFactory.WithItem(item, amount, stackSize);

            var newItem = this.itemFactory.CreateRandom();
            
            var addAmount = this.random.Next(1, stackSize);
            var result = slot.Add(newItem, addAmount);

            Assert.That(result, Is.EqualTo(addAmount));
        }

        [Test]
        public void Add_EmptySlot_AddsItems()
        {
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var slot = this.slotFactory.Empty(stackSize);

            var item = this.itemFactory.CreateDefault();
            var amount = this.random.Next(1, stackSize);
            var result = slot.Add(item, amount);
            
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.EqualTo(0));
                Assert.That(slot.IsEmpty, Is.False);
            });
        }

        [Test]
        public void Add_SlotContainingSameItem_ReturnsNotAddedAmount()
        {
            var item = this.itemFactory.CreateDefault();
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var halfStackSize = stackSize / 2;
            var amount = this.random.Next(1, halfStackSize);
            var slot = this.slotFactory.WithItem(item, amount, stackSize);

            var newAmount = this.random.Next(1, stackSize - amount + 1);
            var result = slot.Add(item, newAmount);

            var expectedAmount = Math.Max(0, amount + newAmount - stackSize);
            Assert.That(result, Is.EqualTo(expectedAmount));
        }

        [Test]
        public void Add_SlotContainingSameItem_AddItems()
        {
            var item = this.itemFactory.CreateDefault();
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var halfStackSize = stackSize / 2;
            var amount = this.random.Next(1, halfStackSize);
            var slot = this.slotFactory.WithItem(item, amount, stackSize);

            var newAmount = this.random.Next(1, stackSize - amount);
            var newItem = this.itemFactory.CreateDefault();
            slot.Add(newItem, newAmount);

            Assert.That(slot.Amount, Is.EqualTo(amount + newAmount));
        }
    }
}
