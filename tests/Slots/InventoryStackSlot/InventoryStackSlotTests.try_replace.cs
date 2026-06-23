using TheChest.Tests.Common.Extensions.Slots;

namespace TheChest.Inventories.Tests.Slots.InventoryStackSlot
{
    public partial class InventoryStackSlotTests<T>
    {
        [Test]
        public void TryReplace_NullItems_ThrowsArgumentNullException()
        {
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var slot = this.slotFactory.Empty(stackSize);

            Assert.That(() => slot.TryReplace(null!, out _), Throws.ArgumentNullException);
        }

        [Test]
        public void TryReplace_ArrayContainingNull_ThrowsArgumentNullException()
        {
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var slot = this.slotFactory.Empty(stackSize);

            var items = new T[] { default! };
            Assert.That(() => slot.TryReplace(items, out _), Throws.ArgumentNullException);
        }

        [Test]
        public void TryReplace_EmptyItems_ReturnsFalse()
        {
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var slotItems = this.itemFactory.CreateMany(stackSize);
            var slot = this.slotFactory.WithItems(slotItems, stackSize);

            var items = Array.Empty<T>();
            var result = slot.TryReplace(items, out _);

            Assert.That(result, Is.False);
        }

        [Test]
        public void TryReplace_EmptyItems_SetsOldItemsToNull()
        {
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var slotItems = this.itemFactory.CreateMany(stackSize);
            var slot = this.slotFactory.WithItems(slotItems, stackSize);

            var items = Array.Empty<T>();
            slot.TryReplace(items, out var oldItems);

            Assert.That(oldItems, Is.Null);
        }

        [Test]
        public void TryReplace_EmptyItems_DoesNotReplaceItems()
        {
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var slotItems = this.itemFactory.CreateMany(stackSize);
            var slot = this.slotFactory.WithItems(slotItems, stackSize);

            var items = Array.Empty<T>();
            slot.TryReplace(items, out _);

            Assert.That(slot.GetContents(), Is.EqualTo(slotItems));
        }

        [Test]
        public void TryReplace_ItemsBiggerThanMaxAmount_ReturnsFalse()
        {
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var slot = this.slotFactory.Empty(stackSize);

            var items = this.itemFactory.CreateMany(stackSize + 1);
            var result = slot.TryReplace(items, out _);

            Assert.That(result, Is.False);
        }

        [Test]
        public void TryReplace_ItemsBiggerThanMaxAmount_SetsOldItemsToNull()
        {
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var slot = this.slotFactory.Empty(stackSize);

            var items = this.itemFactory.CreateMany(stackSize + 1);
            slot.TryReplace(items, out var oldItems);

            Assert.That(oldItems, Is.Null);
        }

        [Test]
        public void TryReplace_ItemsWithDifferentValues_ReturnsFalse()
        {
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var slot = this.slotFactory.Empty(stackSize);

            var items = new[] { this.itemFactory.CreateDefault(), this.itemFactory.CreateRandom() };
            var result = slot.TryReplace(items, out _);

            Assert.That(result, Is.False);
        }

        [Test]
        public void TryReplace_EmptySlot_ReturnsTrue()
        {
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var slot = this.slotFactory.Empty(stackSize);

            var items = this.itemFactory.CreateMany(this.random.Next(1, stackSize));
            var result = slot.TryReplace(items, out _);

            Assert.That(result, Is.True);
        }

        [Test]
        public void TryReplace_EmptySlot_SetsOldItemsToEmptyArray()
        {
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var slot = this.slotFactory.Empty(stackSize);

            var items = this.itemFactory.CreateMany(this.random.Next(1, stackSize));
            slot.TryReplace(items, out var oldItems);

            Assert.That(oldItems, Is.Empty);
        }

        [Test]
        public void TryReplace_EmptySlot_AddsItems()
        {
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var slot = this.slotFactory.Empty(stackSize);

            var items = this.itemFactory.CreateMany(this.random.Next(1, stackSize));
            var expectedItems = (T[])items.Clone();
            slot.TryReplace(items, out _);

            Assert.That(slot.GetContents(), Is.EqualTo(expectedItems));
        }

        [Test]
        public void TryReplace_SlotWithItems_ReturnsTrue()
        {
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var slotItems = this.itemFactory.CreateMany(stackSize);
            var slot = this.slotFactory.WithItems(slotItems, stackSize);

            var replacingItems = this.itemFactory.CreateManyRandom(this.random.Next(1, stackSize));
            var result = slot.TryReplace(replacingItems, out _);

            Assert.That(result, Is.True);
        }

        [Test]
        public void TryReplace_SlotWithItems_SetsOldItemsToReplacedItems()
        {
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var slotItems = this.itemFactory.CreateMany(stackSize);
            var slot = this.slotFactory.WithItems(slotItems, stackSize);

            var replacingItems = this.itemFactory.CreateManyRandom(this.random.Next(1, stackSize));
            slot.TryReplace(replacingItems, out var oldItems);

            Assert.That(oldItems, Is.EqualTo(slotItems));
        }

        [Test]
        public void TryReplace_SlotWithItems_ReplacesItems()
        {
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var slotItems = this.itemFactory.CreateMany(stackSize);
            var slot = this.slotFactory.WithItems(slotItems, stackSize);

            var replacingItems = this.itemFactory.CreateManyRandom(this.random.Next(1, stackSize));
            var expectedItems = (T[])replacingItems.Clone();
            slot.TryReplace(replacingItems, out _);

            Assert.That(slot.GetContents(), Is.EqualTo(expectedItems));
        }
    }
}
