using System;

namespace TheChest.Inventories.Tests.Slots.InventoryLazyStackSlot
{
    public partial class InventoryLazyStackSlotTests<T>
    {
        [TestCase(0)]
        [TestCase(-1)]
        public void TryGetByAmount_InvalidAmount_ThrowsArgumentOutOfRangeException(int amount)
        {
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var slot = this.slotFactory.Empty(stackSize);

            Assert.That(
                () => slot.TryGet(amount, out _),
                Throws.TypeOf<ArgumentOutOfRangeException>().With.Property("ParamName").EqualTo("amount")
            );
        }

        [Test]
        public void TryGetByAmount_EmptySlot_ReturnsFalseAndEmptyArray()
        {
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var slot = this.slotFactory.Empty(stackSize);

            var result = slot.TryGet(1, out var items);

            Assert.Multiple(() =>
            {
                Assert.That(result, Is.False);
                Assert.That(items, Is.Empty);
                Assert.That(slot.IsEmpty, Is.True);
            });
        }

        [Test]
        public void TryGetByAmount_InsufficientItems_ReturnsFalseAndAvailableItems()
        {
            var item = this.itemFactory.CreateDefault();
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var availableAmount = this.random.Next(1, stackSize);
            var slot = this.slotFactory.WithItem(item, availableAmount, stackSize);

            var result = slot.TryGet(availableAmount + 1, out var items);

            Assert.Multiple(() =>
            {
                Assert.That(result, Is.False);
                Assert.That(items, Has.Length.EqualTo(availableAmount));
                Assert.That(items, Has.All.EqualTo(item));
                Assert.That(slot.IsEmpty, Is.True);
            });
        }

        [Test]
        public void TryGetByAmount_SlotWithItems_ReturnsRequestedItemsAndRetainsRemainder()
        {
            var item = this.itemFactory.CreateDefault();
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var availableAmount = this.random.Next(2, stackSize);
            var requestedAmount = this.random.Next(1, availableAmount);
            var slot = this.slotFactory.WithItem(item, availableAmount, stackSize);

            var result = slot.TryGet(requestedAmount, out var items);

            Assert.Multiple(() =>
            {
                Assert.That(result, Is.True);
                Assert.That(items, Has.Length.EqualTo(requestedAmount));
                Assert.That(items, Has.All.EqualTo(item));
                Assert.That(slot.Amount, Is.EqualTo(availableAmount - requestedAmount));
            });
        }
    }
}