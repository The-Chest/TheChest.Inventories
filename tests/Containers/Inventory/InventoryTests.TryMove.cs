using TheChest.Tests.Common.Extensions.Containers;

namespace TheChest.Inventories.Tests.Containers.Inventory
{
    public partial class InventoryTests<T>
    {
        [TestCase(-1)]
        [TestCase(MAX_SIZE_TEST)]
        public void TryMove_InvalidOrigin_ThrowsArgumentOutOfRangeException(int origin)
        {
            var size = this.GenerateRandomSize();
            var item = this.itemFactory.CreateDefault();
            var inventory = this.inventoryFactory.FullContainer(size, item);

            Assert.That(
                () => inventory.TryMove(origin, 0),
                Throws.TypeOf<ArgumentOutOfRangeException>().With.Property("ParamName").EqualTo("origin")
            );
        }

        [Test]
        public void TryMove_OriginEqualToSize_ThrowsArgumentOutOfRangeException()
        {
            var size = this.GenerateRandomSize();
            var item = this.itemFactory.CreateDefault();
            var inventory = this.inventoryFactory.FullContainer(size, item);

            Assert.That(
                () => inventory.TryMove(size, 0),
                Throws.TypeOf<ArgumentOutOfRangeException>().With.Property("ParamName").EqualTo("origin")
            );
        }

        [TestCase(-1)]
        [TestCase(MAX_SIZE_TEST)]
        public void TryMove_InvalidTarget_ThrowsArgumentOutOfRangeException(int target)
        {
            var size = this.GenerateRandomSize();
            var item = this.itemFactory.CreateDefault();
            var inventory = this.inventoryFactory.FullContainer(size, item);

            Assert.That(
                () => inventory.TryMove(0, target),
                Throws.TypeOf<ArgumentOutOfRangeException>().With.Property("ParamName").EqualTo("target")
            );
        }

        [Test]
        public void TryMove_TargetEqualToSize_ThrowsArgumentOutOfRangeException()
        {
            var size = this.GenerateRandomSize();
            var item = this.itemFactory.CreateDefault();
            var inventory = this.inventoryFactory.FullContainer(size, item);

            Assert.That(
                () => inventory.TryMove(0, size),
                Throws.TypeOf<ArgumentOutOfRangeException>().With.Property("ParamName").EqualTo("target")
            );
        }

        [Test]
        public void TryMove_OriginEqualToTarget_ReturnsFalse()
        {
            var size = this.GenerateRandomSize();
            var item = this.itemFactory.CreateDefault();
            var inventory = this.inventoryFactory.FullContainer(size, item);
            var randomIndex = this.random.Next(0, size);

            var moved = inventory.TryMove(randomIndex, randomIndex);

            Assert.That(moved, Is.False);
        }

        [Test]
        public void TryMove_BothSlotsEmpty_ReturnsTrue()
        {
            var size = this.GenerateRandomSize();
            var inventory = this.inventoryFactory.EmptyContainer(size);

            var moved = inventory.TryMove(0, 1);

            Assert.That(moved, Is.True);
        }

        [Test]
        public void TryMove_BothSlotsEmpty_DoesntCallOnMove()
        {
            var size = this.GenerateRandomSize();
            var inventory = this.inventoryFactory.EmptyContainer(size);
            var raised = false;
            inventory.OnMove += (sender, args) => raised = true;

            inventory.TryMove(0, 1);

            Assert.That(raised, Is.False);
        }

        [Test]
        public void TryMove_BothSlotsWithItems_SwapsItems()
        {
            var size = this.GenerateRandomSize();
            var items = this.itemFactory.CreateManyRandom(size);
            var inventory = this.inventoryFactory.ShuffledItemsContainer(size, items);
            var origin = 0;
            var target = 1;
            var itemFromOrigin = inventory.GetItem(origin);
            var itemFromTarget = inventory.GetItem(target);

            inventory.TryMove(origin, target);

            Assert.Multiple(() =>
            {
                Assert.That(inventory.GetItem(origin), Is.EqualTo(itemFromTarget));
                Assert.That(inventory.GetItem(target), Is.EqualTo(itemFromOrigin));
            });
        }

        [Test]
        public void TryMove_BothSlotsWithItems_CallsOnMoveWithTwoMovedItems()
        {
            var size = this.GenerateRandomSize();
            var items = this.itemFactory.CreateManyRandom(size);
            var inventory = this.inventoryFactory.ShuffledItemsContainer(size, items);
            var origin = 0;
            var target = 1;
            var itemFromOrigin = inventory.GetItem(origin);
            var itemFromTarget = inventory.GetItem(target);
            var raised = false;
            inventory.OnMove += (sender, args) =>
            {
                var dataArray = args.Data.ToArray();

                Assert.That(dataArray, Has.Length.EqualTo(2));
                Assert.Multiple(() =>
                {
                    Assert.That(dataArray[0].Item, Is.EqualTo(itemFromOrigin));
                    Assert.That(dataArray[0].FromIndex, Is.EqualTo(origin));
                    Assert.That(dataArray[0].ToIndex, Is.EqualTo(target));
                });
                Assert.Multiple(() =>
                {
                    Assert.That(dataArray[1].Item, Is.EqualTo(itemFromTarget));
                    Assert.That(dataArray[1].FromIndex, Is.EqualTo(target));
                    Assert.That(dataArray[1].ToIndex, Is.EqualTo(origin));
                });
                raised = true;
            };

            inventory.TryMove(origin, target);

            Assert.That(raised, Is.True);
        }

        [Test]
        public void TryMove_EmptyTarget_MovesItem()
        {
            var size = this.GenerateRandomSize();
            var inventory = this.inventoryFactory.EmptyContainer(size);
            var item = this.itemFactory.CreateRandom();
            inventory.Add(item);

            inventory.TryMove(0, 1);

            Assert.Multiple(() =>
            {
                Assert.That(inventory.GetSlot(0).IsEmpty, Is.True);
                Assert.That(inventory.GetItem(1), Is.EqualTo(item));
            });
        }

        [Test]
        public void TryMove_EmptyTarget_CallsOnMoveWithOnlyOriginData()
        {
            var size = this.GenerateRandomSize();
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

            inventory.TryMove(0, 1);

            Assert.That(raised, Is.True);
        }

        [Test]
        public void TryMove_EmptyOrigin_MovesItem()
        {
            var size = this.GenerateRandomSize();
            var inventory = this.inventoryFactory.EmptyContainer(size);
            var item = this.itemFactory.CreateRandom();
            inventory.AddAt(item, 1);

            inventory.TryMove(0, 1);

            Assert.Multiple(() =>
            {
                Assert.That(inventory.GetItem(0), Is.EqualTo(item));
                Assert.That(inventory.GetSlot(1).IsEmpty, Is.True);
            });
        }

        [Test]
        public void TryMove_EmptyOrigin_CallsOnMoveWithOnlyTargetData()
        {
            var size = this.GenerateRandomSize();
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

            inventory.TryMove(0, 1);

            Assert.That(raised, Is.True);
        }
    }
}
