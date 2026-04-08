namespace TheChest.Inventories.Tests.Slots.Interfaces
{
    public partial class IInventoryLazyStackSlotTests<T>
    {
        [Test]
        public void CanAdd_NullItem_ReturnsFalse()
        {
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var slot = this.slotFactory.EmptySlot(stackSize);

            var result = slot.CanAdd(default!, 1);

            Assert.That(result, Is.False);
        }

        [Test]
        public void CanAdd_FullSlot_ReturnsFalse()
        {
            var item = this.itemFactory.CreateDefault();
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var slot = this.slotFactory.FullSlot(item, stackSize);

            var result = slot.CanAdd(item, 1);

            Assert.That(result, Is.False);
        }

        [TestCase(0)]
        [TestCase(-1)]
        public void CanAdd_InvalidAmount_ReturnsFalse(int amount)
        {
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var slot = this.slotFactory.EmptySlot(stackSize);

            var item = this.itemFactory.CreateDefault();
            var result = slot.CanAdd(item, amount);

            Assert.That(result, Is.False);
        }

        [Test]
        public void CanAdd_EmptySlotAndValidItem_ReturnsTrue()
        {
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var slot = this.slotFactory.EmptySlot(stackSize);

            var item = this.itemFactory.CreateDefault();
            var result = slot.CanAdd(item, 1);

            Assert.That(result, Is.True);
        }

        [Test]
        public void CanAdd_NotEmptySlotAndItemMatchesContent_ReturnsTrue()
        {
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var halfStackSize = stackSize / 2;
            var item = this.itemFactory.CreateDefault();
            var slot = this.slotFactory.WithItem(item, halfStackSize, stackSize);

            var secondItem = this.itemFactory.CreateDefault();
            var result = slot.CanAdd(secondItem, halfStackSize);

            Assert.That(result, Is.True);
        }

        [Test]
        public void CanAdd_NotEmptySlotAndItemDoesNotMatchContent_ReturnsFalse()
        {
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var halfStackSize = stackSize / 2;
            var item = this.itemFactory.CreateDefault();
            var slot = this.slotFactory.WithItem(item, halfStackSize, stackSize);

            var secondItem = this.itemFactory.CreateRandom();

            var result = slot.CanAdd(secondItem, halfStackSize);

            Assert.That(result, Is.False);
        }
    }
}
