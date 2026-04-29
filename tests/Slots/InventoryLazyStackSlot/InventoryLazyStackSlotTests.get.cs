using TheChest.Tests.Common.Extensions.Slots;

namespace TheChest.Inventories.Tests.Slots.InventoryLazyStackSlot
{
    public partial class InventoryLazyStackSlotTests<T>
    {
        [TestCase(0)]
        [TestCase(-1)]
        public void Get_InvalidAmount_ThrowsArgumentOutOfRangeException(int amount)
        {
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var slot = this.slotFactory.Empty(stackSize);

            Assert.Throws<ArgumentOutOfRangeException>(() => slot.Get(amount));
        }

        [Test]
        public void Get_NotEmptySlotAndDifferentItem_RemovesItemFromSlot()
        {
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var item = this.itemFactory.CreateDefault();
            var slot = this.slotFactory.WithItem(item, stackSize, stackSize);
            
            var amount = this.random.Next(1, stackSize);
            slot.Get(amount);

            Assert.That(slot.GetContents(), Has.Length.EqualTo(stackSize - amount));
        }


        [Test]
        public void Get_AmountExceedingStackAmount_ClearsSlot()
        {
            var item = this.itemFactory.CreateDefault();
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var slot = this.slotFactory.WithItem(item, stackSize, stackSize);

            slot.Get(stackSize + 1);

            Assert.That(slot.GetContents(), Is.Empty);
        }
    }
}
