using NUnit.Framework;

namespace TheChest.Inventories.Tests.Slots.InventoryLazyStackSlot
{
    public partial class InventoryLazyStackSlotTests<T>
    {
        [Test]
        public void TryGet_EmptySlot_ReturnsFalseAndEmptyArray()
        {
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var slot = this.slotFactory.Empty(stackSize);

            var result = slot.TryGet(out var items);

            Assert.Multiple(() =>
            {
                Assert.That(result, Is.False);
                Assert.That(items, Is.Empty);
                Assert.That(slot.IsEmpty, Is.True);
            });
        }

        [Test]
        public void TryGet_SlotWithContent_ReturnsAllItemsAndClearsSlot()
        {
            var item = this.itemFactory.CreateDefault();
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var amount = this.random.Next(1, stackSize);
            var slot = this.slotFactory.WithItem(item, amount, stackSize);

            var result = slot.TryGet(out var items);

            Assert.Multiple(() =>
            {
                Assert.That(result, Is.True);
                Assert.That(items, Has.Length.EqualTo(amount));
                Assert.That(items, Has.All.EqualTo(item));
                Assert.That(slot.IsEmpty, Is.True);
            });
        }
    }
}