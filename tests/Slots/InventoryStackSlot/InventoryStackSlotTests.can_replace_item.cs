using TheChest.Tests.Common.Attributes;

namespace TheChest.Inventories.Tests.Slots.InventoryStackSlot
{
    public partial class InventoryStackSlotTests<T>
    {
        [Test]
        [IgnoreIfValueType]
        public void CanReplaceItem_NullItem_ReturnsFalse()
        {
            var stackSize = this.GetRandomStackSize();
            var slot = this.slotFactory.Empty(stackSize);

            var item = (T)default!;

            Assert.That(slot.CanReplace(item), Is.False);
        }

        [Test]
        [IgnoreIfReferenceType]
        public void CanReplaceItem_DefaultValue_ReturnsTrue()
        {
            var stackSize = this.GetRandomStackSize();
            var slot = this.slotFactory.Empty(stackSize);

            var item = (T)default!;

            Assert.That(slot.CanReplace(item), Is.True);
        }

        [Test]
        public void CanReplaceItem_NotNullItem_ReturnsTrue()
        {
            var stackSize = this.GetRandomStackSize();
            var slot = this.slotFactory.Empty(stackSize);

            var item = this.itemFactory.CreateDefault();

            Assert.That(slot.CanReplace(item), Is.True);
        }
    }
}
