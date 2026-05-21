using TheChest.Tests.Common.Extensions.Containers;

namespace TheChest.Inventories.Tests.Containers.LazyStackInventory
{
    public partial class LazyStackInventoryTests<T>
    {
        [TestCase(-1)]
        [TestCase(MAX_SIZE_TEST)]
        public void Get_ByIndex_ShouldThrowArgumentOutOfRangeException_WhenIndexIsInvalid(int index)
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var item = this.itemFactory.CreateDefault();
            var inventory = this.inventoryFactory.FullContainer(size, stackSize, item);

            Assert.Throws<ArgumentOutOfRangeException>(() => inventory.Get(index));
        }

        [Test]
        public void Get_ByIndex_ValidIndex_RemovesItemFromSlot()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var expectedItem = this.itemFactory.CreateDefault();
            var inventory = this.inventoryFactory.FullContainer(size, stackSize, expectedItem);

            var index = this.random.Next(0, size - 1);
            inventory.Get(index);

            Assert.That(inventory.GetItems(index), Has.Length.EqualTo(stackSize - 1));
        }

        [Test]
        public void Get_ByIndex_ValidIndex_CallsOnGetEvent()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var expectedItem = this.itemFactory.CreateDefault();
            var inventory = this.inventoryFactory.FullContainer(size, stackSize, expectedItem);

            var index = this.random.Next(0, size - 1);
            var raised = false;
            inventory.OnGet += (sender, args) => {
                Assert.That(args.Data, Has.Count.EqualTo(1));
                Assert.Multiple(() =>
                {
                    var firstEvent = args.Data.First();
                    Assert.That(firstEvent.Item, Is.EqualTo(expectedItem));
                    Assert.That(firstEvent.Index, Is.EqualTo(index));
                    Assert.That(firstEvent.Amount, Is.EqualTo(1));
                });
                raised = true;
            };

            inventory.Get(index);

            Assert.That(raised, Is.True, "OnGet event was not raised");
        }

        [Test]
        public void Get_ByIndex_EmptySlot_DoesNotCallOnGetEvent()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var inventory = this.inventoryFactory.EmptyContainer(size, stackSize);

            inventory.OnGet += (sender, args) => Assert.Fail("OnGet event should not be called for an empty slot.");
            
            var index = this.random.Next(0, size - 1);
            inventory.Get(index);
        }

        [Test]
        public void Get_ByIndex_ValidIndex_ReturnsItem()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var expectedItem = this.itemFactory.CreateDefault();
            var inventory = this.inventoryFactory.FullContainer(size, stackSize, expectedItem);

            var randomIndex = this.random.Next(0, size - 1);
            var result = inventory.Get(randomIndex);

            Assert.That(result, Is.EqualTo(expectedItem));
        }

        [Test]
        public void Get_ByIndex_EmptySlot_ReturnsNull()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var inventory = this.inventoryFactory.EmptyContainer(size, stackSize);

            var randomIndex = this.random.Next(0, size - 1);
            var result = inventory.Get(randomIndex);

            Assert.That(result, Is.Null);
        }
    }
}
