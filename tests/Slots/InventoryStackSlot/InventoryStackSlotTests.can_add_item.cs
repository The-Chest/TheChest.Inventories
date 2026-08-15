using TheChest.Tests.Common.Attributes;

namespace TheChest.Inventories.Tests.Slots.InventoryStackSlot
{
    public partial class InventoryStackSlotTests<T>
    {
        [Test]
        [IgnoreIfValueType]
        public void CanAddItem_NullItem_ReturnsFalse()
        {
            var stackSize = this.GetRandomStackSize();
            var slot = this.slotFactory.Empty(stackSize);

            var result = slot.CanAdd(item: default);

            Assert.That(result, Is.False);
        }

        [Test]
        [IgnoreIfReferenceType]
        public void CanAddItem_DefaultItem_ReturnsTrue()
        {
            var stackSize = this.GetRandomStackSize();
            var slot = this.slotFactory.Empty(stackSize);

            var result = slot.CanAdd(item: default);

            Assert.That(result, Is.True);
        }

        [Test]
        public void CanAddItem_Empty_ReturnsTrue()
        {
            var stackSize = this.GetRandomStackSize();
            var slot = this.slotFactory.Empty(stackSize);

            var item = this.itemFactory.CreateRandom();
            var result = slot.CanAdd(item);

            Assert.That(result, Is.True);
        }

        [Test]
        public void CanAddItem_AvailableSpace_DifferentItem_ReturnsFalse()
        {
            var stackSize = this.GetRandomStackSize();
            var itemsAmount = this.random.Next(1, stackSize - 1);
            var items = this.itemFactory.CreateManyRandom(itemsAmount);
            var slot = this.slotFactory.WithItems(items, stackSize);

            var item = this.itemFactory.CreateRandom();
            var result = slot.CanAdd(item);

            Assert.That(result, Is.False);
        }

        [Test]
        public void CanAddItem_AvailableSpace_SameItem_ReturnsTrue()
        {
            var stackSize = this.GetRandomStackSize();
            var itemsAmount = this.random.Next(1, stackSize - 1);
            var items = this.itemFactory.CreateMany(itemsAmount);
            var slot = this.slotFactory.WithItems(items, stackSize);

            var item = this.itemFactory.CreateDefault();
            var result = slot.CanAdd(item);

            Assert.That(result, Is.True);
        }

        [Test]
        public void CanAddItem_Full_ReturnsFalse()
        {
            var stackSize = this.GetRandomStackSize();
            var items = this.itemFactory.CreateMany(stackSize);
            var slot = this.slotFactory.Full(items);

            var item = this.itemFactory.CreateRandom();
            var result = slot.CanAdd(item);

            Assert.That(result, Is.False);
        }
    }
}
