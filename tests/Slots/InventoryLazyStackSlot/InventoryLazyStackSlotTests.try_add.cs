using TheChest.Tests.Common.Extensions.Slots;

namespace TheChest.Inventories.Tests.Slots.InventoryLazyStackSlot
{
    public partial class InventoryLazyStackSlotTests<T>
    {
        [Test]
        public void TryAdd_FullSlot_DoesNotChangeAmount()
        {
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var item = this.itemFactory.CreateDefault();
            var slot = this.slotFactory.Full(item, stackSize);

            slot.TryAdd(item, 1);

            Assert.That(slot.Amount, Is.EqualTo(stackSize));
        }

        [Test]
        public void TryAdd_NotEmptySlotAndDifferentItem_DoesNotChangeContent()
        {
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var amount = this.random.Next(1, stackSize);
            var item = this.itemFactory.CreateDefault();
            var newItem = this.itemFactory.CreateRandom();
            var slot = this.slotFactory.WithItem(item, amount, stackSize);

            slot.TryAdd(newItem, 1);

            Assert.That(slot.GetContent(), Is.EqualTo(item));
        }

        [Test]
        public void TryAdd_AmountBiggerThanAvailableAmount_DoesNotChangeAmount()
        {
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var amount = this.random.Next(1, stackSize);
            var item = this.itemFactory.CreateDefault();
            var slot = this.slotFactory.WithItem(item, amount, stackSize);
            var addAmount = slot.AvailableAmount + 1;

            slot.TryAdd(item, addAmount);

            Assert.That(slot.Amount, Is.EqualTo(amount));
        }

        [Test]
        public void TryAdd_ValidInput_AddsItems()
        {
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var amount = this.random.Next(1, stackSize);
            var addAmount = this.random.Next(1, stackSize - amount + 1);
            var item = this.itemFactory.CreateDefault();
            var slot = this.slotFactory.WithItem(item, amount, stackSize);

            slot.TryAdd(item, addAmount);

            Assert.That(slot.Amount, Is.EqualTo(amount + addAmount));
        }
    }
}
