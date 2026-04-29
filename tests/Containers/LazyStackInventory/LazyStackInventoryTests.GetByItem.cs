using TheChest.Tests.Common.Extensions.Containers;

using TheChest.Tests.Common.Attributes;
namespace TheChest.Inventories.Tests.Containers.LazyStackInventory
{
    public partial class LazyStackInventoryTests<T>
    {
        [Test]
        [IgnoreIfValueType]
        public void Get_ByItem_NullItem_ThrowsArgumentNullException()
        {
            var inventory = this.inventoryFactory.EmptyContainer();
            Assert.Throws<ArgumentNullException>(() => inventory.Get(item: default!));
        }

        [Test]
        [IgnoreIfReferenceType]
        public void Get_ByItem_DefaultValueTypeItem_DoesNotThrow()
        {
            var inventory = this.inventoryFactory.EmptyContainer();

            Assert.That(() => inventory.Get(item: default!), Throws.Nothing);
        }

        [Test]
        public void Get_ByItem_ExistingItem_RemovesOneFromFirstFoundSlot()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var items = this.itemFactory.CreateDefault();
            var inventory = this.inventoryFactory.FullContainer(size, stackSize, items);

            var expectedItem = this.itemFactory.CreateDefault();
            inventory.Get(expectedItem);

            Assert.That(inventory.GetSlot(0)!.Amount, Is.EqualTo(stackSize - 1));
        }

        [Test]
        public void Get_ByItem_ExistingItem_CallsOnGetEvent()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var items = this.itemFactory.CreateDefault();
            var inventory = this.inventoryFactory.FullContainer(size, stackSize, items);

            var expectedItem = this.itemFactory.CreateDefault();
            var raised = false;
            inventory.OnGet += (sender, args) => {
                Assert.That(args.Data, Has.Count.EqualTo(1));
                var firstEvent = args.Data.First();
                Assert.Multiple(() =>
                {
                    Assert.That(firstEvent.Item, Is.EqualTo(expectedItem));
                    Assert.That(firstEvent.Index, Is.EqualTo(0));
                    Assert.That(firstEvent.Amount, Is.EqualTo(1));
                });
                raised = true;
            };

            inventory.Get(expectedItem);

            Assert.That(raised, Is.True, "OnGet event was not raised");
        }

        [Test]
        public void Get_ByItem_NotFoundItem_DoesNotCallOnGetEvent()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var items = this.itemFactory.CreateDefault();
            var inventory = this.inventoryFactory.ShuffledItemsContainer(size, stackSize, items);

            inventory.OnGet += (sender, args) => Assert.Fail("OnGet event should not be called when an item that is not found.");
            
            var item = this.itemFactory.CreateRandom();
            inventory.Get(item);
        }
    }
}
