using TheChest.Tests.Common.Attributes;
using TheChest.Tests.Common.Extensions.Slots;

namespace TheChest.Inventories.Tests.Slots.InventoryStackSlot
{
    public partial class InventoryStackSlotTests<T>
    {
        [TestCase(0)]
        [TestCase(-1)]
        public void GetAmount_InvalidAmount_ThrowsArgumentException(int amount)
        {
            var stackSize = this.GetRandomStackSize();
            var items = this.itemFactory.CreateMany(stackSize);
            var slot = this.slotFactory.Full(items);

            Assert.That(
                () => slot.Get(amount), 
                Throws.TypeOf<ArgumentOutOfRangeException>().With.Property("ParamName").EqualTo("amount")
            );
        }

        [Test]
        public void GetAmount_EmptySlot_ReturnsEmptyArray()
        {
            var stackSize = this.GetRandomStackSize();
            var slot = this.slotFactory.Empty(stackSize);

            var amount = this.random.Next(1, stackSize / 2);
            var result = slot.Get(amount);

            Assert.That(result, Is.Empty);
        }

        [Test]
        [IgnoreIfReferenceType]
        public void GetAmount_FullSlot_ValueType_ReturnsEmptyArray()
        {
            var stackSize = this.GetRandomStackSize();
            var items = Enumerable.Repeat(default(T), stackSize).ToArray();
            var slot = this.slotFactory.Full(items!);

            var amount = this.random.Next(1, stackSize);
            var result = slot.Get(amount);

            Assert.That(result, Is.EqualTo(items.Take(amount).ToArray()));
        }

        [Test]
        [IgnoreIfReferenceType]
        public void GetAmount_FullSlot_ValueType_DecreasesAmount()
        {
            var stackSize = this.GetRandomStackSize();
            var items = Enumerable.Repeat(default(T), stackSize).ToArray();
            var slot = this.slotFactory.Full(items!);

            var amount = this.random.Next(1, stackSize);
            slot.Get(amount);

            Assert.That(slot.Amount, Is.EqualTo(stackSize - amount));
        }

        [Test]
        [IgnoreIfReferenceType]
        public void GetAmount_FullSlot_ValueType_RemovesItems()
        {
            var stackSize = this.GetRandomStackSize();
            var items = Enumerable.Repeat(default(T), stackSize).ToArray();
            var slot = this.slotFactory.Full(items!);

            var amount = this.random.Next(1, stackSize);
            slot.Get(amount);

            Assert.That(slot.GetContents(), Has.Exactly(stackSize - amount).Items);
        }

        [Test]
        public void GetAmount_SlotWithEnoughItems_DecreasesAmount()
        {
            var stackSize = this.GetRandomStackSize();
            var items = this.itemFactory.CreateManyRandom(stackSize);
            var slot = this.slotFactory.Full(items);

            var amount = this.random.Next(1, stackSize);
            slot.Get(amount);

            Assert.That(slot.Amount, Is.EqualTo(stackSize - amount));
        }

        [Test]
        public void GetAmount_SlotWithEnoughItems_RemovesItems()
        {
            var stackSize = this.GetRandomStackSize();
            var items = this.itemFactory.CreateManyRandom(stackSize);
            var slot = this.slotFactory.Full(items);

            var amount = this.random.Next(1, stackSize);
            slot.Get(amount);

            Assert.That(slot.GetContents(), Has.Exactly(stackSize - amount).Items);
        }

        [Test]
        public void GetAmount_SlotWithEnoughItems_ReturnsItems()
        {
            var stackSize = this.GetRandomStackSize();
            var items = this.itemFactory.CreateManyRandom(stackSize);
            var slot = this.slotFactory.Full(items);

            var amount = this.random.Next(1, stackSize);
            var result = slot.Get(amount);

            Assert.That(result, Is.EquivalentTo(items[0..amount]));
        }

        [Test]
        public void GetAmount_SlotWithoutEnoughItems_ReturnsAllItems()
        {
            var stackSize = this.GetRandomStackSize();
            var halfStack = stackSize / 2;
            var items = this.itemFactory.CreateManyRandom(halfStack);
            var slot = this.slotFactory.WithItems(items, stackSize);

            var amount = this.random.Next(halfStack + 1, stackSize);
            var result = slot.Get(amount);

            Assert.That(result, Is.EquivalentTo(items));
        }

        [Test]
        public void GetAmount_SlotWithoutEnoughItems_RemovesAllItems()
        {
            var stackSize = this.GetRandomStackSize();
            var halfStack = stackSize / 2;
            var items = this.itemFactory.CreateManyRandom(halfStack);
            var slot = this.slotFactory.WithItems(items, stackSize);

            var amount = this.random.Next(halfStack + 1, stackSize);
            slot.Get(amount);

            Assert.That(slot.GetContents(), Is.All.Null);
        }

        [Test]
        public void GetAmount_AmountExceedingAvailable_ReturnsAllItems()
        {
            var stackSize = this.GetRandomStackSize();
            var items = this.itemFactory.CreateManyRandom(stackSize);
            var slot = this.slotFactory.WithItems(items, stackSize);

            var result = slot.Get(stackSize * 2);

            Assert.That(result, Is.EquivalentTo(items));
        }

        [Test]
        public void GetAmount_AmountExceedingAvailable_DecreasesAmountToZero()
        {
            var stackSize = this.GetRandomStackSize();
            var itemSize = this.random.Next(1, stackSize / 2);
            var items = this.itemFactory.CreateManyRandom(itemSize);
            var slot = this.slotFactory.WithItems(items, stackSize);

            var amount = this.random.Next(itemSize, stackSize);
            slot.Get(amount);

            Assert.That(slot.Amount, Is.EqualTo(0));
        }
    }
}
