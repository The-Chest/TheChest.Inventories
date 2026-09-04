using TheChest.Core.Slots.Interfaces;
using TheChest.Tests.Common.Extensions.Containers;
using TheChest.Tests.Common.Extensions.Slots;

namespace TheChest.Inventories.Tests.Containers.LazyStackInventory
{
    public partial class LazyStackInventoryTests<T>
    {
        [Test]
        public void AddAmount_NullItem_ThrowsArgumentNullException()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var inventory = this.inventoryFactory.EmptyContainer(size, stackSize);

            Assert.That(
                () => inventory.Add(item: default!, amount: 1),
                Throws.ArgumentNullException.With.Property("ParamName").EqualTo("item")
            );
        }

        [TestCase(0)]
        [TestCase(-1)]
        public void AddAmount_ZeroOrLessAmount_ThrowsArgumentOutOfRangeException(int amount)
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var inventory = this.inventoryFactory.EmptyContainer(size, stackSize);
            var item = this.itemFactory.CreateDefault();

            Assert.That(
                () => inventory.Add(item, amount),
                Throws.TypeOf<ArgumentOutOfRangeException>().With.Property("ParamName").EqualTo("amount")
            );
        }

        [Test]
        public void AddAmount_FullInventory_ThrowsInvalidOperationException()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var randomItem = this.itemFactory.CreateRandom();
            var inventory = this.inventoryFactory.FullContainer(size, stackSize, randomItem);

            var item = this.itemFactory.CreateDefault();
            var amount = this.random.Next(1, stackSize);


            Assert.That(
                () => inventory.Add(item, amount),
                Throws.InvalidOperationException.With.Message.EqualTo("The inventory is full")
            );
        }

        [Test]
        public void AddAmount_FullInventory_DoesNotCallOnAddEvent()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var randomItem = this.itemFactory.CreateRandom();
            var inventory = this.inventoryFactory.FullContainer(size, stackSize, randomItem);

            inventory.OnAdd += (sender, args) => Assert.Fail("OnAdd should not be called when inventory is full.");

            var item = this.itemFactory.CreateDefault();
            var amount = this.random.Next(1, stackSize);

            Assert.Throws<InvalidOperationException>(() => inventory.Add(item, amount));
        }

        [Test]
        public void AddAmount_FullInventory_DoesNotAddToInventory()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var randomItem = this.itemFactory.CreateRandom();
            var inventory = this.inventoryFactory.FullContainer(size, stackSize, randomItem);

            var item = this.itemFactory.CreateDefault();
            var amount = this.random.Next(1, stackSize);
            Assert.Throws<InvalidOperationException>(() => inventory.Add(item, amount));

            Assert.Multiple(() =>
            {
                Assert.That(inventory.GetSlots(),
                    Has.All.Matches<ILazyStackSlot<T>>(
                        slot => slot.IsFull && randomItem!.Equals(slot.GetContent())
                    )
                );
            });
        }

        [Test]
        public void AddAmount_EmptyInventory_AddsToFirstAvilableSlot()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var inventory = this.inventoryFactory.EmptyContainer(size, stackSize);

            var item = this.itemFactory.CreateDefault();
            var amount = this.random.Next(1, stackSize);
            inventory.Add(item, amount);

            Assert.Multiple(() =>
            {
                var slot = inventory.GetSlot(0);
                Assert.That(slot.GetContent(), Is.EqualTo(item));
                Assert.That(slot.IsEmpty, Is.False);
                Assert.That(slot.Amount, Is.EqualTo(amount));
            });
        }

        [Test]
        public void AddAmount_EmptyInventory_CallsOnAddEvent()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var inventory = this.inventoryFactory.EmptyContainer(size, stackSize);

            var item = this.itemFactory.CreateDefault();
            var amount = this.random.Next(1, stackSize);

            var raised = false;
            inventory.OnAdd += (sender, args) => {
                Assert.That(args.Data, Has.Count.EqualTo(1));
                Assert.Multiple(() =>
                {
                    var firstEvent = args.Data.FirstOrDefault();
                    Assert.That(firstEvent.Item, Is.EqualTo(item));
                    Assert.That(firstEvent.Index, Is.EqualTo(0));
                    Assert.That(firstEvent.Amount, Is.EqualTo(amount));
                });
                raised = true;
            };

            inventory.Add(item, amount);
        
            Assert.That(raised, Is.True, "OnAdd event was not raised");
        }

        [Test]
        public void AddAmount_NotEnoughCompatibleSpace_ThrowsInvalidOperationException()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var randomItems = this.itemFactory.CreateManyRandom(size - 1);
            var inventory = this.inventoryFactory.ShuffledItemsContainer(size, stackSize, randomItems);

            var item = this.itemFactory.CreateDefault();
            var amount = stackSize + this.random.Next(1, 5);

            Assert.That(
                () => inventory.Add(item, amount),
                Throws.InvalidOperationException.With.Message.EqualTo("There is not enough space to add the items.")
            );
        }

        [Test]
        public void AddAmount_NotEnoughCompatibleSpace_DoesNotAddToInventory()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var randomItems = this.itemFactory.CreateManyRandom(size - 1);
            var inventory = this.inventoryFactory.ShuffledItemsContainer(size, stackSize, randomItems);

            var item = this.itemFactory.CreateDefault();
            var originalAmounts = inventory.GetSlots().Select(slot => slot.Amount).ToArray();
            var amount = stackSize + this.random.Next(1, 5);
            Assert.Throws<InvalidOperationException>(() => inventory.Add(item, amount));

            Assert.That(inventory.GetSlots().Select(slot => slot.Amount), Is.EqualTo(originalAmounts));
        }

        [Test]
        public void AddAmount_NotEnoughCompatibleSpace_DoesNotCallOnAddEvent()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var randomItems = this.itemFactory.CreateManyRandom(size - 1);
            var inventory = this.inventoryFactory.ShuffledItemsContainer(size, stackSize, randomItems);
            inventory.OnAdd += (sender, args) => Assert.Fail("OnAdd should not be called without enough compatible space.");

            var item = this.itemFactory.CreateDefault();
            var amount = stackSize + this.random.Next(1, 5);

            Assert.Throws<InvalidOperationException>(() => inventory.Add(item, amount));
        }

        [Test]
        public void AddAmount_CompatibleSpaceAcrossSlots_AddsAllItems()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var inventory = this.inventoryFactory.EmptyContainer(size, stackSize);
            var item = this.itemFactory.CreateDefault();
            var amountPerSlot = Math.Max(1, stackSize / 2);
            inventory.AddAt(item, 0, amountPerSlot);
            inventory.AddAt(item, 1, amountPerSlot);
            var amount = (stackSize - amountPerSlot) * 2;

            inventory.Add(item, amount);

            Assert.That(inventory.GetSlots().Take(2), Has.All.Property("Amount").EqualTo(stackSize));
        }

        [Test]
        public void Add_WithAmount_SuccessfullyAddedItems_ReturnsZero()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var inventory = this.inventoryFactory.EmptyContainer(size, stackSize);
            var item = this.itemFactory.CreateDefault();
            var amount = this.random.Next(1, 5);

            var result = inventory.Add(item, amount);

            Assert.That(result, Is.Zero);
        }
    }
}
