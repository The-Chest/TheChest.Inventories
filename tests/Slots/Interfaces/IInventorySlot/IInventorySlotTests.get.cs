namespace TheChest.Inventories.Tests.Slots.Interfaces
{
    public partial class IInventorySlotTests<T>
    {
        [Test]
        public void GetOne_NoItem_ReturnsNull()
        {
            var slot = this.slotFactory.Empty();

            var result = slot.Get();

            Assert.That(result, Is.Null);
        }

        [Test]
        public void GetOne_FullSlot_ReturnsItem()
        {
            var item = this.itemFactory.CreateDefault();
            var slot = this.slotFactory.Full(item);

            var result = slot.Get();

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(item));
        }
    }
}
