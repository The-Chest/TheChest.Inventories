using TheChest.Tests.Common.Extensions.Slots;

namespace TheChest.Inventories.Tests.Slots
{
    public partial class InventoryLazyStackSlotTests<T>
    {
        [Test]
        public void GetAll_SlotWithContent_ClearsSlot()
        {
            var item = this.itemFactory.CreateDefault();
            var amount = this.random.Next(2, 10);
            var maxAmount = this.random.Next(10, 20);
            var slot = this.slotFactory.WithItem(item, amount, maxAmount);

            slot.GetAll();

            Assert.That(slot.GetContents(), Is.Empty);
        }
    }
}
