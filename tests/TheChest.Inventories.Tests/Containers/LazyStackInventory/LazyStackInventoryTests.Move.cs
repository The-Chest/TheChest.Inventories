namespace TheChest.Inventories.Tests.Containers.LazyStackInventory
{
    public partial class LazyStackInventoryTests<T>
    {
        [TestCase(-1)]
        [TestCase(100)]
        public void Move_InvalidOrigin_ThrowsArgumentOutOfRangeException(int origin)
        {
            var stackSize = this.random.Next(1, 10);
            var item = this.itemFactory.CreateDefault();
            var inventory = this.containerFactory.FullContainer(2, stackSize, item);
            Assert.Throws<ArgumentOutOfRangeException>(() => inventory.Move(origin, 2));
        }

        [TestCase(-1)]
        [TestCase(100)]
        public void Move_InvalidTarget_ThrowsArgumentOutOfRangeException(int target)
        {
            var stackSize = this.random.Next(1, 10);
            var item = this.itemFactory.CreateDefault();
            var inventory = this.containerFactory.FullContainer(2, stackSize, item);
            Assert.Throws<ArgumentOutOfRangeException>(() => inventory.Move(0, target));
        }

        [Test]
        public void Move_OriginEqualsToTarget_ThrowsArgumentException()
        {
            var stackSize = this.random.Next(1, 10);
            var item = this.itemFactory.CreateDefault();
            var inventory = this.containerFactory.FullContainer(2, stackSize, item);

            Assert.Throws<ArgumentException>(() => inventory.Move(1, 1));
        }

        [Test]
        public void Move_BothSlotsWithItems_SwapsItems()
        {
            var size = this.random.Next(2, 20);
            var stackSize = this.random.Next(1, 10);
            var items = this.itemFactory.CreateManyRandom(size);
            var inventory = this.containerFactory.ShuffledItemsContainer(size, stackSize, items);

            var origin = 0;
            var target = 1;
            var itemFromOrigin = inventory[origin].Content;
            var ItemFromTarget = inventory[target].Content;
            inventory.Move(origin, target);

            Assert.Multiple(() =>
            {
                Assert.That(inventory[origin].Content, Is.EqualTo(ItemFromTarget));
                Assert.That(inventory[target].Content, Is.EqualTo(itemFromOrigin));
            });
        }

        [Test]
        public void Move_EmptyTarget_MovesItem()
        {
            var stackSize = this.random.Next(1, 10);
            var inventory = this.containerFactory.EmptyContainer(2, stackSize);

            var item = this.itemFactory.CreateRandom();
            inventory.Add(item);

            inventory.Move(0, 1);

            Assert.Multiple(() =>
            {
                Assert.That(inventory[0].Content, Is.Empty);
                Assert.That(inventory[0].IsEmpty, Is.True);
            });
            Assert.Multiple(() => {
                Assert.That(inventory[1].Content, Has.All.EqualTo(item));
                Assert.That(inventory[1].StackAmount, Is.EqualTo(1));
            });
        }

        [Test]
        public void Move_EmptyOrigin_MovesItem()
        {
            var stackSize = this.random.Next(1, 10);
            var inventory = this.containerFactory.EmptyContainer(2, stackSize);

            var item = this.itemFactory.CreateRandom();
            inventory.AddAt(item, 1, 1);

            inventory.Move(0, 1);

            Assert.Multiple(() => {
                Assert.That(inventory[0].Content, Has.All.EqualTo(item));
                Assert.That(inventory[0].StackAmount, Is.EqualTo(1));
            });
            Assert.Multiple(() =>
            {
                Assert.That(inventory[1].Content, Is.Empty);
                Assert.That(inventory[1].IsEmpty, Is.True);
            });
        }
    }
}
