using TheChest.Tests.Common.Extensions.Slots;

namespace TheChest.Inventories.Tests.Slots.InventorySlot
{
    public partial class InventorySlotTests<T>
    {
        [Test]
        public void Add_EmptySlot_ChangesItem()
        {
            var item = this.itemFactory.CreateDefault();
            var slot = this.slotFactory.Empty();

            slot.Add(item);

            Assert.That(slot.GetContent(), Is.EqualTo(item));
        }

        [Test]
        public void Add_FullSlot_DoesNotChangeItem()
        {
            var item = this.itemFactory.CreateDefault();
            var slot = this.slotFactory.Full(item);

            var newItem = this.itemFactory.CreateRandom();
            Assert.Multiple(() =>
            {
                Assert.That(() => slot.Add(newItem), Throws.InvalidOperationException);
                Assert.That(slot.GetContent(), Is.Not.EqualTo(newItem));
            });
        }

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
        public void Add_NullItem_ThrowsArgumentNullException()
        {
            var slot = this.slotFactory.Empty();

            Assert.That(() => slot.Add(default!), Throws.ArgumentNullException);
        }

        [Test]
        public void Add_EmptySlot_ReturnsTrue_WhenAddingDirectly()
        {
            var slot = this.slotFactory.Empty();
            var item = this.itemFactory.CreateDefault();

            var result = slot.Add(item);

            Assert.That(result, Is.True);
        }
    }
}
