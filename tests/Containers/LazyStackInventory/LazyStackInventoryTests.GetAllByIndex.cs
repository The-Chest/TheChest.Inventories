using TheChest.Tests.Common.Extensions.Containers;
using TheChest.Tests.Common.Extensions.Slots;

namespace TheChest.Inventories.Tests.Containers.LazyStackInventory
{
    public partial class LazyStackInventoryTests<T>
    {
        [TestCase(-1)]
        [TestCase(MAX_SIZE_TEST)]
        public void GetAllByIndex_InvalidIndex_ThrowsArgumentOutOfRangeException(int index)
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var inventory = this.inventoryFactory.EmptyContainer(size, stackSize);

            Assert.That(
                () => inventory.GetAll(index),
                Throws.TypeOf<ArgumentOutOfRangeException>().With.Property("ParamName").EqualTo("index")
            );
        }

        [Test]
        public void GetAllByIndex_EmptySlot_ThrowsInvalidOperationException()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var inventory = this.inventoryFactory.EmptyContainer(size, stackSize);
            var index = this.random.Next(0, size);

            Assert.That(
                () => inventory.GetAll(index),
                Throws.InvalidOperationException.With.Message.EqualTo("The slot is empty.")
            );
        }

        [Test]
        public void GetAllByIndex_SlotWithItems_RemovesAllItemsFromSlot()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
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
        public void GetAllByIndex_SlotWithItems_CallsOnGetEvent()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
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
        public void GetAllByIndex_EmptySlot_DoesNotCallOnGetEvent()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var inventory = this.inventoryFactory.EmptyContainer(size, stackSize);

            inventory.OnGet += (sender, args) => Assert.Fail("OnGet event should not be called for an empty slot.");
            
            var index = this.random.Next(0, size);
            Assert.That(
                () => inventory.GetAll(index),
                Throws.InvalidOperationException.With.Message.EqualTo("The slot is empty.")
            );
        }

        [Test]
        public void GetAllByIndex_SlotWithItems_ReturnsAllItemsFromSlot()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var expectedItem = this.itemFactory.CreateDefault();
            var inventory = this.inventoryFactory.FullContainer(size, stackSize, expectedItem);

            var index = this.random.Next(0, size);
            var result = inventory.GetAll(index);

            Assert.That(result, Is.Not.Empty.And.Length.EqualTo(stackSize).And.All.EqualTo(expectedItem));
        }

    }
}
