using TheChest.Inventories.Tests.Common.Extensions;
using TheChest.Tests.Common.Attributes;
using TheChest.Tests.Common.Extensions.Containers;

namespace TheChest.Inventories.Tests.Containers.Inventory
{
    public partial class InventoryTests<T>
    {
        [TestCase(-1)]
        [TestCase(MAX_SIZE_TEST)]
        public void Move_InvalidOrigin_ThrowsArgumentOutOfRangeException(int origin)
        {
            var size = this.GenerateRandomSize();
            var item = this.itemFactory.CreateDefault();
            var inventory = this.inventoryFactory.FullContainer(size, item);

            Assert.That(
                () => inventory.Move(origin, 0),
                Throws.TypeOf<ArgumentOutOfRangeException>().With.Property("ParamName").EqualTo("origin")
            );
        }

        [Test]
        public void Move_OriginEqualToSize_ThrowsArgumentOutOfRangeException()
        {
            var size = this.GenerateRandomSize();
            var item = this.itemFactory.CreateDefault();
            var inventory = this.inventoryFactory.FullContainer(size, item);

            Assert.That(
                () => inventory.Move(size, 0), 
                Throws.TypeOf<ArgumentOutOfRangeException>().With.Property("ParamName").EqualTo("origin")
            );
        }

        [TestCase(-1)]
        [TestCase(MAX_SIZE_TEST)]
        public void Move_InvalidTarget_ThrowsArgumentOutOfRangeException(int target)
        {
            var size = this.GenerateRandomSize();
            var item = this.itemFactory.CreateDefault();
            var inventory = this.inventoryFactory.FullContainer(size, item);

            Assert.That(
                () => inventory.Move(0, target),
                Throws.TypeOf<ArgumentOutOfRangeException>().With.Property("ParamName").EqualTo("target")
            );
        }

        [Test]
        public void Move_TargetEqualToSize_ThrowsArgumentOutOfRangeException()
        {
            var size = this.GenerateRandomSize();
            var item = this.itemFactory.CreateDefault();
            var inventory = this.inventoryFactory.FullContainer(size, item);

            Assert.That(
                () => inventory.Move(0, size),
                Throws.TypeOf<ArgumentOutOfRangeException>().With.Property("ParamName").EqualTo("target")
            );
        }

        [Test]
        public void Move_OriginEqualToTarget_ThrowsArgumentException()
        {
            var size = this.GenerateRandomSize();
            var item = this.itemFactory.CreateDefault();
            var inventory = this.inventoryFactory.FullContainer(size, item);

            var index = this.random.Next(0, size);

            Assert.That(
                () => inventory.Move(index, index),
                Throws.ArgumentException.With.Message.Contains("Cannot move an item to the same index.")
            );
        }

        [Test]
        public void Move_BothSlotsEmpty_ThrowsInvalidOperationException()
        {
            var size = this.GenerateRandomSize();
            var inventory = this.inventoryFactory.EmptyContainer(size);

            var (origin, target) = this.random.GetRandomOriginAndTarget(size);
            Assert.That(
                () => inventory.Move(origin, target),
                Throws.InvalidOperationException.With.Message.EqualTo("Cannot move items when both origin and target slots are empty.")
            );
        }

        [Test]
        public void Move_BothSlotsWithItems_SwapsItems()
        {
            var size = this.GenerateRandomSize();
            var inventory = this.inventoryFactory.EmptyContainer(size);

            var (origin, target) = this.random.GetRandomOriginAndTarget(size);

            var itemFromOrigin = this.itemFactory.CreateRandom();
            var itemFromTarget = this.itemFactory.CreateRandomDifferentFrom(itemFromOrigin);

            inventory.AddAt(itemFromOrigin, origin);
            inventory.AddAt(itemFromTarget, target);
            
            inventory.Move(origin, target);

            Assert.Multiple(() =>
            {
                Assert.That(inventory.GetItem(origin), Is.EqualTo(itemFromTarget));
                Assert.That(inventory.GetItem(target), Is.EqualTo(itemFromOrigin));
            });
        }

        [Test]
        public void Move_BothSlotsWithItems_CallsOnMoveWithTwoMovedItems()
        {
            var size = this.GenerateRandomSize();
            var inventory = this.inventoryFactory.EmptyContainer(size);

            var (origin, target) = this.random.GetRandomOriginAndTarget(size);
            var (originItem, targetItem) = this.itemFactory.CreateRandomDistinctPair();

            inventory.AddAt(originItem, origin);
            inventory.AddAt(targetItem, target);

            var raised = false;
            inventory.OnMove += (sender, args) =>
            {
                var dataArray = args.Data.ToArray();

                Assert.That(dataArray, Has.Length.EqualTo(2));
                Assert.Multiple(() =>
                {
                    Assert.That(dataArray[0].Item, Is.EqualTo(originItem));
                    Assert.That(dataArray[0].FromIndex, Is.EqualTo(origin));
                    Assert.That(dataArray[0].ToIndex, Is.EqualTo(target));
                });

                Assert.Multiple(() =>
                {
                    Assert.That(dataArray[1].Item, Is.EqualTo(targetItem));
                    Assert.That(dataArray[1].FromIndex, Is.EqualTo(target));
                    Assert.That(dataArray[1].ToIndex, Is.EqualTo(origin));
                });
                raised = true;
            };

            inventory.Move(origin, target);

            Assert.That(raised, Is.True, "OnMove event was not raised");
        }

        [Test]
        public void Move_EmptyTarget_MovesItem()
        {
            var size = this.GenerateRandomSize();
            var inventory = this.inventoryFactory.EmptyContainer(size);

            var (origin, target) = this.random.GetRandomOriginAndTarget(size);
            var item = this.itemFactory.CreateRandom();
            inventory.AddAt(item, origin);

            inventory.Move(origin, target);

            Assert.Multiple(() =>
            {
                Assert.That(inventory.GetSlot(origin).IsEmpty, Is.True);
                Assert.That(inventory.GetItem(target), Is.EqualTo(item));
            });
        }

        [Test]
        public void Move_EmptyTarget_CallsOnMoveWithOnlyOriginData()
        {
            var size = this.GenerateRandomSize();
            var inventory = this.inventoryFactory.EmptyContainer(size);

            var (origin, target) = this.random.GetRandomOriginAndTarget(size);
            var item = this.itemFactory.CreateRandom();
            inventory.AddAt(item, origin);

            var raised = false;
            inventory.OnMove += (sender, args) =>
            {
                var dataArray = args.Data.ToArray();

                Assert.That(dataArray, Has.Length.EqualTo(1));
                Assert.Multiple(() =>
                {
                    Assert.That(dataArray[0].Item, Is.EqualTo(item));
                    Assert.That(dataArray[0].FromIndex, Is.EqualTo(origin));
                    Assert.That(dataArray[0].ToIndex, Is.EqualTo(target));
                });
                raised = true;
            };

            inventory.Move(origin, target);

            Assert.That(raised, Is.True, "OnMove event was not raised");
        }


        [Test]
        [IgnoreIfReferenceType]
        public void Move_DefaultValueItemToEmptyTarget_MovesItem()
        {
            var size = this.GenerateRandomSize();
            var inventory = this.inventoryFactory.EmptyContainer(size);

            var (origin, target) = this.random.GetRandomOriginAndTarget(size);
            var item = default(T);
            inventory.AddAt(item, origin);

            inventory.Move(origin, target);

            Assert.Multiple(() =>
            {
                Assert.That(inventory.GetSlot(origin).IsEmpty, Is.True);
                Assert.That(inventory.GetItem(target), Is.EqualTo(item));
            });
        }

        [Test]
        public void Move_EmptyOrigin_MovesItem()
        {
            var size = this.GenerateRandomSize();
            var inventory = this.inventoryFactory.EmptyContainer(size);

            var (origin, target) = this.random.GetRandomOriginAndTarget(size);
            var item = this.itemFactory.CreateRandom();
            inventory.AddAt(item, target);

            inventory.Move(origin, target);

            Assert.Multiple(() => {
                Assert.That(inventory.GetItem(origin), Is.EqualTo(item));
                Assert.That(inventory.GetSlot(target).IsEmpty, Is.True);
            });
        }

        [Test]
        public void Move_EmptyOrigin_CallsOnMoveWithOnlyTargetData()
        {
            var size = this.GenerateRandomSize();
            var inventory = this.inventoryFactory.EmptyContainer(size);

            var (origin, target) = this.random.GetRandomOriginAndTarget(size);
            var item = this.itemFactory.CreateRandom();
            inventory.AddAt(item, target);

            var raised = false;
            inventory.OnMove += (sender, args) =>
            {
                var dataArray = args.Data.ToArray();

                Assert.That(dataArray, Has.Length.EqualTo(1));
                Assert.Multiple(() =>
                {
                    Assert.That(dataArray[0].Item, Is.EqualTo(item));
                    Assert.That(dataArray[0].FromIndex, Is.EqualTo(target));
                    Assert.That(dataArray[0].ToIndex, Is.EqualTo(origin));
                });
                raised = true;
            };
            inventory.Move(origin, target);

            Assert.That(raised, Is.True, "OnMove event was not raised");
        }
    }
}
