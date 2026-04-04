using TheChest.Tests.Common.Extensions.Slots;

namespace TheChest.Inventories.Tests.Slots
{
    public partial class InventorySlotTests<T>
    {
        [Test]
        public void Add_EmptySlot_ChangesItem()
        {
            var item = this.itemFactory.CreateDefault();
            var slot = this.slotFactory.EmptySlot();

            slot.Add(item);

            Assert.That(slot.GetContent(), Is.EqualTo(item));
        }

        [Test]
        public void Add_FullSlot_DoesNotChangeItem()
        {
            var item = this.itemFactory.CreateDefault();
            var slot = this.slotFactory.FullSlot(item);

            var newItem = this.itemFactory.CreateRandom();
            slot.Add(newItem);

            Assert.That(slot.GetContent(), Is.Not.EqualTo(newItem));
        }
    }
}
