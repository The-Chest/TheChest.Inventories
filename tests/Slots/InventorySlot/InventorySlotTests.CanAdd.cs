using TheChest.Tests.Common.Attributes;

namespace TheChest.Inventories.Tests.Slots.InventorySlot
{
    public partial class InventorySlotTests<T>
    {
        #region Null and Default Item
        [Test]
        [IgnoreIfValueType]
        public void CanAdd_NullItem_ThrowsArgumentNullException()
        {
            var slot = this.slotFactory.Empty();

            var item = default(T);
            Assert.That(
                () => slot.CanAdd(item!),
                Throws.ArgumentNullException.With.Property("ParamName").EqualTo("item")
            );
        } 

        [Test]
        [IgnoreIfReferenceType]
        public void CanAdd_DefaultItem_ReturnsTrue()
        {
            var slot = this.slotFactory.Empty();

            var item = default(T);
            var result = slot.CanAdd(item!);

            Assert.That(result, Is.True);
        }
        #endregion

        #region Empty
        [Test]
        public void CanAdd_EmptySlot_ReturnsTrue()
        {
            var item = this.itemFactory.CreateDefault();
            var slot = this.slotFactory.Empty();

            var result = slot.CanAdd(item);

            Assert.That(result, Is.True);
        }
        #endregion

        #region Full
        [Test]
        public void CanAdd_FullSlot_ReturnsFalse()
        {
            var item = this.itemFactory.CreateDefault();
            var slot = this.slotFactory.Full(item);

            var result = slot.CanAdd(item);

            Assert.That(result, Is.False);
        }
        #endregion
    }
}
