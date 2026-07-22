using TheChest.Tests.Common.Attributes;
using TheChest.Tests.Common.Extensions.Slots;

namespace TheChest.Inventories.Tests.Slots.InventorySlot
{
    public partial class InventorySlotTests<T>
    {
        #region Null Item
        [Test]
        [IgnoreIfValueType]
        public void TryAdd_NullItem_ThrowsArgumentNullException()
        {
            var slot = this.slotFactory.Empty();

            Assert.That(() => slot.TryAdd(default!), Throws.ArgumentNullException);
        }
        #endregion

        #region Empty Slot
        [Test]
        public void TryAdd_EmptySlot_AddsItem()
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

        [Test]
        [IgnoreIfReferenceType]
        public void TryAdd_EmptySlot_DefaultItem_AddsItem()
        {
            var slot = this.slotFactory.Empty();

            var item = default(T);
            slot.TryAdd(item!);

            Assert.That(slot.GetContent(), Is.EqualTo(item));
        }

        [Test]
        [IgnoreIfReferenceType]
        public void TryAdd_EmptySlot_DefaultItem_ReturnsTrue()
        {
            var slot = this.slotFactory.Empty();

            var item = default(T);
            var result = slot.TryAdd(item!);

            Assert.That(result, Is.True);
        }
        #endregion

        #region Full Slot
        [Test]
        public void TryAdd_FullSlot_ReturnsFalse()
        {
            var item = this.itemFactory.CreateDefault();
            var slot = this.slotFactory.Full(item);

            var result = slot.TryAdd(this.itemFactory.CreateRandom());

            Assert.That(result, Is.False);
        }

        [Test]
        public void TryAdd_FullSlot_DoesNotAddItem()
        {
            var item = this.itemFactory.CreateDefault();
            var slot = this.slotFactory.Full(item);

            slot.TryAdd(this.itemFactory.CreateRandom());

            Assert.That(slot.GetContent(), Is.EqualTo(item));
        }

        [Test]
        [IgnoreIfReferenceType]
        public void TryAdd_DefaultItem_FullSlot_DoesNotAddItem()
        {
            var item = this.itemFactory.CreateDefault();
            var slot = this.slotFactory.Full(item);

            var paramItem = default(T);
            slot.TryAdd(paramItem!);

            Assert.That(slot.GetContent(), Is.EqualTo(item));
        }

        [Test]
        [IgnoreIfReferenceType]
        public void TryAdd_DefaultItem_FullSlot_ReturnsFalse()
        {
            var slot = this.slotFactory.Full(default!);

            var item = default(T);
            var result = slot.TryAdd(item!);

            Assert.That(result, Is.False);
        }
        #endregion
    }
}
