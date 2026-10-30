using NUnit.Framework;
using TheChest.Tests.Common.Attributes;

namespace TheChest.Inventories.Tests.Slots.InventorySlot
{
    public partial class InventorySlotTests<T>
    {
        [Test]
        public void CanGet_EmptySlot_ReturnsFalse()
        {
            var slot = this.slotFactory.Empty();

            var result = slot.CanGet();

            Assert.That(result, Is.False);
        }

        [Test]
        public void CanGet_FullSlot_ReturnsTrue()
        {
            var slot = this.slotFactory.Full(this.itemFactory.CreateRandom());

            var result = slot.CanGet();

            Assert.That(result, Is.True);
        }

        [Test]
        [IgnoreIfReferenceType]
        public void CanGet_FullSlot_DefaultItem_ReturnsFalse()
        {
            var item = default(T);
            var slot = this.slotFactory.Full(item!);

            var result = slot.CanGet();

            Assert.That(result, Is.False);
        }
    }
}