﻿namespace TheChest.Inventories.Tests.Containers
{
    public partial class InventoryTests<T>
    {
        [TestCase(-1)]
        [TestCase(100)]
        public void Move_InvalidOrigin_ThrowsArgumentOutOfRangeException(int origin)
        {
            var size = this.random.Next(3, 10);
            var item = this.itemFactory.CreateDefault();
            var inventory = this.containerFactory.FullContainer(size, item);
            Assert.Throws<ArgumentOutOfRangeException>(() => inventory.Move(origin, 2));
        }

        [TestCase(-1)]
        [TestCase(100)]
        public void Move_InvalidTarget_ThrowsArgumentOutOfRangeException(int target)
        {
            var size = this.random.Next(3, 10);
            var item = this.itemFactory.CreateDefault();
            var inventory = this.containerFactory.FullContainer(size, item);
            Assert.Throws<ArgumentOutOfRangeException>(() => inventory.Move(0, target));
        }

        [Test]
        public void Move_BothSlotsWithItems_SwapsItems()
        {
            var size = this.random.Next(2, 20);
            var items = this.itemFactory.CreateManyRandom(size);
            var inventory = this.containerFactory.ShuffledItemsContainer(size, items);

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
            var inventory = this.containerFactory.EmptyContainer(2);

            var item = this.itemFactory.CreateRandom();
            inventory.Add(item);

            inventory.Move(0, 1);

            Assert.Multiple(() =>
            {
                Assert.That(inventory[0].IsEmpty, Is.True);
                Assert.That(inventory[1].Content, Is.EqualTo(item));
            });
        }

        [Test]
        public void Move_EmptyOrigin_MovesItem()
        {
            var inventory = this.containerFactory.EmptyContainer(2);

            var item = this.itemFactory.CreateRandom();
            inventory.AddAt(item, 1);

            inventory.Move(0, 1);

            Assert.Multiple(() => {
                Assert.That(inventory[0].Content, Is.EqualTo(item));
                Assert.That(inventory[1].IsEmpty, Is.True);
            });
        }
    }
}
