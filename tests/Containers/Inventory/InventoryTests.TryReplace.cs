using TheChest.Tests.Common.Extensions.Containers;

namespace TheChest.Inventories.Tests.Containers.Inventory
{
    public partial class InventoryTests<T>
    {
        [Test]
        public void TryReplace_NullItem_ThrowsArgumentNullException()
        {
            var size = this.GenerateRandomSize();
            var inventory = this.inventoryFactory.EmptyContainer(size);

            Assert.That(
                () => inventory.TryReplace(default!, 0, out _),
                Throws.ArgumentNullException.With.Property("ParamName").EqualTo("item")
            );
        }

        [TestCase(-1)]
        [TestCase(MAX_SIZE_TEST)]
        public void TryReplace_InvalidSlotIndex_ThrowsArgumentOutOfRangeException(int index)
        {
            var size = this.GenerateRandomSize();
            var inventory = this.inventoryFactory.EmptyContainer(size);

            Assert.That(
                () => inventory.TryReplace(this.itemFactory.CreateDefault(), index, out _),
                Throws.Exception.TypeOf<ArgumentOutOfRangeException>().With.Property("ParamName").EqualTo("index")
            );
        }

        [Test]
        public void TryReplace_IndexEqualToSize_ThrowsArgumentOutOfRangeException()
        {
            var size = this.GenerateRandomSize();
            var inventory = this.inventoryFactory.EmptyContainer(size);

            Assert.That(
                () => inventory.TryReplace(this.itemFactory.CreateDefault(), size, out _),
                Throws.Exception.TypeOf<ArgumentOutOfRangeException>().With.Property("ParamName").EqualTo("index")
            );
        }

        [Test]
        public void TryReplace_EmptySlot_AddsItemToSlot()
        {
            var size = this.GenerateRandomSize();
            var inventory = this.inventoryFactory.EmptyContainer(size);
            var randomIndex = this.random.Next(0, size);
            var item = this.itemFactory.CreateDefault();

            inventory.TryReplace(item, randomIndex, out _);

            Assert.That(inventory.GetItem<T>(randomIndex), Is.EqualTo(item));
        }

        [Test]
        public void TryReplace_EmptySlot_CallsOnReplaceEventWithEmptyOldItem()
        {
            var size = this.GenerateRandomSize();
            var inventory = this.inventoryFactory.EmptyContainer(size);
            var randomIndex = this.random.Next(0, size);
            var item = this.itemFactory.CreateDefault();
            var calledWithExpectedData = false;

            inventory.OnReplace += (sender, args) =>
            {
                var data = args.Data.Single();
                calledWithExpectedData =
                    sender == inventory &&
                    data.Index == randomIndex &&
                    data.OldItem is null &&
                    data.NewItem!.Equals(item);
            };

            inventory.TryReplace(item, randomIndex, out _);

            Assert.That(calledWithExpectedData, Is.True);
        }

        [Test]
        public void TryReplace_EmptySlot_SetsOldItemToNull()
        {
            var size = this.GenerateRandomSize();
            var inventory = this.inventoryFactory.EmptyContainer(size);
            var randomIndex = this.random.Next(0, size);
            var item = this.itemFactory.CreateDefault();

            inventory.TryReplace(item, randomIndex, out var oldItem);

            Assert.That(oldItem, Is.Null);
        }

        [Test]
        public void TryReplace_EmptySlot_ReturnsTrue()
        {
            var size = this.GenerateRandomSize();
            var inventory = this.inventoryFactory.EmptyContainer(size);
            var randomIndex = this.random.Next(0, size);
            var item = this.itemFactory.CreateDefault();

            var result = inventory.TryReplace(item, randomIndex, out _);

            Assert.That(result, Is.True);
        }

        [Test]
        public void TryReplace_FullSlot_ReplacesItemInSlot()
        {
            var size = this.GenerateRandomSize();
            var initialItem = this.itemFactory.CreateDefault();
            var inventory = this.inventoryFactory.FullContainer(size, initialItem);
            var randomIndex = this.random.Next(0, size);
            var newItem = this.itemFactory.CreateRandom();

            inventory.TryReplace(newItem, randomIndex, out _);

            Assert.That(inventory.GetItem(randomIndex), Is.EqualTo(newItem));
        }

        [Test]
        public void TryReplace_FullSlot_CallsOnReplaceEvent()
        {
            var size = this.GenerateRandomSize();
            var initialItem = this.itemFactory.CreateDefault();
            var inventory = this.inventoryFactory.FullContainer(size, initialItem);
            var randomIndex = this.random.Next(0, size);
            var newItem = this.itemFactory.CreateRandom();
            var calledWithExpectedData = false;

            inventory.OnReplace += (sender, args) =>
            {
                var data = args.Data.Single();
                calledWithExpectedData =
                    sender == inventory &&
                    data.Index == randomIndex &&
                    data.OldItem!.Equals(initialItem) &&
                    data.NewItem!.Equals(newItem);
            };

            inventory.TryReplace(newItem, randomIndex, out _);

            Assert.That(calledWithExpectedData, Is.True);
        }

        [Test]
        public void TryReplace_FullSlot_SetsOldItemToPreviousItem()
        {
            var size = this.GenerateRandomSize();
            var initialItem = this.itemFactory.CreateDefault();
            var inventory = this.inventoryFactory.FullContainer(size, initialItem);
            var randomIndex = this.random.Next(0, size);
            var newItem = this.itemFactory.CreateRandom();

            inventory.TryReplace(newItem, randomIndex, out var oldItem);

            Assert.That(oldItem, Is.EqualTo(initialItem));
        }

        [Test]
        public void TryReplace_FullSlot_ReturnsTrue()
        {
            var size = this.GenerateRandomSize();
            var initialItem = this.itemFactory.CreateDefault();
            var inventory = this.inventoryFactory.FullContainer(size, initialItem);
            var randomIndex = this.random.Next(0, size);
            var newItem = this.itemFactory.CreateRandom();

            var result = inventory.TryReplace(newItem, randomIndex, out _);

            Assert.That(result, Is.True);
        }
    }
}
