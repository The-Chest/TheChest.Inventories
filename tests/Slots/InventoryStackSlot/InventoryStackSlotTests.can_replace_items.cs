using TheChest.Tests.Common.Attributes;

namespace TheChest.Inventories.Tests.Slots.InventoryStackSlot
{
    public partial class InventoryStackSlotTests<T>
    {
        [Test]
        public void CanReplaceItems_EmptyArray_ReturnsFalse()
        {
            var stackSize = this.GetRandomStackSize();
            var startItems = this.itemFactory.CreateMany(stackSize);
            var slot = this.slotFactory.Full(startItems);

            var result = slot.CanReplace(Array.Empty<T>());

            Assert.That(result, Is.False);
        }

        [Test]
        [IgnoreIfValueType]
        public void CanReplaceItems_OneItemNullInArray_ReturnsFalse()
        {
            var stackSize = this.GetRandomStackSize();
            var startItems = this.itemFactory.CreateMany(stackSize);
            var slot = this.slotFactory.Full(startItems);

            var items = this.itemFactory.CreateMany(stackSize / 2)
                .Append(default)
                .ToArray();
            var result = slot.CanReplace(items!);

            Assert.That(result, Is.False);
        }

        [Test]
        public void CanReplaceItems_OneItemDifferentInArray_ReturnsFalse()
        {
            var stackSize = this.GetRandomStackSize();
            var startItems = this.itemFactory.CreateMany(stackSize);
            var slot = this.slotFactory.Full(startItems);

            var items = this.itemFactory.CreateMany(stackSize / 2)
                .Append(this.itemFactory.CreateRandom())
                .ToArray();
            var result = slot.CanReplace(items);

            Assert.That(result, Is.False);
        }

        [Test]
        public void CanReplaceItems_ArrayBiggerThanMaxAmount_ReturnsFalse()
        {
            var stackSize = this.GetRandomStackSize();
            var startItems = this.itemFactory.CreateMany(stackSize);
            var slot = this.slotFactory.Full(startItems);

            var items = this.itemFactory.CreateMany(stackSize + 1);
            var result = slot.CanReplace(items);

            Assert.That(result, Is.False);
        }

        [Test]
        public void CanReplaceItems_ArraySmallerThanMaxAmount_ReturnsTrue()
        {
            var stackSize = this.GetRandomStackSize();
            var startItems = this.itemFactory.CreateMany(stackSize);
            var slot = this.slotFactory.Full(startItems);

            var items = this.itemFactory.CreateMany(stackSize - 1);
            var result = slot.CanReplace(items);

            Assert.That(result, Is.True);
        }

        [Test]
        public void CanReplaceItems_SameItemTypeThanSlot_ReturnsTrue()
        {
            var stackSize = this.GetRandomStackSize();
            var startItems = this.itemFactory.CreateMany(stackSize);
            var slot = this.slotFactory.Full(startItems);

            var items = this.itemFactory.CreateMany(stackSize);
            var result = slot.CanReplace(items); 
            
            Assert.That(result, Is.True);
        }

        [Test]
        public void CanReplaceItems_DifferentItemThanSlot_ReturnsTrue()
        {
            var stackSize = this.GetRandomStackSize();
            var startItems = this.itemFactory.CreateMany(stackSize);
            var slot = this.slotFactory.Full(startItems);

            var items = this.itemFactory.CreateManyRandom(stackSize);
            var result = slot.CanReplace(items);

            Assert.That(result, Is.True);
        }
    }
}
