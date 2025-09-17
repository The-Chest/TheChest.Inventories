namespace TheChest.Inventories.Tests.Slots
{
    public partial class IInventoryStackSlotTests<T>
    {
        [TestCase(0)]
        [TestCase(-1)]
        public void GetAmount_InvalidAmount_ThrowsArgumentExceptions(int amount)
        {
            var items = this.itemFactory.CreateMany(20);
            var slot = this.slotFactory.FullSlot(items);

            Assert.That(() => slot.Get(amount), Throws.TypeOf<ArgumentOutOfRangeException>());
        }

        [Test]
        public void GetAmount_SlotWithEnoughItems_RemovesFromContent()
        {
            var items = this.itemFactory.CreateMany(20);
            var slot = this.slotFactory.FullSlot(items);

            slot.Get(10);

            Assert.That(slot.GetContents()[10..20], Is.EquivalentTo(items[10..20]));
        }

        [Test]
        public void GetAmount_SlotWithEnoughItems_ReturnsWithAmount()
        {
            var items = this.itemFactory.CreateMany(20);
            var slot = this.slotFactory.FullSlot(items);

            var result = slot.Get(10);

            Assert.That(result, Is.EquivalentTo(items[0..10]));
        }

        [Test]
        public void GetAmount_SlotWithNotEnoughItems_RemovesFromContent()
        {
            var items = this.itemFactory.CreateMany(5);
            var slot = this.slotFactory.WithItems(items, 20);

            slot.Get(10);

            Assert.That(slot.GetContents(), Is.All.Null);
        }

        [Test]
        public void GetAmount_SlotWithNotEnoughItems_ReturnsItemsFromSlot()
        {
            var items = this.itemFactory.CreateMany(5);
            var slot = this.slotFactory.WithItems(items, 20);

            var result = slot.Get(10);

            Assert.That(result, Is.EquivalentTo(items));
        }

        [Test]
        public void GetAmount_AmountBiggerThanSlotMaxAmount_RemovesFromContent()
        {
            var items = this.itemFactory.CreateMany(20);
            var slot = this.slotFactory.FullSlot(items);

            slot.Get(30);

            Assert.That(slot.GetContents(), Has.All.Null);
        }

        [Test]
        public void GetAmount_SlotWithLessItemsThanRequested_ReturnsAllItemsFromSlot()
        {
            var items = this.itemFactory.CreateMany(20);
            var slot = this.slotFactory.WithItems(items, 20);

            var result = slot.Get(30);

            Assert.That(result, Is.EquivalentTo(items));
        }

        [Test]
        public void GetAmount_SlotWithLessItemsThanRequested_DecreasesAmountToZero()
        {
            var slotSize = this.random.Next(11, 20);
            var itemSize = this.random.Next(1, 10);
            var items = this.itemFactory.CreateMany(itemSize);
            var slot = this.slotFactory.WithItems(items, slotSize);

            var amount = itemSize + this.random.Next(1, 10);
            slot.Get(amount);
            Assert.That(slot.Amount, Is.EqualTo(0));
        }

        [Test]
        public void GetAmount_EmptySlot_ReturnsEmptyArray()
        {
            var slot = this.slotFactory.EmptySlot(20);

            var result = slot.Get(10);

            Assert.That(result, Is.EquivalentTo(Array.Empty<T>()));
        }

        [Test]
        public void GetAmount_FullSlot_DecreasesAmount()
        {
            var slotSize = this.random.Next(11, 20);
            var items = this.itemFactory.CreateMany(slotSize);
            var slot = this.slotFactory.FullSlot(items);

            var amount = this.random.Next(1, 10);
            slot.Get(amount);
            Assert.That(slot.Amount, Is.EqualTo(slotSize - amount));
        }
    }
}
