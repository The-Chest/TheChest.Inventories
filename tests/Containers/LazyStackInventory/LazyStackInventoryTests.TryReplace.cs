using TheChest.Tests.Common.Extensions.Containers;

namespace TheChest.Inventories.Tests.Containers.LazyStackInventory
{
    public partial class LazyStackInventoryTests<T>
    {
        [Test]
        public void TryReplace_NullItem_ThrowsArgumentNullException()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var inventory = this.inventoryFactory.EmptyContainer(size, stackSize);

            Assert.That(
                () => inventory.TryReplace(default!, 0, 1, out _),
                Throws.ArgumentNullException.With.Property("ParamName").EqualTo("item")
            );
        }

        [TestCase(0)]
        [TestCase(-1)]
        public void TryReplace_InvalidAmount_ThrowsArgumentOutOfRangeException(int amount)
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var item = this.itemFactory.CreateDefault();
            var inventory = this.inventoryFactory.EmptyContainer(size, stackSize);
            var index = this.random.Next(0, size);

            Assert.That(
                () => inventory.TryReplace(item, index, amount, out _),
                Throws.TypeOf<ArgumentOutOfRangeException>().With.Property("ParamName").EqualTo("amount")
            );
        }

        [TestCase(-1)]
        [TestCase(MAX_SIZE_TEST)]
        public void TryReplace_InvalidIndex_ThrowsArgumentOutOfRangeException(int index)
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var item = this.itemFactory.CreateDefault();
            var inventory = this.inventoryFactory.EmptyContainer(size, stackSize);

            Assert.That(
                () => inventory.TryReplace(item, index, 1, out _),
                Throws.TypeOf<ArgumentOutOfRangeException>().With.Property("ParamName").EqualTo("index")
            );
        }

        [Test]
        public void TryReplace_IndexEqualToSize_ThrowsArgumentOutOfRangeException()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var item = this.itemFactory.CreateDefault();
            var inventory = this.inventoryFactory.EmptyContainer(size, stackSize);

            Assert.That(
                () => inventory.TryReplace(item, size, 1, out _),
                Throws.TypeOf<ArgumentOutOfRangeException>().With.Property("ParamName").EqualTo("index")
            );
        }

        [Test]
        public void TryReplace_MoreItemsThanStackSize_ThrowsArgumentOutOfRangeException()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var item = this.itemFactory.CreateDefault();
            var inventory = this.inventoryFactory.EmptyContainer(size, stackSize);
            var index = this.random.Next(0, size);

            Assert.That(
                () => inventory.TryReplace(item, index, stackSize + 1, out _),
                Throws.TypeOf<ArgumentOutOfRangeException>().With.Property("ParamName").EqualTo("amount")
            );
        }

        [Test]
        public void TryReplace_SlotWithItems_ReplacesItemsInSlot()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var initialItem = this.itemFactory.CreateDefault();
            var inventory = this.inventoryFactory.FullContainer(size, stackSize, initialItem);
            var newItem = this.itemFactory.CreateRandom();
            var index = this.random.Next(0, size);
            var amount = this.random.Next(1, stackSize + 1);

            inventory.TryReplace(newItem, index, amount, out _);

            Assert.That(inventory.GetItems(index), Has.Length.EqualTo(amount).And.All.EqualTo(newItem));
        }

        [Test]
        public void TryReplace_SlotWithItems_SetsOldItemsToPreviousItems()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var initialItem = this.itemFactory.CreateDefault();
            var inventory = this.inventoryFactory.FullContainer(size, stackSize, initialItem);
            var newItem = this.itemFactory.CreateRandom();
            var index = this.random.Next(0, size);
            var amount = this.random.Next(1, stackSize + 1);

            inventory.TryReplace(newItem, index, amount, out var oldItems);

            Assert.That(oldItems, Has.Length.EqualTo(stackSize).And.All.EqualTo(initialItem));
        }

        [Test]
        public void TryReplace_SlotWithItems_CallsOnReplaceEvent()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var initialItem = this.itemFactory.CreateDefault();
            var inventory = this.inventoryFactory.FullContainer(size, stackSize, initialItem);
            var newItem = this.itemFactory.CreateRandom();
            var index = this.random.Next(0, size);
            var amount = this.random.Next(1, stackSize + 1);
            var calledWithExpectedData = false;

            inventory.OnReplace += (sender, args) =>
            {
                var data = args.Data.Single();
                calledWithExpectedData =
                    sender == inventory &&
                    data.Index == index &&
                    data.OldItem!.Equals(initialItem) &&
                    data.OldAmount == stackSize &&
                    data.NewItem!.Equals(newItem) &&
                    data.NewAmount == amount;
            };
            inventory.TryReplace(newItem, index, amount, out _);

            Assert.That(calledWithExpectedData, Is.True);
        }

        [Test]
        public void TryReplace_SlotWithItems_ReturnsTrue()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var initialItem = this.itemFactory.CreateDefault();
            var inventory = this.inventoryFactory.FullContainer(size, stackSize, initialItem);
            var newItem = this.itemFactory.CreateRandom();
            var index = this.random.Next(0, size);
            var amount = this.random.Next(1, stackSize + 1);

            var result = inventory.TryReplace(newItem, index, amount, out _);

            Assert.That(result, Is.True);
        }

        [Test]
        public void TryReplace_EmptySlot_ReplacesItemsInSlot()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var inventory = this.inventoryFactory.EmptyContainer(size, stackSize);
            var newItem = this.itemFactory.CreateDefault();
            var index = this.random.Next(0, size);
            var amount = this.random.Next(1, stackSize + 1);

            inventory.TryReplace(newItem, index, amount, out _);

            Assert.That(inventory.GetItems(index), Has.Length.EqualTo(amount).And.All.EqualTo(newItem));
        }

        [Test]
        public void TryReplace_EmptySlot_SetsOldItemsToEmptyArray()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var inventory = this.inventoryFactory.EmptyContainer(size, stackSize);
            var newItem = this.itemFactory.CreateDefault();
            var index = this.random.Next(0, size);
            var amount = this.random.Next(1, stackSize + 1);

            inventory.TryReplace(newItem, index, amount, out var oldItems);

            Assert.That(oldItems, Is.Empty);
        }

        [Test]
        public void TryReplace_EmptySlot_CallsOnReplaceEvent()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var inventory = this.inventoryFactory.EmptyContainer(size, stackSize);
            var newItem = this.itemFactory.CreateDefault();
            var index = this.random.Next(0, size);
            var amount = this.random.Next(1, stackSize + 1);
            var calledWithExpectedData = false;

            inventory.OnReplace += (sender, args) =>
            {
                var data = args.Data.Single();
                calledWithExpectedData =
                    sender == inventory &&
                    data.Index == index &&
                    data.OldItem == null &&
                    data.OldAmount == 0 &&
                    data.NewItem!.Equals(newItem) &&
                    data.NewAmount == amount;
            };
            inventory.TryReplace(newItem, index, amount, out _);

            Assert.That(calledWithExpectedData, Is.True);
        }

        [Test]
        public void TryReplace_EmptySlot_ReturnsTrue()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var inventory = this.inventoryFactory.EmptyContainer(size, stackSize);
            var newItem = this.itemFactory.CreateDefault();
            var index = this.random.Next(0, size);
            var amount = this.random.Next(1, stackSize + 1);

            var result = inventory.TryReplace(newItem, index, amount, out _);

            Assert.That(result, Is.True);
        }
    }
}
