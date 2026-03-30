using TheChest.Inventories.Tests.Common.Extensions.Containers;
using TheChest.Tests.Common.Extensions;

namespace TheChest.Inventories.Tests.Containers.Interfaces
{
    public partial class IStackInventoryTests<T>
    {
        [Test]
        public void AddItemsAt_EmptySlot_ReturnsEmpty()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var stackSize = this.random.Next(1, 10);
            var inventory = this.inventoryFactory.EmptyContainer(size, stackSize);

            var items = this.itemFactory.CreateMany(10);
            var index = this.random.Next(0, size);
            var result = inventory.AddAt(items, index);

            Assert.That(result, Is.Empty);
        }

        [Test]
        public void AddItemsAt_SlotWithDifferentItem_ReturnsItemsFromParams()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var stackSize = this.random.Next(1, 10);
            var inventoryItem = this.itemFactory.CreateRandom();
            var inventory = this.inventoryFactory.FullContainer(size, stackSize, inventoryItem);

            var amount = stackSize;
            var items = this.itemFactory.CreateMany(amount);
            var index = this.random.Next(0, size); 
            var result = inventory.AddAt(items, index);

            Assert.That(result, Is.EqualTo(items));
        }

        [Test]
        public void AddItemsAt_SlotWithSameItem_AddsToStack()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var amount = this.random.Next(5, 10);
            var slotItem = this.itemFactory.CreateDefault();
            var inventory = this.inventoryFactory.FullContainer(size, amount, slotItem);

            var index = this.random.Next(0, size);
            inventory.Get(index, 2); 

            var items = this.itemFactory.CreateMany(2);
            inventory.AddAt(items, index);

            Assert.Multiple(() =>
            {
                var slot = inventory.GetSlot(index);
                Assert.That(slot!.IsFull, Is.True);
                Assert.That(slot!.Amount, Is.EqualTo(amount));
                Assert.That(slot!.GetContents()?.Reverse().Take(2), Is.EqualTo(items));
            });
        }

        [Test]
        public void AddItemsAt_FullSlotSlotWithSameItem_ReturnsNotAddedItemsFromParam()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var slotItem = this.itemFactory.CreateDefault();
            var amount = this.random.Next(1, 10);
            var inventory = this.inventoryFactory.FullContainer(size, amount, slotItem);

            var index = this.random.Next(0, size);
            var items = this.itemFactory.CreateMany(amount);
            var result = inventory.AddAt(items, index);

            Assert.That(result, Is.EqualTo(items));
        }

        [Test]
        public void AddItemsAt_FullSlotWithSameItem_DoNotAddsToStack()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var slotItem = this.itemFactory.CreateDefault();
            var amount = this.random.Next(1, 10);
            var inventory = this.inventoryFactory.FullContainer(size, amount, slotItem);

            var index = this.random.Next(0, size);
            var items = this.itemFactory.CreateMany(10);
            inventory.AddAt(items, index);

            Assert.That(inventory.GetItems(index), Has.No.AnyOf(items));
        }

        [Test]
        public void AddItemsAt_FullSlotWithSameItem_DoesNotCallOnAddEvent()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var slotItem = this.itemFactory.CreateDefault();
            var amount = this.random.Next(1, 10);
            var inventory = this.inventoryFactory.FullContainer(size, amount, slotItem);

            inventory.OnAdd += (sender, e) => Assert.Fail("OnAdd event should not be called when is not possible to Add.");
            
            var items = this.itemFactory.CreateMany(10);
            var index = this.random.Next(0, size);
            inventory.AddAt(items, index);
        }

        [Test]
        public void AddItemsAt_FullSlotWithSameItem_ReturnsNotAddedItems()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var slotItem = this.itemFactory.CreateDefault();
            var amount = this.random.Next(1, 10);
            var inventory = this.inventoryFactory.FullContainer(size, amount, slotItem);

            var items = this.itemFactory.CreateMany(10);
            var index = this.random.Next(0, size);
            var result = inventory.AddAt(items, index);

            Assert.That(result, Is.Not.Empty.And.EqualTo(items));
        }
    }
}
