namespace TheChest.Inventories.Tests.Containers
{
    public partial class StackInventoryTests<T>
    {
        [TestCase(-1)]
        [TestCase(100)]
        public void Move_InvalidOriginIndex_ThrowsArgumentOutOfRangeException(int origin)
        {
            var inventory = this.containerFactory.EmptyContainer();
            var wasRaised = false;
            inventory.OnMove += (sender, args) => wasRaised = true;
            Assert.Multiple(() =>
            {
                Assert.That(
                    () => inventory.Move(origin, 0),
                    Throws.TypeOf<ArgumentOutOfRangeException>()
                );
                Assert.That(
                    wasRaised, Is.False, 
                    "OnMove event should not be raised on exception."
                );
            });
        }
        [TestCase(-1)]
        [TestCase(100)]
        public void Move_InvalidTargetIndex_ThrowsArgumentOutOfRangeException(int target)
        {
            var inventory = this.containerFactory.EmptyContainer();

            var wasRaised = false;
            inventory.OnMove += (sender, args) => wasRaised = true;
            Assert.Multiple(() =>
            {
                Assert.That(
                    () => inventory.Move(0, target),
                    Throws.TypeOf<ArgumentOutOfRangeException>()
                );
                Assert.That(
                    wasRaised, Is.False, 
                    "OnMove event should not be raised on exception."
                );
            });
        }

        [Test]
        public void Move_EmptyOriginAndTarget_DoesNotCallOnMoveEvent()
        {
            var inventorySize = this.random.Next(10, 20);
            var stackSize = this.random.Next(1, 20);
            var originIndex = this.random.Next(5, inventorySize - 1);
            var targetIndex = this.random.Next(0, originIndex - 1);
            var inventory = this.containerFactory.EmptyContainer(inventorySize, stackSize);
            inventory.GetAll(targetIndex);
            inventory.GetAll(originIndex);

            inventory.OnMove += (sender, args) => Assert.Fail("OnMove event should not be raised on exception.");
            inventory.Move(originIndex, targetIndex);
        }
        [Test]
        public void Move_SameOriginAndTarget_DoesNotCallOnMoveEvent()
        {
            var inventorySize = this.random.Next(10, 20);
            var stackSize = this.random.Next(1, 20);
            var originIndex = this.random.Next(0, inventorySize - 1);
            var inventory = this.containerFactory.EmptyContainer(inventorySize, stackSize);

            inventory.OnMove += (sender, args) => Assert.Fail("OnMove event should not be raised on exception.");
            inventory.Move(originIndex, originIndex);
        }

        [Test]
        public void Move_EmptyOrigin_TargetWithItems_MovesItem()
        {
            var inventorySize = this.random.Next(10, 20);
            var stackSize = this.random.Next(1, 20);
            var slotItem = this.itemFactory.CreateDefault();

            var originIndex = this.random.Next(5, inventorySize - 1);
            var targetIndex = this.random.Next(0, originIndex - 1);

            var inventory = this.containerFactory.FullContainer(inventorySize, stackSize, slotItem);
            inventory.GetAll(originIndex);
            var targetItems = inventory.GetItems(targetIndex);

            inventory.Move(originIndex, targetIndex);

            Assert.Multiple(() =>
            {
                Assert.That(inventory.GetItems(originIndex), Is.EqualTo(targetItems));
                Assert.That(inventory.GetSlot(targetIndex)?.IsEmpty, Is.True);
            });
        }
        [Test]
        public void Move_EmptyOrigin_TargetWithItems_CallsOnMoveEvent()
        {
            var inventorySize = this.random.Next(10, 20);
            var stackSize = this.random.Next(1, 20);
            var slotItem = this.itemFactory.CreateDefault();

            var originIndex = this.random.Next(5, inventorySize - 1);
            var targetIndex = this.random.Next(0, originIndex - 1);

            var inventory = this.containerFactory.FullContainer(inventorySize, stackSize, slotItem);
            inventory.GetAll(originIndex);
            var targetItems = inventory.GetItems(targetIndex);

            var wasRaised = false;
            inventory.OnMove += (sender, args) =>
            {
                wasRaised = true;
                Assert.That(sender, Is.EqualTo(inventory));

                var dataArray = args.Data.ToArray();
                Assert.That(dataArray, Has.Length.EqualTo(1));
                Assert.Multiple(() =>
                {
                    Assert.That(dataArray[0].Items, Is.EqualTo(targetItems));
                    Assert.That(dataArray[0].FromIndex, Is.EqualTo(targetIndex));
                    Assert.That(dataArray[0].ToIndex, Is.EqualTo(originIndex));
                });
            };
            inventory.Move(originIndex, targetIndex);
            Assert.That(wasRaised, "Expected OnMove event to be raised.");
        }

        [Test]
        public void Move_OriginWithItems_EmptyTarget_MovesItem()
        {
            var inventorySize = this.random.Next(10, 20);
            var stackSize = this.random.Next(1, 20);
            var slotItems = this.itemFactory.CreateMany(inventorySize / 2);
            var randomItems = this.itemFactory.CreateManyRandom(inventorySize / 2);
            var inventoryItems = slotItems.Concat(randomItems).ToArray();

            var originIndex = this.random.Next(5, inventorySize - 1);
            var targetIndex = this.random.Next(0, originIndex - 1);

            var inventory = this.containerFactory.ShuffledItemsContainer(inventorySize, stackSize, inventoryItems);
            inventory.GetAll(targetIndex);
            var originItems = inventory.GetItems(originIndex);

            inventory.Move(originIndex, targetIndex);

            Assert.Multiple(() =>
            {
                Assert.That(inventory.GetSlot(originIndex)?.IsEmpty, Is.True);
                Assert.That(inventory.GetItems(targetIndex), Is.EqualTo(originItems));
            });
        }
        [Test]
        public void Move_OriginWithItems_EmptyTarget_CallsOnMoveEvent()
        {
            var inventorySize = this.random.Next(10, 20);
            var stackSize = this.random.Next(1, 20);
            var randomItem = this.itemFactory.CreateRandom();

            var originIndex = this.random.Next(5, inventorySize - 1);
            var targetIndex = this.random.Next(0, originIndex - 1);

            var inventory = this.containerFactory.FullContainer(inventorySize, stackSize, randomItem);
            inventory.GetAll(targetIndex);
            var originItems = inventory.GetItems(originIndex);

            var wasRaised = false;
            inventory.OnMove += (sender, args) =>
            {
                wasRaised = true;
                Assert.That(sender, Is.EqualTo(inventory));

                var dataArray = args.Data.ToArray();
                Assert.That(dataArray, Has.Length.EqualTo(1));
                Assert.Multiple(() =>
                {
                    Assert.That(dataArray[0].Items, Is.EqualTo(originItems));
                    Assert.That(dataArray[0].FromIndex, Is.EqualTo(originIndex));
                    Assert.That(dataArray[0].ToIndex, Is.EqualTo(targetIndex));
                });
            };
            inventory.Move(originIndex, targetIndex);
            Assert.That(wasRaised, "Expected OnMove event to be raised.");
        }

        [Test]
        public void Move_OriginAndTargetWithSameItems_MovesItems()
        {
            var inventorySize = this.random.Next(10, 20);
            var stackSize = this.random.Next(5, 20);
            var slotItem = this.itemFactory.CreateDefault();

            var originIndex = this.random.Next(5, inventorySize - 1);
            var targetIndex = this.random.Next(0, originIndex - 1);

            var inventory = this.containerFactory.FullContainer(inventorySize, stackSize, slotItem);
            inventory.Get(originIndex, random.Next(1, stackSize - 2));
            var originItems = inventory.GetItems(originIndex);
            var targetItems = inventory.GetItems(targetIndex);

            inventory.Move(originIndex, targetIndex);

            Assert.Multiple(() =>
            {
                Assert.That(inventory.GetItems(originIndex), Is.EqualTo(targetItems));
                Assert.That(inventory.GetItems(targetIndex), Is.EqualTo(originItems));
            });
        }
        [Test]
        public void Move_OriginAndTargetWithSameItems_CallsOnMoveEvent()
        {
            var inventorySize = this.random.Next(10, 20);
            var stackSize = this.random.Next(2, 20);
            var slotItem = this.itemFactory.CreateDefault();

            var originIndex = this.random.Next(5, inventorySize - 1);
            var targetIndex = this.random.Next(0, originIndex - 1);

            var inventory = this.containerFactory.FullContainer(inventorySize, stackSize, slotItem);
            inventory.Get(originIndex, random.Next(1, stackSize - 1));
            var originItems = inventory.GetItems(originIndex);
            var targetItems = inventory.GetItems(targetIndex);

            var wasRaised = false;
            inventory.OnMove += (i, args) =>
            {
                wasRaised = true;
                Assert.That(i, Is.EqualTo(inventory));

                var dataArray = args.Data.ToArray();
                Assert.That(dataArray, Has.Length.EqualTo(2));
                Assert.Multiple(() =>
                {
                    Assert.That(dataArray[0].Items, Is.EqualTo(originItems));
                    Assert.That(dataArray[0].FromIndex, Is.EqualTo(originIndex));
                    Assert.That(dataArray[0].ToIndex, Is.EqualTo(targetIndex));
                });
                Assert.Multiple(() =>
                {
                    Assert.That(dataArray[1].Items, Is.EqualTo(targetItems));
                    Assert.That(dataArray[1].FromIndex, Is.EqualTo(targetIndex));
                    Assert.That(dataArray[1].ToIndex, Is.EqualTo(originIndex));
                });
            };
            inventory.Move(originIndex, targetIndex);
            Assert.That(wasRaised, "Expected OnMove event to be raised.");
        }

        [Test]
        public void Move_OriginAndTargetWithDifferentItems_MovesItemToOrigin()
        {
            var inventorySize = this.random.Next(10, 20);
            var stackSize = this.random.Next(1, 20);
            var slotItems = this.itemFactory.CreateMany(inventorySize / 2);
            var randomItems = this.itemFactory.CreateManyRandom(inventorySize / 2);
            var inventoryItems = slotItems.Concat(randomItems).ToArray();

            var inventory = this.containerFactory.ShuffledItemsContainer(inventorySize, stackSize, inventoryItems);

            var originIndex = this.random.Next(5, inventorySize - 1);
            var originItems = inventory.GetItems(originIndex);

            var targetIndex = this.random.Next(0, originIndex - 1);
            var targetItems = inventory.GetItems(targetIndex);

            inventory.Move(originIndex, targetIndex);

            Assert.Multiple(() =>
            {
                Assert.That(inventory[originIndex].GetContents(), Is.EqualTo(targetItems));
                Assert.That(inventory[targetIndex].GetContents(), Is.EqualTo(originItems));
            });
        }
        [Test]
        public void Move_OriginAndTargetWithDifferentItems_CallsOnMoveEvent()
        {
            var inventorySize = this.random.Next(10, 20);
            var stackSize = this.random.Next(1, 20);
            var randomItems = this.itemFactory.CreateManyRandom(inventorySize);

            var originIndex = this.random.Next(5, inventorySize - 1);
            var targetIndex = this.random.Next(0, originIndex - 1);

            var inventory = this.containerFactory.ShuffledItemsContainer(inventorySize, stackSize, randomItems);
            var originItems = inventory.GetItems(originIndex);
            var targetItems = inventory.GetItems(targetIndex);
            inventory.OnMove += (o, e) =>
            {
                Assert.That(o, Is.EqualTo(inventory));
                Assert.Multiple(() =>
                {
                    var firstEvent = e.Data.First();
                    Assert.That(firstEvent.Items, Is.EqualTo(originItems));
                    Assert.That(firstEvent.FromIndex, Is.EqualTo(originIndex));
                    Assert.That(firstEvent.ToIndex, Is.EqualTo(targetIndex));
                });
                Assert.Multiple(() =>
                {
                    var secondEvent = e.Data.Skip(1).First();
                    Assert.That(secondEvent.Items, Is.EqualTo(targetItems));
                    Assert.That(secondEvent.FromIndex, Is.EqualTo(targetIndex));
                    Assert.That(secondEvent.ToIndex, Is.EqualTo(originIndex));
                });
            };
            inventory.Move(originIndex, targetIndex);
        }
    }
}
