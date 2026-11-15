using NUnit.Framework;

namespace TheChest.Inventories.Tests.Slots.InventoryStackSlot
{
    public partial class InventoryStackSlotTests<T>
    {
        [Test]
        public void TryGet_EmptySlot_ReturnsFalseAndEmptyArray()
        {
            var stackSize = this.GetRandomStackSize();
            var slot = this.slotFactory.Empty(stackSize);

            var result = slot.TryGet(out var items);

            Assert.That(result, Is.False);
            Assert.That(items, Is.Empty);
        }

        [Test]
        public void TryGet_FullSlot_ReturnsAllItemsAndEmptiesSlot()
        {
            var stackSize = this.GetRandomStackSize();
            var expectedItems = this.itemFactory.CreateManyRandom(stackSize);
            var slot = this.slotFactory.Full(expectedItems);

            var result = slot.TryGet(out var items);

            Assert.That(result, Is.True);
            Assert.That(items, Is.EqualTo(expectedItems));
            Assert.That(slot.IsEmpty, Is.True);
        }
    }
}
