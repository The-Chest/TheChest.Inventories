namespace TheChest.Inventories.Tests.Containers.LazyStackInventory
{
    public partial class LazyStackInventoryTests<T>
    {
        [TestCase(-1, 1)]
        [TestCase(MAX_SIZE_TEST + 1, 1)]
        public void Get_ByIndexAndAmount_InvalidIndex_ThrowsArgumentOutOfRangeException(int index, int amount)
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var item = this.itemFactory.CreateDefault();
            var inventory = this.inventoryFactory.FullContainer(size, stackSize, item);

            Assert.That(
                () => inventory.Get(index, amount),
                Throws.TypeOf<ArgumentOutOfRangeException>().And.With.Message.Contains("index")
            );
        }

        [TestCase(1, 0)]
        [TestCase(0, -1)]
        public void Get_ByIndexAndAmount_InvalidAmount_ThrowsArgumentOutOfRangeException(int index, int amount)
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var item = this.itemFactory.CreateDefault();
            var inventory = this.inventoryFactory.FullContainer(size, stackSize, item);

            Assert.That(
                () => inventory.Get(index, amount),
                Throws.TypeOf<ArgumentOutOfRangeException>().And.With.Message.Contains("amount")
            );
        }

        [Test]
        public void Get_ByIndexAndAmount_ValidIndexEmptySlot_DoesNotCallOnGetEvent()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var inventory = this.inventoryFactory.EmptyContainer(size, stackSize);

            inventory.OnGet += (sender, args) => Assert.Fail("OnGet event should not be called for empty slot.");

            var index = this.random.Next(0, size);
            var amount = this.random.Next(1, stackSize);
            inventory.Get(index, amount);
        }

        [Test]
        public void Get_ByIndexAndAmount_ValidIndexAndAmount_CallsOnGetEvent()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var item = this.itemFactory.CreateDefault();
            var inventory = this.inventoryFactory.FullContainer(size, stackSize, item);

            var index = this.random.Next(0, size);
            var amount = this.random.Next(1, stackSize);

            var raised = false;
            inventory.OnGet += (sender, args) => {
                Assert.That(sender, Is.EqualTo(inventory));
                var firstData = args.Data.FirstOrDefault();
                Assert.Multiple(() =>
                {
                    Assert.That(firstData.Item, Is.EqualTo(item));
                    Assert.That(firstData.Index, Is.EqualTo(index));
                    Assert.That(firstData.Amount, Is.EqualTo(amount));
                });
                raised = true;
            };
            inventory.Get(index, amount);

            Assert.That(raised, Is.True, "OnGet event was not raised");
        }

        [Test]
        public void Get_ByIndexAndAmount_ValidIndexAndAmountBiggerThanSlotSize_CallsOnGetEvent()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var item = this.itemFactory.CreateDefault();
            var inventory = this.inventoryFactory.FullContainer(size, stackSize, item);

            var index = this.random.Next(0, size);
            var amount = this.random.Next(stackSize + 1, stackSize + 10);
            var raised = false;
            inventory.OnGet += (sender, args) => {
                Assert.That(sender, Is.EqualTo(inventory));
                var firstData = args.Data.FirstOrDefault();
                Assert.Multiple(() =>
                {
                    Assert.That(firstData.Item, Is.EqualTo(item));
                    Assert.That(firstData.Index, Is.EqualTo(index));
                    Assert.That(firstData.Amount, Is.EqualTo(stackSize));
                });
                raised = true;
            };

            inventory.Get(index, amount);

            Assert.That(raised, Is.True, "OnGet event was not raised");
        }
    }
}
