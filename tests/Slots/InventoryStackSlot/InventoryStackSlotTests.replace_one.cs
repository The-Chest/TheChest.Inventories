using TheChest.Tests.Common.Attributes;
using TheChest.Tests.Common.Extensions.Slots;

namespace TheChest.Inventories.Tests.Slots.InventoryStackSlot
{
    public partial class InventoryStackSlotTests<T>
    {
        [Test]
        [IgnoreIfValueType]
        public void ReplaceItem_NullItem_ThrowsArgumentNullException()
        {
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var slotItems = this.itemFactory.CreateMany(stackSize);
            var slot = this.slotFactory.Full(slotItems);

            Assert.That(() => slot.Replace(default(T)!), Throws.ArgumentNullException);
        }

        [Test]
        public void ReplaceItem_EmptySlot_ReplacesItem()
        {
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var slot = this.slotFactory.Empty(stackSize);

            var item = this.itemFactory.CreateDefault();
            var expectedResult = new T[1];
            expectedResult[0] = item;

            slot.Replace(item);

            Assert.That(slot.GetContents(), Is.EqualTo(expectedResult));
        }

        [Test]
        public void ReplaceItem_SlotWithDifferentItem_ReplacesItem()
        {
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var items = this.itemFactory.CreateMany(stackSize);
            var slot = this.slotFactory.WithItems(items, stackSize);

            var replacingItem = this.itemFactory.CreateRandom();
            var expectedResult = new T[1];
            expectedResult[0] = replacingItem;

            slot.Replace(replacingItem);

            Assert.That(slot.GetContents(), Is.EqualTo(expectedResult));
        }

        [Test]
        public void ReplaceItem_SlotWithSameItem_ReplacesItem()
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
