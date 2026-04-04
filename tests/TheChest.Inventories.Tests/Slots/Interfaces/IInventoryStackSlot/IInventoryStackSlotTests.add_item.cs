namespace TheChest.Inventories.Tests.Slots.Interfaces
{
    public partial class IInventoryStackSlotTests<T>
    {
        [Test]
        public void Add_EmptySlot_IncreasesAmountToOne()
        {
            var slot = this.slotFactory.EmptySlot();
            var item = this.itemFactory.CreateDefault();
            slot.Add(item);
            Assert.That(slot.Amount, Is.EqualTo(1));
        }
    }
}
