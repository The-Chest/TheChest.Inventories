using TheChest.Tests.Common.Extensions.Slots;

namespace TheChest.Inventories.Tests.Slots.InventoryStackSlot
{
    public partial class InventoryStackSlotTests<T>
    {
        [Test]
        public void GetAll_EmptySlot_ReturnsEmptyArray()
        {
            var stackSize = this.GetRandomStackSize();
            var slot = this.slotFactory.Empty(stackSize);

            var result = slot.GetAll();

            Assert.That(result, Is.Empty);
        }

        [Test]
        public void GetAll_FullSlot_RemovesAllItems()
        {
            var stackSize = this.GetRandomStackSize();
            var items = this.itemFactory.CreateManyRandom(stackSize);
            var slot = this.slotFactory.Full(items);

            slot.GetAll();

            Assert.That(slot.GetContents(), Is.Empty);
        }

        [Test]
        public void GetAll_FullSlot_DecreasesAmountToZero()
        {
            var stackSize = this.GetRandomStackSize();
            var items = this.itemFactory.CreateManyRandom(stackSize);
            var slot = this.slotFactory.Full(items);

            slot.GetAll();

            Assert.That(slot.Amount, Is.Zero);
        }

        [Test]
        public void GetAll_FullSlot_ReturnsAllItems()
        {
            var stackSize = this.GetRandomStackSize();
            var items = this.itemFactory.CreateManyRandom(stackSize);
            var slot = this.slotFactory.Full(items);

            var result = slot.GetAll();

            Assert.That(result, Is.EquivalentTo(items));
        }

        [Test]
        public void GetAll_PartiallyFilledSlot_ReturnsAllItems()
        {
            var stackSize = this.GetRandomStackSize();
            var amount = this.random.Next(1, stackSize - 1);
            var items = this.itemFactory.CreateManyRandom(amount);
            var slot = this.slotFactory.WithItems(items, stackSize);

            var result = slot.GetAll();

            Assert.That(result, Is.EqualTo(items));
        }
    }
}
