using TheChest.Tests.Common.Extensions.Containers;

namespace TheChest.Inventories.Tests.Containers
{
    public partial class InventoryTests<T>
    {
        [TestCase(-1)]
        [TestCase(MAX_SIZE_TEST + 1)]
        public void Move_InvalidOrigin_ThrowsArgumentOutOfRangeException(int origin)
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var item = this.itemFactory.CreateDefault();
            var inventory = this.inventoryFactory.FullContainer(size, item);
            Assert.Throws<ArgumentOutOfRangeException>(() => inventory.Move(origin, 2));
        }

        [TestCase(-1)]
        [TestCase(100)]
        public void Move_InvalidTarget_ThrowsArgumentOutOfRangeException(int target)
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var item = this.itemFactory.CreateDefault();
            var inventory = this.inventoryFactory.FullContainer(size, item);
            Assert.Throws<ArgumentOutOfRangeException>(() => inventory.Move(0, target));
        }

        [Test]
        public void Move_BothSlotsWithItems_SwapsItems()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var items = this.itemFactory.CreateManyRandom(size);
            var inventory = this.inventoryFactory.ShuffledItemsContainer(size, items);

            var origin = 0;
            var target = 1;
            var itemFromOrigin = inventory.GetItem<T>(origin);
            var ItemFromTarget = inventory.GetItem<T>(target);
            inventory.Move(origin, target);

            Assert.Multiple(() =>
            {
                Assert.That(inventory.GetItem<T>(origin), Is.EqualTo(ItemFromTarget));
                Assert.That(inventory.GetItem<T>(target), Is.EqualTo(itemFromOrigin));
            });
        }

        [Test]
        public void Move_BothSlotsWithItems_CallsOnMoveWithTwoMovedItems()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var items = this.itemFactory.CreateManyRandom(size);
            var inventory = this.inventoryFactory.ShuffledItemsContainer(size, items);

            var origin = 0;
            var target = 1;
            var itemFromOrigin = inventory.GetItem<T>(origin);
            var ItemFromTarget = inventory.GetItem<T>(target);

            var raised = false;
            inventory.OnMove += (sender, args) =>
            {
                var dataArray = args.Data.ToArray();

                Assert.That(dataArray, Has.Length.EqualTo(2));
                Assert.Multiple(() =>
                {
                    Assert.That(dataArray[0].Item, Is.EqualTo(itemFromOrigin));
                    Assert.That(dataArray[0].FromIndex, Is.EqualTo(0));
                    Assert.That(dataArray[0].ToIndex, Is.EqualTo(1));
                });

                Assert.Multiple(() =>
                {
                    Assert.That(dataArray[1].Item, Is.EqualTo(ItemFromTarget));
                    Assert.That(dataArray[1].FromIndex, Is.EqualTo(1));
                    Assert.That(dataArray[1].ToIndex, Is.EqualTo(0));
                });
                raised = true;
            };

            inventory.Move(origin, target);

            Assert.That(raised, Is.True, "OnMove event was not raised");
        }

        [Test]
        public void Move_EmptyTarget_MovesItem()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var inventory = this.inventoryFactory.EmptyContainer(size);

            var item = this.itemFactory.CreateRandom();
            inventory.Add(item);

            inventory.Move(0, 1);

            Assert.Multiple(() =>
            {
                Assert.That(inventory.GetSlot<T>(0).IsEmpty, Is.True);
                Assert.That(inventory.GetItem<T>(1), Is.EqualTo(item));
            });
        }

        [Test]
        public void Move_EmptyTarget_CallsOnMoveWithOnlyOriginData()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var inventory = this.inventoryFactory.EmptyContainer(size);

            var item = this.itemFactory.CreateRandom();
            inventory.Add(item);

            var raised = false;
            inventory.OnMove += (sender, args) =>
            {
                var dataArray = args.Data.ToArray();

                Assert.That(dataArray, Has.Length.EqualTo(1));
                Assert.Multiple(() =>
                {
                    Assert.That(dataArray[0].Item, Is.EqualTo(item));
                    Assert.That(dataArray[0].FromIndex, Is.EqualTo(0));
                    Assert.That(dataArray[0].ToIndex, Is.EqualTo(1));
                });
                raised = true;
            };

            inventory.Move(0, 1);

            Assert.That(raised, Is.True, "OnMove event was not raised");
        }

        [Test]
        public void Move_EmptyOrigin_MovesItem()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var inventory = this.inventoryFactory.EmptyContainer(size);

            var item = this.itemFactory.CreateRandom();
            inventory.AddAt(item, 1);

            inventory.Move(0, 1);

            Assert.Multiple(() => {
                Assert.That(inventory.GetItem<T>(0), Is.EqualTo(item));
                Assert.That(inventory.GetSlot<T>(1).IsEmpty, Is.True);
            });
        }

        [Test]
        public void Move_EmptyOrigin_CallsOnMoveWithOnlyTargetData()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var inventory = this.inventoryFactory.EmptyContainer(size);

            var item = this.itemFactory.CreateRandom();
            inventory.AddAt(item, 1);

            var raised = false;
            inventory.OnMove += (sender, args) =>
            {
                var dataArray = args.Data.ToArray();

                Assert.That(dataArray, Has.Length.EqualTo(1));
                Assert.Multiple(() =>
                {
                    Assert.That(dataArray[0].Item, Is.EqualTo(item));
                    Assert.That(dataArray[0].FromIndex, Is.EqualTo(1));
                    Assert.That(dataArray[0].ToIndex, Is.EqualTo(0));
                });
                raised = true;
            };
            inventory.Move(0, 1);

            Assert.That(raised, Is.True, "OnMove event was not raised");
        }
    }
}
