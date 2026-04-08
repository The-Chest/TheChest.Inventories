namespace TheChest.Inventories.Tests.Slots.Interfaces
{
    public partial class IInventorySlotTests<T>
    {
        [Test]
        public void Replace_ReplacingItemOnEmptySlot_ReturnsNull()
        {
            var slot = this.slotFactory.EmptySlot();
            var newItem = this.itemFactory.CreateDefault();

            var result = slot.Replace(newItem);

            Assert.That(result, Is.Null);
        }

        [Test]
        public void Replace_ReplacingItemOnFullSlot_ReturnsItem()
        {
            var item = this.itemFactory.CreateDefault();
            var slot = this.slotFactory.FullSlot(item);

            var newItem = this.itemFactory.CreateDefault();
            var result = slot.Replace(newItem);

            Assert.That(result, Is.Not.Null.And.EqualTo(item));
        }
    }
}
