namespace TheChest.Inventories.Tests.Slots.Interfaces
{
    public partial class IInventoryStackSlotTests<T>
    {
        [Test]
        public void ReplaceOne_EmptySlot_ReturnsEmptyArray()
        {
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var slot = this.slotFactory.Empty(stackSize);

            var item = this.itemFactory.CreateDefault();
            var result = slot.Replace(item);

            Assert.That(result, Is.Empty);
        }

        [Test]
        public void ReplaceOne_ItemDifferentFromSlot_ReturnsItemsFromSlot()
        {
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var items = this.itemFactory.CreateMany(stackSize);
            var slot = this.slotFactory.WithItems(items, stackSize);

            var replacingItem = this.itemFactory.CreateRandom();
            var result = slot.Replace(replacingItem);

            Assert.That(result, Is.EqualTo(items));
        }

        [Test]
        public void ReplaceOne_ItemEqualToSlot_ReturnsItemsFromSlot()
        {
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var items = this.itemFactory.CreateMany(stackSize);
            var slot = this.slotFactory.WithItems(items, stackSize);

            var replacingItem = this.itemFactory.CreateDefault();
            var result = slot.Replace(replacingItem);

            Assert.That(result, Is.EqualTo(items));
        }
    }
}
