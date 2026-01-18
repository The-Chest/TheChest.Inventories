namespace TheChest.Inventories.Tests.Containers
{
    public partial class InventoryTests<T>
    {
        [TestCase(-1)]
        [TestCase(22)]
        public void Replace_InvalidSlotIndex_ThrowsArgumentOutOfRangeException(int index)
        {
            var inventory = this.containerFactory.EmptyContainer();

            Assert.That(
                () => inventory.Replace(this.itemFactory.CreateDefault(), index),
                Throws.Exception.TypeOf<ArgumentOutOfRangeException>()
            );
        }

        [Test]
        public void Replace_NullItem_ThrowsArgumentNullException()
        {
            var inventory = this.containerFactory.EmptyContainer();
            Assert.That(() => inventory.Replace(default!, 0), Throws.ArgumentNullException);
        }

        [Test]
        public void Replace_EmptySlot_AddsItemToSlot()
        {
            var size = this.random.Next(10, 20);
            var inventory = this.containerFactory.EmptyContainer(size);

            var randomIndex = this.random.Next(0, size);
            var item = this.itemFactory.CreateDefault();
            inventory.Replace(item, randomIndex);

            Assert.That(inventory[randomIndex].GetContent(), Is.EqualTo(item));
        }

        [Test]
        public void Replace_EmptySlot_ReturnsNull()
        {
            var size = this.random.Next(10, 20);
            var inventory = this.containerFactory.EmptyContainer(size);

            var randomIndex = this.random.Next(0, size);
            var item = this.itemFactory.CreateDefault();
            var result = inventory.Replace(item, randomIndex);

            Assert.That(result, Is.Null);
        }

        [Test]
        public void Replace_EmptySlot_CallsOnAddEventWithEmptyOldItem()
        {
            var size = this.random.Next(10, 20);
            var inventory = this.containerFactory.EmptyContainer(size);

            var randomIndex = this.random.Next(0, size);
            var item = this.itemFactory.CreateDefault();

            var raised = false;
            inventory.OnReplace += (sender, args) =>
            {
                Assert.Multiple(() =>
                {
                    Assert.That(sender, Is.EqualTo(inventory));
                    Assert.That(args.Data.Select(x => x.OldItem), Has.All.Null);
                    Assert.That(args.Data.Select(x => x.NewItem), Has.All.EqualTo(item));
                });
                raised = true;
            };

            inventory.Replace(item, randomIndex);

            Assert.That(raised, Is.True, "OnReplace event was not raised");
        }

        [Test]
        public void Replace_FullSlot_ReplacesItemInSlot()
        {
            var size = this.random.Next(10, 20);
            var initialItem = this.itemFactory.CreateDefault();
            var inventory = this.containerFactory.FullContainer(size, initialItem);

            var randomIndex = this.random.Next(0, size);
            var newItem = this.itemFactory.CreateRandom();
            inventory.Replace(newItem, randomIndex);

            Assert.That(inventory[randomIndex].GetContent(), Is.EqualTo(newItem));
        }

        [Test]
        public void Replace_FullSlot_ReturnsOldItemFromSlot()
        {
            var size = this.random.Next(10, 20);
            var initialItem = this.itemFactory.CreateDefault();
            var inventory = this.containerFactory.FullContainer(size, initialItem);

            var randomIndex = this.random.Next(0, size);
            var newItem = this.itemFactory.CreateRandom();
            var result = inventory.Replace(newItem, randomIndex);

            Assert.That(result, Is.EqualTo(initialItem));
        }

        [Test]
        public void Replace_FullSlot_CallsOnReplaceEvent()
        {
            var size = this.random.Next(10, 20);
            var initialItem = this.itemFactory.CreateDefault();
            var inventory = this.containerFactory.FullContainer(size, initialItem);

            var randomIndex = this.random.Next(0, size);
            var newItem = this.itemFactory.CreateRandom();

            var raised = false;
            inventory.OnReplace += (sender, args) =>
            {
                Assert.Multiple(() =>
                {
                    Assert.That(sender, Is.EqualTo(inventory));
                    Assert.That(args.Data.Select(x => x.OldItem), Has.All.EqualTo(initialItem));
                    Assert.That(args.Data.Select(x => x.NewItem), Has.All.EqualTo(newItem));
                });
                raised = true;
            };
            inventory.Replace(newItem, randomIndex);

            Assert.That(raised, Is.True, "OnReplace event was not raised");
        }
    }
}
