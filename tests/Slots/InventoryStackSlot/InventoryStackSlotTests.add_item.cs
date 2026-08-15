using TheChest.Tests.Common.Attributes;
using TheChest.Tests.Common.Extensions.Slots;

namespace TheChest.Inventories.Tests.Slots.InventoryStackSlot
{
    public partial class InventoryStackSlotTests<T>
    {
        [Test]
        [IgnoreIfValueType]
        public void Add_NullItem_ThrowsArgumentNullException()
        {
            var stackSize = this.GetRandomStackSize();
            var slot = this.slotFactory.Empty(stackSize);

            Assert.That(
                () => slot.Add(item: default), 
                Throws.ArgumentNullException.With.Property("ParamName").EqualTo("item")
            );
        }

        [Test]
        public void Add_FullSlot_ThrowsInvalidOperationException()
        {
            var stackSize = this.GetRandomStackSize();
            var items = this.itemFactory.CreateMany(stackSize);
            var slot = this.slotFactory.Full(items);

            var item = this.itemFactory.CreateRandom();
            Assert.That(
                () => slot.Add(item), 
                Throws.InvalidOperationException.With.Message.EqualTo("The slot is full")
            );
        }

        [Test]
        public void Add_SlotWithDifferentItem_ThrowsInvalidOperationException()
        {
            var stackSize = this.GetRandomStackSize();
            var halfStackSize = stackSize / 2;
            var items = this.itemFactory.CreateMany(halfStackSize);
            var slot = this.slotFactory.WithItems(items, stackSize);

            var item = this.itemFactory.CreateRandom();
            Assert.That(
                () => slot.Add(item),
                Throws.InvalidOperationException.With.Message.EqualTo("Cannot add items that are different from the items already in the slot")
            );
        }

        [Test]
        public void Add_EmptySlot_AddsItem()
        {
            var stackSize = this.GetRandomStackSize();
            var slot = this.slotFactory.Empty(stackSize);

            var item = this.itemFactory.CreateRandom();
            slot.Add(item);

            Assert.That(slot.GetContents(), Has.One.EqualTo(item));
        }

        [Test]
        public void Add_EmptySlot_IncreasesAmount()
        {
            var stackSize = this.GetRandomStackSize();
            var slot = this.slotFactory.Empty(stackSize);

            var item = this.itemFactory.CreateDefault();
            slot.Add(item);

            Assert.That(slot.Amount, Is.EqualTo(1));
        }

        [Test]
        [IgnoreIfReferenceType]
        public void Add_EmptySlot_DefaultValueItem_AddsItem()
        {
            var stackSize = this.GetRandomStackSize();
            var slot = this.slotFactory.Empty(stackSize);

            var item = default(T);
            slot.Add(item);

            Assert.That(slot.GetContents(), Has.One.EqualTo(item));
        }

        [Test]
        [IgnoreIfReferenceType]
        public void Add_EmptySlot_DefaultValueItem_IncreasesAmount()
        {
            var stackSize = this.GetRandomStackSize();
            var slot = this.slotFactory.Empty(stackSize);

            var item = default(T);
            slot.Add(item);

            Assert.That(slot.Amount, Is.EqualTo(1));
        }

        [Test]
        public void Add_SlotWithSameItem_AddsItem()
        {
            var stackSize = this.GetRandomStackSize();
            var halfStackSize = stackSize / 2;
            var items = this.itemFactory.CreateMany(halfStackSize);
            var slot = this.slotFactory.WithItems(items, stackSize);

            var item = this.itemFactory.CreateDefault();
            var expectedItems = items.Append(item).ToArray();

            slot.Add(item);

            Assert.That(slot.GetContents()[0..(halfStackSize + 1)], Is.EquivalentTo(expectedItems));
        }
    }
}
