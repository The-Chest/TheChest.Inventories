namespace TheChest.Inventories.Tests.Containers
{
    public partial class StackInventoryTests<T>
    {
        [TestCase(-1)]
        [TestCase(100)]
        public void Replace_InvalidIndex_ThrowsArgumentOutOfRangeException(int index)
        {
            var items = this.itemFactory.CreateMany(20);
            var inventory = this.containerFactory.EmptyContainer(20);

            Assert.That(() => inventory.Replace(items, index), Throws.TypeOf<ArgumentOutOfRangeException>());
        }

        [Test]
        public void Replace_EmptyItems_ThrowsArgumentOutOfRangeException()
        {
            var item = this.itemFactory.CreateDefault();
            var stackSize = this.random.Next(10, 20);
            var size = this.random.Next(10, 20);
            var inventory = this.containerFactory.FullContainer(size, stackSize, item);

            var randomIndex = this.random.Next(0, size);
            Assert.That(() => 
                inventory.Replace(Array.Empty<T>(), randomIndex), 
                Throws.TypeOf<ArgumentException>()
            );
        }

        [Test]
        public void Replace_MoreItemsThanStackSize_ThrowsArgumentOutOfRangeException()
        {
            var item = this.itemFactory.CreateDefault();
            var stackSize = this.random.Next(10, 20);
            var size = this.random.Next(10, 20);
            var inventory = this.containerFactory.FullContainer(size, stackSize, item);
            var randomIndex = this.random.Next(0, size);
            var items = this.itemFactory.CreateMany(stackSize + 1);

            Assert.That(() =>
                inventory.Replace(items, randomIndex),
                Throws.InvalidOperationException
                    .With.Message.EqualTo("The amount of items to replace exceeds the stack size of the slot.")
            );
        }

        [Test]
        public void Replace_NullItems_DoesNotReplace()
        {
            var item = this.itemFactory.CreateDefault();
            var stackSize = this.random.Next(10, 20);
            var size = this.random.Next(10, 20);
            var inventory = this.containerFactory.FullContainer(size, stackSize, item);

            var randomIndex = this.random.Next(0, size);
            Assert.That(() =>
                inventory.Replace(null!, randomIndex),
                Throws.TypeOf<ArgumentNullException>()
            );
        }

        [Test]
        public void Replace_SlotWithItems_ReplacesItemsInSlot()
        {
            var item = this.itemFactory.CreateDefault();
            var stackSize = this.random.Next(10, 20);
            var size = this.random.Next(10, 20);
            var inventory = this.containerFactory.FullContainer(size, stackSize, item);

            var randomIndex = this.random.Next(0, size);
            var newItems = this.itemFactory.CreateManyRandom(stackSize);
            inventory.Replace(newItems, randomIndex);

            Assert.That(inventory[randomIndex].GetContents(), Is.EqualTo(newItems));
        }

        [Test]
        public void Replace_SlotWithItems_ReturnsOldItems()
        {
            var item = this.itemFactory.CreateDefault();
            var stackSize = this.random.Next(10, 20);
            var size = this.random.Next(10, 20);
            var inventory = this.containerFactory.FullContainer(size, stackSize, item);

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
            var item = this.itemFactory.CreateDefault();
            var stackSize = this.random.Next(10, 20);
            var size = this.random.Next(10, 20);
            var inventory = this.containerFactory.FullContainer(size, stackSize, item);

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
            var stackSize = this.random.Next(10, 20);
            var size = this.random.Next(10, 20);
            var inventory = this.containerFactory.EmptyContainer(size, stackSize);

            var randomIndex = this.random.Next(0, size);
            var newItems = this.itemFactory.CreateManyRandom(stackSize);
            inventory.Replace(newItems, randomIndex);

            Assert.That(inventory[randomIndex].GetContents(), Is.EqualTo(newItems));
        }

        [Test]
        public void Replace_EmptySlot_ReturnsEmptyArray()
        {
            var stackSize = this.random.Next(10, 20);
            var size = this.random.Next(10, 20);
            var inventory = this.containerFactory.EmptyContainer(size, stackSize);

            var randomIndex = this.random.Next(0, size);
            var newItems = this.itemFactory.CreateManyRandom(this.random.Next(1, stackSize));
            var result = inventory.Replace(newItems, randomIndex);

            Assert.That(result, Is.Empty);
        }

        [Test]
        public void Replace_EmptySlot_CallsOnReplaceEvent()
        {
            var stackSize = this.random.Next(10, 20);
            var size = this.random.Next(10, 20);
            var inventory = this.containerFactory.FullContainer(size, stackSize);

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
    }
}
