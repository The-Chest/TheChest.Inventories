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
            var stackSize = this.GetRandomStackSize();
            var slotItems = this.itemFactory.CreateMany(stackSize);
            var slot = this.slotFactory.Full(slotItems);

            Assert.That(
                () => slot.Replace(item: default), 
                Throws.ArgumentNullException.With.Property("ParamName").EqualTo("item")
            );
        }

        [Test]
        public void ReplaceOne_EmptySlot_ReturnsEmptyArray()
        {
            var stackSize = this.GetRandomStackSize();
            var slot = this.slotFactory.Empty(stackSize);

            var item = this.itemFactory.CreateDefault();
            var result = slot.Replace(item);

            Assert.That(result, Is.Empty);
        }

        [Test]
        public void ReplaceOne_ItemDifferentFromSlot_ReturnsItemsFromSlot()
        {
            var stackSize = this.GetRandomStackSize();
            var items = this.itemFactory.CreateMany(stackSize);
            var slot = this.slotFactory.WithItems(items, stackSize);

            var replacingItem = this.itemFactory.CreateRandom();
            var result = slot.Replace(replacingItem);

            Assert.That(result, Is.EqualTo(items));
        }

        [Test]
        public void ReplaceOne_ItemEqualToSlot_ReturnsItemsFromSlot()
        {
            var stackSize = this.GetRandomStackSize();
            var items = this.itemFactory.CreateMany(stackSize);
            var slot = this.slotFactory.WithItems(items, stackSize);

            var replacingItem = this.itemFactory.CreateDefault();
            var result = slot.Replace(replacingItem);

            Assert.That(result, Is.EqualTo(items));
        }

        [Test]
        public void ReplaceItem_EmptySlot_ReplacesItem()
        {
            var stackSize = this.GetRandomStackSize();
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
            var stackSize = this.GetRandomStackSize();
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
            var stackSize = this.GetRandomStackSize();
            var items = this.itemFactory.CreateMany(stackSize / 2);
            var slot = this.slotFactory.WithItems(items, stackSize);

            var replacingItem = this.itemFactory.CreateDefault();
            slot.Replace(replacingItem);

            Assert.That(slot.GetContents(), Has.Exactly(1).EqualTo(replacingItem));
        }
    }
}
