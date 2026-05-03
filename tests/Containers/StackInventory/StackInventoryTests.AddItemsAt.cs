using TheChest.Tests.Common.Extensions.Containers;
using TheChest.Tests.Common.Extensions.Slots;

namespace TheChest.Inventories.Tests.Containers.StackInventory
{
    public partial class StackInventoryTests<T>
    {
        [Test]
        public void AddItemsAt_NullItems_ThrowsArgumentNullException()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var inventory = this.inventoryFactory.EmptyContainer(size, stackSize);
            Assert.That(
                () => inventory.AddAt(null!, 0),
                Throws.ArgumentNullException.With.Property("ParamName").EqualTo("items")
            );
        }

        [TestCase(-1)]
        [TestCase(MAX_SIZE_TEST + 1)]
        public void AddItemsAt_InvalidIndex_ThrowsArgumentOutOfRangeException(int index)
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var inventory = this.inventoryFactory.EmptyContainer(size, stackSize);

            var items = this.itemFactory.CreateMany(stackSize);
            Assert.That(
                () => inventory.AddAt(items, index), 
                Throws.TypeOf<ArgumentOutOfRangeException>().With.Property("ParamName").EqualTo("index")
            );
        }

        [Test]
        public void AddItemsAt_EmptyArray_ThrowsArgumentException()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var inventory = this.inventoryFactory.EmptyContainer(size, stackSize);

            Assert.That(
                () => inventory.AddAt(Array.Empty<T>(), 0), 
                Throws.ArgumentException.With.Property("ParamName").EqualTo("items").And.Message.Contains("Cannot add using an empty item array")
            );
        }

        [Test]
        public void AddItemsAt_ItemsContainsNull_ThrowsArgumentNullException()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var inventory = this.inventoryFactory.EmptyContainer(size, stackSize);

            var validItem = this.itemFactory.CreateDefault();
            var items = new T[] { validItem, default!, validItem };
            Assert.That(
                () => inventory.AddAt(items, 0), 
                Throws.ArgumentNullException.With.Property("ParamName").EqualTo("items").And.Message.Contains("One of the items to add is null")
            );
        }

        [Test]
        public void AddItemsAt_ItemsAreNotAllEqual_ThrowsArgumentException()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var inventory = this.inventoryFactory.EmptyContainer(size, stackSize);

            var first = this.itemFactory.CreateDefault();
            var second = this.itemFactory.CreateRandom();
            var items = new[] { first, second };

            Assert.That(
                () => inventory.AddAt(items, 0), 
                Throws.ArgumentException.With.Property("ParamName").EqualTo("items").And.Message.Contains("Cannot add an array of items with different types")
            );
        }

        [Test]
        public void AddItemsAt_SlotWithDifferentItem_ThrowsInvalidOperationException()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var inventoryItem = this.itemFactory.CreateRandom();
            var inventory = this.inventoryFactory.FullContainer(size, stackSize, inventoryItem);

            var amount = stackSize;
            var items = this.itemFactory.CreateMany(amount);
            var index = this.random.Next(0, size);
            Assert.That(() => inventory.AddAt(items, index), Throws.InvalidOperationException);
        }

        [Test]
        public void AddItemsAt_FullSlotWithSameItem_ThrowsInvalidOperationException()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var slotItem = this.itemFactory.CreateDefault();
            var inventory = this.inventoryFactory.FullContainer(size, stackSize, slotItem);

            var items = this.itemFactory.CreateMany(stackSize);
            var index = this.random.Next(0, size);
            Assert.That(() => inventory.AddAt(items, index), Throws.InvalidOperationException);
        }

        [Test]
        public void AddItemsAt_EmptySlot_AddsToStack()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var inventory = this.inventoryFactory.EmptyContainer(size, stackSize);

            var items = this.itemFactory.CreateMany(stackSize);
            var index = this.random.Next(0, size);
            inventory.AddAt(items, index);

            Assert.That(inventory.GetItems(index), Is.EqualTo(items));
        }

        [Test]
        public void AddItemsAt_EmptySlot_CallsOnAddEvent()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var inventory = this.inventoryFactory.EmptyContainer(size, stackSize);

            var items = this.itemFactory.CreateMany(stackSize);
            var index = this.random.Next(0, size);

            var raised = false;
            inventory.OnAdd += (sender, e) => {
                Assert.That(e.Data, Has.Count.EqualTo(1));
                Assert.Multiple(() =>
                {
                    var firstEvent = e.Data.First();
                    Assert.That(firstEvent.Items, Is.EqualTo(items));
                    Assert.That(firstEvent.Index, Is.EqualTo(index));
                });
                raised = true;
            };

            inventory.AddAt(items, index);

            Assert.That(raised, Is.True, "OnAdd event was not raised");
        }

        [Test]
        public void AddItemsAt_SlotWithSameItem_AddsToStack()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var slotItem = this.itemFactory.CreateDefault();
            var inventory = this.inventoryFactory.FullContainer(size, stackSize, slotItem);

            var index = this.random.Next(0, size);
            var amount = stackSize - this.random.Next(2, stackSize - 1);
            inventory.Get(index, amount);

            var items = this.itemFactory.CreateMany(amount);
            inventory.AddAt(items, index);

            Assert.Multiple(() =>
            {
                var slot = inventory.GetSlot(index);
                Assert.That(slot!.IsFull, Is.True);
                Assert.That(slot!.Amount, Is.EqualTo(stackSize));
                Assert.That(slot!.GetContents()?.Reverse().Take(amount), Is.EqualTo(items));
            });
        }

        [Test]
        public void AddItemsAt_SlotWithSameItem_CallsOnAddEvent()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var slotItem = this.itemFactory.CreateDefault();
            var inventory = this.inventoryFactory.FullContainer(size, stackSize, slotItem);
            
            var index = this.random.Next(0, size);
            var amount = stackSize - this.random.Next(2, stackSize - 1);
            inventory.Get(index, amount);

            var items = this.itemFactory.CreateMany(amount);
            var raised = false;
            inventory.OnAdd += (sender, e) => {
                Assert.That(e.Data, Has.Count.EqualTo(1));
                Assert.Multiple(() =>
                {
                    var firstEvent = e.Data.First();
                    Assert.That(firstEvent.Items, Is.EqualTo(items));
                    Assert.That(firstEvent.Index, Is.EqualTo(index));
                });
                raised = true;
            };
            inventory.AddAt(items, index);

            Assert.That(raised, Is.True, "OnAdd event was not raised");
        }
    }
}
