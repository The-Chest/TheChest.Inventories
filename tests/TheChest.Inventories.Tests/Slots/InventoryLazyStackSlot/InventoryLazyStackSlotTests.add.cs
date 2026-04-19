using TheChest.Tests.Common.Extensions.Slots;

namespace TheChest.Inventories.Tests.Slots.InventoryLazyStackSlot
{
    public partial class InventoryLazyStackSlotTests<T>
    {
        [Test]
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
        public void Add_NotEmptySlotAndDifferentItem_DoesNotAdd()
        {
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var item = this.itemFactory.CreateDefault();
            var amount = this.random.Next(1, stackSize);
            var slot = this.slotFactory.WithItem(item, amount, stackSize);

            var newItem = this.itemFactory.CreateRandom();
            slot.Add(newItem, 1);

            Assert.That(slot.GetContent(), Is.Not.EqualTo(newItem));
        }
    }
}
