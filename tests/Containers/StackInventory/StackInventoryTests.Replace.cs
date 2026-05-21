using TheChest.Tests.Common.Extensions.Containers;

namespace TheChest.Inventories.Tests.Containers.StackInventory
{
    public partial class StackInventoryTests<T>
    {
        [TestCase(-1)]
        [TestCase(MAX_SIZE_TEST)]
        public void Replace_InvalidIndex_ThrowsArgumentOutOfRangeException(int index)
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var inventory = this.inventoryFactory.EmptyContainer(size, stackSize);

            var items = this.itemFactory.CreateMany(stackSize);
            Assert.That(
                () => inventory.Replace(items, index), 
                Throws.TypeOf<ArgumentOutOfRangeException>().With.Property("ParamName").EqualTo("index")
            );
        }

        [Test]
        public void Replace_EmptyItems_ThrowsArgumentException()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var item = this.itemFactory.CreateDefault();
            var inventory = this.inventoryFactory.FullContainer(size, stackSize, item);

            var randomIndex = this.random.Next(0, size);
            Assert.That(
                () => inventory.Replace(Array.Empty<T>(), randomIndex),
                Throws.TypeOf<ArgumentException>()
                    .With.Property("ParamName").EqualTo("items").And
                    .Message.StartsWith("Cannot replace using an empty item array")
            );
        }

        [Test]
        public void Replace_MoreItemsThanStackSize_ThrowsArgumentOutOfRangeException()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var item = this.itemFactory.CreateDefault();
            var inventory = this.inventoryFactory.FullContainer(size, stackSize, item);
            var randomIndex = this.random.Next(0, size);
            var items = this.itemFactory.CreateMany(stackSize + 1);

            Assert.That(() =>
                inventory.Replace(items, randomIndex),
                Throws.TypeOf<ArgumentOutOfRangeException>()
                    .With.Property("ParamName").EqualTo("items").And
                    .Message.StartsWith("The max stack size is smaller than the number of items to replace.")
            );
        }

        [Test]
        public void Replace_NullItems_ThrowsArgumentNullException()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var item = this.itemFactory.CreateDefault();
            var inventory = this.inventoryFactory.FullContainer(size, stackSize, item);

            var randomIndex = this.random.Next(0, size);
            Assert.That(
                () => inventory.Replace(null!, randomIndex), 
                Throws.TypeOf<ArgumentNullException>().With.Property("ParamName").EqualTo("items")
            );
        }

        [Test]
        public void Replace_SlotWithItems_ReplacesItemsInSlot()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var item = this.itemFactory.CreateDefault();
            var inventory = this.inventoryFactory.FullContainer(size, stackSize, item);

            var randomIndex = this.random.Next(0, size);
            var newItems = this.itemFactory.CreateManyRandom(stackSize);
            inventory.Replace(newItems, randomIndex);

            Assert.That(inventory.GetItems(randomIndex), Is.EqualTo(newItems));
        }

        [Test]
        public void Replace_SlotWithItems_ReturnsOldItems()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var item = this.itemFactory.CreateDefault();
            var inventory = this.inventoryFactory.FullContainer(size, stackSize, item);

            var randomIndex = this.random.Next(0, size);
            var newStackSize = this.random.Next(1, stackSize - 1);
            var newItems = this.itemFactory.CreateManyRandom(newStackSize);

            var result = inventory.Replace(newItems, randomIndex);

            Assert.That(result, Has.Length.EqualTo(stackSize));
            Assert.That(result, Has.All.EqualTo(item));
        }

        [Test]
        public void Replace_SlotWithItems_CallsOnReplaceEvent()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var item = this.itemFactory.CreateDefault();
            var inventory = this.inventoryFactory.FullContainer(size, stackSize, item);

            var randomIndex = this.random.Next(0, size);
            var newItems = this.itemFactory.CreateManyRandom(stackSize);
            inventory.OnReplace += (sender, args) =>
            {
                Assert.That(sender, Is.EqualTo(inventory));
                var data = args.Data.Single();
                Assert.Multiple(() =>
                {
                    Assert.That(data.Index, Is.EqualTo(randomIndex));
                    Assert.That(data.OldItems, Has.Length.EqualTo(stackSize).And.All.EqualTo(item));
                    Assert.That(data.NewItems, Is.EqualTo(newItems));
                });
            };
            inventory.Replace(newItems, randomIndex);
        }

        [Test]
        public void Replace_EmptySlot_ReplacesItemsInSlot()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var inventory = this.inventoryFactory.EmptyContainer(size, stackSize);

            var randomIndex = this.random.Next(0, size);
            var newItems = this.itemFactory.CreateManyRandom(stackSize);
            inventory.Replace(newItems, randomIndex);

            Assert.That(inventory.GetItems(randomIndex), Is.EqualTo(newItems));
        }

        [Test]
        public void Replace_EmptySlot_ReturnsEmptyArray()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var inventory = this.inventoryFactory.EmptyContainer(size, stackSize);

            var randomIndex = this.random.Next(0, size);
            var newItems = this.itemFactory.CreateManyRandom(this.random.Next(1, stackSize));
            var result = inventory.Replace(newItems, randomIndex);

            Assert.That(result, Is.Empty);
        }

        [Test]
        public void Replace_EmptySlot_CallsOnReplaceEvent()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var inventory = this.inventoryFactory.FullContainer(size, stackSize);

            var randomIndex = this.random.Next(0, size);
            var newItems = this.itemFactory.CreateManyRandom(stackSize);
            inventory.OnReplace += (sender, args) =>
            {
                Assert.That(sender, Is.EqualTo(inventory));
                var data = args.Data.Single();
                Assert.Multiple(() =>
                {
                    Assert.That(data.Index, Is.EqualTo(randomIndex));
                    Assert.That(data.OldItems, Is.Empty);
                    Assert.That(data.NewItems, Is.EqualTo(newItems));
                });
            };
            inventory.Replace(newItems, randomIndex);
        }


        [Test]
        public void CanReplace_EmptyItems_ReturnsFalse()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var item = this.itemFactory.CreateDefault();
            var inventory = this.inventoryFactory.FullContainer(size, stackSize, item);

            var randomIndex = this.random.Next(0, size);
            var canReplace = inventory.CanReplace(Array.Empty<T>(), randomIndex);
            
            Assert.That(canReplace, Is.False);
        }

        [Test]
        public void CanReplace_SlotWithItems_ReturnsTrue()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var item = this.itemFactory.CreateDefault();
            var inventory = this.inventoryFactory.FullContainer(size, stackSize, item);

            var randomIndex = this.random.Next(0, size);
            var newItems = this.itemFactory.CreateManyRandom(stackSize);
            var canReplace = inventory.CanReplace(newItems, randomIndex);

            Assert.That(canReplace, Is.True);
        }

        [Test]
        public void CanReplace_EmptySlot_ReturnsTrue()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var inventory = this.inventoryFactory.EmptyContainer(size, stackSize);

            var randomIndex = this.random.Next(0, size);
            var newItems = this.itemFactory.CreateManyRandom(stackSize);
            var canReplace = inventory.CanReplace(newItems, randomIndex);

            Assert.That(canReplace, Is.True);
        }
    }
}
