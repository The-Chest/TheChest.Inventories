using TheChest.Tests.Common.Extensions.Containers;

namespace TheChest.Inventories.Tests.Containers.StackInventory
{
    public partial class StackInventoryTests<T>
    {
        [TestCase(-1)]
        [TestCase(MAX_SIZE_TEST + 1)]
        public void Move_InvalidOriginIndex_ThrowsArgumentOutOfRangeException(int origin)
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var inventory = this.inventoryFactory.EmptyContainer(size);

            inventory.OnMove += (sender, args) => Assert.Fail("OnMove event should not be raised on exception.");

            Assert.That(() => inventory.Move(origin, 0), Throws.TypeOf<ArgumentOutOfRangeException>());
        }

        [TestCase(-1)]
        [TestCase(MAX_SIZE_TEST + 1)]
        public void Move_InvalidTargetIndex_ThrowsArgumentOutOfRangeException(int target)
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var inventory = this.inventoryFactory.EmptyContainer(size);

            inventory.OnMove += (sender, args) => Assert.Fail("OnMove event should not be raised on exception.");

            Assert.That(() => inventory.Move(0, target), Throws.TypeOf<ArgumentOutOfRangeException>());
        }

        [Test]
        public void Move_EmptyOriginAndTarget_DoesNotCallOnMoveEvent()
        {
            var inventorySize = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var inventory = this.inventoryFactory.EmptyContainer(inventorySize, stackSize);

            var originIndex = this.random.Next(inventorySize / 2, inventorySize - 1);
            var targetIndex = this.random.Next(0, originIndex - 1);
            inventory.GetAll(targetIndex);
            inventory.GetAll(originIndex);

            inventory.OnMove += (sender, args) => Assert.Fail("OnMove event should not be raised on exception.");

            inventory.Move(originIndex, targetIndex);
        }

        [Test]
        public void Move_SameOriginAndTarget_DoesNotCallOnMoveEvent()
        {
            var inventorySize = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var inventory = this.inventoryFactory.EmptyContainer(inventorySize, stackSize);

            inventory.OnMove += (sender, args) => Assert.Fail("OnMove event should not be raised on exception.");
            
            var originIndex = this.random.Next(0, inventorySize - 1);
            inventory.Move(originIndex, originIndex);
        }

        [Test]
        public void Move_EmptyOrigin_TargetWithItems_MovesItem()
        {
            var inventorySize = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var slotItem = this.itemFactory.CreateDefault();

            var originIndex = this.random.Next(inventorySize / 2, inventorySize - 1);
            var targetIndex = this.random.Next(0, originIndex - 1);

            var inventory = this.inventoryFactory.FullContainer(inventorySize, stackSize, slotItem);
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
            var inventorySize = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var slotItem = this.itemFactory.CreateDefault();

            var originIndex = this.random.Next(inventorySize / 2, inventorySize - 1);
            var targetIndex = this.random.Next(0, originIndex - 1);

            var inventory = this.inventoryFactory.FullContainer(inventorySize, stackSize, slotItem);
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
            var inventorySize = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var slotItems = this.itemFactory.CreateMany(inventorySize / 2);
            var randomItems = this.itemFactory.CreateManyRandom(inventorySize / 2);
            var inventoryItems = slotItems.Concat(randomItems).ToArray();
            var inventory = this.inventoryFactory.ShuffledItemsContainer(inventorySize, stackSize, inventoryItems);

            var originIndex = this.random.Next(inventorySize / 2, inventorySize - 1);
            var targetIndex = this.random.Next(0, originIndex - 1);
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
            var inventorySize = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var randomItem = this.itemFactory.CreateRandom();

            var inventory = this.inventoryFactory.FullContainer(inventorySize, stackSize, randomItem);

            var originIndex = this.random.Next(inventorySize / 2, inventorySize - 1);
            var targetIndex = this.random.Next(0, originIndex - 1);
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
            var inventorySize = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var slotItem = this.itemFactory.CreateDefault();

            var inventory = this.inventoryFactory.FullContainer(inventorySize, stackSize, slotItem);

            var originIndex = this.random.Next(inventorySize / 2, inventorySize - 1);
            var targetIndex = this.random.Next(0, originIndex - 1);
            inventory.Get(originIndex, random.Next(1, stackSize - 1));
            var originItems = inventory.GetItems(originIndex);
            var targetItems = inventory.GetItems(targetIndex);

            inventory.Move(originIndex, targetIndex);

            Assert.Multiple(() =>
            {
                Assert.That(inventory.GetItems(originIndex), Is.EquivalentTo(targetItems));
                Assert.That(inventory.GetItems(targetIndex), Is.EquivalentTo(originItems));
            });
        }

        [Test]
        public void Move_OriginAndTargetWithSameItems_CallsOnMoveEvent()
        {
            var inventorySize = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var slotItem = this.itemFactory.CreateDefault();

            var inventory = this.inventoryFactory.FullContainer(inventorySize, stackSize, slotItem);

            var originIndex = this.random.Next(inventorySize / 2, inventorySize - 1);
            var targetIndex = this.random.Next(0, originIndex - 1);
            inventory.Get(originIndex, random.Next(1, stackSize - 1));
            var originItems = inventory.GetItems(originIndex).Where(x => x is not null);
            var targetItems = inventory.GetItems(targetIndex).Where(x => x is not null);

            var wasRaised = false;
            inventory.OnMove += (i, args) =>
            {
                wasRaised = true;
                Assert.That(i, Is.EqualTo(inventory));

                var dataArray = args.Data.ToArray();
                Assert.That(dataArray, Has.Length.EqualTo(2));
                Assert.Multiple(() =>
                {
                    Assert.That(dataArray[0].Items, Is.EquivalentTo(originItems));
                    Assert.That(dataArray[0].FromIndex, Is.EqualTo(originIndex));
                    Assert.That(dataArray[0].ToIndex, Is.EqualTo(targetIndex));
                });
                Assert.Multiple(() =>
                {
                    Assert.That(dataArray[1].Items, Is.EquivalentTo(targetItems));
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
            var inventorySize = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var slotItems = this.itemFactory.CreateMany(inventorySize / 2);
            var randomItems = this.itemFactory.CreateManyRandom(inventorySize / 2);
            var inventoryItems = slotItems.Concat(randomItems).ToArray();

            var inventory = this.inventoryFactory.ShuffledItemsContainer(inventorySize, stackSize, inventoryItems);

            var originIndex = this.random.Next(inventorySize / 2, inventorySize - 1);
            var originItems = inventory.GetItems(originIndex);

            var targetIndex = this.random.Next(0, originIndex - 1);
            var targetItems = inventory.GetItems(targetIndex);

            inventory.Move(originIndex, targetIndex);

            Assert.Multiple(() =>
            {
                Assert.That(inventory.GetItems(originIndex), Is.EqualTo(targetItems));
                Assert.That(inventory.GetItems(targetIndex), Is.EqualTo(originItems));
            });
        }

        [Test]
        public void Move_OriginAndTargetWithDifferentItems_CallsOnMoveEvent()
        {
            var inventorySize = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var randomItems = this.itemFactory.CreateManyRandom(inventorySize);

            var inventory = this.inventoryFactory.ShuffledItemsContainer(inventorySize, stackSize, randomItems);

            var originIndex = this.random.Next(inventorySize / 2, inventorySize - 1);
            var targetIndex = this.random.Next(0, originIndex - 1);
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
