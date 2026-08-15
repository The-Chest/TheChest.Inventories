using TheChest.Tests.Common.Attributes;
using TheChest.Tests.Common.Extensions.Slots;

namespace TheChest.Inventories.Tests.Slots.InventoryStackSlot
{
    public partial class InventoryStackSlotTests<T>
    {
        [Test]
        public void Get_Empty_ThrowsInvalidOperationException()
        {
            var stackSize = this.GetRandomStackSize();
            var slot = this.slotFactory.Empty(stackSize);

            Assert.That(
                () => slot.Get(), 
                Throws.InvalidOperationException.With.Message.EqualTo("The slot is empty")
            );
        }

        [Test]
        [IgnoreIfReferenceType]
        public void Get_Full_ValueType_ReturnsItem()
        {
            var stackSize = this.GetRandomStackSize();
            var items = Enumerable.Repeat(default(T), stackSize).ToArray();
            var slot = this.slotFactory.Full(items!);

            var result = slot.Get();

            Assert.That(result, Is.EqualTo(items[0]));
        }

        [Test]
        public void Get_Full_ReturnsItem()
        {
            var stackSize = this.GetRandomStackSize();
            var items = this.itemFactory.CreateManyRandom(stackSize);
            var slot = this.slotFactory.Full(items);

            var result = slot.Get();

            Assert.That(result, Is.EqualTo(items[0]));
        }

        [Test]
        public void Get_Full_DecreasesAmountByOne()
        {
            var stackSize = this.GetRandomStackSize();
            var items = this.itemFactory.CreateMany(stackSize);
            var slot = this.slotFactory.Full(items);

            slot.Get();

            Assert.That(slot.Amount, Is.EqualTo(stackSize - 1));
        }

        [Test]
        public void Get_Full_RemovesOneItemFromSlot()
        {
            var stackSize = this.GetRandomStackSize();
            var items = this.itemFactory.CreateMany(stackSize);
            var slot = this.slotFactory.Full(items);

            slot.Get();

            var expectedItems = items.Skip(1).ToArray();
            Assert.That(slot.GetContents(), Is.EqualTo(expectedItems));
        }
    }
}
