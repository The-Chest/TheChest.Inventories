using TheChest.Tests.Common.Extensions.Slots;

namespace TheChest.Inventories.Tests.Slots.InventoryStackSlot
{
    public partial class InventoryStackSlotTests<T>
    {
        [Test]
        public void ReplaceOne_NullItemReplace_ThrowsArgumentNullException()
        {
            var slotItems = this.itemFactory.CreateMany(10);
            var slot = this.slotFactory.FullSlot(slotItems);

            Assert.That(() => slot.Replace(default(T)!), Throws.ArgumentNullException);
        }

        [Test]
        public void ReplaceOne_EmptySlot_AddsItem()
        {
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var slot = this.slotFactory.EmptySlot(stackSize);

            var item = this.itemFactory.CreateDefault();
            var expectedResult = new T[stackSize];
            expectedResult[0] = item;

            slot.Replace(item);

            Assert.That(slot.GetContents(), Is.EqualTo(expectedResult));
        }

        [Test]
        public void ReplaceOne_ItemDifferentFromSlot_AddsItemsToSlot()
        {
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var items = this.itemFactory.CreateMany(stackSize);
            var slot = this.slotFactory.WithItems(items, stackSize);

            var replacingItem = this.itemFactory.CreateRandom();
            var expectedResult = new T[stackSize];
            expectedResult[0] = replacingItem;

            slot.Replace(replacingItem);

            Assert.That(slot.GetContents(), Is.EqualTo(expectedResult));
        }

        [Test]
        public void ReplaceOne_ItemEqualToSlot_AddsItemsToSlot()
        {
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var items = this.itemFactory.CreateMany(stackSize / 2);
            var slot = this.slotFactory.WithItems(items, stackSize);

            var replacingItem = this.itemFactory.CreateDefault();

            slot.Replace(replacingItem);

            Assert.That(slot.GetContents(), Has.Exactly(1).EqualTo(replacingItem));
        }
    }
}
