using TheChest.Tests.Common.Extensions.Containers;
using TheChest.Tests.Common.Extensions.Slots;

using TheChest.Tests.Common.Attributes;
namespace TheChest.Inventories.Tests.Containers.StackInventory
{
    public partial class StackInventoryTests<T>
    {
        [Test]
        [IgnoreIfValueTypeAttribute]
        public void GetAllItems_InvalidItem_ThrowsArgumentNullException()
        {
            var inventory = this.inventoryFactory.EmptyContainer();
            Assert.That(() => inventory.GetAll(default(T)!), Throws.ArgumentNullException);
        }

        [Test]
        public void GetAllItems_EmptyInventory_DoesNotCallOnGetEvent()
        {
            var item = this.itemFactory.CreateRandom();
            var inventory = this.inventoryFactory.EmptyContainer();

            inventory.OnGet += (sender, args) => Assert.Fail("OnGet event should not be called when no item is found");

            inventory.GetAll(item);
        }

        [Test]
        public void GetAllItems_InventoryWithItems_RemovesItemsFromInventory()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var slotItems = this.itemFactory.CreateMany(size / 2);
            var randomItems = this.itemFactory.CreateManyRandom(size / 2);
            var inventoryItems = slotItems.Concat(randomItems).ToArray();
            var inventory = this.inventoryFactory.ShuffledItemsContainer(size, stackSize, inventoryItems);

            inventory.GetAll(slotItems[0]);

            Assert.That(
                inventory.GetSlots().Any(x => x.GetContents()?.Contains(slotItems[0]) ?? false),
                Is.False
            );
        }

        [Test]
        public void GetAllItems_InventoryWithItems_CallsOnGetEvent()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var slotItems = this.itemFactory.CreateMany(size / 2);
            var randomItems = this.itemFactory.CreateManyRandom(size / 2);
            var inventoryItems = slotItems.Concat(randomItems).ToArray();
            var inventory = this.inventoryFactory.ShuffledItemsContainer(size, stackSize, inventoryItems);

            var raised = false;
            inventory.OnGet += (sender, args) =>
            {
                Assert.Multiple(() =>
                {
                    var firstEvent = args.Data.FirstOrDefault();
                    Assert.That(args.Data, Has.Count.EqualTo(size / 2));
                    Assert.That(firstEvent.Items, Has.All.EqualTo(slotItems[0]));
                    // TODO: improve this test by improving container creation using "WithItem" method
                    // Creating a better factory will allow to create an inventory with ordered items
                    // Assert.That(firstEvent.Index, Is.EqualTo(10));
                });
                raised = true;
            };
            inventory.GetAll(slotItems[0]);

            Assert.That(raised, Is.True, "OnGet event was not raised");
        }
    }
}
