using TheChest.Tests.Common.Attributes;
using TheChest.Tests.Common.Extensions.Slots;

namespace TheChest.Inventories.Tests.Slots.InventorySlot
{
    public partial class InventorySlotTests<T>
    {
        #region Null and Default Item Argument
        [Test]
        [IgnoreIfValueType]
        public void Replace_NullItem_ThrowsArgumentNullException()
        {
            var slot = this.slotFactory.Empty();

            Assert.That(
                () => slot.Replace(default!),
                Throws.ArgumentNullException.With.Property("ParamName").EqualTo("item")
            );
        }
        #endregion

        #region Empty Slot
        [Test]
        public void Replace_EmptySlot_ThrowsInvalidOperationException()
        {
            var slot = this.slotFactory.Empty();

            var newItem = this.itemFactory.CreateRandom();
            Assert.That(
                () => slot.Replace(newItem),
                Throws.InvalidOperationException.With.Message.EqualTo("The slot is empty.")
            );
        }

        [Test]
        public void Replace_EmptySlot_DoesNotReplacesItem()
        {
            var slot = this.slotFactory.Empty();

            var newItem = this.itemFactory.CreateRandom();
            
            Assert.That(() => slot.Replace(newItem), Throws.InvalidOperationException);
            Assert.That(slot.GetContent(), Is.Not.EqualTo(newItem));
        }
        #endregion

        #region Full Slot
        [Test]
        public void Replace_FullSlot_ReturnsOldItem()
        {
            var initialItem = this.itemFactory.CreateRandom();
            var slot = this.slotFactory.Full(initialItem);
            var newItem = this.itemFactory.CreateDefault();

            var result = slot.Replace(newItem);

            Assert.That(result, Is.EqualTo(initialItem));
        }

        [Test]
        public void Replace_FullSlot_AddsNewItem()
        {
            var initialItem = this.itemFactory.CreateDefault();
            var slot = this.slotFactory.Full(initialItem);
            var newItem = this.itemFactory.CreateRandom();

            slot.Replace(newItem);

            Assert.That(slot.GetContent(), Is.EqualTo(newItem));
        }

        [Test]
        [IgnoreIfReferenceType]
        public void Replace_DefaultItem_FullSlot_ReturnsItem()
        {
            var item = this.itemFactory.CreateRandom();
            var slot = this.slotFactory.Full(item);

            var newItem = default(T);
            var result = slot.Replace(newItem!);

            Assert.That(result, Is.EqualTo(item));
        }

        [Test]
        [IgnoreIfReferenceType]
        public void Replace_DefaultItem_FullSlot_AddsDefault()
        {
            var item = this.itemFactory.CreateRandom();
            var slot = this.slotFactory.Full(item);

            var newItem = default(T);
            slot.Replace(newItem!);

            Assert.Multiple(() =>
            {
                Assert.That(slot.GetContent(), Is.EqualTo(default(T)));
                Assert.That(slot.IsFull, Is.True);
            });
        }

        [Test]
        [IgnoreIfReferenceType]
        public void Replace_DefaultItem_FullSlot_DefaultContent_ReturnsDefault()
        {
            var slot = this.slotFactory.Full(default!);

            var newItem = this.itemFactory.CreateRandom();
            var result = slot.Replace(newItem);

            Assert.That(result, Is.EqualTo(default(T)));
        }

        [Test]
        [IgnoreIfReferenceType]
        public void Replace_DefaultItem_FullSlot_DefaultContent_AddsItem()
        {
            var slot = this.slotFactory.Full(default!);

            var newItem = this.itemFactory.CreateRandom();
            slot.Replace(newItem);

            Assert.Multiple(() =>
            {
                Assert.That(slot.GetContent(), Is.EqualTo(newItem));
                Assert.That(slot.IsFull, Is.True);
            });
        }
        #endregion
    }
}