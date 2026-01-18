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
            var itemFromOrigin = inventory.GetItem(origin);
            var ItemFromTarget = inventory.GetItem(target);
            inventory.Move(origin, target);

            Assert.Multiple(() =>
            {
                Assert.That(inventory.GetItem(origin), Is.EqualTo(ItemFromTarget));
                Assert.That(inventory.GetItem(target), Is.EqualTo(itemFromOrigin));
            });
        }

        [Test]
        public void Move_BothSlotsWithItems_CallsOnMove()
        {
            var size = this.random.Next(2, 20);
            var stackSize = this.random.Next(1, 10);
            var items = this.itemFactory.CreateManyRandom(size);
            var inventory = this.containerFactory.ShuffledItemsContainer(size, stackSize, items);

            var origin = 0;
            var target = 1;
            var itemFromOrigin = inventory.GetItem(origin);
            var ItemFromTarget = inventory.GetItem(target);

            var raised = false;
            inventory.OnMove += (o, args) =>
            {
                Assert.That(args.Data, Has.Count.EqualTo(2));
                Assert.Multiple(() =>
                {
                    var firstEvent = args.Data.FirstOrDefault();
                    Assert.That(firstEvent.Item, Is.EqualTo(itemFromOrigin));
                    Assert.That(firstEvent.FromIndex, Is.EqualTo(origin));
                    Assert.That(firstEvent.ToIndex, Is.EqualTo(target));
                });

                Assert.Multiple(() =>
                {
                    var secondEvent = args.Data.Skip(1).FirstOrDefault();
                    Assert.That(secondEvent.Item, Is.EqualTo(ItemFromTarget));
                    Assert.That(secondEvent.FromIndex, Is.EqualTo(target));
                    Assert.That(secondEvent.ToIndex, Is.EqualTo(origin));
                });
                raised = true;
            };
            inventory.Move(origin, target);

            Assert.That(raised, Is.True, "OnMove event was not raised");
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
                var firstSlot = inventory.GetSlot(0);
                Assert.That(firstSlot!.GetContent(), Is.Null);
                Assert.That(firstSlot!.IsEmpty, Is.True);
            });
            Assert.Multiple(() => {
                var secondSlot = inventory.GetSlot(1);
                Assert.That(secondSlot.GetContent(), Is.EqualTo(item));
                Assert.That(secondSlot.Amount, Is.EqualTo(1));
            });
        }

        [Test]
        public void Move_EmptyTarget_CallsOnMoveEventOnlyWithOrigin()
        {
            var stackSize = this.random.Next(1, 10);
            var inventory = this.containerFactory.EmptyContainer(2, stackSize);

            var item = this.itemFactory.CreateRandom();
            var originIndex = 0;
            var targetIndex = 1;
            inventory.Add(item);

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
                });
                raised = true;
            };
            inventory.Move(originIndex, targetIndex);

            Assert.That(raised, Is.True, "OnMove event was not raised");
        }

        [Test]
        public void Move_EmptyOrigin_MovesItem()
        {
            var stackSize = this.random.Next(1, 10);
            var inventory = this.containerFactory.EmptyContainer(2, stackSize);

            var item = this.itemFactory.CreateRandom();
            var originIndex = 0;
            var targetIndex = 1;
            inventory.AddAt(item, targetIndex, 1);

            inventory.Move(originIndex, targetIndex);

            Assert.Multiple(() => {
                var firstSlot = inventory.GetSlot(0);

                Assert.That(firstSlot!.GetContent(), Is.EqualTo(item));
                Assert.That(firstSlot!.Amount, Is.EqualTo(1));
            });
            Assert.Multiple(() =>
            {
                var secondSlot = inventory.GetSlot(1);
              
                Assert.That(secondSlot!.GetContent(), Is.Null);
                Assert.That(secondSlot!.IsEmpty, Is.True);
            });
        }

        [Test]
        public void Move_EmptyOrigin_CallsOnMoveEventOnlyWithTarget()
        {
            var stackSize = this.random.Next(1, 10);
            var inventory = this.containerFactory.EmptyContainer(2, stackSize);

            var item = this.itemFactory.CreateRandom();
            var originIndex = 0;
            var targetIndex = 1;
            inventory.AddAt(item, targetIndex, 1);

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
