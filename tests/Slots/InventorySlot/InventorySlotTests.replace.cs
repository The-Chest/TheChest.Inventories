using TheChest.Tests.Common.Attributes;
using TheChest.Tests.Common.Extensions.Slots;

namespace TheChest.Inventories.Tests.Slots.InventorySlot
{
    public partial class InventorySlotTests<T>
    {
        [Test]
        [IgnoreIfValueType]
        public void Replace_NullItem_ThrowsArgumentNullException()
        {
            var slot = this.slotFactory.Empty();

            Assert.That(() => slot.Replace(default!), Throws.ArgumentNullException);
        }

        [Test]
        [IgnoreIfReferenceType]
        public void Replace_EmptySlot_DefaultItem_ReturnsDefault()
        {
            var slot = this.slotFactory.Empty();

            var newItem = default(T);
            var result = slot.Replace(newItem!);

            Assert.That(result, Is.EqualTo(default(T)));
        }

        [Test]
        [IgnoreIfReferenceType]
        public void Replace_EmptySlot_DefaultItem_AddsDefault()
        {
            var slot = this.slotFactory.Empty();

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
        public void Replace_FullSlot_DefaultItem_ReturnsItem()
        {
            var item = this.itemFactory.CreateRandom();
            var slot = this.slotFactory.Full(item);

            var newItem = default(T);
            var result = slot.Replace(newItem!);

            Assert.That(result, Is.EqualTo(item));
        }

        [Test]
        [IgnoreIfReferenceType]
        public void Replace_FullSlot_DefaultItem_AddsDefault()
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
        [IgnoreIfValueType]
        public void Replace_EmptySlot_ReturnsNull()
        {
            var slot = this.slotFactory.Empty();

            var newItem = this.itemFactory.CreateDefault();
            var result = slot.Replace(newItem);

            Assert.That(result, Is.Null);
        }

        [Test]
        [IgnoreIfValueType]
        public void Replace_EmptySlot_AddsItem()
        {
            var slot = this.slotFactory.Empty();

            var newItem = this.itemFactory.CreateDefault();
            slot.Replace(newItem);

            Assert.That(slot.GetContent(), Is.EqualTo(newItem));
        }

        [Test]
        public void Replace_FullSlot_ReturnsItem()
        {
            var item = this.itemFactory.CreateRandom();
            var slot = this.slotFactory.Full(item);

            var newItem = this.itemFactory.CreateDefault();
            var result = slot.Replace(newItem);

            Assert.That(result, Is.EqualTo(item));
        }

        [Test]
        public void Replace_FullSlot_AddsItem()
        {
            var item = this.itemFactory.CreateDefault();
            var slot = this.slotFactory.Full(item);

            var newItem = this.itemFactory.CreateRandom();
            slot.Replace(newItem);

            Assert.That(slot.GetContent(), Is.EqualTo(newItem));
        }
    }
}
