namespace TheChest.Inventories.Tests.Slots.Interfaces
{
    public partial class IInventoryStackSlotTests<T>
    {
        [Test]
        public void Get_NoItem_ReturnsNull()
        {
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var slot = this.slotFactory.Empty(stackSize);

            var result = slot.Get();

            Assert.That(result, Is.Null);
        }

        [Test]
        public void Get_FullSlot_ReturnsItem()
        {
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var items = this.itemFactory.CreateMany(stackSize);
            var slot = this.slotFactory.Full(items);

            var result = slot.Get();

            Assert.That(result, Is.EqualTo(items[0]));
        }

        [Test]
        public void Get_FullSlot_DecreasesAmountByOne()
        {
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var items = this.itemFactory.CreateMany(stackSize);
            var slot = this.slotFactory.Full(items);

            slot.Get();

            Assert.That(slot.Amount, Is.EqualTo(stackSize - 1));
        }
    }
}
