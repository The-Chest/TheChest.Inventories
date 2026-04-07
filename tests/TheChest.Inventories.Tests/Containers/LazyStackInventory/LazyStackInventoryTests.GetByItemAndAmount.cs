using TheChest.Tests.Common.Attributes;
﻿namespace TheChest.Inventories.Tests.Containers.LazyStackInventory
{
    public partial class LazyStackInventoryTests<T>
    {
        [Test]
        [IgnoreIfValueTypeAttribute]
        public void Get_ByItemAndAmount_Nulltem_ThrowsArgumentNullException()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var inventory = this.inventoryFactory.EmptyContainer(size, stackSize);

            Assert.Throws<ArgumentNullException>(() => inventory.Get(item: default!, amount: 1));
        }

        [TestCase(-1)]
        [TestCase(0)]
        public void Get_ByItemAndAmount_InvalidAmount_ThrowsArgumentOutOfRangeException(int amount)
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var inventory = this.inventoryFactory.EmptyContainer(size, stackSize);

            var item = this.itemFactory.CreateDefault();
            Assert.Throws<ArgumentOutOfRangeException>(() => inventory.Get(item, amount));
        }

        [Test]
        public void Get_ByItemAndAmount_ExistingItemsAndAmountBiggerThanInventorySize_CallsOnGetEvent()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var item = this.itemFactory.CreateDefault();
            var inventory = this.inventoryFactory.FullContainer(size, stackSize, item);

            var amount = size * stackSize * 2;
            var raised = false;
            inventory.OnGet += (sender, args) => {
                Assert.That(args.Data, Has.Count.EqualTo(size));
                Assert.Multiple(() =>
                {
                    Assert.That(args.Data.Select(x => x.Item), Has.All.EqualTo(item));
                    Assert.That(args.Data.Select(x => x.Index), Has.All.InRange(0, size));
                    Assert.That(args.Data.Select(x => x.Amount), Has.All.EqualTo(stackSize));
                });
                raised = true;
            };
            inventory.Get(item, amount);
           
            Assert.That(raised, Is.True, "OnGet event was not raised");
        }

        [Test]
        public void Get_ByItemAndAmount_ExistingItems_CallsOnGetEvent()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var item = this.itemFactory.CreateDefault();
            var inventory = this.inventoryFactory.FullContainer(size, stackSize, item);
            
            var multiplier = this.random.Next(2, size);
            var amount = stackSize * multiplier;
            
            var raised = false;
            inventory.OnGet += (sender, args) => {
                Assert.That(args.Data, Has.Count.EqualTo(multiplier));
                Assert.Multiple(() =>
                {
                    Assert.That(args.Data.Select(x => x.Item), Has.All.EqualTo(item));
                    Assert.That(args.Data.Select(x => x.Index), Has.All.InRange(0, size));
                    Assert.That(args.Data.Select(x => x.Amount), Has.All.EqualTo(stackSize));
                });
                raised = true;
            };

            inventory.Get(item, amount);

            Assert.That(raised, Is.True, "OnGet event was not raised");
        }

        [Test]
        public void Get_ByItemAndAmount_NotFoundItem_DoesNotCallOnGetEvent()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var randomItem = this.itemFactory.CreateRandom();
            var inventory = this.inventoryFactory.FullContainer(size, stackSize, randomItem);

            inventory.OnGet += (sender, args) => Assert.Fail("OnGet event should not be called for non-existing item.");
            
            var amount = this.random.Next(2, size);
            var item = this.itemFactory.CreateDefault();
            inventory.Get(item, amount);
        }
    }
}
