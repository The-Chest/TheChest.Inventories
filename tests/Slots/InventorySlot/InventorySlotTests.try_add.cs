using TheChest.Tests.Common.Extensions.Slots;

namespace TheChest.Inventories.Tests.Slots.InventorySlot
{
    public partial class InventorySlotTests<T>
    {
        [Test]
        public void TryAdd_NullItem_ThrowsArgumentNullException()
        {
            var slot = this.slotFactory.Empty();

            Assert.That(() => slot.TryAdd(default!), Throws.ArgumentNullException);
        }

        [Test]
        public void TryAdd_FullSlot_ReturnsFalse()
        {
            var item = this.itemFactory.CreateDefault();
            var slot = this.slotFactory.Full(item);

            var result = slot.TryAdd(this.itemFactory.CreateRandom());

            Assert.That(result, Is.False);
        }

        [Test]
        public void TryAdd_FullSlot_DoesNotChangeContent()
        {
            var item = this.itemFactory.CreateDefault();
            var slot = this.slotFactory.Full(item);

            slot.TryAdd(this.itemFactory.CreateRandom());

            Assert.That(slot.GetContent(), Is.EqualTo(item));
        }

        [Test]
        public void TryAdd_EmptySlot_ChangesContent()
        {
            var slot = this.slotFactory.Empty();
            var item = this.itemFactory.CreateDefault();

            slot.TryAdd(item);

            Assert.That(slot.GetContent(), Is.EqualTo(item));
        }

        [Test]
        public void TryAdd_EmptySlot_ReturnsTrue()
        {
            var slot = this.slotFactory.Empty();

            var result = slot.TryAdd(this.itemFactory.CreateDefault());

            Assert.That(result, Is.True);
        }
    }
}
