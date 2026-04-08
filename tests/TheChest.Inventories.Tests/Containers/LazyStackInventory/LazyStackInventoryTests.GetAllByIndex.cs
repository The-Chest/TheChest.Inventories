using TheChest.Tests.Common.Extensions.Containers;
using TheChest.Tests.Common.Extensions.Slots;

namespace TheChest.Inventories.Tests.Containers.LazyStackInventory
{
    public partial class LazyStackInventoryTests<T>
    {
        [TestCase(-1)]
        [TestCase(MAX_SIZE_TEST + 1)]
        public void GetAll_ByIndex_InvalidIndex_ThrowsArgumentOutOfRangeException(int index)
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var inventory = this.inventoryFactory.EmptyContainer(size, stackSize);

            Assert.Throws<ArgumentOutOfRangeException>(() => inventory.GetAll(index));
        }

        [Test]
        public void GetAll_ByIndex_ExistingIndex_RemovesAllItemsFromSlot()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var expectedItem = this.itemFactory.CreateDefault();
            var inventory = this.inventoryFactory.FullContainer(size, stackSize, expectedItem);

            var index = this.random.Next(0, size);
            inventory.GetAll(index);
            
            Assert.Multiple(() =>
            {
                var slot = inventory.GetSlot(index);

                Assert.That(slot.IsEmpty, Is.True);
                Assert.That(slot.GetContent(), Is.Null);
            });
        }

        [Test]
        public void GetAll_ByIndex_ExistingIndex_CallsOnGetEvent()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var expectedItem = this.itemFactory.CreateDefault();
            var inventory = this.inventoryFactory.FullContainer(size, stackSize, expectedItem);

            var index = this.random.Next(0, size);
            var raised = false;
            inventory.OnGet += (sender, args) => {
                Assert.That(args.Data, Has.Count.EqualTo(1));

                Assert.Multiple(() =>
                {
                    var firstEvent = args.Data.First();
                    Assert.That(firstEvent.Item, Is.EqualTo(expectedItem));
                    Assert.That(firstEvent.Index, Is.EqualTo(index));
                    Assert.That(firstEvent.Amount, Is.EqualTo(stackSize));
                });
                raised = true;
            };

            inventory.GetAll(index);

            Assert.That(raised, Is.True, "OnGet event was not raised");
        }

        [Test]
        public void GetAll_ByIndex_NotExistingIndex_DoesNotCallOnGetEvent()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var inventory = this.inventoryFactory.EmptyContainer(size, stackSize);

            inventory.OnGet += (sender, args) => Assert.Fail("OnGet event should not be called for an empty slot.");
            
            var index = this.random.Next(0, size);
            inventory.GetAll(index);
        }
    }
}
