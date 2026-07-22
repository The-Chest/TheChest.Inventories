using TheChest.Tests.Common.Attributes;
using TheChest.Tests.Common.Extensions.Slots;

namespace TheChest.Inventories.Tests.Slots.InventorySlot
{
    public partial class InventorySlotTests<T>
    {
        #region Empty
        [Test]
        public void GetOne_EmptySlot_ThrowsInvalidOperationException()
        {
            var slot = this.slotFactory.Empty();

            Assert.That(
                () => slot.Get(), 
                Throws.InvalidOperationException.With.Message.EqualTo("The slot is empty.")
            );
        }
        #endregion

        #region Full
        [Test]
        [IgnoreIfValueType]
        public void GetOne_FullSlot_ReturnsItem()
        {
            var item = this.itemFactory.CreateDefault();
            var slot = this.slotFactory.Full(item);

            var result = slot.Get();

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(item));
        }

        [Test]
        [IgnoreIfValueType]
        public void GetOne_FullSlot_RemovesItemFromSlot()
        {
            var item = this.itemFactory.CreateDefault();
            var slot = this.slotFactory.Full(item);

            slot.Get();

            Assert.Multiple(() =>
            {
                Assert.That(slot.IsEmpty, Is.True);
                Assert.That(slot.GetContent(), Is.Null);
            });
        }

        [Test]
        [IgnoreIfReferenceType]
        public void GetOne_FullSlot_DefaultValue_RemovesItemFromSlot()
        {
            var item = default(T);
            var slot = this.slotFactory.Full(item!);

            slot.Get();

            Assert.Multiple(() =>
            {
                Assert.That(slot.IsEmpty, Is.True);
                Assert.That(slot.GetContent(), Is.EqualTo(default(T)));
            });
        }

        [Test]
        [IgnoreIfReferenceType]
        public void GetOne_FullSlot_DefaultValue_ReturnsItem()
        {
            var item = default(T);
            var slot = this.slotFactory.Full(item!);

            var result = slot.Get();

            Assert.That(result, Is.EqualTo(item));
        }
        #endregion
    }
}
