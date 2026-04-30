using TheChest.Tests.Common.Extensions.Slots;

namespace TheChest.Inventories.Tests.Slots.InventorySlot
{
    public partial class InventorySlotTests<T>
    {
        [Test]
        public void Replace_ReplacingItemOnEmptySlot_AddsItem()
        {
            var slot = this.slotFactory.Empty();
            var newItem = this.itemFactory.CreateDefault();

            slot.Replace(newItem);

            Assert.That(slot.GetContent(), Is.Not.Null.And.EqualTo(newItem));
        }

        [Test]
        public void Replace_ReplacingItemOnFullSlot_AddsItem()
        {
            var item = this.itemFactory.CreateDefault();
            var slot = this.slotFactory.Full(item);

            var newItem = this.itemFactory.CreateDefault();
            slot.Replace(newItem);

            Assert.That(slot.GetContent(), Is.Not.Null.And.EqualTo(newItem));
        }

        [Test]
        public void Replace_ReplacingItemOnEmptySlot_ReturnsNull_WhenReplacingDirectly()
        {
            var slot = this.slotFactory.Empty();
            var newItem = this.itemFactory.CreateDefault();

            var result = slot.Replace(newItem);

            Assert.That(result, Is.Null);
        }

        [Test]
        public void Replace_ReplacingItemOnFullSlot_ReturnsPreviousItem()
        {
            var existingItem = this.itemFactory.CreateDefault();
            var slot = this.slotFactory.Full(existingItem);
            var newItem = this.itemFactory.CreateRandom();

            var result = slot.Replace(newItem);

            Assert.That(result, Is.EqualTo(existingItem));
        }

        [Test]
        public void Replace_NullItem_ThrowsArgumentNullException()
        {
            var slot = this.slotFactory.Empty();

            Assert.That(() => slot.Replace(default!), Throws.ArgumentNullException);
        }
    }
}
