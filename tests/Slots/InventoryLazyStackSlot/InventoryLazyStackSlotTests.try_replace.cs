using TheChest.Tests.Common.Extensions.Slots;

namespace TheChest.Inventories.Tests.Slots.InventoryLazyStackSlot
{
    public partial class InventoryLazyStackSlotTests<T>
    {
        [Test]
        public void TryReplace_NullItem_ThrowsArgumentNullException()
        {
            var maxAmount = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var slot = this.slotFactory.Empty(maxAmount);

            Assert.That(
                () => slot.TryReplace(default!, 1, out _),
                Throws.ArgumentNullException.With.Property("ParamName").EqualTo("item")
            );
        }

        [TestCase(0)]
        [TestCase(-1)]
        [TestCase(MAX_STACK_SIZE_TEST + 1)]
        public void TryReplace_InvalidAmount_ThrowsArgumentOutOfRangeException(int amount)
        {
            var maxAmount = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var slot = this.slotFactory.Empty(maxAmount);
            var item = this.itemFactory.CreateDefault();

            Assert.That(
                () => slot.TryReplace(item, amount, out _),
                Throws.Exception.TypeOf<ArgumentOutOfRangeException>().With.Property("ParamName").EqualTo("amount")
            );
        }

        [Test]
        public void TryReplace_EmptySlot_SetsOldItemsToEmptyArray()
        {
            var maxAmount = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var slot = this.slotFactory.Empty(maxAmount);

            var item = this.itemFactory.CreateDefault();
            var amount = this.random.Next(1, maxAmount);
            slot.TryReplace(item, amount, out var oldItems);

            Assert.That(oldItems, Is.Empty);
        }

        [Test]
        public void TryReplace_EmptySlot_SetsContent()
        {
            var maxAmount = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var slot = this.slotFactory.Empty(maxAmount);

            var item = this.itemFactory.CreateDefault();
            var amount = this.random.Next(1, maxAmount);
            slot.TryReplace(item, amount, out _);

            Assert.That(slot.GetContent(), Is.EqualTo(item));
        }

        [Test]
        public void TryReplace_EmptySlot_SetsAmount()
        {
            var maxAmount = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var slot = this.slotFactory.Empty(maxAmount);

            var item = this.itemFactory.CreateDefault();
            var amount = this.random.Next(1, maxAmount);
            slot.TryReplace(item, amount, out _);

            Assert.That(slot.Amount, Is.EqualTo(amount));
        }

        [Test]
        public void TryReplace_EmptySlot_ReturnsTrue()
        {
            var maxAmount = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var slot = this.slotFactory.Empty(maxAmount);

            var item = this.itemFactory.CreateDefault();
            var amount = this.random.Next(1, maxAmount);
            var result = slot.TryReplace(item, amount, out _);

            Assert.That(result, Is.True);
        }

        [Test]
        public void TryReplace_SlotWithItems_SetsOldItemsToPreviousItems()
        {
            var maxAmount = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var initialItem = this.itemFactory.CreateDefault();
            var initialAmount = this.random.Next(1, maxAmount);
            var expectedOldItems = Enumerable.Repeat(initialItem, initialAmount).ToArray();
            var slot = this.slotFactory.WithItem(initialItem, initialAmount, maxAmount);

            var item = this.itemFactory.CreateRandom();
            var amount = this.random.Next(1, maxAmount);
            slot.TryReplace(item, amount, out var oldItems);

            Assert.That(oldItems, Is.EqualTo(expectedOldItems));
        }

        [Test]
        public void TryReplace_SlotWithItems_ReplacesContent()
        {
            var maxAmount = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var initialItem = this.itemFactory.CreateDefault();
            var initialAmount = this.random.Next(1, maxAmount);
            var slot = this.slotFactory.WithItem(initialItem, initialAmount, maxAmount);

            var item = this.itemFactory.CreateRandom();
            var amount = this.random.Next(1, maxAmount);
            slot.TryReplace(item, amount, out _);

            Assert.That(slot.GetContent(), Is.EqualTo(item));
        }

        [Test]
        public void TryReplace_SlotWithItems_ReplacesAmount()
        {
            var maxAmount = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var initialItem = this.itemFactory.CreateDefault();
            var initialAmount = this.random.Next(1, maxAmount);
            var slot = this.slotFactory.WithItem(initialItem, initialAmount, maxAmount);

            var item = this.itemFactory.CreateRandom();
            var amount = this.random.Next(1, maxAmount);
            slot.TryReplace(item, amount, out _);

            Assert.That(slot.Amount, Is.EqualTo(amount));
        }

        [Test]
        public void TryReplace_SlotWithItems_ReturnsTrue()
        {
            var maxAmount = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var initialItem = this.itemFactory.CreateDefault();
            var initialAmount = this.random.Next(1, maxAmount);
            var slot = this.slotFactory.WithItem(initialItem, initialAmount, maxAmount);

            var item = this.itemFactory.CreateRandom();
            var amount = this.random.Next(1, maxAmount);
            var result = slot.TryReplace(item, amount, out _);

            Assert.That(result, Is.True);
        }
    }
}
