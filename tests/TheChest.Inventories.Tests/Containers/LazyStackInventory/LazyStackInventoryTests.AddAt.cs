using TheChest.Tests.Common.Extensions.Containers;
using TheChest.Tests.Common.Extensions.Slots;

namespace TheChest.Inventories.Tests.Containers.LazyStackInventory
{
    public partial class LazyStackInventoryTests<T>
    {
        [Test]
        [TheChest.Tests.Common.Attributes.IgnoreIfValueTypeAttribute]
        public void AddAt_NullItem_ThrowsArgumentNullException()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var inventory = this.inventoryFactory.EmptyContainer(size, stackSize);

            var index = this.random.Next(0, size);
            Assert.Throws<ArgumentNullException>(() => inventory.AddAt(default!, index, 1));
        }

        [TestCase(0)]
        [TestCase(-1)]
        public void AddAt_ZeroOrNegativeAmount_ThrowsArgumentOutOfRangeException(int amount)
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var inventory = this.inventoryFactory.EmptyContainer(size, stackSize);

            var index = this.random.Next(0, size);
            var item = this.itemFactory.CreateDefault();
            Assert.Throws<ArgumentOutOfRangeException>(() => inventory.AddAt(item, index, amount));
        }

        [TestCase(-1)]
        [TestCase(MAX_SIZE_TEST + 1)]
        public void AddAt_ThrowsArgumentOutOfRangeException_WhenIndexIsInvalid(int index)
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var inventory = this.inventoryFactory.EmptyContainer(size, stackSize);

            var item = this.itemFactory.CreateDefault();
            Assert.Throws<ArgumentOutOfRangeException>(() => inventory.AddAt(item, index, 1));
        }

        [Test]
        public void AddAt_AvailableSlot_AddsItemToSlot()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var inventory = this.inventoryFactory.EmptyContainer(size, stackSize);
            var item = this.itemFactory.CreateDefault();

            var randomIndex = this.random.Next(0, size);
            inventory.AddAt(item, randomIndex, stackSize);

            Assert.Multiple(() =>
            {
                var slot = inventory.GetSlot(randomIndex);
                Assert.That(slot.GetContent(), Is.EqualTo(item));
                Assert.That(slot.IsEmpty, Is.False);
                Assert.That(slot.Amount, Is.EqualTo(stackSize));
            });
        }

        [Test]
        public void AddAt_AllItemsSuccessfullyAdded_CallsOnAddEvent()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var inventory = this.inventoryFactory.EmptyContainer(size, stackSize);
            var item = this.itemFactory.CreateDefault();

            var index = this.random.Next(0, size);
            var raised = false;
            inventory.OnAdd += (sender, args) =>
            {
                Assert.That(args.Data, Has.Count.EqualTo(1));
                Assert.Multiple(() =>
                {
                    var firstEvent = args.Data.FirstOrDefault();
                    Assert.That(firstEvent.Item, Is.EqualTo(item));
                    Assert.That(firstEvent.Index, Is.EqualTo(index));
                    Assert.That(firstEvent.Amount, Is.EqualTo(stackSize));
                });
                raised = true;
            };
            inventory.AddAt(item, index, stackSize);

            Assert.That(raised, Is.True, "OnAdd event was not raised");
        }

        [Test]
        public void AddAt_AmountBiggerThanSlotSize_DoesntAddItems()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var inventory = this.inventoryFactory.EmptyContainer(size, stackSize);
            var item = this.itemFactory.CreateDefault();

            var randomIndex = this.random.Next(0, size);
            var amount = this.random.Next(1, 10) + stackSize;
            inventory.AddAt(item, randomIndex, amount);

            Assert.Multiple(() =>
            {
                var slot = inventory.GetSlot(randomIndex);
                Assert.That(slot.GetContent(), Is.Not.EqualTo(item));
                Assert.That(slot.IsEmpty, Is.True);
            });
        }

        [Test]
        public void AddAt_AmountBiggerThanSlotSize_DoesntCallsOnAddEvent()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var inventory = this.inventoryFactory.EmptyContainer(size, stackSize);
            var item = this.itemFactory.CreateDefault();

            inventory.OnAdd += (sender, args) => Assert.Fail("OnAdd event should not be called.");

            var randomIndex = this.random.Next(0, size);
            var amount = this.random.Next(1, 10) + stackSize;
            inventory.AddAt(item, randomIndex, amount);
        }

        [Test]
        public void AddAt_SlotWithDifferentItem_DoesNotAddItems()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var randomItems = this.itemFactory.CreateManyRandom(size);
            var inventory = this.inventoryFactory.ShuffledItemsContainer(size, stackSize, randomItems);

            var item = this.itemFactory.CreateDefault();
            var randomIndex = this.random.Next(0, size);
            inventory.AddAt(item, randomIndex, stackSize);

            Assert.That(inventory.GetItems(randomIndex), Has.None.EqualTo(item));
        }

        [Test]
        public void AddAt_SlotWithDifferentItem_DoesNotCallOnAddEvent()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var randomItems = this.itemFactory.CreateManyRandom(size);
            var inventory = this.inventoryFactory.ShuffledItemsContainer(size, stackSize, randomItems);

            inventory.OnAdd += (sender, args) => Assert.Fail("OnAdd event should not be called when adding to a slot with a different item.");
            
            var item = this.itemFactory.CreateDefault();
            var randomIndex = this.random.Next(0, size);
            inventory.AddAt(item, randomIndex, stackSize);
        }
    }
}
