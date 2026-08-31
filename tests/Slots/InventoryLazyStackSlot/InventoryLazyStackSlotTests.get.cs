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
