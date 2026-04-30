using TheChest.Tests.Common.Extensions.Slots;

namespace TheChest.Inventories.Tests.Slots.InventorySlot
{
    public partial class InventorySlotTests<T>
    {
        [Test]
        public void GetOne_FullSlot_RemovesItemFromSlot()
        {
            var item = this.itemFactory.CreateDefault();
            var slot = this.slotFactory.Full(item);

            slot.Get();

            Assert.That(slot.GetContent(), Is.Null);
        }

        [Test]
        public void GetOne_FullSlot_ReturnsExistingItem()
        {
            var item = this.itemFactory.CreateDefault();
            var slot = this.slotFactory.Full(item);

            var result = slot.Get();

            Assert.That(result, Is.EqualTo(item));
        }

        [Test]
        public void GetOne_EmptySlot_ReturnsNull()
        {
            var slot = this.slotFactory.Empty();

            var result = slot.Get();

            Assert.That(result, Is.Null);
        }
    }
}
