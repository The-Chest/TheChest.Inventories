using TheChest.Tests.Common.Extensions.Containers;

namespace TheChest.Inventories.Tests.Containers.StackInventory
{
    public partial class StackInventoryTests<T>
    {
        [TestCase(-1)]
        [TestCase(MAX_SIZE_TEST)]
        public void Move_InvalidOriginIndex_ThrowsArgumentOutOfRangeException(int origin)
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var inventory = this.inventoryFactory.EmptyContainer(size, stackSize);

            inventory.OnMove += (sender, args) => Assert.Fail("OnMove event should not be raised on exception.");

            Assert.That(
                () => inventory.Move(origin, 0), 
                Throws.TypeOf<ArgumentOutOfRangeException>().With.Property("ParamName").EqualTo("origin")
            );
        }

        [Test]
        public void Move_OriginEqualToSize_ThrowsArgumentOutOfRangeException()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var inventory = this.inventoryFactory.EmptyContainer(size, stackSize);

            inventory.OnMove += (sender, args) => Assert.Fail("OnMove event should not be raised on exception.");
            
            Assert.That(
                () => inventory.Move(size, 0), 
                Throws.TypeOf<ArgumentOutOfRangeException>().With.Property("ParamName").EqualTo("origin")
            );
        }

        [TestCase(-1)]
        [TestCase(MAX_SIZE_TEST)]
        public void Move_InvalidTargetIndex_ThrowsArgumentOutOfRangeException(int target)
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var inventory = this.inventoryFactory.EmptyContainer(size, stackSize);

            inventory.OnMove += (sender, args) => Assert.Fail("OnMove event should not be raised on exception.");

            Assert.That(
                () => inventory.Move(0, target), 
                Throws.TypeOf<ArgumentOutOfRangeException>().With.Property("ParamName").EqualTo("target")
            );
        }

        [Test]
        public void Move_TargetEqualToSize_ThrowsArgumentOutOfRangeException()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var inventory = this.inventoryFactory.EmptyContainer(size, stackSize);

            inventory.OnMove += (sender, args) => Assert.Fail("OnMove event should not be raised on exception.");

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

            var originIndex = this.random.Next(size / 2, size - 1);
            var targetIndex = this.random.Next(0, originIndex - 1);
            Assert.That(
                () => inventory.Move(originIndex, targetIndex), 
                Throws.InvalidOperationException.With.Message.EqualTo("Cannot move empty slots.")
            );
        }

        [Test]
        public void Move_SameOriginAndTarget_ThrowsArgumentException()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var inventory = this.inventoryFactory.EmptyContainer(size, stackSize);
            
            var originIndex = this.random.Next(0, size - 1);
            var targetIndex = originIndex;
            Assert.That(
                () => inventory.Move(originIndex, targetIndex), 
                Throws.ArgumentException
                    .With.Property("ParamName").EqualTo("target").And
                    .Message.Contain("Cannot move an item to the same index.")
            );
        }

        [Test]
        [Ignore("This test is only relevant for inventories that allow different max stack sizes per slot.")]
        public void Move_OriginAndTargetWithDifferentMaxStackSize_ThrowsInvalidOperationException()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var inventory = this.inventoryFactory.EmptyContainer(size, stackSize);
            var originIndex = this.random.Next(size / 2, size - 1);
            var targetIndex = this.random.Next(0, originIndex - 1);
            Assert.That(
                () => inventory.Move(originIndex, targetIndex), 
                Throws.InvalidOperationException.With.Message.EqualTo("Cannot move items between slots with different max stack sizes.")
            );
        }

        [Test]
        public void Move_EmptyOrigin_TargetWithItems_MovesItem()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var slotItem = this.itemFactory.CreateDefault();

            var originIndex = this.random.Next(size / 2, size - 1);
            var targetIndex = this.random.Next(0, originIndex - 1);

            var inventory = this.inventoryFactory.FullContainer(size, stackSize, slotItem);
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
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var slotItem = this.itemFactory.CreateDefault();

            var originIndex = this.random.Next(size / 2, size - 1);
            var targetIndex = this.random.Next(0, originIndex - 1);

            var inventory = this.inventoryFactory.FullContainer(size, stackSize, slotItem);
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
        public void Move_OriginWithItems_EmptyTarget_MovesItems()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var slotItems = this.itemFactory.CreateMany(size / 2);
            var randomItems = this.itemFactory.CreateManyRandom(size / 2);
            var inventoryItems = slotItems.Concat(randomItems).ToArray();
            var inventory = this.inventoryFactory.ShuffledItemsContainer(size, stackSize, inventoryItems);

            var originIndex = this.random.Next(size / 2, size - 1);
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
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var randomItem = this.itemFactory.CreateRandom();

            var inventory = this.inventoryFactory.FullContainer(size, stackSize, randomItem);

            var originIndex = this.random.Next(size / 2, size - 1);
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
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var slotItem = this.itemFactory.CreateDefault();

            var inventory = this.inventoryFactory.FullContainer(size, stackSize, slotItem);

            var originIndex = this.random.Next(size / 2, size - 1);
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
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var slotItem = this.itemFactory.CreateDefault();

            var inventory = this.inventoryFactory.FullContainer(size, stackSize, slotItem);

            var originIndex = this.random.Next(size / 2, size - 1);
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
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var slotItems = this.itemFactory.CreateMany(size / 2);
            var randomItems = this.itemFactory.CreateManyRandom(size / 2);
            var inventoryItems = slotItems.Concat(randomItems).ToArray();

            var inventory = this.inventoryFactory.ShuffledItemsContainer(size, stackSize, inventoryItems);

            var originIndex = this.random.Next(size / 2, size - 1);
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
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var randomItems = this.itemFactory.CreateManyRandom(size);

            var inventory = this.inventoryFactory.ShuffledItemsContainer(size, stackSize, randomItems);

            var originIndex = this.random.Next(size / 2, size - 1);
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
