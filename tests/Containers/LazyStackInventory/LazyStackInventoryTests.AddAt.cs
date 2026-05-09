using TheChest.Tests.Common.Extensions.Containers;
using TheChest.Tests.Common.Extensions.Slots;

namespace TheChest.Inventories.Tests.Containers.LazyStackInventory
{
    public partial class LazyStackInventoryTests<T>
    {
        [Test]
        public void AddAt_NullItem_ThrowsArgumentNullException()
        {
            var inventory = this.inventoryFactory.EmptyContainer();
            Assert.That(
                () => inventory.AddAt(default!, 0, 1),
                Throws.ArgumentNullException.With.Property("ParamName").EqualTo("item")
            );
        }

        [TestCase(0)]
        [TestCase(-1)]
        public void AddAt_ZeroOrLessAmount_ThrowsArgumentOutOfRangeException(int amount)
        {
            var inventory = this.inventoryFactory.EmptyContainer();
            var item = this.itemFactory.CreateDefault();

            Assert.That(
                () => inventory.AddAt(item, 0, amount),
                Throws.Exception.TypeOf<ArgumentOutOfRangeException>().With.Property("ParamName").EqualTo("amount")
            );
        }

        [Test]
        public void AddAt_AmountBiggerThanSlotSize_ThrowsArgumentOutOfRangeException()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var inventory = this.inventoryFactory.EmptyContainer(size, stackSize);

            var item = this.itemFactory.CreateDefault();
            var randomIndex = this.random.Next(0, size);
            var amount = this.random.Next(1, 10) + stackSize;

            Assert.That(
                () => inventory.AddAt(item, randomIndex, amount),
                Throws.Exception.TypeOf<ArgumentOutOfRangeException>().With.Property("ParamName").EqualTo("amount")
            );
        }

        [TestCase(-1)]
        [TestCase(MAX_SIZE_TEST + 1)]
        public void AddAt_InvalidIndex_ThrowsArgumentOutOfRangeException(int index)
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var inventory = this.inventoryFactory.EmptyContainer(size, stackSize);

            var item = this.itemFactory.CreateDefault();
            Assert.That(
                () => inventory.AddAt(item, index, 1),
                Throws.Exception.TypeOf<ArgumentOutOfRangeException>().With.Property("ParamName").EqualTo("index")
            );
        }

        [Test]
        public void AddAt_SlotWithDifferentItem_ThrowsInvalidOperationException()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var randomItems = this.itemFactory.CreateManyRandom(size);
            var inventory = this.inventoryFactory.ShuffledItemsContainer(size, stackSize, randomItems);

            var item = this.itemFactory.CreateDefault();
            var randomIndex = this.random.Next(0, size);

            Assert.That(
                () => inventory.AddAt(item, randomIndex, 1),
                Throws.Exception.TypeOf<InvalidOperationException>().With.Message.EqualTo("Cannot add items that are different from the items already in the slot")
            );
        }

        [Test]
        public void AddAt_SlotWithDifferentItem_DoesNotAddItem()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var randomItems = this.itemFactory.CreateManyRandom(size);
            var inventory = this.inventoryFactory.ShuffledItemsContainer(size, stackSize, randomItems);

            var item = this.itemFactory.CreateDefault();
            var randomIndex = this.random.Next(0, size);
            var originalSlot = inventory.GetSlot(randomIndex);
            var originalContent = originalSlot.GetContent();
            var originalAmount = originalSlot.Amount;

            Assert.That(
                () => inventory.AddAt(item, randomIndex, 1),
                Throws.Exception.TypeOf<InvalidOperationException>()
            );

            Assert.Multiple(() =>
            {
                var slot = inventory.GetSlot(randomIndex);
                Assert.That(slot.GetContent(), Is.EqualTo(originalContent));
                Assert.That(slot.Amount, Is.EqualTo(originalAmount));
            });
        }

        [Test]
        public void AddAt_SlotWithDifferentItem_DoesNotCallOnAddEvent()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var randomItems = this.itemFactory.CreateManyRandom(size);
            var inventory = this.inventoryFactory.ShuffledItemsContainer(size, stackSize, randomItems);

            inventory.OnAdd += (sender, args) => Assert.Fail("OnAdd should not be called when adding an item to a slot with a different item.");

            var item = this.itemFactory.CreateDefault();
            var randomIndex = this.random.Next(0, size);
            Assert.That(
                () => inventory.AddAt(item, randomIndex, 1),
                Throws.Exception.TypeOf<InvalidOperationException>()
            );
        }

        [Test]
        public void AddAt_AmountBiggerThanAvailableAmount_ThrowsInvalidOperationException()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var item = this.itemFactory.CreateDefault();
            var inventory = this.inventoryFactory.ShuffledItemsContainer(size, stackSize, item);

            var index = Array.FindIndex(inventory.GetSlots(), slot => item!.Equals(slot.GetContent()));
            var amount = stackSize;

            Assert.That(
                () => inventory.AddAt(item, index, amount),
                Throws.Exception.TypeOf<InvalidOperationException>().With.Message.EqualTo("Cannot add more items than the available amount")
            );
        }

        [Test]
        public void AddAt_AmountBiggerThanAvailableAmount_DoesNotAddItem()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var item = this.itemFactory.CreateDefault();
            var inventory = this.inventoryFactory.ShuffledItemsContainer(size, stackSize, item);
            
            var randomIndex = Array.FindIndex(
                inventory.GetSlots(), 
                slot => item!.Equals(slot.GetContent())
            );
            var originalSlot = inventory.GetSlot(randomIndex);
            var originalAmount = originalSlot.Amount;
            var amount = stackSize;
            
            Assert.That(
                () => inventory.AddAt(item, randomIndex, amount),
                Throws.Exception.TypeOf<InvalidOperationException>()
            );

            Assert.Multiple(() =>
            {
                var slot = inventory.GetSlot(randomIndex);
                Assert.That(slot.Amount, Is.EqualTo(originalAmount));
            });
        }

        [Test]
        public void AddAt_AmountBiggerThanAvailableAmount_DoesNotCallOnAddEvent()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var item = this.itemFactory.CreateDefault();
            var inventory = this.inventoryFactory.ShuffledItemsContainer(size, stackSize, item);
            inventory.OnAdd += (sender, args) => Assert.Fail("OnAdd should not be called when adding more items than the available amount.");
            
            var randomIndex = Array.FindIndex(
                inventory.GetSlots(), 
                slot => item!.Equals(slot.GetContent())
            );
            var amount = stackSize;
            Assert.That(
                () => inventory.AddAt(item, randomIndex, amount),
                Throws.Exception.TypeOf<InvalidOperationException>()
            );
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
    }
}
