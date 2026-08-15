using TheChest.Tests.Common.Attributes;
using TheChest.Tests.Common.Extensions.Slots;

namespace TheChest.Inventories.Tests.Slots.InventoryStackSlot
{
    public partial class InventoryStackSlotTests<T>
    {
        [Test]
        public void TryAdd_NullItems_ThrowsArgumentNullException()
        {
            var stackSize = this.GetRandomStackSize();
            var slot = this.slotFactory.Empty(stackSize);

            Assert.That(
                () => slot.TryAdd(null!), 
                Throws.ArgumentNullException.With.Property("ParamName").EqualTo("items")
            );
        }

        [Test]
        [IgnoreIfValueType]
        public void TryAdd_ItemsContainingNull_ThrowsArgumentNullException()
        {
            var stackSize = this.GetRandomStackSize();
            var slot = this.slotFactory.Empty(stackSize);
            var items = new T[] { default! };

            Assert.That(
                () => slot.TryAdd(items),
                Throws.ArgumentNullException.With.Property("ParamName").EqualTo("items")
            );
        }

        [Test]
        public void TryAdd_FullSlot_ReturnsFalse()
        {
            var stackSize = this.GetRandomStackSize();
            var slotItems = this.itemFactory.CreateMany(stackSize);
            var slot = this.slotFactory.Full(slotItems);

            var addingAmount = this.random.Next(1, stackSize);
            var addingItems = this.itemFactory.CreateMany(addingAmount);
            var result = slot.TryAdd(addingItems);

            Assert.That(result, Is.False);
        }

        [Test]
        public void TryAdd_FullSlot_DoesntAddItems()
        {
            var stackSize = this.GetRandomStackSize();
            var slotItems = this.itemFactory.CreateMany(stackSize);
            var slot = this.slotFactory.Full(slotItems);

            var addingAmount = this.random.Next(1, stackSize);
            var addingItems = this.itemFactory.CreateMany(addingAmount);
            slot.TryAdd(addingItems);

            Assert.That(slot.GetContents(), Is.EquivalentTo(slotItems));
        }

        [Test]
        public void TryAdd_SlotWithLimitedSpace_ReturnsFalse()
        {
            var stackSize = this.GetRandomStackSize();
            var currentAmount = this.random.Next(1, stackSize - 1);
            var slotItems = this.itemFactory.CreateMany(currentAmount);
            var slot = this.slotFactory.WithItems(slotItems, stackSize);

            var addingAmount = stackSize - currentAmount + 1;
            var addingItems = this.itemFactory.CreateMany(addingAmount);
            var result = slot.TryAdd(addingItems);

            Assert.That(result, Is.False);
        }

        [Test]
        public void TryAdd_SlotWithLimitedSpace_DoesntAddItems()
        {
            var stackSize = this.GetRandomStackSize();
            var currentAmount = this.random.Next(1, stackSize - 1);
            var slotItems = this.itemFactory.CreateMany(currentAmount);
            var slot = this.slotFactory.WithItems(slotItems, stackSize);

            var addingAmount = stackSize - currentAmount + 1;
            var addingItems = this.itemFactory.CreateMany(addingAmount);
            slot.TryAdd(addingItems);

            Assert.That(slot.GetContents(), Is.EquivalentTo(slotItems));
        }

        [Test]
        [IgnoreIfReferenceType]
        public void TryAdd_EmptySlot_ItemsContainingDefault_AddsItems()
        {
            var stackSize = this.GetRandomStackSize();
            var slot = this.slotFactory.Empty(stackSize);

            var items = new T[] { default! };
            slot.TryAdd(items);

            Assert.That(slot.GetContents(), Is.EquivalentTo(items));
        }

        [Test]
        [IgnoreIfReferenceType]
        public void TryAdd_EmptySlot_ItemsContainingDefault_IncreasesAmount()
        {
            var stackSize = this.GetRandomStackSize();
            var slot = this.slotFactory.Empty(stackSize);

            var items = new T[] { default! };
            slot.TryAdd(items);

            Assert.That(slot.Amount, Is.EqualTo(1));
        }

        [Test]
        [IgnoreIfReferenceType]
        public void TryAdd_EmptySlot_ItemsContainingDefault_ReturnsTrue()
        {
            var stackSize = this.GetRandomStackSize();
            var slot = this.slotFactory.Empty(stackSize);

            var items = new T[] { default! };
            var result = slot.TryAdd(items);

            Assert.That(result, Is.True);
        }

        [Test]
        public void TryAdd_SlotWithDifferentItems_DoesntAddItems()
        {
            var stackSize = this.GetRandomStackSize();
            var currentAmount = this.random.Next(1, stackSize - 1);
            var slotItems = this.itemFactory.CreateMany(currentAmount);
            var slot = this.slotFactory.WithItems(slotItems, stackSize);

            var addingAmount = this.random.Next(1, stackSize - currentAmount);
            var addingItems = this.itemFactory.CreateManyRandom(addingAmount);
            slot.TryAdd(addingItems);

            Assert.That(slot.GetContents(), Is.EquivalentTo(slotItems));
        }

        [Test]
        public void TryAdd_SlotWithDifferentItems_ReturnsFalse()
        {
            var stackSize = this.GetRandomStackSize();
            var currentAmount = this.random.Next(1, stackSize - 1);
            var slotItems = this.itemFactory.CreateMany(currentAmount);
            var slot = this.slotFactory.WithItems(slotItems, stackSize);

            var addingAmount = this.random.Next(1, stackSize - currentAmount);
            var addingItems = this.itemFactory.CreateManyRandom(addingAmount);
            var result = slot.TryAdd(addingItems);

            Assert.That(result, Is.False);
        }

        [Test]
        public void TryAdd_ItemsContainingDifferentValues_ReturnsFalse()
        {
            var stackSize = this.GetRandomStackSize();
            var slot = this.slotFactory.Empty(stackSize);

            var addingItems = this.itemFactory.CreateMany(stackSize - 1)
                .Append(this.itemFactory.CreateRandom())
                .ToArray();
            var result = slot.TryAdd(addingItems);

            Assert.That(result, Is.False);
        }

        [Test]
        public void TryAdd_ItemsContainingDifferentValues_DoesntAddItems()
        {
            var stackSize = this.GetRandomStackSize();
            var slot = this.slotFactory.Empty(stackSize);

            var addingItems = this.itemFactory.CreateMany(stackSize - 1)
                .Append(this.itemFactory.CreateRandom())
                .ToArray();
            slot.TryAdd(addingItems);

            Assert.That(slot.GetContents(), Is.Empty);
        }

        [Test]
        public void TryAdd_SlotWithAvailableSpace_ValidItems_ReturnsTrue()
        {
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var currentAmount = this.random.Next(1, stackSize - 1);
            var slotItems = this.itemFactory.CreateMany(currentAmount);
            var slot = this.slotFactory.WithItems(slotItems, stackSize);

            var addingAmount = this.random.Next(1, stackSize - currentAmount);
            var addingItems = this.itemFactory.CreateMany(addingAmount);
            var result = slot.TryAdd(addingItems);

            Assert.That(result, Is.True);
        }
    }
}
