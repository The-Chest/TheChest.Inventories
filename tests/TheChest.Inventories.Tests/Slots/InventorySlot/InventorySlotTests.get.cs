using TheChest.Tests.Common.Extensions.Slots;

namespace TheChest.Inventories.Tests.Slots.InventorySlot
{
    public partial class InventorySlotTests<T>
    {
        [Test]
        public void GetOne_FullSlot_RemovesItemFromSlot()
        {
            var item = this.itemFactory.CreateDefault();
            var slot = this.slotFactory.FullSlot(item);

            slot.Get();

            Assert.That(slot.GetContent(), Is.Null);
        }
    }
}
