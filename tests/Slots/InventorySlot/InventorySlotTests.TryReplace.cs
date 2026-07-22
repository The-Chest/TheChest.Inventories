using TheChest.Tests.Common.Attributes;
using TheChest.Tests.Common.Extensions.Slots;

namespace TheChest.Inventories.Tests.Slots.InventorySlot
{
    public partial class InventorySlotTests<T>
    {
        #region Null Item
        [Test]
        [IgnoreIfValueType]
        public void TryReplace_NullItem_ThrowsArgumentNullException()
        {
            var slot = this.slotFactory.Empty();

            Assert.That(
                () => slot.TryReplace(default!, out _),
                Throws.ArgumentNullException.With.Property("ParamName").EqualTo("item")
            );
        }
        #endregion

        #region Empty Slot
        [Test]
        public void TryReplace_EmptySlot_ReturnsFalse()
        {
            var slot = this.slotFactory.Empty();
            var item = this.itemFactory.CreateDefault();

            var result = slot.TryReplace(item, out _);

            Assert.That(result, Is.False);
        }

        [Test]
        public void TryReplace_EmptySlot_DoesNotAddsItem()
        {
            var slot = this.slotFactory.Empty();

            var item = this.itemFactory.CreateRandom();
            slot.TryReplace(item, out _);

            Assert.That(slot.GetContent(), Is.Not.EqualTo(item));
        }

        [Test]
        public void TryReplace_EmptySlot_SetsOldItemToDefault()
        {
            var slot = this.slotFactory.Empty();

            var item = this.itemFactory.CreateDefault();
            slot.TryReplace(item, out var oldItem);

            Assert.That(oldItem, Is.EqualTo(default(T)));
        }

        [Test]
        [IgnoreIfReferenceType]
        public void TryReplace_EmptySlot_DefaultItem_ReturnsFalse()
        {
            var slot = this.slotFactory.Empty();

            var item = default(T);
            var result = slot.TryReplace(item!, out _);

            Assert.That(result, Is.False);
        }

        [Test]
        [IgnoreIfReferenceType]
        public void TryReplace_EmptySlot_DefaultItem_DoesNotAddsItem()
        {
            var slot = this.slotFactory.Empty();

            var item = default(T);
            slot.TryReplace(item!, out _);

            Assert.Multiple(() =>
            {
                Assert.That(slot.GetContent(), Is.EqualTo(default(T)));
                Assert.That(slot.IsFull, Is.False);
            });
        }

        [Test]
        [IgnoreIfReferenceType]
        public void TryReplace_EmptySlot_DefaultItem_SetsOldItemToDefault()
        {
            var slot = this.slotFactory.Empty();

            var item = default(T);
            slot.TryReplace(item!, out var oldItem);

            Assert.That(oldItem, Is.EqualTo(default(T)));
        }
        #endregion

        #region Full Slot
        [Test]
        public void TryReplace_FullSlot_ReturnsTrue()
        {
            var initialItem = this.itemFactory.CreateRandom();
            var slot = this.slotFactory.Full(initialItem);
            var newItem = this.itemFactory.CreateDefault();

            var result = slot.TryReplace(newItem, out _);

            Assert.That(result, Is.True);
        }

        [Test]
        public void TryReplace_FullSlot_ReplacesItem()
        {
            var initialItem = this.itemFactory.CreateRandom();
            var slot = this.slotFactory.Full(initialItem);
            var newItem = this.itemFactory.CreateDefault();

            slot.TryReplace(newItem, out _);

            Assert.That(slot.GetContent(), Is.EqualTo(newItem));
        }

        [Test]
        public void TryReplace_FullSlot_SetsOldItemToInitialItem()
        {
            var initialItem = this.itemFactory.CreateRandom();
            var slot = this.slotFactory.Full(initialItem);
            var newItem = this.itemFactory.CreateDefault();

            slot.TryReplace(newItem, out var oldItem);

            Assert.That(oldItem, Is.EqualTo(initialItem));
        }

        [Test]
        [IgnoreIfReferenceType]
        public void TryReplace_DefaultItem_FullSlot_ReturnsTrue()
        {
            var item = this.itemFactory.CreateRandom();
            var slot = this.slotFactory.Full(item);

            var newItem = default(T);
            var result = slot.TryReplace(newItem!, out _);

            Assert.That(result, Is.True);
        }

        [Test]
        [IgnoreIfReferenceType]
        public void TryReplace_DefaultItem_FullSlot_AddsDefault()
        {
            var item = this.itemFactory.CreateRandom();
            var slot = this.slotFactory.Full(item);

            var newItem = default(T);
            slot.TryReplace(newItem!, out _);

            Assert.Multiple(() =>
            {
                Assert.That(slot.GetContent(), Is.EqualTo(default(T)));
                Assert.That(slot.IsFull, Is.True);
            });
        }

        [Test]
        [IgnoreIfReferenceType]
        public void TryReplace_DefaultItem_FullSlot_SetsOldItemToInitialItem()
        {
            var item = this.itemFactory.CreateRandom();
            var slot = this.slotFactory.Full(item);

            var newItem = default(T);
            slot.TryReplace(newItem!, out var oldItem);

            Assert.That(oldItem, Is.EqualTo(item));
        }

        [Test]
        [IgnoreIfReferenceType]
        public void TryReplace_DefaultItem_FullSlot_DefaultContent_ReturnsTrue()
        {
            var slot = this.slotFactory.Full(default!);

            var newItem = this.itemFactory.CreateRandom();
            var result = slot.TryReplace(newItem, out _);

            Assert.That(result, Is.True);
        }

        [Test]
        [IgnoreIfReferenceType]
        public void TryReplace_DefaultItem_FullSlot_DefaultContent_AddsItem()
        {
            var slot = this.slotFactory.Full(default!);

            var newItem = this.itemFactory.CreateRandom();
            slot.TryReplace(newItem, out _);

            Assert.That(slot.GetContent(), Is.EqualTo(newItem));
        }

        [Test]
        [IgnoreIfReferenceType]
        public void TryReplace_DefaultItem_FullSlot_DefaultContent_SetsOldItemToDefault()
        {
            var slot = this.slotFactory.Full(default!);

            var newItem = this.itemFactory.CreateRandom();
            slot.TryReplace(newItem, out var oldItem);

            Assert.That(oldItem, Is.EqualTo(default(T)));
        }
        #endregion
    }
}