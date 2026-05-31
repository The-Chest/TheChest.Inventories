using TheChest.Tests.Common.Extensions.Slots;

namespace TheChest.Inventories.Tests.Slots.Interfaces
{
    public partial class IInventoryStackSlotTests<T>
    {
        [Test]
        public void TryAddItems_NullItems_ThrowsArgumentNullException()
        {
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var slot = this.slotFactory.Empty(stackSize);

            Assert.That(() => slot.TryAdd(null!), Throws.ArgumentNullException);
        }

        [Test]
        public void TryAddItems_ArrayContainingNull_ThrowsArgumentNullException()
        {
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var slot = this.slotFactory.Empty(stackSize);
            var items = new T[] { default! };

            Assert.That(() => slot.TryAdd(items), Throws.ArgumentNullException);
        }

        [Test]
        public void TryAddItems_FullSlot_ReturnsFalse()
        {
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var slotItems = this.itemFactory.CreateMany(stackSize);
            var slot = this.slotFactory.Full(slotItems);
            var addingItems = this.itemFactory.CreateMany(1);

            var result = slot.TryAdd(addingItems);

            Assert.That(result, Is.False);
        }

        [Test]
        public void TryAddItems_FullSlot_DoesNotAddItems()
        {
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var slotItems = this.itemFactory.CreateMany(stackSize);
            var slot = this.slotFactory.Full(slotItems);
            var addingItems = this.itemFactory.CreateMany(1);

            slot.TryAdd(addingItems);

            Assert.That(slot.GetContents(), Is.EquivalentTo(slotItems));
        }

        [Test]
        public void TryAddItems_SlotWithDifferentItems_ReturnsFalse()
        {
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var currentAmount = this.random.Next(1, stackSize - 1);
            var slotItems = this.itemFactory.CreateMany(currentAmount);
            var slot = this.slotFactory.WithItems(slotItems, stackSize);
            var addingItems = this.itemFactory.CreateManyRandom(1);

            var result = slot.TryAdd(addingItems);

            Assert.That(result, Is.False);
        }

        [Test]
        public void TryAddItems_NotEnoughCapacity_ReturnsFalse()
        {
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var currentAmount = this.random.Next(1, stackSize - 1);
            var slotItems = this.itemFactory.CreateMany(currentAmount);
            var slot = this.slotFactory.WithItems(slotItems, stackSize);
            var addingItems = this.itemFactory.CreateMany(stackSize - currentAmount + 1);

            var result = slot.TryAdd(addingItems);

            Assert.That(result, Is.False);
        }

        [Test]
        public void TryAddItems_NotEnoughCapacity_DoesNotAddItems()
        {
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var currentAmount = this.random.Next(1, stackSize - 1);
            var slotItems = this.itemFactory.CreateMany(currentAmount);
            var slot = this.slotFactory.WithItems(slotItems, stackSize);
            var addingItems = this.itemFactory.CreateMany(stackSize - currentAmount + 1);

            slot.TryAdd(addingItems);

            Assert.That(slot.GetContents(), Is.EquivalentTo(slotItems));
        }

        [Test]
        public void TryAddItems_ValidItems_ReturnsTrue()
        {
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var currentAmount = this.random.Next(1, stackSize - 1);
            var slotItems = this.itemFactory.CreateMany(currentAmount);
            var slot = this.slotFactory.WithItems(slotItems, stackSize);
            var addingItems = this.itemFactory.CreateMany(stackSize - currentAmount);

            var result = slot.TryAdd(addingItems);

            Assert.That(result, Is.True);
        }
    }
}
