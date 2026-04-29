using TheChest.Tests.Common.Extensions.Slots;

namespace TheChest.Inventories.Tests.Slots.InventoryLazyStackSlot
{
    public partial class InventoryLazyStackSlotTests<T>
    {
        [Test]
        public void GetAll_SlotWithContent_ClearsSlot()
        {
            var item = this.itemFactory.CreateDefault();
            var maxAmount = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var amount = this.random.Next(2, maxAmount);
            var slot = this.slotFactory.WithItem(item, amount, maxAmount);

            slot.GetAll();

            Assert.That(slot.GetContents(), Is.Empty);
        }
    }
}
