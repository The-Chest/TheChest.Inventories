using TheChest.Tests.Common.Extensions.Slots;

namespace TheChest.Inventories.Tests.Slots.InventoryLazyStackSlot
{
    public partial class InventoryLazyStackSlotTests<T>
    {
        [Test]
        public void Replace_NullItem_ThrowsArgumentNullException()
        {
            var maxAmount = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var slot = this.slotFactory.Empty(maxAmount);

            Assert.Throws<ArgumentNullException>(() => slot.Replace(default!, 1));
        }

        [TestCase(0)]
        [TestCase(-1)]
        [TestCase(MAX_STACK_SIZE_TEST + 1)]
        public void Replace_InvalidAmount_ThrowsArgumentOutOfRangeException(int amount)
        {
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var slot = this.slotFactory.Empty(stackSize);
            var item = this.itemFactory.CreateDefault();

            Assert.Throws<ArgumentOutOfRangeException>(() => slot.Replace(item, amount));
        }

        [Test]
        public void Replace_EmptySlot_AddItems()
        {
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var slot = this.slotFactory.Empty(stackSize);
            var item = this.itemFactory.CreateDefault();

            int amount = this.random.Next(1, stackSize);
            slot.Replace(item, amount);

            Assert.Multiple(() =>
            {
                Assert.That(slot.IsEmpty, Is.False);
                Assert.That(slot.Amount, Is.EqualTo(amount));
                Assert.That(slot.GetContent(), Is.EqualTo(item));
            });
        }

        [Test]
        public void Replace_SlotWithDifferentItems_ReplacesContent()
        {
            var initialItem = this.itemFactory.CreateDefault();
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var halfStackSize = stackSize / 2;
            int initialAmount = this.random.Next(1, halfStackSize);
            var slot = this.slotFactory.WithItem(initialItem, initialAmount, stackSize);

            var newItem = this.itemFactory.CreateRandom();
            int newAmount = this.random.Next(1, halfStackSize);
            slot.Replace(newItem, newAmount);

            Assert.Multiple(() =>
            {
                Assert.That(slot.Amount, Is.EqualTo(newAmount));
                Assert.That(slot.GetContent(), Is.EqualTo(newItem));
            });
        }

        [Test]
        public void Replace_SlotWithSameItems_ReplacesContent()
        {
            var initialItem = this.itemFactory.CreateDefault();
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var halfStackSize = stackSize / 2;
            int initialAmount = this.random.Next(1, halfStackSize);
            var slot = this.slotFactory.WithItem(initialItem, initialAmount, stackSize);

            var newItem = this.itemFactory.CreateDefault();
            int newAmount = this.random.Next(1, halfStackSize);
            slot.Replace(newItem, newAmount);

            Assert.Multiple(() =>
            {
                Assert.That(slot.Amount, Is.EqualTo(newAmount));
                Assert.That(slot.GetContent(), Is.EqualTo(newItem));
            });
        }
    }
}
