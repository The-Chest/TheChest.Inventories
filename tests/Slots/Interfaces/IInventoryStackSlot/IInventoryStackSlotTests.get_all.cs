namespace TheChest.Inventories.Tests.Slots.Interfaces
{
    public partial class IInventoryStackSlotTests<T>
    {
        [Test]
        public void GetAll_RemovesContentFromSlot()
        {
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var items = this.itemFactory.CreateMany(stackSize);
            var slot = this.slotFactory.Full(items);

            slot.GetAll();

            Assert.That(slot.IsEmpty, Is.True);
        }

        [Test]
        public void GetAll_DecreasesAmountToZero()
        {
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var items = this.itemFactory.CreateMany(stackSize);
            var slot = this.slotFactory.Full(items);

            slot.GetAll();

            Assert.That(slot.Amount, Is.EqualTo(0));
        }

        [Test]
        public void GetAll_FullSlot_ReturnsAllItemFromFullSlot()
        {
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var items = this.itemFactory.CreateMany(stackSize);
            var slot = this.slotFactory.Full(items);

            var result = slot.GetAll();
        
            Assert.That(result, Is.EquivalentTo(items));
        }

        [Test]
        public void GetAll_SlotWithItems_ReturnsAllItemFromSlot()
        {
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var items = this.itemFactory.CreateMany(stackSize / 2);
            var slot = this.slotFactory.WithItems(items, stackSize);

            var result = slot.GetAll();

            Assert.That(result, Is.EquivalentTo(items));
        }

        [Test]
        public void GetAll_EmptySlot_ReturnsEmptyArray()
        {
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var slot = this.slotFactory.Empty(stackSize);

            var result = slot.GetAll();

            Assert.That(result, Is.EquivalentTo(Array.Empty<T>()));
        }
    }
}
