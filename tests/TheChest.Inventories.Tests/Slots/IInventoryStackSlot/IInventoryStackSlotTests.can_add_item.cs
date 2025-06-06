﻿namespace TheChest.Inventories.Tests.Slots
{
    public partial class IInventoryStackSlotTests<T>
    {
        [Test]
        public void CanAddItem_NullItem_ReturnsFalse()
        {
            var slot = this.slotFactory.EmptySlot();

            var item = default(T);

            Assert.That(slot.CanAdd(item!), Is.False);
        }

        [Test]
        public void CanAddItem_FullSlot_ReturnsFalse()
        {
            var randomSize = this.random.Next(1, 20);
            var items = this.itemFactory.CreateMany(randomSize);
            var slot = this.slotFactory.FullSlot(items);

            var item = this.itemFactory.CreateDefault();

            Assert.That(slot.CanAdd(item), Is.False);
        }

        [Test]
        public void CanAddItem_ItemDifferentFromSlot_ReturnsFalse()
        {
            var randomSize = this.random.Next(1, 20);
            var items = this.itemFactory.CreateManyRandom(randomSize);
            var slot = this.slotFactory.FullSlot(items);

            var item = this.itemFactory.CreateDefault();

            Assert.That(slot.CanAdd(item), Is.False);
        }

        [Test]
        public void CanAddItem_EmptySlot_ReturnsTrue()
        {
            var slot = this.slotFactory.EmptySlot();

            var item = this.itemFactory.CreateDefault();

            Assert.That(slot.CanAdd(item), Is.True);
        }

        [Test]
        public void CanAddItem_ItemEqualsToSlot_ReturnsTrue()
        {
            var randomSize = this.random.Next(1, 10);
            var items = this.itemFactory.CreateMany(randomSize);
            var slot = this.slotFactory.WithItems(items, 20);

            var item = this.itemFactory.CreateDefault();

            Assert.That(slot.CanAdd(item), Is.True);
        }
    }
}
