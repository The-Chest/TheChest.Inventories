namespace TheChest.Inventories.Tests.Slots.Interfaces
{
    public partial class IInventoryStackSlotTests<T>
    {
        [Test]
        public void Replace_EmptySlot_ReturnsEmptyArray()
        {
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var slot = this.slotFactory.EmptySlot(stackSize);

            var items = this.itemFactory.CreateMany(stackSize);
            var result = slot.Replace(items);

            Assert.That(result, Is.Empty);
        }

        [Test]
        public void Replace_ItemsDifferentFromSlot_ReturnsItemsFromSlot()
        {
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var items = this.itemFactory.CreateMany(stackSize);
            var slot = this.slotFactory.WithItems(items, stackSize);

            var amount = this.random.Next(1, stackSize);
            var replacingItems = this.itemFactory.CreateManyRandom(amount);
            var result = slot.Replace(replacingItems);

            Assert.That(result, Is.EqualTo(items));
        }

        [Test]
        public void Replace_ItemsEqualToSlot_ReturnsItemsFromSlot()
        {
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var items = this.itemFactory.CreateMany(stackSize);
            var slot = this.slotFactory.WithItems(items, stackSize);

            var amount = this.random.Next(1, stackSize);
            var replacingItems = this.itemFactory.CreateMany(amount);
            var result = slot.Replace(replacingItems);

            Assert.That(result, Is.EqualTo(items));
        }
    }
}
