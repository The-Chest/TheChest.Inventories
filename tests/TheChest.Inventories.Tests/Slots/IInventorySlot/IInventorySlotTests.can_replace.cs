namespace TheChest.Inventories.Tests.Slots
{
    public partial class IInventorySlotTests<T>
    {
        [Test]
        public void CanReplace_NullItem_ReturnsFalse()
        {
            var slot = this.slotFactory.EmptySlot();

            var result = slot.CanReplace(default!);

            Assert.That(result, Is.False);
        }

        [Test]
        public void CanReplace_EmptySlot_ReturnsTrue()
        {
            var slot = this.slotFactory.EmptySlot();
            var newItem = this.itemFactory.CreateDefault();

            var result = slot.CanReplace(newItem);

            Assert.That(result, Is.True);
        }

        [Test]
        public void CanReplace_FullSlot_ReturnsTrue()
        {
            var item = this.itemFactory.CreateRandom();
            var slot = this.slotFactory.FullSlot(item);

            var newItem = this.itemFactory.CreateDefault();
            var result = slot.CanReplace(newItem);

            Assert.That(result, Is.True);
        }
    }
}
