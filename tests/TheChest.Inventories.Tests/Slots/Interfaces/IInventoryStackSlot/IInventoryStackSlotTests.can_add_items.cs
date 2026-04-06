namespace TheChest.Inventories.Tests.Slots.Interfaces
{
    public partial class IInventoryStackSlotTests<T>
    {
        [Test]
        public void CanAddItems_NullArray_ReturnsFalse()
        {
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var slot = this.slotFactory.EmptySlot(stackSize);

            Assert.That(slot.CanAdd(items: default), Is.False);
        }

        [Test]
        public void CanAddItems_EmptyArray_ReturnsFalse()
        {
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var slot = this.slotFactory.EmptySlot(stackSize);

            var items = Array.Empty<T>();

            Assert.That(slot.CanAdd(items), Is.False);
        }

        [Test]
        public void CanAddItems_FullSlot_ReturnsFalse()
        {
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var items = this.itemFactory.CreateMany(stackSize);
            var slot = this.slotFactory.FullSlot(items);

            var item = this.itemFactory.CreateMany(10);

            Assert.That(slot.CanAdd(item), Is.False);
        }

        [Test]
        public void CanAddItems_ItemsDifferentFromSlot_ReturnsFalse()
        {
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var items = this.itemFactory.CreateManyRandom(stackSize);
            var slot = this.slotFactory.FullSlot(items);

            var addItems = this.itemFactory.CreateMany(stackSize / 2)
                .Append(this.itemFactory.CreateRandom())
                .ToArray();

            Assert.That(slot.CanAdd(addItems), Is.False);
        }

        [Test]
        public void CanAddItems_NoAvailableSpace_ReturnsFalse()
        {
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var items = this.itemFactory.CreateMany(stackSize);
            var slot = this.slotFactory.FullSlot(items);

            var item = this.itemFactory.CreateMany(stackSize / 2);

            Assert.That(slot.CanAdd(item), Is.False);
        }

        [Test]
        public void CanAddItems_AvailableSpace_ReturnsTrue()
        {
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var itemsSize = stackSize / 2;
            var items = this.itemFactory.CreateMany(itemsSize);
            var slot = this.slotFactory.WithItems(items, stackSize);

            var itemsAmount = stackSize - itemsSize;
            var checkItems = this.itemFactory.CreateMany(itemsAmount);

            Assert.That(slot.CanAdd(checkItems), Is.True);
        }

        [Test]
        public void CanAddItems_EmptySlot_ReturnsTrue()
        {
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var slot = this.slotFactory.EmptySlot(stackSize);

            var items = this.itemFactory.CreateMany(stackSize);

            Assert.That(slot.CanAdd(items), Is.True);
        }

        [Test]
        public void CanAddItems_ItemEqualsToSlot_ReturnsTrue()
        {
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var items = this.itemFactory.CreateMany(stackSize);
            var halfStackSize = stackSize / 2;
            var slot = this.slotFactory.WithItems(items, halfStackSize);

            var addItems = this.itemFactory.CreateMany(halfStackSize);

            Assert.That(slot.CanAdd(addItems), Is.True);
        }
    }
}
