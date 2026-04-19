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
    }
}
