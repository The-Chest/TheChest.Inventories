using TheChest.Tests.Common.Extensions.Containers;

namespace TheChest.Inventories.Tests.Containers.StackInventory
{
    public partial class StackInventoryTests<T>
    {
        [TestCase(-1)]
        [TestCase(MAX_SIZE_TEST)]
        public void GetAmountFrom_InvalidIndex_ThrowsArgumentOutOfRangeException(int index)
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var inventory = this.inventoryFactory.EmptyContainer(size, stackSize);

            Assert.That(
                () => inventory.Get(index, stackSize), 
                Throws.InstanceOf<ArgumentOutOfRangeException>().With.Property("ParamName").EqualTo("index")
            );
        }

        [TestCase(-1)]
        [TestCase(0)]
        public void GetAmountFrom_InvalidAmount_ThrowsArgumentOutOfRangeException(int amount)
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var inventory = this.inventoryFactory.EmptyContainer(size, stackSize);

            var index = this.random.Next(0, size);
            Assert.That(
                () => inventory.Get(index, amount), 
                Throws.InstanceOf<ArgumentOutOfRangeException>().With.Property("ParamName").EqualTo("amount")
            );
        }

        [Test]
        public void GetAmountFrom_EmptySlot_CallsOnGetEvent()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var inventory = this.inventoryFactory.EmptyContainer(size, stackSize);

            inventory.OnGet += (sender, args) => Assert.Fail("OnGet event should not be called when no item is found");

            var randomIndex = this.random.Next(0, size);
            inventory.Get(randomIndex, stackSize);
        }

        [Test]
        public void GetAmountFrom_SlotWithItems_RemovesItemsFromSlot()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var slotItem = this.itemFactory.CreateRandom();
            var inventory = this.inventoryFactory.FullContainer(size, stackSize, slotItem);

            var amount = this.random.Next(1, stackSize);
            var index = this.random.Next(0, size);
            inventory.Get(index, amount);

            Assert.That(inventory.GetSlot(index)!.Amount, Is.EqualTo(stackSize - amount));
        }

        [Test]
        public void GetAmountFrom_SlotWithItems_CallsOnGetEvent()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var slotItem = this.itemFactory.CreateRandom();
            var inventory = this.inventoryFactory.FullContainer(size, stackSize, slotItem);

            var index = this.random.Next(0, size);
            var amount = this.random.Next(1, stackSize);

            var raised = false;
            inventory.OnGet += (sender, args) => {
                Assert.Multiple(() =>
                {
                    var firstEvent = args.Data.First();
                    Assert.That(firstEvent.Items, Has.All.EqualTo(slotItem));
                    Assert.That(firstEvent.Items, Has.Length.EqualTo(amount));
                    Assert.That(firstEvent.Index, Is.EqualTo(index));
                });
                raised = true;
            };

            inventory.Get(index, amount);

            Assert.That(raised, Is.True, "OnGet event was not raised");
        }

        [Test]
        public void GetAmountFrom_AmountBiggerThanItemsInsideSlot_RemovesAllItemsFromSlot()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var slotItem = this.itemFactory.CreateRandom();
            var inventory = this.inventoryFactory.FullContainer(size, stackSize, slotItem);

            var index = this.random.Next(0, size);
            var amount = this.random.Next(stackSize + 1, stackSize * 2);
            inventory.Get(index, amount);

            Assert.That(inventory.GetSlot(index)!.Amount, Is.Zero);
        }

        [Test]
        public void GetAmountFrom_AmountBiggerThanItemsInsideSlot_CallsOnGetEventWithTheMaximumAmountPossible()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var slotItem = this.itemFactory.CreateRandom();
            var inventory = this.inventoryFactory.FullContainer(size, stackSize, slotItem);

            var index = this.random.Next(0, size);
            var amount = this.random.Next(stackSize + 1, stackSize * 2);

            var raised = false;
            inventory.OnGet += (sender, args) => {
                Assert.Multiple(() =>
                {
                    var firstEvent = args.Data.First();
                    Assert.That(firstEvent.Items, Has.All.EqualTo(slotItem));
                    Assert.That(firstEvent.Items, Has.Length.EqualTo(stackSize));
                    Assert.That(firstEvent.Index, Is.EqualTo(index));
                });
                raised = true;
            };

            inventory.Get(index, amount);

            Assert.That(raised, Is.True, "OnGet event was not raised");
        }


        [Test]
        public void GetAmountFrom_EmptySlot_ReturnsEmptyArray()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var inventory = this.inventoryFactory.EmptyContainer(size, stackSize);
            
            var index = this.random.Next(0, size);
            var amount = this.random.Next(1, stackSize);
            var item = inventory.Get(index, amount);
            
            Assert.That(item, Is.Empty);
        }

        [Test]
        public void GetAmountFrom_SlotWithItems_ReturnsItems()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var slotItem = this.itemFactory.CreateRandom();
            var inventory = this.inventoryFactory.FullContainer(size, stackSize, slotItem);

            var amount = this.random.Next(1, stackSize);
            var index = this.random.Next(0, size);
            var items = inventory.Get(index, amount);

            Assert.That(items, Is.Not.Empty);
            Assert.That(items, Has.Length.EqualTo(amount));
            Assert.That(items, Has.All.EqualTo(slotItem));
        }

        [Test]
        public void GetAmountFrom_AmountBiggerThanItemsInsideSlot_ReturnsTheMaximumAmountPossible()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var slotItem = this.itemFactory.CreateRandom();
            var inventory = this.inventoryFactory.FullContainer(size, stackSize, slotItem);

            var amount = this.random.Next(stackSize + 1, stackSize * 2);
            var index = this.random.Next(0, size);
            var items = inventory.Get(index, amount);

            Assert.That(items.Count, Is.EqualTo(stackSize));
        }
    }
}
