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

        [Test]
        public void CanReplace_FullSlot_SameItem_ReturnsTrue()
        {
            var item = this.itemFactory.CreateRandom();
            var slot = this.slotFactory.FullSlot(item);

            var result = slot.CanReplace(item);

            Assert.That(result, Is.True);
        }

        [Test]
        public void CanReplace_FullSlot_DifferentItem_ReturnsTrue()
        {
            var originalItem = this.itemFactory.CreateRandom();
            var slot = this.slotFactory.FullSlot(originalItem);

            var differentItem = this.itemFactory.CreateDefault();
            var result = slot.CanReplace(differentItem);

            Assert.That(result, Is.True);
        }
    }
}
