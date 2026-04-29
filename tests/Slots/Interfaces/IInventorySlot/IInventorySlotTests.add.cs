namespace TheChest.Inventories.Tests.Slots.Interfaces
{
    public partial class IInventorySlotTests<T>
    {
        [Test]
        public void Add_EmptySlot_ReturnsTrue()
        {
            var item = this.itemFactory.CreateDefault();
            var slot = this.slotFactory.Empty();

            var result = slot.Add(item);

            Assert.That(result, Is.True);
        }
        [Test]
        public void Add_FullSlot_ReturnsFalse()
        {
            var item = this.itemFactory.CreateDefault();
            var slot = this.slotFactory.Full(item);

            var result = slot.Add(item);

            Assert.That(result, Is.False);
        }
    }
}
