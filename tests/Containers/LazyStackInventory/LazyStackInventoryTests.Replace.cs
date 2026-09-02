using TheChest.Tests.Common.Attributes;
using TheChest.Tests.Common.Extensions.Containers;

namespace TheChest.Inventories.Tests.Containers.LazyStackInventory
{
    public partial class LazyStackInventoryTests<T>
    {
        [Test]
        [IgnoreIfValueType]
        public void Replace_NullItem_ThrowsArgumentNullException()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var inventory = this.inventoryFactory.FullContainer(size, stackSize, this.itemFactory.CreateDefault());

            Assert.That(
                () => inventory.Replace(default!, 0, 1),
                Throws.ArgumentNullException.With.Property("ParamName").EqualTo("item")
            );
        }

        [TestCase(0)]
        [TestCase(-1)]
        public void Replace_InvalidAmount_ThrowsArgumentOutOfRangeException(int amount)
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var item = this.itemFactory.CreateDefault();
            var inventory = this.inventoryFactory.FullContainer(size, stackSize, item);

            Assert.That(
                () => inventory.Replace(item, 0, amount),
                Throws.TypeOf<ArgumentOutOfRangeException>().With.Property("ParamName").EqualTo("amount")
            );
        }

        [Test]
        public void Replace_NegativeIndex_ThrowsArgumentOutOfRangeException()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var item = this.itemFactory.CreateDefault();
            var inventory = this.inventoryFactory.FullContainer(size, stackSize, item);

            Assert.That(
                () => inventory.Replace(item, -1, 1),
                Throws.TypeOf<ArgumentOutOfRangeException>().With.Property("ParamName").EqualTo("index")
            );
        }

        [Test]
        public void Replace_IndexEqualToSize_ThrowsArgumentOutOfRangeException()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var item = this.itemFactory.CreateDefault();
            var inventory = this.inventoryFactory.FullContainer(size, stackSize, item);

            Assert.That(
                () => inventory.Replace(item, size, 1),
                Throws.TypeOf<ArgumentOutOfRangeException>().With.Property("ParamName").EqualTo("index")
            );
        }

        [Test]
        public void Replace_AmountExceedingStackSize_ThrowsArgumentOutOfRangeException()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var oldItem = this.itemFactory.CreateDefault();
            var inventory = this.inventoryFactory.FullContainer(size, stackSize, oldItem);
            var newItem = this.itemFactory.CreateRandom();
            var index = this.random.Next(0, size);

            Assert.That(
                () => inventory.Replace(newItem, index, stackSize + 1),
                Throws.TypeOf<ArgumentOutOfRangeException>().With.Property("ParamName").EqualTo("amount")
            );
        }

        [Test]
        public void Replace_EmptySlot_ThrowsInvalidOperationException()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var inventory = this.inventoryFactory.EmptyContainer(size, stackSize);
            var newItem = this.itemFactory.CreateRandom();
            var index = this.random.Next(0, size);

            Assert.That(
                () => inventory.Replace(newItem, index, 1),
                Throws.InvalidOperationException.With.Message.EqualTo("The slot is empty.")
            );
        }

        [Test]
        public void Replace_EmptySlot_DoesNotAddItems()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var inventory = this.inventoryFactory.EmptyContainer(size, stackSize);
            var newItem = this.itemFactory.CreateRandom();
            var index = this.random.Next(0, size);

            Assert.That(
                () => inventory.Replace(newItem, index, 1),
                Throws.InvalidOperationException.With.Message.EqualTo("The slot is empty.")
            );

            Assert.That(inventory.GetItems(index), Is.Empty);
        }

        [Test]
        public void Replace_EmptySlot_DoesNotCallOnReplaceEvent()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var inventory = this.inventoryFactory.EmptyContainer(size, stackSize);
            var newItem = this.itemFactory.CreateRandom();
            var index = this.random.Next(0, size);
            var called = false;
            inventory.OnReplace += (_, _) => called = true;

            Assert.That(
                () => inventory.Replace(newItem, index, 1),
                Throws.InvalidOperationException.With.Message.EqualTo("The slot is empty.")
            );

            Assert.That(called, Is.False);
        }

        [Test]
        public void Replace_SlotWithItems_ReplacesItemsInSlot()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var oldItem = this.itemFactory.CreateDefault();
            var inventory = this.inventoryFactory.FullContainer(size, stackSize, oldItem);
            var newItem = this.itemFactory.CreateRandom();
            var index = this.random.Next(0, size);
            var amount = this.random.Next(1, stackSize + 1);

            inventory.Replace(newItem, index, amount);

            Assert.That(inventory.GetItems(index), Has.Length.EqualTo(amount).And.All.EqualTo(newItem));
        }

        [Test]
        public void Replace_SlotWithItems_CallsOnReplaceEvent()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var oldItem = this.itemFactory.CreateDefault();
            var inventory = this.inventoryFactory.FullContainer(size, stackSize, oldItem);
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
                    data.OldItem!.Equals(oldItem) &&
                    data.OldAmount == stackSize &&
                    data.NewItem!.Equals(newItem) &&
                    data.NewAmount == amount;
            };

            inventory.Replace(newItem, index, amount);

            Assert.That(calledWithExpectedData, Is.True);
        }

        [Test]
        public void Replace_SlotWithItems_ReturnsPreviousItems()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var oldItem = this.itemFactory.CreateDefault();
            var inventory = this.inventoryFactory.FullContainer(size, stackSize, oldItem);
            var newItem = this.itemFactory.CreateRandom();
            var index = this.random.Next(0, size);
            var amount = this.random.Next(1, stackSize + 1);

            var oldItems = inventory.Replace(newItem, index, amount);

            Assert.That(oldItems, Has.Length.EqualTo(stackSize).And.All.EqualTo(oldItem));
        }
    }
}
