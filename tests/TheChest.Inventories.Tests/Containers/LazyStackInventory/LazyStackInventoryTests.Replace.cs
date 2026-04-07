using TheChest.Tests.Common.Extensions.Containers;
using TheChest.Tests.Common.Extensions.Slots;

namespace TheChest.Inventories.Tests.Containers.LazyStackInventory
{
    public partial class LazyStackInventoryTests<T>
    {
        [TestCase(-1, 1)]
        [TestCase(MAX_SIZE_TEST + 1, 1)]
        public void Replace_InvalidIndex_ThrowsArgumentOutOfRangeException(int index, int amount)
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var item = this.itemFactory.CreateDefault();
            var inventory = this.inventoryFactory.FullContainer(size, stackSize, item);

            Assert.That(
                () => inventory.Replace(item, index, amount),
                Throws.TypeOf<ArgumentOutOfRangeException>().And.With.Message.Contains("index")
            );
        }

        [TestCase(1, 0)]
        [TestCase(0, -1)]
        public void Replace_InvalidAmount_ThrowsArgumentOutOfRangeException(int index, int amount)
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var item = this.itemFactory.CreateDefault();
            var inventory = this.inventoryFactory.FullContainer(size, stackSize, item);

            Assert.That(
                () => inventory.Replace(item, index, amount),
                Throws.TypeOf<ArgumentOutOfRangeException>().And.With.Message.Contains("amount")
            );
        }

        [Test]
        public void Replace_InvalidAmount_ThrowsArgumentOutOfRangeException()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var item = this.itemFactory.CreateDefault();
            var inventory = this.inventoryFactory.FullContainer(size, stackSize, item);

            var randomIndex = this.random.Next(0, size - 1);
            var randonAmount = this.random.Next(1, stackSize);
            Assert.That(
                () => inventory.Replace(default! , randomIndex, randonAmount),
                Throws.TypeOf<ArgumentNullException>().And.With.Message.Contains("item")
            );
        }

        [Test]
        public void Replace_MoreItemsThanStackSize_ThrowsArgumentOutOfRangeException()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var oldItem = this.itemFactory.CreateDefault();
            var inventory = this.inventoryFactory.FullContainer(size, stackSize, oldItem);

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
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var oldItem = this.itemFactory.CreateDefault();
            var inventory = this.inventoryFactory.FullContainer(size, stackSize, oldItem);

            var newItem = this.itemFactory.CreateRandom();
            var index = this.random.Next(0, size - 1);
            inventory.Replace(newItem, index, stackSize);

            Assert.That(inventory.GetSlots()![index].GetContent(), Is.EqualTo(newItem));
        }

        [Test]
        public void Replace_SlotWithItems_CallsOnReplaceEventWithItems()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var oldItem = this.itemFactory.CreateDefault();
            var inventory = this.inventoryFactory.FullContainer(size, stackSize, oldItem);

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
        public void Replace_Empty_AddsItemToSlot()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var inventory = this.inventoryFactory.EmptyContainer(size, stackSize);

            var newItem = this.itemFactory.CreateRandom();
            var index = this.random.Next(0, size - 1);
            inventory.Replace(newItem, index, stackSize);

            Assert.That(inventory.GetSlots()![index].GetContent(), Is.EqualTo(newItem));
        }

        [Test]
        [TheChest.Tests.Common.Attributes.IgnoreIfValueTypeAttribute]
        public void Replace_EmptySlot_CallsOnReplaceEventWithNullOldItem()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var inventory = this.inventoryFactory.EmptyContainer(size, stackSize);

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
