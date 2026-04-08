namespace TheChest.Inventories.Tests.Slots.Interfaces
{
    public partial class IInventoryStackSlotTests<T>
    {
        [Test]
        public void CanAddItem_NullItem_ReturnsFalse()
        {
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var slot = this.slotFactory.EmptySlot(stackSize);

            var item = default(T);

            Assert.That(slot.CanAdd(item!), Is.False);
        }

        [Test]
        public void CanAddItem_FullSlot_ReturnsFalse()
        {
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var items = this.itemFactory.CreateMany(stackSize);
            var slot = this.slotFactory.FullSlot(items);

            var item = this.itemFactory.CreateDefault();

            Assert.That(slot.CanAdd(item), Is.False);
        }

        [Test]
        public void CanAddItem_ItemDifferentFromSlot_ReturnsFalse()
        {
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var items = this.itemFactory.CreateManyRandom(stackSize);
            var slot = this.slotFactory.FullSlot(items);

            var item = this.itemFactory.CreateDefault();

            Assert.That(slot.CanAdd(item), Is.False);
        }

        [Test]
        public void CanAddItem_EmptySlot_ReturnsTrue()
        {
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var slot = this.slotFactory.EmptySlot(stackSize);

            var item = this.itemFactory.CreateDefault();

            Assert.That(slot.CanAdd(item), Is.True);
        }

        [Test]
        public void CanAddItem_ItemEqualsToSlot_ReturnsTrue()
        {
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var randomSize = this.random.Next(1, stackSize - 1);
            var items = this.itemFactory.CreateMany(randomSize);
            var slot = this.slotFactory.WithItems(items, stackSize);

            var item = this.itemFactory.CreateDefault();

            Assert.That(slot.CanAdd(item), Is.True);
        }
    }
}
