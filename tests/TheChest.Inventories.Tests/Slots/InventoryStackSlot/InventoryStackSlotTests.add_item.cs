using TheChest.Tests.Common.Extensions.Slots;

namespace TheChest.Inventories.Tests.Slots
{
    public partial class InventoryStackSlotTests<T>
    {
        [Test]
        public void Add_NullItem_ThrowsNullArgumentException()
        {
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var slot = this.slotFactory.EmptySlot(stackSize);
            var item = (T)default!;

            Assert.That(() => slot.Add(item), Throws.ArgumentNullException);
        }

        [Test]
        public void Add_FullSlot_DoNotAddToContent()
        {
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var items = this.itemFactory.CreateMany(stackSize);
            var slot = this.slotFactory.FullSlot(items);

            var item = this.itemFactory.CreateDefault();
            slot.Add(item);

            Assert.That(slot.GetContents(), Has.No.AnyOf(item));
        }

        [Test]
        public void Add_EmptySlot_AddsToContent()
        {
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var slot = this.slotFactory.EmptySlot(stackSize);

            var item = this.itemFactory.CreateDefault();
            var expecteditem = new T[1] { item };
            slot.Add(item);

            Assert.That(slot.GetContents(), Has.One.EqualTo(expecteditem[0]));
        }

        [Test]
        public void Add_SlotWithSameItem_AddsToContent()
        {
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var items = this.itemFactory.CreateMany(stackSize / 2 );
            var slot = this.slotFactory.WithItems(items, stackSize);

            var item = this.itemFactory.CreateDefault();
            var expectedItems = items.Append(item).ToArray();

            slot.Add(item);

            Assert.Fail("Improve this test validation");
            Assert.That(slot.GetContents()[0..((stackSize / 2) - 1)], Is.EqualTo(expectedItems));
        }

        [Test]
        public void Add_SlotWithDifferentItem_DoNotAddToContent()
        {
            var items = this.itemFactory.CreateMany(5);
            var slot = this.slotFactory.WithItems(items, 10);

            var item = this.itemFactory.CreateRandom();
            slot.Add(item);

            Assert.That(slot.GetContents(), Is.Not.Contains(item));
        }
    }
}
