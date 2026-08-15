namespace TheChest.Inventories.Tests.Slots.InventoryStackSlot
{
    public partial class InventoryStackSlotTests<T>
    {
        [Test]
        public void CanAddItems_NullArray_ReturnsFalse()
        {
            var stackSize = this.GetRandomStackSize();
            var slot = this.slotFactory.Empty(stackSize);

            var result = slot.CanAdd(items: default);

            Assert.That(result, Is.False);
        }

        [Test]
        public void CanAddItems_EmptyArray_ReturnsFalse()
        {
            var stackSize = this.GetRandomStackSize();
            var slot = this.slotFactory.Empty(stackSize);

            var result = slot.CanAdd(Array.Empty<T>());

            Assert.That(result, Is.False);
        }

        [Test]
        public void CanAddItems_Empty_ReturnsTrue()
        {
            var stackSize = this.GetRandomStackSize();
            var slot = this.slotFactory.Empty(stackSize);

            var checkItems = this.itemFactory.CreateMany(stackSize);
            var result = slot.CanAdd(checkItems);

            Assert.That(result, Is.True);
        }

        [Test]
        public void CanAddItems_Full_ReturnsFalse()
        {
            var stackSize = this.GetRandomStackSize();
            var items = this.itemFactory.CreateMany(stackSize);
            var slot = this.slotFactory.Full(items);

            var checkItems = this.itemFactory.CreateMany(10);
            var result = slot.CanAdd(checkItems);

            Assert.That(result, Is.False);
        }

        [Test]
        public void CanAddItems_DifferentItems_ReturnsFalse()
        {
            var stackSize = this.GetRandomStackSize();
            var items = this.itemFactory.CreateManyRandom(stackSize);
            var slot = this.slotFactory.Full(items);

            var checkItems = this.itemFactory.CreateMany(stackSize / 2)
                .Append(this.itemFactory.CreateRandom())
                .ToArray();
            var result = slot.CanAdd(checkItems);

            Assert.That(result, Is.False);
        }

        [Test]
        public void CanAddItems_NoAvailableSpace_ReturnsFalse()
        {
            var stackSize = this.GetRandomStackSize();
            var items = this.itemFactory.CreateMany(stackSize);
            var slot = this.slotFactory.Full(items);

            var checkItems = this.itemFactory.CreateMany(stackSize / 2);
            var result = slot.CanAdd(checkItems);

            Assert.That(result, Is.False);
        }

        [Test]
        public void CanAddItems_AvailableSpace_ReturnsTrue()
        {
            var stackSize = this.GetRandomStackSize();
            var itemsSize = stackSize / 2;
            var items = this.itemFactory.CreateMany(itemsSize);
            var slot = this.slotFactory.WithItems(items, stackSize);

            var itemsAmount = stackSize - itemsSize;
            var checkItems = this.itemFactory.CreateMany(itemsAmount);
            var result = slot.CanAdd(checkItems);

            Assert.That(result, Is.True);
        }

        [Test]
        public void CanAddItems_EqualItems_ReturnsTrue()
        {
            var stackSize = this.GetRandomStackSize();
            var halfStackSize = stackSize / 2;
            var items = this.itemFactory.CreateMany(halfStackSize);
            var slot = this.slotFactory.WithItems(items, stackSize);

            var checkItems = this.itemFactory.CreateMany(halfStackSize);
            var result = slot.CanAdd(checkItems);

            Assert.That(result, Is.True);
        }
    }
}
