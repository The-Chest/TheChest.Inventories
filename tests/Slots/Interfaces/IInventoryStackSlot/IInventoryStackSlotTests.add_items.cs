namespace TheChest.Inventories.Tests.Slots.Interfaces
{
    public partial class IInventoryStackSlotTests<T>
    {
        [Test]
        public void AddItems_EmptySlot_WithLessItemsThanMaxAmount_IncreasesAmount()
        {
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var slot = this.slotFactory.Empty(stackSize);

            var addingAmount = this.random.Next(1, stackSize - 1);
            var addingItems = this.itemFactory.CreateManyRandom(addingAmount);
            slot.Add(addingItems);

            Assert.That(slot.Amount, Is.EqualTo(addingAmount));
        }

        [Test]
        public void AddItems_EmptySlot_WithMoreItemsThanMaxAmount_IncreasesAmountToMaxAvailable()
        {
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var slot = this.slotFactory.Empty(stackSize);

            var addingItems = this.itemFactory.CreateManyRandom(stackSize);
            slot.Add(addingItems);

            Assert.That(slot.Amount, Is.EqualTo(stackSize));
        }

        [Test]
        public void Add_PartiallyFilledSlot_IncreasesAmount()
        {
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var itemsSize = this.random.Next(1, stackSize);
            var initialItems = this.itemFactory.CreateMany(itemsSize);
            var slot = this.slotFactory.WithItems(initialItems, stackSize);

            var itemsToAdd = this.itemFactory.CreateMany(stackSize - itemsSize);
            slot.Add(itemsToAdd);
            
            Assert.That(slot.Amount, Is.EqualTo(stackSize));
        }

        [Test]
        public void Add_FullSlot_ThrowsInvalidOperationException()
        {
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var items = this.itemFactory.CreateMany(stackSize);
            var slot = this.slotFactory.Full(items);
            var item = this.itemFactory.CreateDefault();
            Assert.That(() => slot.Add(item), Throws.InvalidOperationException);
        }
    }
}
