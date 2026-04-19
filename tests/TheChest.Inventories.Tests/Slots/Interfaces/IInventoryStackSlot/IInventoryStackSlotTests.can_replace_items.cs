namespace TheChest.Inventories.Tests.Slots.Interfaces
{
    public partial class IInventoryStackSlotTests<T>
    {
        [Test]
        public void CanReplaceItems_EmptyArray_ReturnsFalse() 
        {
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var startItems = this.itemFactory.CreateMany(stackSize);
            var slot = this.slotFactory.FullSlot(startItems);

            var items = Array.Empty<T>();
            Assert.That(slot.CanReplace(items), Is.False);
        }

        [Test]
        public void CanReplaceItems_OneItemNullInArray_ReturnsFalse()
        {
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var startItems = this.itemFactory.CreateMany(stackSize);
            var slot = this.slotFactory.FullSlot(startItems);

            var items = this.itemFactory.CreateMany(stackSize / 2)
                .Append(default)
                .ToArray();
            Assert.That(slot.CanReplace(items!), Is.False);
        }

        [Test]
        public void CanReplaceItems_OneItemDifferentInArray_ReturnsFalse()
        {
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var startItems = this.itemFactory.CreateMany(stackSize);
            var slot = this.slotFactory.FullSlot(startItems);

            var items = this.itemFactory.CreateMany(stackSize / 2)
                .Append(this.itemFactory.CreateRandom())
                .ToArray();
            Assert.That(slot.CanReplace(items!), Is.False);
        }

        [Test]
        public void CanReplaceItems_ArrayBiggerThanMaxAmount_ReturnsFalse()
        {
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var startItems = this.itemFactory.CreateMany(stackSize);
            var slot = this.slotFactory.FullSlot(startItems);

            var items = this.itemFactory.CreateMany(stackSize + 1);
            Assert.That(slot.CanReplace(items), Is.False);
        }

        [Test]
        public void CanReplaceItems_ArrayEqualThanMaxAmount_ReturnsTrue()
        {
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var startItems = this.itemFactory.CreateMany(stackSize);
            var slot = this.slotFactory.FullSlot(startItems);

            var items = this.itemFactory.CreateMany(stackSize);
            Assert.That(slot.CanReplace(items), Is.True);
        }

        [Test]
        public void CanReplaceItems_ArraySmallerThanMaxAmount_ReturnsTrue()
        {
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var startItems = this.itemFactory.CreateMany(stackSize);
            var slot = this.slotFactory.FullSlot(startItems);

            var items = this.itemFactory.CreateMany(stackSize - 1);
            Assert.That(slot.CanReplace(items), Is.True);
        }

        [Test]
        public void CanReplaceItems_SameItemsThanSlot_ReturnsTrue()
        {
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var startItems = this.itemFactory.CreateMany(stackSize);
            var slot = this.slotFactory.FullSlot(startItems);

            Assert.That(slot.CanReplace(startItems), Is.True);
        }

        [Test]
        public void CanReplaceItems_DifferentItemThanSlot_ReturnsTrue()
        {
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var startItems = this.itemFactory.CreateMany(stackSize);
            var slot = this.slotFactory.FullSlot(startItems);

            var items = this.itemFactory.CreateManyRandom(stackSize);
            Assert.That(slot.CanReplace(items), Is.True);
        }
    }
}
 