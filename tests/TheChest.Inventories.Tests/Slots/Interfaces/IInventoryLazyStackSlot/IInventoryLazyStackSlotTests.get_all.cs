namespace TheChest.Inventories.Tests.Slots.Interfaces
{
    public abstract partial class IInventoryLazyStackSlotTests<T>
    {
        [Test]
        public void GetAll_EmptySlot_ReturnsEmptyArray()
        {
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var slot = this.slotFactory.Empty(stackSize);
            var result = slot.GetAll();

            Assert.That(result, Is.Empty);
        }

        [Test]
        public void GetAll_SlotWithContent_ReturnsAllItemsAndClearSlot()
        {
            var item = this.itemFactory.CreateDefault();
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var amount = this.random.Next(1, stackSize);
            var slot = this.slotFactory.WithItem(item, amount, stackSize);

            var result = slot.GetAll();

            Assert.That(result, Has.Length.EqualTo(amount));
            Assert.That(result, Has.All.EqualTo(item));
        }
    }
}
