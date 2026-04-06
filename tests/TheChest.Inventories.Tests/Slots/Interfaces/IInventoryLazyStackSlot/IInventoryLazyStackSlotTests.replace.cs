namespace TheChest.Inventories.Tests.Slots.Interfaces
{
    public partial class IInventoryLazyStackSlotTests<T> 
    {
        [Test]
        public void Replace_EmptySlot_ReturnsEmptyArray()
        {
            var slot = this.slotFactory.EmptySlot();
            var item = this.itemFactory.CreateDefault();
            int amount = this.random.Next(1, 10);

            var result = slot.Replace(item, amount);

            Assert.That(result, Is.Empty);
        }

        [Test]
        public void Replace_SlotWithItems_ReturnsItemFromSlot()
        {
            var initialItem = this.itemFactory.CreateDefault();
            var maxAmount = this.random.Next(11, 20);
            int initialAmount = this.random.Next(1, 10);
            var slot = this.slotFactory.WithItem(initialItem, initialAmount, maxAmount);

            var newItem = this.itemFactory.CreateRandom();
            int newAmount = this.random.Next(1, 10);
            var result = slot.Replace(newItem, newAmount);

            Assert.Multiple(() =>
            {
                Assert.That(result, Has.Length.EqualTo(initialAmount));
                Assert.That(result, Has.All.EqualTo(initialItem));
            });
        }
    }
}
