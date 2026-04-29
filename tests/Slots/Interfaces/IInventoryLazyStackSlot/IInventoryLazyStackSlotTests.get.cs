namespace TheChest.Inventories.Tests.Slots.Interfaces
{
    public partial class IInventoryLazyStackSlotTests<T>
    {
        [Test]
        public void Get_EmptySlot_ReturnsEmptyArray()
        {
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var slot = this.slotFactory.Empty(stackSize);

            var result = slot.Get(1);

            Assert.That(result, Is.Empty);
        }

        [Test]
        public void Get_AmountExceedingStackAmount_ReturnsAllItems()
        {
            var item = this.itemFactory.CreateDefault();
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var slot = this.slotFactory.WithItem(item, stackSize, stackSize);

            var result = slot.Get(stackSize + 1);

            Assert.That(result, Has.Length.EqualTo(stackSize));
        }

        [Test]
        public void Get_ValidAmount_ReturnsSpecifiedAmountOfItems()
        {
            var item = this.itemFactory.CreateDefault();
            var maxAmount = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var amount = this.random.Next(2, maxAmount);
            var slot = this.slotFactory.WithItem(item, amount, maxAmount);

            var result = slot.Get(amount - 1);

            Assert.That(result, Has.Length.EqualTo(amount - 1));
            Assert.That(result, Has.All.EqualTo(item));
        }

        [Test]
        public void Get_ValidAmount_ReducesStackAmount()
        {
            var item = this.itemFactory.CreateDefault();
            var maxAmount = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var amount = this.random.Next(2, maxAmount);
            var slot = this.slotFactory.WithItem(item, amount, maxAmount);

            var getAmount = this.random.Next(1, amount);
            slot.Get(getAmount);

            Assert.That(slot.Amount, Is.EqualTo(amount - getAmount));
        }
    }
}
