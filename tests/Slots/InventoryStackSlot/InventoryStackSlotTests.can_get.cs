using NUnit.Framework;
using System;

namespace TheChest.Inventories.Tests.Slots.InventoryStackSlot
{
    public partial class InventoryStackSlotTests<T>
    {
        [TestCase(0)]
        [TestCase(-1)]
        public void CanGet_InvalidAmount_ThrowsArgumentOutOfRangeException(int amount)
        {
            var stackSize = this.GetRandomStackSize();
            var slot = this.slotFactory.Empty(stackSize);

            Assert.That(
                () => slot.CanGet(amount),
                Throws.TypeOf<ArgumentOutOfRangeException>().With.Property("ParamName").EqualTo("amount")
            );
        }

        [Test]
        public void CanGet_EmptySlot_ReturnsFalse()
        {
            var stackSize = this.GetRandomStackSize();
            var slot = this.slotFactory.Empty(stackSize);

            var result = slot.CanGet(1);

            Assert.That(result, Is.False);
        }

        [Test]
        public void CanGet_SlotWithEnoughItems_ReturnsTrue()
        {
            var stackSize = this.GetRandomStackSize();
            var slot = this.slotFactory.Full(this.itemFactory.CreateManyRandom(stackSize));
            var amount = this.random.Next(1, stackSize + 1);

            var result = slot.CanGet(amount);

            Assert.That(result, Is.True);
        }

        [Test]
        public void CanGet_SlotWithoutEnoughItems_ReturnsFalse()
        {
            var stackSize = this.GetRandomStackSize();
            var slot = this.slotFactory.WithItems(this.itemFactory.CreateManyRandom(1), stackSize);

            var result = slot.CanGet(2);

            Assert.That(result, Is.False);
        }
    }
}
