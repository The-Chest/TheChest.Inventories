namespace TheChest.Inventories.Tests.Slots.Interfaces
{
    public partial class IInventoryLazyStackSlotTests<T> 
    {
        [Test]
        public void Replace_EmptySlot_ReturnsEmptyArray()
        {
            var maxAmount = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var slot = this.slotFactory.Empty(maxAmount);

            var item = this.itemFactory.CreateDefault();
            int amount = this.random.Next(1, maxAmount);
            var result = slot.Replace(item, amount);

            Assert.That(result, Is.Empty);
        }

        [Test]
        public void Replace_SlotWithItems_ReturnsItemFromSlot()
        {
            var initialItem = this.itemFactory.CreateDefault();
            var maxAmount = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            int initialAmount = this.random.Next(1, maxAmount - 1);
            var slot = this.slotFactory.WithItem(initialItem, initialAmount, maxAmount);

            var newItem = this.itemFactory.CreateRandom();
            int newAmount = this.random.Next(1, maxAmount);
            var result = slot.Replace(newItem, newAmount);

            Assert.Multiple(() =>
            {
                Assert.That(result, Has.Length.EqualTo(initialAmount));
                Assert.That(result, Has.All.EqualTo(initialItem));
            });
        }
    }
}
