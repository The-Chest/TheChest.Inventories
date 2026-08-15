using TheChest.Tests.Common.Attributes;
using TheChest.Tests.Common.Extensions;
using TheChest.Tests.Common.Extensions.Slots;

namespace TheChest.Inventories.Tests.Slots.InventoryStackSlot
{
    public partial class InventoryStackSlotTests<T>
    {
        #region Param Validation Tests
        [Test]
        public void AddItems_EmptyItems_ThrowsArgumentException()
        {
            var stackSize = this.GetRandomStackSize();
            var slot = this.slotFactory.Empty(stackSize);

            Assert.That(
                () => slot.Add(Array.Empty<T>()),
                Throws.ArgumentException
                    .With.Message.StartsWith("Cannot add empty list of items")
                    .And.Property("ParamName").EqualTo("items")
            );
        }

        [Test]
        [IgnoreIfValueType]
        public void AddItems_ItemsContainingNull_ThrowsArgumentNullException()
        {
            var stackSize = this.GetRandomStackSize();
            var slot = this.slotFactory.Empty(stackSize);

            var items = this.itemFactory.CreateMany(stackSize - 1)
                .Append(default!)
                .ToShuffledArray(this.random);

            Assert.That(
                () => slot.Add(items),
                Throws.ArgumentNullException
                    .With.Message.StartsWith("Cannot add an array of items with null values")
                    .And.Property("ParamName").EqualTo("items")
            );
        }

        [Test]
        public void AddItems_ItemsContainingDifferentValues_ThrowsArgumentException()
        {
            var stackSize = this.GetRandomStackSize();
            var slot = this.slotFactory.Empty(stackSize);

            var addingItems = this.itemFactory.CreateMany(stackSize - 1)
                .Append(this.itemFactory.CreateRandom())
                .ToShuffledArray(this.random);

            Assert.That(
                () => slot.Add(addingItems),
                Throws.ArgumentException
                    .With.Message.StartsWith("Cannot add an array of items with different types")
                    .And.Property("ParamName").EqualTo("items")
            );
        }
        #endregion

        #region State Validation Tests
        [Test]
        public void AddItems_FullSlot_ThrowsInvalidOperationException()
        {
            var stackSize = this.GetRandomStackSize();
            var items = this.itemFactory.CreateMany(stackSize);
            var slot = this.slotFactory.Full(items);

            var addingAmount = this.random.Next(1, stackSize);
            var addingItems = this.itemFactory.CreateMany(addingAmount);

            Assert.That(
                () => slot.Add(addingItems),
                Throws.InvalidOperationException.With.Message.EqualTo("The slot is full")
            );
        }

        [Test]
        public void AddItems_EmptySlot_ItemsExceedingMaxAmount_ThrowsInvalidOperationException()
        {
            var stackSize = this.GetRandomStackSize();
            var slot = this.slotFactory.Empty(stackSize);

            var addingAmount = this.random.Next(slot.MaxAmount + 1, stackSize * 2);
            var addingItems = this.itemFactory.CreateManyRandom(addingAmount);

            Assert.That(
                () => slot.Add(addingItems),
                Throws.InvalidOperationException.With.Message.EqualTo("Cannot add more items than the available amount")
            );
        }

        [Test]
        public void AddItems_SlotWithLimitedSpace_ItemsExceedingAvailableAmount_ThrowsInvalidOperationException()
        {
            var stackSize = this.GetRandomStackSize();
            var halfStackSize = stackSize / 2;
            var items = this.itemFactory.CreateMany(halfStackSize);
            var slot = this.slotFactory.WithItems(items, stackSize);

            var addingAmount = this.random.Next(slot.AvailableAmount + 1, stackSize * 2);
            var addingItems = this.itemFactory.CreateMany(addingAmount);

            Assert.That(
                () => slot.Add(addingItems),
                Throws.InvalidOperationException.With.Message.EqualTo("Cannot add more items than the available amount")
            );
        }

        [Test]
        public void AddItems_SlotWithDifferentItems_ThrowsInvalidOperationException()
        {
            var stackSize = this.GetRandomStackSize();
            var halfStackSize = stackSize / 2;
            var items = this.itemFactory.CreateMany(halfStackSize);
            var slot = this.slotFactory.WithItems(items, stackSize);

            var addingAmount = this.random.Next(1, halfStackSize);
            var addingItems = this.itemFactory.CreateManyRandom(addingAmount);

            Assert.That(
                () => slot.Add(addingItems),
                Throws.InvalidOperationException.With.Message.EqualTo("Cannot add items that are different from the items already in the slot")
            );
        }
        #endregion

        #region Behavior Tests
        [Test]
        public void AddItems_EmptySlot_IncreasesAmount()
        {
            var stackSize = this.GetRandomStackSize();
            var slot = this.slotFactory.Empty(stackSize);

            var addingAmount = this.random.Next(1, stackSize - 1);
            var addingItems = this.itemFactory.CreateManyRandom(addingAmount);
            slot.Add(addingItems);

            Assert.That(slot.Amount, Is.EqualTo(addingAmount));
        }

        [Test]
        public void AddItems_EmptySlot_AddsItems()
        {
            var stackSize = this.GetRandomStackSize();
            var slot = this.slotFactory.Empty(stackSize);

            var addingAmount = this.random.Next(1, stackSize);
            var addingItems = this.itemFactory.CreateManyRandom(addingAmount);

            slot.Add(addingItems);

            Assert.That(slot.GetContents()[0..addingAmount], Is.EquivalentTo(addingItems));
        }

        [Test]
        public void AddItems_SlotWithSameItems_IncreasesAmount()
        {
            var stackSize = this.GetRandomStackSize();
            var halfStackSize = stackSize / 2;
            var items = this.itemFactory.CreateMany(halfStackSize);
            var slot = this.slotFactory.WithItems(items, stackSize);

            var addingAmount = this.random.Next(1, halfStackSize);
            var addingItems = this.itemFactory.CreateMany(addingAmount);

            slot.Add(addingItems);

            Assert.That(slot.Amount, Is.EqualTo(halfStackSize + addingAmount));
        }

        [Test]
        public void AddItems_SlotWithSameItems_AddsItems()
        {
            var stackSize = this.GetRandomStackSize();
            var halfStackSize = stackSize / 2;
            var items = this.itemFactory.CreateMany(halfStackSize);
            var slot = this.slotFactory.WithItems(items, stackSize);

            var addingAmount = this.random.Next(1, halfStackSize);
            var addingItems = this.itemFactory.CreateMany(addingAmount);

            slot.Add(addingItems);

            Assert.That(slot.GetContents()[0..addingAmount], Is.EquivalentTo(addingItems));
        }
        #endregion
    }
}
