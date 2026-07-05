using TheChest.Tests.Common.Attributes;
using TheChest.Tests.Common.Extensions.Slots;

namespace TheChest.Inventories.Tests.Slots.InventorySlot
{
    public partial class InventorySlotTests<T>
    {
        #region Null and Default Item Argument
        [Test]
        [IgnoreIfValueType]
        public void Add_NullItem_ThrowsArgumentNullException()
        {
            var slot = this.slotFactory.Empty();

            Assert.That(
                () => slot.Add(default!),
                Throws.ArgumentNullException.With.Property("ParamName").EqualTo("item")
            );
        }
        #endregion

        #region Empty Slot
        [Test]
        public void Add_EmptySlot_ReturnsTrue()
        {
            var item = this.itemFactory.CreateDefault();
            var slot = this.slotFactory.Empty();

            var result = slot.Add(item);

            Assert.That(result, Is.True);
        }

        [Test]
        public void Add_EmptySlot_AddsItem()
        {
            var slot = this.slotFactory.Empty();

            var item = this.itemFactory.CreateDefault();
            slot.Add(item);

            Assert.That(slot.GetContent(), Is.EqualTo(item));
        }

        [Test]
        [IgnoreIfReferenceType]
        public void Add_EmptySlot_DefaultItem_AddsItem()
        {
            var slot = this.slotFactory.Empty();

            var item = default(T);
            slot.Add(item!);

            Assert.That(slot.GetContent(), Is.EqualTo(item));
        }
        #endregion

        #region Full Slot
        [Test]
        public void Add_FullSlot_ThrowsInvalidOperationException()
        {
            var item = this.itemFactory.CreateDefault();
            var slot = this.slotFactory.Full(item);

            var newItem = this.itemFactory.CreateRandom();
            Assert.That(
                () => slot.Add(newItem), 
                Throws.InvalidOperationException.With.Message.EqualTo("The slot is already full.")
            );
        }

        [Test]
        public void Add_FullSlot_DoesNotAddItem()
        {
            var item = this.itemFactory.CreateDefault();
            var slot = this.slotFactory.Full(item);

            Assert.Multiple(() =>
            {
                var newItem = this.itemFactory.CreateRandom();
                Assert.That(() => slot.Add(newItem), Throws.InvalidOperationException);
                Assert.That(slot.GetContent(), Is.Not.EqualTo(newItem));
            });
        }

        [Test]
        [IgnoreIfReferenceType]
        public void Add_DefaultItem_FullSlot_ThrowsInvalidOperationException()
        {
            var slot = this.slotFactory.Full(default!);

            var item = default(T);
            Assert.That(
                () => slot.Add(item!),
                Throws.InvalidOperationException.With.Message.EqualTo("The slot is already full.")
            );
        }

        [Test]
        [IgnoreIfReferenceType]
        public void Add_DefaultItem_FullSlot_DoesNotAddsItem()
        {
            var item = this.itemFactory.CreateRandom();
            var slot = this.slotFactory.Full(item);

            Assert.Multiple(() =>
            {
                var paramItem = default(T);
                Assert.That(() => slot.Add(paramItem!), Throws.InvalidOperationException);
                Assert.That(slot.GetContent(), Is.Not.EqualTo(paramItem));
            });
        }
        #endregion
    }
}
