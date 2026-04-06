namespace TheChest.Inventories.Tests.Slots.Interfaces
{
    public partial class IInventoryLazyStackSlotTests<T>
    {
        [Test]
        public void CanReplace_NullItem_ReturnsFalse()
        {
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var slot = this.slotFactory.EmptySlot(stackSize);

            var result = slot.CanReplace(default!, 1);

            Assert.That(result, Is.False);
        }

        [TestCase(0)]
        [TestCase(-1)]
        public void CanReplace_InvalidAmount_ReturnsFalse(int amount)
        {
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var slot = this.slotFactory.EmptySlot(stackSize);

            var item = this.itemFactory.CreateDefault();
            var result = slot.CanReplace(item, amount);

            Assert.That(result, Is.False);
        }

        [Test]
        public void CanReplace_AmountExceedingMaxStackAmount_ReturnsFalse()
        {
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var slot = this.slotFactory.EmptySlot(stackSize);

            var item = this.itemFactory.CreateDefault();
            var result = slot.CanReplace(item, stackSize + 1);

            Assert.That(result, Is.False);
        }

        [Test]
        public void CanReplace_EmptySlotValidItemAndAmount_ReturnsTrue()
        {
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var slot = this.slotFactory.EmptySlot(stackSize);

            var item = this.itemFactory.CreateDefault();
            var result = slot.CanReplace(item, stackSize / 2);

            Assert.That(result, Is.True);
        }

        [Test]
        public void CanReplace_FullSlotSlotValidItemAndAmount_ReturnsTrue()
        {
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var slot = this.slotFactory.FullSlot(this.itemFactory.CreateRandom(), stackSize);

            var item = this.itemFactory.CreateDefault();
            var result = slot.CanReplace(item, stackSize / 2);

            Assert.That(result, Is.True);
        }
    }
}
