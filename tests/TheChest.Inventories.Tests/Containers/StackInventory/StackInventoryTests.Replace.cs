﻿namespace TheChest.Inventories.Tests.Containers
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
        public void Replace_EmptyItems_ReturnsEmptyArray()
        {
            var item = this.itemFactory.CreateDefault();
            var stackSize = this.random.Next(10, 20);
            var size = this.random.Next(10, 20);
            var inventory = this.containerFactory.FullContainer(size, stackSize, item);

            var randomIndex = this.random.Next(0, size);
            var result = inventory.Replace(Array.Empty<T>(), randomIndex);

            Assert.That(result, Is.Empty);
        }

        [Test]
        public void Replace_EmptyItems_DoesNotReplace()
        {
            var item = this.itemFactory.CreateDefault();
            var stackSize = this.random.Next(10, 20);
            var size = this.random.Next(10, 20);
            var inventory = this.containerFactory.FullContainer(size, stackSize, item);

            var randomIndex = this.random.Next(0, size);
            inventory.Replace(Array.Empty<T>(), randomIndex);

            Assert.That(inventory[randomIndex].GetContents(), Has.All.EqualTo(item));
        }

        [Test]
        public void Replace_EmptyItems_DoesNotCallOnReplaceEvent()
        {
            var item = this.itemFactory.CreateDefault();
            var stackSize = this.random.Next(10, 20);
            var size = this.random.Next(10, 20);
            var inventory = this.containerFactory.FullContainer(size, stackSize, item);

            var randomIndex = this.random.Next(0, size);
            inventory.OnReplace += 
                (sender, args) => Assert.Fail("OnReplace event should not be called when replacing empty items");
            inventory.Replace(Array.Empty<T>(), randomIndex);
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
            var newItems = this.itemFactory.CreateManyRandom(this.random.Next(1, 20));
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
    }
}
