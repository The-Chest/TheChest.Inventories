using TheChest.Tests.Common.Extensions.Slots;

namespace TheChest.Inventories.Tests.Slots.InventoryStackSlot
{
    public partial class InventoryStackSlotTests<T>
    {
        [Test]
        public void GetAll_Full_RemovesAllContents()
        {
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var items = this.itemFactory.CreateManyRandom(stackSize);
            var slot = this.slotFactory.Full(items);

            slot.GetAll();

            Assert.That(slot.GetContents(), Is.Empty.And.All.Null);
        }

        [Test]
        public void GetAll_Full_DecreasesAmountToZero()
        {
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var items = this.itemFactory.CreateManyRandom(stackSize);
            var slot = this.slotFactory.Full(items);

            slot.GetAll();

            Assert.That(slot.Amount, Is.EqualTo(0));
        }

        [Test]
        public void GetAll_Full_ReturnsAllItems()
        {
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var items = this.itemFactory.CreateManyRandom(stackSize);
            var slot = this.slotFactory.Full(items);

            var result = slot.GetAll();

            Assert.That(result, Is.EquivalentTo(items));
        }

        [Test]
        public void GetAll_PartiallyFilled_ReturnsAllItemFromSlot()
        {
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var amount = this.random.Next(1, stackSize - 1);
            var items = this.itemFactory.CreateManyRandom(amount);
            var slot = this.slotFactory.WithItems(items, stackSize);

            var result = slot.GetAll();

            Assert.That(result, Is.EquivalentTo(items));
        }

        [Test]
        public void GetAll_Empty_ReturnsEmptyArray()
        {
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var slot = this.slotFactory.Empty(stackSize);

            var result = slot.GetAll();

            Assert.That(result, Is.EquivalentTo(Array.Empty<T>()));
        }
    }
}
