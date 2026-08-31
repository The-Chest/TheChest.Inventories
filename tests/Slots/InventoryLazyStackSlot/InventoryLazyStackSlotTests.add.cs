using TheChest.Tests.Common.Attributes;
using TheChest.Tests.Common.Extensions.Slots;

namespace TheChest.Inventories.Tests.Slots.InventoryLazyStackSlot
{
    public partial class InventoryLazyStackSlotTests<T>
    {
        [Test]
        [IgnoreIfValueType]
        public void Add_ItemIsNull_ThrowsArgumentNullException()
        {
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var slot = this.slotFactory.Empty(stackSize);

            Assert.Throws<ArgumentNullException>(() => slot.Add(default!, 1));
        }

        [TestCase(-1)]
        [TestCase(0)]
        public void Add_InvalidAmount_ThrowsArgumentOutOfRangeException(int amount)
        {
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var slot = this.slotFactory.Empty(stackSize);

            var item = this.itemFactory.CreateDefault();
            Assert.Throws<ArgumentOutOfRangeException>(() => slot.Add(item, amount));
        }

        [Test]
        public void Add_AmountBiggerThanSlotSize_ThrowsArgumentOutOfRangeException()
        {
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var slot = this.slotFactory.Empty(stackSize);

            var item = this.itemFactory.CreateDefault();
            var amount = stackSize + this.random.Next(1, 5);
            Assert.Throws<ArgumentOutOfRangeException>(() => slot.Add(item, amount));
        }

        [Test]
        public void Add_NotEmptySlotAndDifferentItem_ThrowsInvalidOperationExceptionAndDoesNotAdd()
        {
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var item = this.itemFactory.CreateDefault();
            var amount = this.random.Next(1, stackSize);
            var slot = this.slotFactory.WithItem(item, amount, stackSize);


            var newItem = this.itemFactory.CreateRandom();
            Assert.Throws<InvalidOperationException>(() => slot.Add(newItem, 1));
            Assert.That(slot.GetContent(), Is.Not.EqualTo(newItem));
        }

        [Test]
        public void Add_FullSlot_ThrowsInvalidOperationException()
        {
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var item = this.itemFactory.CreateDefault();
            var slot = this.slotFactory.FullSlot(item, stackSize);

            var amount = this.random.Next(1, stackSize);

            Assert.Throws<InvalidOperationException>(() => slot.Add(item, amount));
        }

        [Test]
        public void Add_NotEmptySlotAndDifferentItem_ThrowsInvalidOperationException()
        {
            var item = this.itemFactory.CreateDefault();
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var halfStackSize = stackSize / 2;
            var amount = this.random.Next(1, halfStackSize);
            var slot = this.slotFactory.WithItem(item, amount, stackSize);

            var newItem = this.itemFactory.CreateRandom();
            var addAmount = this.random.Next(1, stackSize - amount + 1);

            Assert.Throws<InvalidOperationException>(() => slot.Add(newItem, addAmount));
        }

        [Test]
        public void Add_EmptySlot_AddsItems()
        {
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var slot = this.slotFactory.Empty(stackSize);

            var item = this.itemFactory.CreateDefault();
            var amount = this.random.Next(1, stackSize);
            var result = slot.Add(item, amount);

            Assert.Multiple(() =>
            {
                Assert.That(result, Is.EqualTo(0));
                Assert.That(slot.IsEmpty, Is.False);
            });
        }

        [Test]
        public void Add_SlotContainingSameItem_ReturnsZero()
        {
            var item = this.itemFactory.CreateDefault();
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var halfStackSize = stackSize / 2;
            var amount = this.random.Next(1, halfStackSize);
            var slot = this.slotFactory.WithItem(item, amount, stackSize);

            var newAmount = this.random.Next(1, stackSize - amount + 1);
            var result = slot.Add(item, newAmount);

            Assert.That(result, Is.Zero);
        }

        [Test]
        public void Add_SlotContainingSameItem_AddItems()
        {
            var item = this.itemFactory.CreateDefault();
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var halfStackSize = stackSize / 2;
            var amount = this.random.Next(1, halfStackSize);
            var slot = this.slotFactory.WithItem(item, amount, stackSize);

            var newAmount = this.random.Next(1, stackSize - amount);
            var newItem = this.itemFactory.CreateDefault();
            slot.Add(newItem, newAmount);

            Assert.That(slot.Amount, Is.EqualTo(amount + newAmount));
        }

    }
}
