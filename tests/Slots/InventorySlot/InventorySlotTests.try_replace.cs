using TheChest.Tests.Common.Extensions.Slots;

namespace TheChest.Inventories.Tests.Slots.InventorySlot
{
    public partial class InventorySlotTests<T>
    {
        [Test]
        public void TryReplace_NullItem_ThrowsArgumentNullException()
        {
            var slot = this.slotFactory.Empty();

            Assert.That(
                () => slot.TryReplace(default!, out _),
                Throws.ArgumentNullException.With.Property("ParamName").EqualTo("item")
            );
        }

        [Test]
        public void TryReplace_EmptySlot_AddsItem()
        {
            var slot = this.slotFactory.Empty();
            var item = this.itemFactory.CreateDefault();

            slot.TryReplace(item, out _);

            Assert.That(slot.GetContent(), Is.EqualTo(item));
        }

        [Test]
        public void TryReplace_EmptySlot_SetsOldItemToNull()
        {
            var slot = this.slotFactory.Empty();
            var item = this.itemFactory.CreateDefault();

            slot.TryReplace(item, out var oldItem);

            Assert.That(oldItem, Is.Null);
        }

        [Test]
        public void TryReplace_EmptySlot_ReturnsTrue()
        {
            var slot = this.slotFactory.Empty();
            var item = this.itemFactory.CreateDefault();

            var result = slot.TryReplace(item, out _);

            Assert.That(result, Is.True);
        }

        [Test]
        public void TryReplace_FullSlot_ReplacesItem()
        {
            var initialItem = this.itemFactory.CreateDefault();
            var slot = this.slotFactory.Full(initialItem);
            var newItem = this.itemFactory.CreateRandom();

            slot.TryReplace(newItem, out _);

            Assert.That(slot.GetContent(), Is.EqualTo(newItem));
        }

        [Test]
        public void TryReplace_FullSlot_SetsOldItemToPreviousItem()
        {
            var initialItem = this.itemFactory.CreateDefault();
            var slot = this.slotFactory.Full(initialItem);
            var newItem = this.itemFactory.CreateRandom();

            slot.TryReplace(newItem, out var oldItem);

            Assert.That(oldItem, Is.EqualTo(initialItem));
        }

        [Test]
        public void TryReplace_FullSlot_ReturnsTrue()
        {
            var initialItem = this.itemFactory.CreateDefault();
            var slot = this.slotFactory.Full(initialItem);
            var newItem = this.itemFactory.CreateRandom();

            var result = slot.TryReplace(newItem, out _);

            Assert.That(result, Is.True);
        }
    }
}
