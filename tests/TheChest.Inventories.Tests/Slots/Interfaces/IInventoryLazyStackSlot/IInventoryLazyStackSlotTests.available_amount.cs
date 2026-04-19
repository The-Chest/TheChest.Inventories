namespace TheChest.Inventories.Tests.Slots.Interfaces
{
    public partial class IInventoryLazyStackSlotTests<T>
    {
        [Test]
        public void AvailableAmount_EmptySlot_ReturnsMaxStackAmount()
        {
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var slot = this.slotFactory.Empty(stackSize);

            Assert.That(slot.AvailableAmount, Is.EqualTo(slot.MaxAmount));
        }

        [Test]
        public void AvailableAmount_PartiallyFilledSlot_ReturnsMaxStackAmountMinusAmount()
        {
            var item = this.itemFactory.CreateDefault();
            var maxAmount = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var amount = this.random.Next(1, maxAmount - 1);
            var slot = this.slotFactory.WithItem(item, amount, maxAmount);

            Assert.That(slot.AvailableAmount, Is.EqualTo(slot.MaxAmount - slot.Amount));
        }

        [Test]
        public void AvailableAmount_FullSlot_ReturnsZero()
        {
            var item = this.itemFactory.CreateDefault();
            var maxAmount = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var slot = this.slotFactory.WithItem(item, maxAmount, maxAmount);

            Assert.That(slot.AvailableAmount, Is.EqualTo(0));
        }
    }
}
