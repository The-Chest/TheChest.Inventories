namespace TheChest.Inventories.Tests.Slots
{
    public partial class IInventoryStackSlotTests<T>
    {
        [Test]
        public void CanAddItems_NullArray_ReturnsFalse()
        {
            var slot = this.slotFactory.EmptySlot();

            Assert.That(slot.CanAdd(items: default), Is.False);
        }

        [Test]
        public void CanAddItems_EmptyArray_ReturnsFalse()
        {
            var slot = this.slotFactory.EmptySlot();

            var items = Array.Empty<T>();

            Assert.That(slot.CanAdd(items), Is.False);
        }

        [Test]
        public void CanAddItems_FullSlot_ReturnsFalse()
        {
            var randomSize = this.random.Next(1, 20);
            var items = this.itemFactory.CreateMany(randomSize);
            var slot = this.slotFactory.FullSlot(items);

            var item = this.itemFactory.CreateMany(10);

            Assert.That(slot.CanAdd(item), Is.False);
        }

        [Test]
        public void CanAddItems_ItemsDifferentFromSlot_ReturnsFalse()
        {
            var randomSize = this.random.Next(1, 20);
            var items = this.itemFactory.CreateManyRandom(randomSize);
            var slot = this.slotFactory.FullSlot(items);

            var addItems = this.itemFactory.CreateMany(5)
                .Append(this.itemFactory.CreateRandom())
                .ToArray();

            Assert.That(slot.CanAdd(addItems), Is.False);
        }

        [Test]
        public void CanAddItems_NoAvailableSpace_ReturnsFalse()
        {
            var randomSize = this.random.Next(1, 15);
            var items = this.itemFactory.CreateMany(randomSize);
            var slot = this.slotFactory.FullSlot(items);

            var item = this.itemFactory.CreateMany(10);

            Assert.That(slot.CanAdd(item), Is.False);
        }

        [Test]
        public void CanAddItems_AvailableSpace_ReturnsTrue()
        {
            var itemsSize = this.random.Next(1, 10);
            var items = this.itemFactory.CreateMany(itemsSize);
            var slotSize = this.random.Next(11, 20);
            var slot = this.slotFactory.WithItems(items, slotSize);

            var itemsAmount = slotSize - itemsSize;
            var checkItems = this.itemFactory.CreateMany(itemsAmount);

            Assert.That(slot.CanAdd(checkItems), Is.True);
        }

        [Test]
        public void CanAddItems_EmptySlot_ReturnsTrue()
        {
            var slot = this.slotFactory.EmptySlot();

            var items = this.itemFactory.CreateMany(10);

            Assert.That(slot.CanAdd(items), Is.True);
        }

        [Test]
        public void CanAddItems_ItemEqualsToSlot_ReturnsTrue()
        {
            var randomSize = this.random.Next(1, 10);
            var items = this.itemFactory.CreateMany(randomSize);
            var slot = this.slotFactory.WithItems(items, 20);

            var addItems = this.itemFactory.CreateMany(10);

            Assert.That(slot.CanAdd(addItems), Is.True);
        }
    }
}
