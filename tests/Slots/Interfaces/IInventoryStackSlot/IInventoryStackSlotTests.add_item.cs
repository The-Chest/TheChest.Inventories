namespace TheChest.Inventories.Tests.Slots.Interfaces
{
    public partial class IInventoryStackSlotTests<T>
    {
        [Test]
        public void Add_EmptySlot_IncreasesAmountToOne()
        {
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var slot = this.slotFactory.Empty(stackSize);
            var item = this.itemFactory.CreateDefault();
            
            slot.Add(item);
            
            Assert.That(slot.Amount, Is.EqualTo(1));
        }
    }
}
