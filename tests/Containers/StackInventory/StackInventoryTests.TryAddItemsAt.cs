using TheChest.Inventories.Tests.Containers.Extensions;
using TheChest.Tests.Common.Extensions.Containers;

namespace TheChest.Inventories.Tests.Containers.StackInventory
{
    public partial class StackInventoryTests<T>
    {
        [Test]
        public void TryAddItemAt_NullItem_ThrowsArgumentNullException()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var inventory = this.inventoryFactory.EmptyContainer(size, stackSize);

            Assert.That(
                () => inventory.TryAddAt(default!, 0),
                Throws.ArgumentNullException.With.Property("ParamName").EqualTo("items")
            );
        }

        [TestCase(-1)]
        [TestCase(MAX_SIZE_TEST)]
        public void TryAddItemAt_InvalidIndex_ThrowsArgumentOutOfRangeException(int index)
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var inventory = this.inventoryFactory.EmptyContainer(size, stackSize);

            var amount = this.random.Next(1, stackSize);
            var items = this.itemFactory.CreateMany(amount);

            Assert.That(
                () => inventory.TryAddAt(items, index),
                Throws.TypeOf<ArgumentOutOfRangeException>().With.Property("ParamName").EqualTo("index")
            );
        }

        [Test]
        public void TryAddItemAt_ValidInput_ReturnsTrue()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var inventory = this.inventoryFactory.EmptyContainer(size, stackSize);

            var amount = this.random.Next(1, stackSize);
            var items = this.itemFactory.CreateMany(amount);
            var randomIndex = this.random.Next(0, size);
            var result = inventory.TryAddAt(items, randomIndex);

            Assert.That(result, Is.True);
        }

        [Test]
        public void TryAddItemAt_ValidInput_AddsItemsToInventory()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var inventory = this.inventoryFactory.EmptyContainer(size, stackSize);

            var amount = this.random.Next(1, stackSize);
            var items = this.itemFactory.CreateMany(amount);
            var randomIndex = this.random.Next(0, size);
            inventory.TryAddAt(items, randomIndex);

            Assert.That(inventory.GetItems(randomIndex), Is.EqualTo(items));
        }

        [Test]
        public void TryAddItemAt_ValidInput_CallsOnAddEvent()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var inventory = this.inventoryFactory.EmptyContainer(size, stackSize);
            var amount = this.random.Next(1, stackSize);
            var items = this.itemFactory.CreateMany(amount);
            var randomIndex = this.random.Next(0, size); 
            
            var raised = false;
            inventory.OnAdd += (sender, args) =>
            {
                Assert.Multiple(() =>
                {
                    var firstEvent = args.Data.First();
                    Assert.That(args.Data, Has.Count.EqualTo(1));
                    Assert.That(firstEvent.Items, Is.EqualTo(items));
                    Assert.That(firstEvent.Index, Is.EqualTo(randomIndex));
                });
                raised = true;
            };

            inventory.TryAddAt(items, randomIndex);

            Assert.That(raised, Is.True, "OnAdd event was not raised");
        }

        [Test]
        public void TryAddItemAt_ItemAmountExceedsStackSize_ReturnsFalse()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var inventory = this.inventoryFactory.EmptyContainer(size, stackSize);

            var amount = this.random.Next(1, stackSize) + stackSize;
            var items = this.itemFactory.CreateMany(amount);
            var randomIndex = this.random.Next(0, size);
            var result = inventory.TryAddAt(items, randomIndex);

            Assert.That(result, Is.False);
        }

        [Test]
        public void TryAddItemAt_ItemAmountExceedsStackSize_DoesNotAddItemsToInventory()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var inventory = this.inventoryFactory.EmptyContainer(size, stackSize);
            var amount = this.random.Next(1, stackSize) + stackSize;
            var items = this.itemFactory.CreateMany(amount);
            var randomIndex = this.random.Next(0, size);
            inventory.TryAddAt(items, randomIndex);
            Assert.That(inventory.GetItems(randomIndex), Is.Empty);
        }

        [Test]
        public void TryAddItemAt_SlotWithDifferentItem_ReturnsFalse()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var slotItem = this.itemFactory.CreateDefault();
            var inventory = this.inventoryFactory.FullContainer(size, stackSize, slotItem);

            var amount = this.random.Next(1, stackSize);
            var items = this.itemFactory.CreateMany(amount);
            var randomIndex = this.random.Next(0, size);
            var result = inventory.TryAddAt(items, randomIndex);

            Assert.That(result, Is.False);
        }

        [Test]
        public void TryAddItemAt_SlotWithDifferentItem_DoesNotAddItemsToInventory()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var slotItems = this.itemFactory.CreateMany(size);
            var inventory = this.inventoryFactory.ShuffledItemsContainer(size, stackSize, slotItems);
            var amount = this.random.Next(1, stackSize);

            var items = this.itemFactory.CreateMany(amount);
            var randomIndex = this.random.Next(0, size);
            inventory.TryAddAt(items, randomIndex);

            Assert.That(inventory.GetItems(randomIndex), Is.Not.EqualTo(items));
        }

        [Test]
        public void TryAddItemAt_SlotWithSameItem_EnoughSpace_ReturnsTrue()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var slotItem = this.itemFactory.CreateDefault();
            var inventory = this.inventoryFactory.FullContainer(size, stackSize, slotItem);

            var getAmount = this.random.Next(1, stackSize);
            var randomIndex = this.random.Next(0, size);
            inventory.Get(randomIndex, getAmount);

            var items = this.itemFactory.CreateMany(getAmount);
            var result = inventory.TryAddAt(items, randomIndex);

            Assert.That(result, Is.True);
        }

        [Test]
        public void TryAddItemAt_SlotWithSameItem_EnoughSpace_AddsItemsToInventory()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var slotItem = this.itemFactory.CreateDefault();
            var inventory = this.inventoryFactory.FullContainer(size, stackSize, slotItem);
           
            var randomIndex = this.random.Next(0, size);
            var getAmount = inventory.RemoveRandomAt(randomIndex, stackSize);
            
            var items = this.itemFactory.CreateMany(getAmount);
            inventory.TryAddAt(items, randomIndex);
            
            Assert.That(inventory.GetItems(randomIndex).Reverse().Take(getAmount), Has.All.EqualTo(slotItem));
        }

        [Test]
        public void TryAddItemAt_SlotWithSameItem_EnoughSpace_CallsOnAddEvent()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var slotItem = this.itemFactory.CreateDefault();
            var inventory = this.inventoryFactory.FullContainer(size, stackSize, slotItem);
            
            var randomIndex = this.random.Next(0, size);
            var getAmount = inventory.RemoveRandomAt(randomIndex, stackSize);
            
            var items = this.itemFactory.CreateMany(getAmount);
            
            var raised = false;
            inventory.OnAdd += (sender, args) =>
            {
                Assert.Multiple(() =>
                {
                    var firstEvent = args.Data.First();
                    Assert.That(args.Data, Has.Count.EqualTo(1));
                    Assert.That(firstEvent.Items, Is.EqualTo(items));
                    Assert.That(firstEvent.Index, Is.EqualTo(randomIndex));
                });
                raised = true;
            };
            inventory.TryAddAt(items, randomIndex);
            
            Assert.That(raised, Is.True, "OnAdd event was not raised");
        }
    }
}
