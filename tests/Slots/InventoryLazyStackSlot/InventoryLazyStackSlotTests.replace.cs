using TheChest.Tests.Common.Attributes;
using TheChest.Tests.Common.Extensions.Slots;

namespace TheChest.Inventories.Tests.Slots.InventoryLazyStackSlot
{
    public partial class InventoryLazyStackSlotTests<T>
    {
        [Test]
        [IgnoreIfValueType]
        public void Replace_NullItem_ThrowsArgumentNullException()
        {
            var maxAmount = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var slot = this.slotFactory.WithItem(this.itemFactory.CreateDefault(), 1, maxAmount);

            Assert.That(
                () => slot.Replace(default!, 1),
                Throws.ArgumentNullException.With.Property("ParamName").EqualTo("item")
            );
        }

        [TestCase(0)]
        [TestCase(-1)]
        [TestCase(MAX_STACK_SIZE_TEST + 1)]
        public void Replace_InvalidAmount_ThrowsArgumentOutOfRangeException(int amount)
        {
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var item = this.itemFactory.CreateDefault();
            var slot = this.slotFactory.WithItem(item, 1, stackSize);

            Assert.That(
                () => slot.Replace(item, amount),
                Throws.TypeOf<ArgumentOutOfRangeException>().With.Property("ParamName").EqualTo("amount")
            );
        }

        [Test]
        public void Replace_EmptySlot_ThrowsInvalidOperationException()
        {
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var slot = this.slotFactory.Empty(stackSize);
            var item = this.itemFactory.CreateDefault();

            Assert.That(
                () => slot.Replace(item, 1),
                Throws.InvalidOperationException.With.Message.EqualTo("Cannot replace an empty slot")
            );
        }

        [Test]
        public void Replace_EmptySlot_DoesNotAddItems()
        {
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var slot = this.slotFactory.Empty(stackSize);
            var item = this.itemFactory.CreateDefault();

            Assert.That(
                () => slot.Replace(item, 1),
                Throws.InvalidOperationException.With.Message.EqualTo("Cannot replace an empty slot")
            );

            Assert.That(slot.IsEmpty, Is.True);
        }

        [Test]
        public void Replace_SlotWithDifferentItem_ReplacesItems()
        {
            var initialItem = this.itemFactory.CreateDefault();
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var initialAmount = this.random.Next(1, stackSize + 1);
            var slot = this.slotFactory.WithItem(initialItem, initialAmount, stackSize);
            var newItem = this.itemFactory.CreateRandom();
            var newAmount = this.random.Next(1, stackSize + 1);

            slot.Replace(newItem, newAmount);

            Assert.Multiple(() =>
            {
                Assert.That(slot.GetContent(), Is.EqualTo(newItem));
                Assert.That(slot.Amount, Is.EqualTo(newAmount));
            });
        }

        [Test]
        public void Replace_SlotWithSameItem_ReplacesAmount()
        {
            var item = this.itemFactory.CreateDefault();
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var initialAmount = this.random.Next(1, stackSize + 1);
            var slot = this.slotFactory.WithItem(item, initialAmount, stackSize);
            var newAmount = initialAmount == stackSize ? initialAmount - 1 : initialAmount + 1;

            slot.Replace(item, newAmount);

            Assert.That(slot.Amount, Is.EqualTo(newAmount));
        }

        [Test]
        public void Replace_SlotWithItems_ReturnsPreviousItems()
        {
            var initialItem = this.itemFactory.CreateDefault();
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var initialAmount = this.random.Next(1, stackSize + 1);
            var slot = this.slotFactory.WithItem(initialItem, initialAmount, stackSize);
            var newItem = this.itemFactory.CreateRandom();
            var newAmount = this.random.Next(1, stackSize + 1);

            var result = slot.Replace(newItem, newAmount);

            Assert.That(result, Has.Length.EqualTo(initialAmount).And.All.EqualTo(initialItem));
        }
    }
}
