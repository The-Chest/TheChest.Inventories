using TheChest.Tests.Common.Extensions.Slots;

namespace TheChest.Inventories.Tests.Slots.InventoryStackSlot
{
    public partial class InventoryStackSlotTests<T>
    {
        [Test]
        public void Get_FullSlot_RemovesOneItemFromSlot()
        {
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var items = this.itemFactory.CreateMany(stackSize);
            var slot = this.slotFactory.FullSlot(items);

            slot.Get();

            var expectedItems = items.Skip(1).ToArray();
            Assert.That(slot.GetContents(), Is.EqualTo(expectedItems));
        }
    }
}
