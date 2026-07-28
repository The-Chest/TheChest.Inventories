using TheChest.Tests.Common.Attributes;

namespace TheChest.Inventories.Tests.Slots.InventoryStackSlot
{
    public partial class InventoryStackSlotTests<T>
    {
        [Test]
        public void AvailableAmount_Empty_ReturnsMaxStackAmount()
        {
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var slot = this.slotFactory.Empty(stackSize);

            Assert.That(slot.AvailableAmount, Is.EqualTo(slot.MaxAmount));
        }

        [Test]
        [IgnoreIfReferenceType]
        public void AvailableAmount_PartiallyFilled_ValueType_ReturnsMaxStackAmountMinusAmount()
        {
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var amount = this.random.Next(1, stackSize);
            var slot = this.slotFactory.WithItem(default!, amount, stackSize);

            Assert.That(slot.AvailableAmount, Is.EqualTo(slot.MaxAmount - slot.Amount));
        }

        [Test]
        public void AvailableAmount_PartiallyFilled_ReturnsMaxStackAmountMinusAmount()
        {
            var item = this.itemFactory.CreateRandom();
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var amount = this.random.Next(1, stackSize);
            var slot = this.slotFactory.WithItem(item, amount, stackSize);

            Assert.That(slot.AvailableAmount, Is.EqualTo(slot.MaxAmount - slot.Amount));
        }

        [Test]
        public void AvailableAmount_Full_ReturnsZero()
        {
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var items = this.itemFactory.CreateManyRandom(stackSize);
            var slot = this.slotFactory.Full(items);

            Assert.That(slot.AvailableAmount, Is.EqualTo(0));
        }
        // TODO: why are these features inverse?
        // Maybe the Factory has a problem :(
        [Test]
        [IgnoreIfReferenceType]
        public void AvailableAmount_Full_ValueType_ReturnsMaxAmount()
        {
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var items = Enumerable.Repeat(default(T), stackSize).ToArray();
            var slot = this.slotFactory.Full(items!);

            Assert.That(slot.AvailableAmount, Is.EqualTo(stackSize));
        }

        [Test]
        [IgnoreIfValueType]
        public void AvailableAmount_Full_ReferenceType_ReturnsZero()
        {
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var items = Enumerable.Repeat(default(T), stackSize).ToArray();
            var slot = this.slotFactory.Full(items!);

            Assert.That(slot.AvailableAmount, Is.Zero);
        }
    }
}
