using TheChest.Tests.Common.Extensions.Slots;

namespace TheChest.Inventories.Tests.Slots
{
    public partial class InventoryStackSlotTests<T>
    {
        [TestCase(0)]
        [TestCase(-1)]
        public void GetAmount_InvalidAmount_ThrowsArgumentExceptions(int amount)
        {
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var items = this.itemFactory.CreateMany(stackSize);
            var slot = this.slotFactory.FullSlot(items);

            Assert.That(() => slot.Get(amount), Throws.TypeOf<ArgumentOutOfRangeException>());
        }

        [Test]
        public void GetAmount_SlotWithEnoughItems_RemovesFromContent()
        {
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var items = this.itemFactory.CreateMany(stackSize);
            var slot = this.slotFactory.FullSlot(items);

            var amount = this.random.Next(1, stackSize);
            slot.Get(amount);

            Assert.That(slot.GetContents()[amount..stackSize], Is.EquivalentTo(items[amount..stackSize]));
        }

        [Test]
        public void GetAmount_SlotWithNotEnoughItems_RemovesFromContent()
        {
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var halfStack = stackSize / 2;
            var items = this.itemFactory.CreateMany(halfStack);
            var slot = this.slotFactory.WithItems(items, stackSize);

            var amount = this.random.Next(halfStack + 1, stackSize);
            slot.Get(amount);

            Assert.That(slot.GetContents(), Is.All.Null);
        }

        [Test]
        public void GetAmount_AmountBiggerThanSlotMaxAmount_RemovesAllFromContent()
        {
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var items = this.itemFactory.CreateMany(stackSize);
            var slot = this.slotFactory.FullSlot(items);

            slot.Get(stackSize * 2);

            Assert.That(slot.GetContents(), Has.All.Null);
        }
    }
}
