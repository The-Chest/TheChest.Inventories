namespace TheChest.Inventories.Tests.Containers.LazyStackInventory
{
    public partial class LazyStackInventoryTests<T>
    {
        [TestCase(-1, 1)]
        [TestCase(2000, 1)]
        public void Replace_InvalidIndex_ThrowsArgumentOutOfRangeException(int index, int amount)
        {
            var item = this.itemFactory.CreateDefault();
            var inventory = this.containerFactory.FullContainer(20, 1, item);

            Assert.That(
                () => inventory.Replace(item, index, amount),
                Throws.TypeOf<ArgumentOutOfRangeException>().And.With.Message.Contains("index")
            );
        }

        [TestCase(1, 0)]
        [TestCase(0, -1)]
        public void Replace_InvalidAmount_ThrowsArgumentOutOfRangeException(int index, int amount)
        {
            var item = this.itemFactory.CreateDefault();
            var inventory = this.containerFactory.FullContainer(20, 1, item);

            Assert.That(
                () => inventory.Replace(item, index, amount),
                Throws.TypeOf<ArgumentOutOfRangeException>().And.With.Message.Contains("amount")
            );
        }

        [Test]
        public void Replace_InvalidAmount_ThrowsArgumentOutOfRangeException()
        {
            var item = this.itemFactory.CreateDefault();
            var inventory = this.containerFactory.FullContainer(20, 1, item);

            Assert.That(
                () => inventory.Replace(default! , 1, 1),
                Throws.TypeOf<ArgumentNullException>().And.With.Message.Contains("item")
            );
        }

        [Test]
        public void Replace_MoreItemsThanStackSize_ThrowsArgumentOutOfRangeException()
        {
            var oldItem = this.itemFactory.CreateDefault();
            var size = this.random.Next(2, 20);
            var stackSize = this.random.Next(1, 10);
            var inventory = this.containerFactory.FullContainer(size, stackSize, oldItem);

            var newItem = this.itemFactory.CreateRandom();
            var index = this.random.Next(0, size - 1);

            Assert.That(() =>
                inventory.Replace(newItem, index, stackSize + 1),
                Throws.InvalidOperationException
                    .With.Message.EqualTo("The amount of items to replace exceeds the stack size of the slot.")
            );
        }

        [Test]
        public void Replace_SlotWithItems_ReplacesItemOnSlot()
        {
            var oldItem = this.itemFactory.CreateDefault();
            var size = this.random.Next(2, 20);
            var stackSize = this.random.Next(1, 10);
            var inventory = this.containerFactory.FullContainer(size, stackSize, oldItem);

            var newItem = this.itemFactory.CreateRandom();
            var index = this.random.Next(0, size - 1);
            inventory.Replace(newItem, index, stackSize);

            Assert.That(inventory.GetSlots()![index].GetContent(), Is.EqualTo(newItem));
        }

        [Test]
        public void Replace_SlotWithItems_CallsOnReplaceEventWithItems()
        {
            var oldItem = this.itemFactory.CreateDefault();
            var size = this.random.Next(2, 20);
            var stackSize = this.random.Next(1, 10);
            var inventory = this.containerFactory.FullContainer(size, stackSize, oldItem);

            var newItem = this.itemFactory.CreateRandom();
            var index = this.random.Next(0, size - 1);

            var raised = false;
            inventory.OnReplace += (sender, args) =>
            {
                Assert.That(args.Data, Has.Count.EqualTo(1));
                var eventData = args.Data.First();
                Assert.Multiple(() =>
                {
                    Assert.That(eventData.Index, Is.EqualTo(index));

                    Assert.That(eventData.NewItem, Is.EqualTo(newItem));
                    Assert.That(eventData.NewAmount, Is.EqualTo(stackSize));

                    Assert.That(eventData.OldItem, Is.EqualTo(oldItem));
                    Assert.That(eventData.OldAmount, Is.EqualTo(stackSize));
                });
                raised = true;
            };
            inventory.Replace(newItem, index, stackSize);

            Assert.That(raised, Is.True, "OnReplace event was not raised");
        }

        [Test]
        public void Replace_SlotWithItems_ReturnsItemFromSlot()
        {
            var oldItem = this.itemFactory.CreateDefault();
            var size = this.random.Next(2, 20);
            var stackSize = this.random.Next(1, 10);
            var inventory = this.containerFactory.FullContainer(size, stackSize, oldItem);

            var newItem = this.itemFactory.CreateRandom();
            var index = this.random.Next(0, size - 1);
            var oldItems = inventory.Replace(newItem, index, stackSize);

            Assert.That(oldItems, Has.Length.EqualTo(stackSize).And.All.EqualTo(oldItem));
        }

        [Test]
        public void Replace_Empty_AddsItemToSlot()
        {
            var size = this.random.Next(2, 20);
            var stackSize = this.random.Next(1, 10);
            var inventory = this.containerFactory.EmptyContainer(size, stackSize);

            var newItem = this.itemFactory.CreateRandom();
            var index = this.random.Next(0, size - 1);
            inventory.Replace(newItem, index, stackSize);

            Assert.That(inventory.GetSlots()![index].GetContent(), Is.EqualTo(newItem));
        }

        [Test]
        public void Replace_EmptySlot_ReturnsEmptyArray()
        {
            var size = this.random.Next(2, 20);
            var stackSize = this.random.Next(1, 10);
            var inventory = this.containerFactory.EmptyContainer(size, stackSize);

            var newItem = this.itemFactory.CreateRandom();
            var index = this.random.Next(0, size - 1);
            var oldItems = inventory.Replace(newItem, index, stackSize);

            Assert.That(oldItems, Is.Empty);
        }

        [Test]
        public void Replace_EmptySlot_CallsOnReplaceEventWithNullOldItem()
        {
            var size = this.random.Next(2, 20);
            var stackSize = this.random.Next(1, 10);
            var inventory = this.containerFactory.EmptyContainer(size, stackSize);

            var newItem = this.itemFactory.CreateRandom();
            var index = this.random.Next(0, size - 1);

            var raised = false;
            inventory.OnReplace += (sender, args) =>
            {
                Assert.That(args.Data, Has.Count.EqualTo(1));
                var eventData = args.Data.First();
                Assert.Multiple(() =>
                {
                    Assert.That(eventData.Index, Is.EqualTo(index));

                    Assert.That(eventData.NewItem, Is.EqualTo(newItem));
                    Assert.That(eventData.NewAmount, Is.EqualTo(stackSize));

                    Assert.That(eventData.OldItem, Is.Null);
                    Assert.That(eventData.OldAmount, Is.Zero);
                });
                raised = true;
            };

            inventory.Replace(newItem, index, stackSize);

            Assert.That(raised, Is.True, "OnReplace event was not raised");
        }
    }
}
