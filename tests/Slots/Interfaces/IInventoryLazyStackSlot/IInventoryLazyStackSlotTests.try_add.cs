namespace TheChest.Inventories.Tests.Slots.Interfaces
{
    public partial class IInventoryLazyStackSlotTests<T>
    {
        [Test]
        public void TryAdd_NullItem_ThrowsArgumentNullException()
        {
            var slot = this.slotFactory.Empty(this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST));

            Assert.Throws<ArgumentNullException>(() => slot.TryAdd(default!, 1));
        }

        [TestCase(-1)]
        [TestCase(0)]
        public void TryAdd_InvalidAmount_ThrowsArgumentOutOfRangeException(int amount)
        {
            var item = this.itemFactory.CreateDefault();
            var slot = this.slotFactory.Empty(this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST));

            Assert.Throws<ArgumentOutOfRangeException>(() => slot.TryAdd(item, amount));
        }

        [Test]
        public void TryAdd_FullSlot_ReturnsFalse()
        {
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var item = this.itemFactory.CreateDefault();
            var slot = this.slotFactory.FullSlot(item, stackSize);

            var result = slot.TryAdd(item, 1);

            Assert.That(result, Is.False);
        }

        [Test]
        public void TryAdd_NotEmptySlotAndDifferentItem_ReturnsFalse()
        {
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var amount = this.random.Next(1, stackSize);
            var item = this.itemFactory.CreateDefault();
            var newItem = this.itemFactory.CreateRandom();
            var slot = this.slotFactory.WithItem(item, amount, stackSize);

            var result = slot.TryAdd(newItem, 1);

            Assert.That(result, Is.False);
        }

        [Test]
        public void TryAdd_AmountBiggerThanAvailableAmount_ReturnsFalse()
        {
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var amount = this.random.Next(1, stackSize);
            var item = this.itemFactory.CreateDefault();
            var slot = this.slotFactory.WithItem(item, amount, stackSize);
            var addAmount = slot.AvailableAmount + 1;

            var result = slot.TryAdd(item, addAmount);

            Assert.That(result, Is.False);
        }

        [Test]
        public void TryAdd_AmountBiggerThanStackSizeOnEmptySlot_ReturnsFalse()
        {
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var item = this.itemFactory.CreateDefault();
            var slot = this.slotFactory.Empty(stackSize);
            var addAmount = stackSize + this.random.Next(1, 5);

            var result = slot.TryAdd(item, addAmount);

            Assert.That(result, Is.False);
        }

        [Test]
        public void TryAdd_ValidInput_ReturnsTrue()
        {
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var item = this.itemFactory.CreateDefault();
            var slot = this.slotFactory.Empty(stackSize);
            var amount = this.random.Next(1, stackSize);

            var result = slot.TryAdd(item, amount);

            Assert.That(result, Is.True);
        }
    }
}
