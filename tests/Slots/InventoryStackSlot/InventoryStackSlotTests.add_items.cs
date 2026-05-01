using TheChest.Tests.Common.Extensions.Slots;

namespace TheChest.Inventories.Tests.Slots.InventoryStackSlot
{
    public partial class InventoryStackSlotTests<T>
    {
        [Test]
        public void AddItems_AddingDifferentItems_ThrowsArgumentException()
        {
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var slot = this.slotFactory.Empty(stackSize);

            var randomSize = this.random.Next(1, stackSize);
            var addingItems = this.itemFactory
                .CreateManyRandom(randomSize)
                .Append(this.itemFactory.CreateRandom())
                .ToArray();

            Assert.That(() => slot.Add(addingItems), Throws.ArgumentException);
        }

        [Test]
        public void AddItems_DifferentItemsFromSlot_WithMoreThanAvailableAmount_ThrowsInvalidOperationException()
        {
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var items = new T[1] {  this.itemFactory.CreateRandom() };
            var slot = this.slotFactory.WithItems(items, stackSize);

            var addingItems = this.itemFactory.CreateManyRandom(stackSize);

            Assert.That(() => slot.Add(addingItems), Throws.InvalidOperationException);
        }

        [Test]
        public void AddItems_OneItemDifferentFromSlot_ThrowsArgumentException()
        {
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var items = new T[1] { this.itemFactory.CreateRandom() };
            var slot = this.slotFactory.WithItems(items, stackSize);

            var addingItems = this.itemFactory.CreateMany(stackSize)
                .Append(this.itemFactory.CreateRandom())
                .ToArray();

            Assert.That(() => slot.Add(addingItems), Throws.ArgumentException);
        }

        [Test]
        public void AddItems_AddingNoItems_ThrowsArgumentException()
        {
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var slot = this.slotFactory.Empty(stackSize);

            var addingItems = Array.Empty<T>();

            Assert.That(() => slot.Add(addingItems), Throws.ArgumentException);
        }

        [Test]
        public void AddItems_EmptySlot_WithMoreItemsThanMaxAmount_ThrowsInvalidOperationException()
        {
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var slot = this.slotFactory.Empty(stackSize);

            var randomSize = this.random.Next(stackSize + 1, stackSize + 20);
            var addingItems = this.itemFactory.CreateManyRandom(randomSize);
            Assert.That(() => slot.Add(addingItems), Throws.InvalidOperationException);
        }


        [Test]
        public void AddItems_FullSlot_WithMultipleItems_ThrowsInvalidOperationException()
        {
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var items = this.itemFactory.CreateMany(stackSize);
            var slot = this.slotFactory.Full(items);

            var addingItems = this.itemFactory.CreateMany(stackSize);
            Assert.That(() => slot.Add(addingItems), Throws.InvalidOperationException);
        }

        [Test]
        public void AddItems_EmptySlot_ReturnsEmptyRemainingItems()
        {
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var slot = this.slotFactory.Empty(stackSize);

            var addingAmount = this.random.Next(1, stackSize);
            var addingItems = this.itemFactory.CreateManyRandom(addingAmount);

            var remainingItems = slot.Add(addingItems);

            Assert.That(remainingItems, Is.Empty);
        }

        [Test]
        public void AddItems_FullSlot_ThrowsInvalidOperationException()
        {
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var items = this.itemFactory.CreateMany(stackSize);
            var slot = this.slotFactory.Full(items);

            var addingItems = this.itemFactory.CreateMany(1);

            Assert.That(() => slot.Add(addingItems), Throws.InvalidOperationException);
        }

        [Test]
        public void AddItems_DifferentItemsFromSlot_ThrowsInvalidOperationException()
        {
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var items = new T[1] { this.itemFactory.CreateRandom() };
            var slot = this.slotFactory.WithItems(items, stackSize);

            var addingItems = this.itemFactory.CreateManyRandom(1);

            Assert.That(() => slot.Add(addingItems), Throws.InvalidOperationException);
        }
    }
}
