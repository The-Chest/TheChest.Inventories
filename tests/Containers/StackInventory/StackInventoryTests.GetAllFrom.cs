using TheChest.Tests.Common.Extensions.Containers;
using TheChest.Tests.Common.Extensions.Slots;

namespace TheChest.Inventories.Tests.Containers.StackInventory
{
    public partial class StackInventoryTests<T>
    {
        [TestCase(-1)]
        [TestCase(MAX_SIZE_TEST)]
        public void GetAllFrom_InvalidIndex_ThrowsArgumentOutOfRangeException(int index)
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var inventory = this.inventoryFactory.EmptyContainer(size, stackSize);

            Assert.That(
                () => inventory.GetAll(index), 
                Throws.InstanceOf<ArgumentOutOfRangeException>().With.Property("ParamName").EqualTo("index")
            );
        }

        [Test]
        public void GetAllFrom_EmptySlot_DoesNotCallOnGetEvent()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var inventory = this.inventoryFactory.EmptyContainer(size, stackSize);

            inventory.OnGet += (sender, args) => Assert.Fail("OnGet event should not be called for empty slot");

            var index = this.random.Next(0, size - 1);
            inventory.GetAll(index);
        }

        [Test]
        public void GetAllFrom_SlotWithItems_RemovesAllItemsFromSlot()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var slotItem = this.itemFactory.CreateRandom();
            var inventory = this.inventoryFactory.FullContainer(size, stackSize, slotItem);

            var index = this.random.Next(0, size);
            inventory.GetAll(index);

            Assert.Multiple(() =>
            {
                var slot = inventory.GetSlot(index);
                Assert.That(slot!.IsEmpty, Is.True);
                Assert.That(slot!.GetContents(), Has.All.Null);
            });
        }

        [Test]
        public void GetAllFrom_SlotWithItems_CallsOnGetEvent()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var slotItem = this.itemFactory.CreateRandom();
            var inventory = this.inventoryFactory.FullContainer(size, stackSize, slotItem);

            var index = this.random.Next(0, size);
            var raised = false;
            inventory.OnGet += (sender, args) =>
            {
                Assert.Multiple(() =>
                {
                    var firstEvent = args.Data.FirstOrDefault();
                    Assert.That(args.Data, Has.Count.EqualTo(1));
                    Assert.That(firstEvent.Items, Has.All.EqualTo(slotItem));
                    Assert.That(firstEvent.Index, Is.EqualTo(index));
                });
                raised = true;
            };

            inventory.GetAll(index);

            Assert.That(raised, Is.True, "OnGet event was not raised");
        }


        [Test]
        public void GetAllFrom_EmptySlot_ReturnsEmptyArray()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var inventory = this.inventoryFactory.EmptyContainer(size, stackSize);

            var index = this.random.Next(0, size - 1);
            var items = inventory.GetAll(index);

            Assert.That(items, Is.Empty);
        }

        [Test]
        public void GetAllFrom_SlotWithItems_ReturnItems()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var slotItem = this.itemFactory.CreateRandom();
            var inventory = this.inventoryFactory.FullContainer(size, stackSize, slotItem);

            var index = this.random.Next(0, size);
            var items = inventory.GetAll(index);

            Assert.Multiple(() =>
            {
                Assert.That(items, Has.Length.EqualTo(stackSize));
                Assert.That(items, Has.All.EqualTo(slotItem));
            });
        }
    }
}
