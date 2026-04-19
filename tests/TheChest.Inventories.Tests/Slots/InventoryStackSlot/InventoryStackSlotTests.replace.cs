using TheChest.Tests.Common.Extensions.Slots;

namespace TheChest.Inventories.Tests.Slots.InventoryStackSlot
{
    public partial class InventoryStackSlotTests<T>
    {
        [Test]
        public void Replace_EmptyItemReplace_ThrowsArgumentException()
        {
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var slotItems = this.itemFactory.CreateMany(stackSize);
            var slot = this.slotFactory.Full(slotItems);

            var items = Array.Empty<T>();
            Assert.That(() => slot.Replace(items), Throws.ArgumentException);
        }

        [Test]
        public void Replace_ItemsBiggerThanMaxAmount_ThrowsArgumentOutOfRangeException()
        {
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var slot = this.slotFactory.Empty(stackSize);

            var items = this.itemFactory.CreateMany(stackSize + 1);
            Assert.That(() => slot.Replace(items), Throws.Exception.TypeOf<ArgumentOutOfRangeException>());
        }

        [Test]
        public void Replace_EmptySlot_AddsItem()
        {
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var slot = this.slotFactory.Empty(stackSize);

            var items = this.itemFactory.CreateMany(stackSize / 2);
            var expectedResult = (T[])items.Clone();
            slot.Replace(items);

            Assert.That(slot.GetContents(), Is.EquivalentTo(expectedResult));
        }

        [Test]
        public void Replace_ItemsDifferentFromSlot_AddsItemsToSlot()
        {
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var items = this.itemFactory.CreateMany(stackSize);
            var slot = this.slotFactory.WithItems(items, stackSize);

            var replacingItems = this.itemFactory.CreateManyRandom(stackSize);
            var expectedResult = (T[])replacingItems.Clone();

            slot.Replace(replacingItems);

            Assert.That(slot.GetContents(), Is.EqualTo(expectedResult));
        }

        [Test]
        public void Replace_ItemsEqualToSlot_AddsItemsToSlot()
        {
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var halfStackSize = stackSize / 2;
            var items = this.itemFactory.CreateMany(halfStackSize);
            var slot = this.slotFactory.WithItems(items, stackSize);

            var amount = this.random.Next(1, stackSize);
            var replacingItems = this.itemFactory.CreateMany(amount);
            var expectedResult = (T[])replacingItems.Clone();

            slot.Replace(replacingItems);

            Assert.That(slot.GetContents()[0..amount], Is.EqualTo(expectedResult));
        }
    }
}
