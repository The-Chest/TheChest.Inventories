namespace TheChest.Inventories.Tests.Slots
{
    public partial class IInventorySlotTests<T>
    {
        [Test]
        public void CanAdd_NullItem_ReturnsFalse()
        {
            var slot = this.slotFactory.EmptySlot();

            var result = slot.CanAdd(default!);

            Assert.That(result, Is.False);
        }

        [Test]
        public void CanAdd_EmptySlot_ReturnsTrue()
        {
            var item = this.itemFactory.CreateDefault();
            var slot = this.slotFactory.EmptySlot();

            var result = slot.CanAdd(item);

            Assert.That(result, Is.True);
        }

        [Test]
        public void CanAdd_FullSlot_ReturnsFalse()
        {
            var item = this.itemFactory.CreateDefault();
            var slot = this.slotFactory.FullSlot(item);

            var result = slot.CanAdd(item);

            Assert.That(result, Is.False);
        }
    }
}
