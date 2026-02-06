namespace TheChest.Inventories.Tests.Slots
{
    public partial class IInventoryStackSlotTests<T>
    {
        [Test]
        public void AvailableAmount_EmptySlot_ReturnsMaxStackAmount()
        {
            var slot = this.slotFactory.EmptySlot();

            Assert.That(slot.AvailableAmount, Is.EqualTo(slot.MaxAmount));
        }

        [Test]
        public void AvailableAmount_PartiallyFilledSlot_ReturnsMaxStackAmountMinusAmount()
        {
            var item = this.itemFactory.CreateDefault();
            var amount = this.random.Next(1, 10);
            var maxAmount = this.random.Next(10, 20);
            var slot = this.slotFactory.WithItem(item, amount, maxAmount);

            Assert.That(slot.AvailableAmount, Is.EqualTo(slot.MaxAmount - slot.Amount));
        }

        [Test]
        public void AvailableAmount_FullSlot_ReturnsZero()
        {
            var item = this.itemFactory.CreateDefault();
            var maxAmount = this.random.Next(10, 20);
            var slot = this.slotFactory.WithItem(item, maxAmount, maxAmount);
            
            Assert.That(slot.AvailableAmount, Is.EqualTo(0));
        }
    }
}
