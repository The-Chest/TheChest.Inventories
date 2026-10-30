using NUnit.Framework;
using TheChest.Tests.Common.Attributes;

namespace TheChest.Inventories.Tests.Slots.InventorySlot
{
    public partial class InventorySlotTests<T>
    {
        [Test]
        public void TryGet_EmptySlot_ReturnsFalseAndDefaultItem()
        {
            var slot = this.slotFactory.Empty();

            var result = slot.TryGet(out var item);

            Assert.Multiple(() =>
            {
                Assert.That(result, Is.False);
                Assert.That(item, Is.EqualTo(default(T)));
            });
        }

        [Test]
        [IgnoreIfReferenceType]
        public void TryGet_FullSlot_DefaultItem_ReturnsFalseAndDefaultItem()
        {
            var slot = this.slotFactory.Full(default!);

            var result = slot.TryGet(out var item);

            Assert.Multiple(() =>
            {
                Assert.That(result, Is.False);
                Assert.That(item, Is.EqualTo(default(T)));
            });
        }

        [Test]
        public void TryGet_FullSlot_ReturnsTrueAndItem()
        {
            var expectedItem = this.itemFactory.CreateRandom();
            var slot = this.slotFactory.Full(expectedItem);

            var result = slot.TryGet(out var item);

            Assert.Multiple(() =>
            {
                Assert.That(result, Is.True);
                Assert.That(item, Is.EqualTo(expectedItem));
            });
        }

        [Test]
        public void TryGet_FullSlot_RemovesItem()
        {
            var slot = this.slotFactory.Full(this.itemFactory.CreateRandom());

            slot.TryGet(out _);

            Assert.That(slot.IsEmpty, Is.True);
        }
    }
}