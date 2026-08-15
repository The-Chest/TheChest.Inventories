using TheChest.Tests.Common.Extensions.Slots;

namespace TheChest.Inventories.Tests.Slots.InventoryStackSlot
{
    public partial class InventoryStackSlotTests<T>
    {
        [Test]
        public void ReplaceItems_EmptyItems_ThrowsArgumentException()
        {
            var stackSize = this.GetRandomStackSize();
            var slotItems = this.itemFactory.CreateMany(stackSize);
            var slot = this.slotFactory.Full(slotItems);

            var items = Array.Empty<T>();
            Assert.That(
                () => slot.Replace(items), 
                Throws.ArgumentException.With.Property("ParamName").EqualTo("items")
            );
        }

        [Test]
        public void ReplaceItems_ItemsExceedingMaxAmount_ThrowsArgumentOutOfRangeException()
        {
            var stackSize = this.GetRandomStackSize();
            var slot = this.slotFactory.Empty(stackSize);

            var items = this.itemFactory.CreateMany(stackSize + 1);
            Assert.That(
                () => slot.Replace(items), 
                Throws.Exception.TypeOf<ArgumentOutOfRangeException>().With.Property("ParamName").EqualTo("items")
             );
        }

        [Test]
        public void ReplaceItems_EmptySlot_ReplacesItems()
        {
            var stackSize = this.GetRandomStackSize();
            var slot = this.slotFactory.Empty(stackSize);

            var items = this.itemFactory.CreateMany(stackSize / 2);
            var expectedResult = (T[])items.Clone();
            slot.Replace(items);

            Assert.That(slot.GetContents(), Is.EquivalentTo(expectedResult));
        }

        [Test]
        public void ReplaceItems_SlotWithDifferentItems_ReplacesItems()
        {
            var stackSize = this.GetRandomStackSize();
            var items = this.itemFactory.CreateMany(stackSize);
            var slot = this.slotFactory.WithItems(items, stackSize);

            var replacingItems = this.itemFactory.CreateManyRandom(stackSize);
            var expectedResult = (T[])replacingItems.Clone();

            slot.Replace(replacingItems);

            Assert.That(slot.GetContents(), Is.EqualTo(expectedResult));
        }

        [Test]
        public void ReplaceItems_SlotWithSameItems_ReplacesItems()
        {
            var stackSize = this.GetRandomStackSize();
            var halfStackSize = stackSize / 2;
            var items = this.itemFactory.CreateMany(halfStackSize);
            var slot = this.slotFactory.WithItems(items, stackSize);

            var amount = this.random.Next(1, stackSize);
            var replacingItems = this.itemFactory.CreateMany(amount);
            var expectedResult = (T[])replacingItems.Clone();

            slot.Replace(replacingItems);

            Assert.That(slot.GetContents()[0..amount], Is.EqualTo(expectedResult));
        }

        [Test]
        public void Replace_EmptySlot_ReturnsEmptyArray()
        {
            var stackSize = this.GetRandomStackSize();
            var slot = this.slotFactory.Empty(stackSize);

            var items = this.itemFactory.CreateMany(stackSize);
            var result = slot.Replace(items);

            Assert.That(result, Is.Empty);
        }

        [Test]
        public void Replace_ItemsDifferentFromSlot_ReturnsItemsFromSlot()
        {
            var stackSize = this.GetRandomStackSize();
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
            var stackSize = this.GetRandomStackSize();
            var items = this.itemFactory.CreateMany(stackSize);
            var slot = this.slotFactory.WithItems(items, stackSize);

            var amount = this.random.Next(1, stackSize);
            var replacingItems = this.itemFactory.CreateMany(amount);
            var result = slot.Replace(replacingItems);

            Assert.That(result, Is.EqualTo(items));
        }
    }
}
