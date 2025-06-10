namespace TheChest.Inventories.Tests.Containers
{
    public partial class InventoryTests<T>
    {
        [TestCase(-1)]
        [TestCase(22)]
        public void AddAt_InvalidSlotIndex_ThrowsArgumentOutOfRangeException(int index)
        {
            var inventory = this.containerFactory.EmptyContainer();

            Assert.That(
                () => inventory.AddAt(this.itemFactory.CreateDefault(), index),
                Throws.Exception.TypeOf<ArgumentOutOfRangeException>()
            );
        }

        [Test]
        public void AddAt_NullItem_ThrowsArgumentNullException()
        {
            var inventory = this.containerFactory.EmptyContainer();
            Assert.That(() => inventory.AddAt(default!, 0), Throws.ArgumentNullException);
        }

        [Test]
        public void AddAt_EmptySlot_AddsItem()
        {
            var size = this.random.Next(10,20);
            var inventory = this.containerFactory.EmptyContainer(size);

            var randomIndex = this.random.Next(0, size);
            var item = this.itemFactory.CreateDefault();
            inventory.AddAt(item, randomIndex);

            Assert.That(inventory[randomIndex].Content, Is.EqualTo(item));
        }

        [Test]
        public void AddAt_EmptySlot_CallsOnAddEvent()
        {
            var size = this.random.Next(10, 20);
            var inventory = this.containerFactory.EmptyContainer(size);

            var randomIndex = this.random.Next(0, size);
            var item = this.itemFactory.CreateDefault();
            inventory.OnAdd += (sender, args) =>
            {
                Assert.Multiple(() =>
                {
                    Assert.That(sender, Is.EqualTo(inventory));
                    Assert.That(args.Data.Select(x => x.Item), Has.All.EqualTo(item));
                });
            };
            inventory.AddAt(item, randomIndex);
        }

        [Test]
        public void AddAt_EmptySlot_ReturnsNull()
        {
            var size = this.random.Next(10, 20);
            var inventory = this.containerFactory.EmptyContainer(size);

            var randomIndex = this.random.Next(0, size);
            var item = this.itemFactory.CreateDefault();
            var result = inventory.AddAt(item, randomIndex);

            Assert.That(result, Is.Null);
        }

        [Test]
        public void AddAt_FullSlot_ReplacesTheItem()
        {
            var size = this.random.Next(10, 20);
            var oldItem = this.itemFactory.CreateDefault();
            var inventory = this.containerFactory.FullContainer(size, oldItem);

            var randomIndex = this.random.Next(0, size);
            var item = this.itemFactory.CreateRandom();
            inventory.AddAt(item, randomIndex);

            Assert.Multiple(() =>
            {
                var randomSlot = inventory[randomIndex];
                Assert.That(randomSlot.Content, Is.EqualTo(item));
                Assert.That(randomSlot.Content, Is.Not.EqualTo(oldItem));
            });
        }

        [Test]
        public void AddAt_FullSlotReplaceTrue_ReturnsOldItem()
        {
            var size = this.random.Next(10, 20);
            var oldItem = this.itemFactory.CreateDefault();
            var inventory = this.containerFactory.FullContainer(size, oldItem);

            var randomIndex = this.random.Next(0, size);
            var item = this.itemFactory.CreateRandom();
            var result = inventory.AddAt(item, randomIndex);

            Assert.That(result, Is.EqualTo(oldItem));
        }

        [Test]
        public void AddAt_FullSlotReplaceFalse_DoNotCallOnAddEvent()
        {
            var size = this.random.Next(10, 20);
            var oldItem = this.itemFactory.CreateDefault();
            var inventory = this.containerFactory.FullContainer(size, oldItem);

            var randomIndex = this.random.Next(0, size);
            var item = this.itemFactory.CreateRandom();
            inventory.OnAdd += (sender, args) => Assert.Fail("OnAdd should not be called if inventory is full");
            inventory.AddAt(item, randomIndex, false);
        }

        [Test]
        public void AddAt_FullSlotReplaceFalse_DoNotReplaceTheItem()
        {
            var size = this.random.Next(10, 20);
            var oldItem = this.itemFactory.CreateDefault();
            var inventory = this.containerFactory.FullContainer(size, oldItem);

            var randomIndex = this.random.Next(0, size);
            var item = this.itemFactory.CreateRandom();
            inventory.AddAt(item, randomIndex, false);

            Assert.Multiple(() =>
            {
                var randomSlot = inventory[randomIndex];
                Assert.That(randomSlot.Content, Is.Not.EqualTo(item));
                Assert.That(randomSlot.Content, Is.EqualTo(oldItem));
            });
        }

        [Test]
        public void AddAt_FullSlotReplaceFalse_ReturnsSameItem()
        {
            var size = this.random.Next(10, 20);
            var oldItem = this.itemFactory.CreateDefault();
            var inventory = this.containerFactory.FullContainer(size, oldItem);

            var randomIndex = this.random.Next(0, size);
            var item = this.itemFactory.CreateDefault();
            var result = inventory.AddAt(item, randomIndex, false);

            Assert.That(result, Is.EqualTo(item));
        }
    }
}
