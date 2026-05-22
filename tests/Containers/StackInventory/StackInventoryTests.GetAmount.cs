using TheChest.Tests.Common.Extensions.Containers;

namespace TheChest.Inventories.Tests.Containers.StackInventory
{
    public partial class StackInventoryTests<T>
    {
        [TestCase(-1)]
        [TestCase(0)]
        public void GetAmount_InvalidAmount_ThrowsArgumentOutOfRangeException(int amount)
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var item = this.itemFactory.CreateRandom();
            var inventory = this.inventoryFactory.EmptyContainer(size, stackSize);

            Assert.That(
                () => inventory.Get(item, amount), 
                Throws.InstanceOf<ArgumentOutOfRangeException>().With.Property("ParamName").EqualTo("amount")
            );
        }

        [Test]
        public void GetAmount_InvalidItem_ThrowsArgumentNullException()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var inventory = this.inventoryFactory.EmptyContainer(size, stackSize);

            Assert.That(
                () => inventory.Get(default(T)!, 10),
                Throws.InstanceOf<ArgumentNullException>().With.Property("ParamName").EqualTo("item")
            );
        }

        [Test]
        public void GetAmount_EmptyInventory_DoesNotCallOnGetEvent()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var item = this.itemFactory.CreateRandom();
            var inventory = this.inventoryFactory.EmptyContainer(size, stackSize);

            inventory.OnGet += (sender, args) => Assert.Fail("OnGet event should not be called when no item is found");

            var amount = this.random.Next(1, 20);
            inventory.Get(item, amount);
        }

        [Test]
        public void GetAmount_InventoryWithItems_RemovesItemsFromSlot()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var slotItem = this.itemFactory.CreateDefault();
            var inventory = this.inventoryFactory.FullContainer(size, stackSize, slotItem);

            inventory.Get(slotItem, stackSize);

            Assert.That(inventory.GetSlot(0)!.IsEmpty, Is.True);
        }

        [Test]
        public void GetAmount_InventoryWithItems_CallsOnGetEvent()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var slotItem = this.itemFactory.CreateDefault();
            var inventory = this.inventoryFactory.FullContainer(size, stackSize, slotItem);

            var raised = false;
            inventory.OnGet += (sender, args) => {
                Assert.Multiple(() =>
                {
                    var firstEvent = args.Data.First();
                    Assert.That(firstEvent.Items, Has.All.EqualTo(slotItem));
                    Assert.That(firstEvent.Items, Has.Length.EqualTo(stackSize));
                    Assert.That(firstEvent.Index, Is.EqualTo(0));
                });
                raised = true;
            };
            inventory.Get(slotItem, stackSize);

            Assert.That(raised, Is.True, "OnGet event was not raised");
        }

        [Test]
        public void GetAmount_InventoryWithItems_RemovesItemsFromMultipleSlotsInOrder()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var slotItem = this.itemFactory.CreateDefault();
            var inventory = this.inventoryFactory.FullContainer(size, stackSize, slotItem);

            inventory.Get(slotItem, stackSize + (stackSize - 2));

            Assert.Multiple(() =>
            {
                Assert.That(inventory.GetSlot(0)!.Amount, Is.EqualTo(0));
                Assert.That(inventory.GetSlot(1)!.Amount, Is.EqualTo(2));
            });
        }

        [Test]
        public void GetAmount_InventoryWithItems_CallsOnGetEventFromMultipleSlotsInOrder()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var slotItem = this.itemFactory.CreateDefault();
            var inventory = this.inventoryFactory.FullContainer(size, stackSize, slotItem);

            var raised = false;
            inventory.OnGet += (sender, args) => {
                Assert.That(args.Data, Has.Count.EqualTo(2));
                Assert.Multiple(() =>
                {
                    var firstEvent = args.Data.First();
                    Assert.That(firstEvent.Items, Has.All.EqualTo(slotItem));
                    Assert.That(firstEvent.Items, Has.Length.EqualTo(stackSize));
                    Assert.That(firstEvent.Index, Is.EqualTo(0));
                });
                Assert.Multiple(() =>
                {
                    var secondEvent = args.Data.Skip(1).First();
                    Assert.That(secondEvent.Items, Has.All.EqualTo(slotItem));
                    Assert.That(secondEvent.Items, Has.Length.EqualTo(stackSize - 2));
                    Assert.That(secondEvent.Index, Is.EqualTo(1));
                });
                raised = true;
            };

            inventory.Get(slotItem, stackSize + (stackSize - 2));

            Assert.That(raised, Is.True, "OnGet event was not raised");
        }


        [Test]
        public void GetAmount_EmptyInventory_ReturnsEmptyArray()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var item = this.itemFactory.CreateRandom();
            var inventory = this.inventoryFactory.EmptyContainer(size, stackSize);
            
            var amount = inventory.Get(item, 10);
            
            Assert.That(amount, Is.Empty);
        }

        [Test]
        public void GetAmount_InventoryWithItems_ReturnsSearchedItems()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var item = this.itemFactory.CreateDefault();
            var inventoryItems = this.itemFactory.CreateManyRandom(size / 2)
                .Append(item)
                .Append(item)
                .ToArray();
            var inventory = this.inventoryFactory.ShuffledItemsContainer(size, stackSize, inventoryItems);

            var items = inventory.Get(item, stackSize);

            Assert.Multiple(() =>
            {
                Assert.That(items, Has.Length.EqualTo(stackSize));
                Assert.That(items, Has.All.EqualTo(item));
            });
        }
    }
}
