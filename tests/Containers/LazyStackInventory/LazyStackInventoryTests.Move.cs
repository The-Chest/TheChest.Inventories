using TheChest.Tests.Common.Extensions.Containers;
using TheChest.Tests.Common.Extensions.Slots;

namespace TheChest.Inventories.Tests.Containers.LazyStackInventory
{
    public partial class LazyStackInventoryTests<T>
    {
        [TestCase(-1)]
        [TestCase(MAX_SIZE_TEST)]
        public void Move_InvalidOrigin_ThrowsArgumentOutOfRangeException(int origin)
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var item = this.itemFactory.CreateDefault();
            var inventory = this.inventoryFactory.FullContainer(size, stackSize, item);

            Assert.That(
                () => inventory.Move(origin, 0),
                Throws.TypeOf<ArgumentOutOfRangeException>().With.Property("ParamName").EqualTo("origin")
            );
        }

        [Test]
        public void Move_OriginEqualToSize_ThrowsArgumentOutOfRangeException()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var item = this.itemFactory.CreateDefault();
            var inventory = this.inventoryFactory.FullContainer(size, stackSize, item);
            Assert.That(
                () => inventory.Move(size, 0),
                Throws.TypeOf<ArgumentOutOfRangeException>().With.Property("ParamName").EqualTo("origin")
            );
        }

        [TestCase(-1)]
        [TestCase(MAX_SIZE_TEST)]
        public void Move_InvalidTarget_ThrowsArgumentOutOfRangeException(int target)
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var item = this.itemFactory.CreateDefault();
            var inventory = this.inventoryFactory.FullContainer(size, stackSize, item);

            Assert.That(
                () => inventory.Move(0, target),
                Throws.TypeOf<ArgumentOutOfRangeException>().With.Property("ParamName").EqualTo("target")
            );
        }

        [Test]
        public void Move_TargetEqualToSize_ThrowsArgumentOutOfRangeException()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var item = this.itemFactory.CreateDefault();
            var inventory = this.inventoryFactory.FullContainer(size, stackSize, item);
            Assert.That(
                () => inventory.Move(0, size),
                Throws.TypeOf<ArgumentOutOfRangeException>().With.Property("ParamName").EqualTo("target")
            );
        }

        [Test]
        public void Move_EmptyOriginAndTarget_ThrowsInvalidOperationException()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var inventory = this.inventoryFactory.EmptyContainer(size, stackSize);

            var originIndex = this.random.Next(size / 2, size);
            var targetIndex = this.random.Next(0, size / 2 - 1);

            Assert.That(
                () => inventory.Move(originIndex, targetIndex),
                Throws.TypeOf<InvalidOperationException>().With.Message.EqualTo("Cannot move empty slots.")
            );
        }

        [Test]
        public void Move_OriginEqualsToTarget_ThrowsArgumentException()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var item = this.itemFactory.CreateDefault();
            var inventory = this.inventoryFactory.FullContainer(size, stackSize, item);

            var originIndex = this.random.Next(0, size);
            var targetIndex = originIndex;
            Assert.That(
                () => inventory.Move(originIndex, targetIndex),
                Throws.TypeOf<ArgumentException>()
                    .With.Message.StartsWith("Cannot move an item to the same index.").And
                    .With.Property("ParamName").EqualTo("target")
            );
        }

        [Test]
        public void Move_BothSlotsWithItems_SwapsItems()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var items = this.itemFactory.CreateManyRandom(size);
            var inventory = this.inventoryFactory.ShuffledItemsContainer(size, stackSize, items);

            var originIndex = this.random.Next(size / 2, size);
            var originItems = inventory.GetItems(originIndex);

            var targetIndex = this.random.Next(0, size / 2 - 1);
            var targetItems = inventory.GetItems(targetIndex);

            inventory.Move(originIndex, targetIndex);

            Assert.Multiple(() =>
            {
                Assert.Multiple(() =>
                {
                    var originSlot = inventory.GetSlot(originIndex);

                    Assert.That(originSlot.GetContent(), Is.EqualTo(targetItems[0]));
                    Assert.That(originSlot.Amount, Is.EqualTo(targetItems.Length));
                });
                Assert.Multiple(() => {
                    var targetSlot = inventory.GetSlot(targetIndex);
                    Assert.That(targetSlot.GetContent(), Is.EqualTo(originItems[0]));
                    Assert.That(targetSlot.Amount, Is.EqualTo(originItems.Length));
                });
            });
        }

        [Test]
        public void Move_BothSlotsWithItems_CallsOnMove()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var items = this.itemFactory.CreateManyRandom(size);
            var inventory = this.inventoryFactory.ShuffledItemsContainer(size, stackSize, items);

            var originIndex = this.random.Next(size / 2, size);
            var originItems = inventory.GetItems(originIndex);

            var targetIndex = this.random.Next(0, size / 2 - 1);
            var targetItems = inventory.GetItems(targetIndex);

            var raised = false;
            inventory.OnMove += (o, args) =>
            {
                Assert.That(args.Data, Has.Count.EqualTo(2));
                Assert.Multiple(() =>
                {
                    var firstEvent = args.Data.FirstOrDefault();
                    Assert.That(firstEvent.Item, Is.EqualTo(originItems[0]));
                    Assert.That(firstEvent.Amount, Is.EqualTo(originItems.Length));
                    Assert.That(firstEvent.FromIndex, Is.EqualTo(originIndex));
                    Assert.That(firstEvent.ToIndex, Is.EqualTo(targetIndex));
                });

                Assert.Multiple(() =>
                {
                    var secondEvent = args.Data.Skip(1).FirstOrDefault();
                    Assert.That(secondEvent.Item, Is.EqualTo(targetItems[0]));
                    Assert.That(secondEvent.Amount, Is.EqualTo(targetItems.Length));
                    Assert.That(secondEvent.FromIndex, Is.EqualTo(targetIndex));
                    Assert.That(secondEvent.ToIndex, Is.EqualTo(originIndex));
                });
                raised = true;
            };
            inventory.Move(originIndex, targetIndex);

            Assert.That(raised, Is.True, "OnMove event was not raised");
        }

        [Test]
        public void Move_EmptyTarget_MovesItem()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var inventory = this.inventoryFactory.EmptyContainer(size, stackSize);

            var item = this.itemFactory.CreateRandom();
            var originIndex = this.random.Next(size / 2, size);
            var targetIndex = this.random.Next(0, size / 2 - 1);
            var amount = this.random.Next(1, stackSize);
            inventory.AddAt(item, originIndex, amount);

            inventory.Move(originIndex, targetIndex);

            Assert.Multiple(() =>
            {
                var originSlot = inventory.GetSlot(originIndex);
                Assert.That(originSlot.GetContent(), Is.Null);
                Assert.That(originSlot.IsEmpty, Is.True);
            });
            Assert.Multiple(() => {
                var targetSlot = inventory.GetSlot(targetIndex);
                Assert.That(targetSlot.GetContent(), Is.EqualTo(item));
                Assert.That(targetSlot.Amount, Is.EqualTo(amount));
            });
        }

        [Test]
        public void Move_EmptyTarget_CallsOnMoveEventOnlyWithOrigin()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var inventory = this.inventoryFactory.EmptyContainer(size, stackSize);

            var originIndex = this.random.Next(size /2, size);
            var targetIndex = this.random.Next(0, size / 2 - 1);
            var amount = this.random.Next(1, stackSize);

            var item = this.itemFactory.CreateRandom();
            inventory.AddAt(item, originIndex, amount);

            var raised = false;
            inventory.OnMove += (o, args) =>
            {
                Assert.That(args.Data, Has.Count.EqualTo(1));
                Assert.Multiple(() =>
                {
                    var firstEvent = args.Data.FirstOrDefault();
                    Assert.That(firstEvent.Item, Is.EqualTo(item));
                    Assert.That(firstEvent.FromIndex, Is.EqualTo(originIndex));
                    Assert.That(firstEvent.ToIndex, Is.EqualTo(targetIndex));
                    Assert.That(firstEvent.Amount, Is.EqualTo(amount));
                });
                raised = true;
            };
            inventory.Move(originIndex, targetIndex);

            Assert.That(raised, Is.True, "OnMove event was not raised");
        }

        [Test]
        public void Move_EmptyOrigin_MovesItem()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var inventory = this.inventoryFactory.EmptyContainer(size, stackSize);

            var item = this.itemFactory.CreateRandom();
            var originIndex = this.random.Next(size / 2, size);
            var targetIndex = this.random.Next(0, size / 2 - 1);
            var amount = this.random.Next(1, stackSize);
            inventory.AddAt(item, targetIndex, amount);

            inventory.Move(originIndex, targetIndex);

            Assert.Multiple(() => {
                var originSlot = inventory.GetSlot(originIndex);

                Assert.That(originSlot!.GetContent(), Is.EqualTo(item));
                Assert.That(originSlot!.Amount, Is.EqualTo(amount));
            });
            Assert.Multiple(() =>
            {
                var targetSlot = inventory.GetSlot(targetIndex);
              
                Assert.That(targetSlot!.GetContent(), Is.Null);
                Assert.That(targetSlot!.IsEmpty, Is.True);
            });
        }

        [Test]
        public void Move_EmptyOrigin_CallsOnMoveEventOnlyWithTarget()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var inventory = this.inventoryFactory.EmptyContainer(size, stackSize);

            var item = this.itemFactory.CreateRandom();
            var originIndex = this.random.Next(size / 2, size);
            var targetIndex = this.random.Next(0, size / 2 - 1);
            var amount = this.random.Next(1, stackSize);
            inventory.AddAt(item, targetIndex, amount);

            var raised = false;
            inventory.OnMove += (o, args) =>
            {
                Assert.That(args.Data, Has.Count.EqualTo(1));
                Assert.Multiple(() =>
                {
                    var firstEvent = args.Data.FirstOrDefault();
                    Assert.That(firstEvent.Item, Is.EqualTo(item));
                    Assert.That(firstEvent.FromIndex, Is.EqualTo(targetIndex));
                    Assert.That(firstEvent.ToIndex, Is.EqualTo(originIndex));
                });
                raised = true;
            };
            inventory.Move(originIndex, targetIndex);

            Assert.That(raised, Is.True, "OnMove event was not raised");
        }
    }
}
