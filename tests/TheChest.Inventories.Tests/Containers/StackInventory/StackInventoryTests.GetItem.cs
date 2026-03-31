using TheChest.Inventories.Tests.Common.Extensions.Containers;

namespace TheChest.Inventories.Tests.Containers
{
    public partial class StackInventoryTests<T>
    {
        [Test]
        public void GetItem_InvalidItem_ThrowsArgumentNullException()
        {
            var inventory = this.inventoryFactory.EmptyContainer();

            Assert.That(() => inventory.Get(default(T)!), Throws.ArgumentNullException);
        }

        [Test]
        public void GetItem_EmptyInventory_DoesNotCallOnGetEvent()
        {
            var inventory = this.inventoryFactory.EmptyContainer();
            
            inventory.OnGet += (sender, args) => Assert.Fail("OnGet event should not be called when no item is found");
            
            var item = this.itemFactory.CreateDefault();
            inventory.Get(item);
        }

        [Test]
        public void GetItem_InventoryWithItems_RemovesItemFromFirstFoundSlot()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var slotItem = this.itemFactory.CreateRandom();
            var inventory = this.inventoryFactory.FullContainer(size, stackSize, slotItem);

            inventory.Get(slotItem);

            Assert.That(inventory.GetSlot(0)!.Amount, Is.EqualTo(stackSize - 1));
        }

        [Test]
        public void GetItem_InventoryWithItems_CallsOnGetEvent()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var slotItem = this.itemFactory.CreateRandom();
            var inventory = this.inventoryFactory.FullContainer(size, stackSize, slotItem);

            var raised = false;
            inventory.OnGet += (sender, args) =>
            {
                Assert.Multiple(() =>
                {
                    var firstEvent = args.Data.FirstOrDefault();
                    Assert.That(args.Data, Has.Count.EqualTo(1));
                    Assert.That(firstEvent.Items, Has.All.EqualTo(slotItem));
                    Assert.That(firstEvent.Index, Is.EqualTo(0));
                });
                raised = true;
            };

            inventory.Get(slotItem);

            Assert.That(raised, Is.True, "OnGet event was not raised");
        }

        [Test]
        public void GetItem_InventoryWithDifferentItems_DoesNotCallOnGetEvent()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var slotItem = this.itemFactory.CreateRandom();
            var inventory = this.inventoryFactory.FullContainer(size, stackSize, slotItem);

            inventory.OnGet += (sender, args) => Assert.Fail("OnGet event should not be called when no item is found");
            
            var item = this.itemFactory.CreateDefault();
            inventory.Get(item);
        }
    }
}
