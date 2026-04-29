using TheChest.Core.Slots.Interfaces;
using TheChest.Tests.Common.Extensions.Containers;
using TheChest.Tests.Common.Extensions.Slots;

using TheChest.Tests.Common.Attributes;
namespace TheChest.Inventories.Tests.Containers.LazyStackInventory
{
    public partial class LazyStackInventoryTests<T>
    {
        [Test]
        [IgnoreIfValueType]
        public void Add_WithAmount_NullItem_ThrowsArgumentNullException()
        {
            var inventory = this.inventoryFactory.EmptyContainer();

            Assert.Throws<ArgumentNullException>(() => inventory.Add(item: default!, amount: 1));
        }

        [TestCase(0)]
        [TestCase(-1)]
        public void Add_WithAmount_ZeroOrLessAmount_ThrowsArgumentOutOfRangeException(int amount)
        {
            var inventory = this.inventoryFactory.EmptyContainer();
            var item = this.itemFactory.CreateDefault();

            Assert.Throws<ArgumentOutOfRangeException>(() => inventory.Add(item, amount));
        }

        [Test]
        public void Add_WithAmount_EmptyInventory_AddsToFirstAvilableSlot()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
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
        public void Add_WithAmount_EmptyInventory_CallsOnAddEvent()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
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
        public void Add_WithAmount_FullInventory_DoesNotCallOnAddEvent()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var randomItem = this.itemFactory.CreateRandom();
            var inventory = this.inventoryFactory.FullContainer(size, stackSize, randomItem);
            
            inventory.OnAdd += (sender, args) => Assert.Fail("OnAdd should not be called when inventory is full.");
            
            var item = this.itemFactory.CreateDefault();
            var amount = this.random.Next(1, stackSize);
            inventory.Add(item, amount);
        }

        [Test]
        public void Add_WithAmount_FullInventory_DoesNotAddToInventory()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var randomItem = this.itemFactory.CreateRandom();
            var inventory = this.inventoryFactory.FullContainer(size, stackSize, randomItem);

            var item = this.itemFactory.CreateDefault();
            var amount = this.random.Next(1, stackSize);
            inventory.Add(item, amount);

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
        public void Add_WithAmount_NotAllItemsBeAdded_CallsOnAddEvent()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var randomItem = this.itemFactory.CreateManyRandom(size - 1);
            var inventory = this.inventoryFactory.ShuffledItemsContainer(size, stackSize, randomItem);

            var item = this.itemFactory.CreateDefault();
            var amount = stackSize + this.random.Next(1, 5);

            var raised = false;
            inventory.OnAdd += (sender, args) =>
            {
                Assert.That(args.Data, Has.Count.EqualTo(1));
                Assert.Multiple(() =>
                {
                    var firstEvent = args.Data.FirstOrDefault();
                    Assert.That(firstEvent.Item, Is.EqualTo(item));
                    Assert.That(firstEvent.Index, Is.InRange(0, size));
                    Assert.That(firstEvent.Amount, Is.EqualTo(stackSize));
                });
                raised = true;
            };

            inventory.Add(item, amount);

            Assert.That(raised, Is.True, "OnAdd event was not raised");
        }

        [Test]
        public void Add_WithAmount_NotAllItemsBeAdded_AddsToFirstAvailableSlot()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var randomItems = this.itemFactory.CreateManyRandom(size - 1);
            var inventory = this.inventoryFactory.ShuffledItemsContainer(size, stackSize, randomItems);

            var item = this.itemFactory.CreateDefault();
            var amount = stackSize + this.random.Next(1, 5);
            inventory.Add(item, amount);

            Assert.Multiple(() =>
            {
                Assert.That(inventory.GetSlots(),
                    Has.One.Matches<ILazyStackSlot<T>>(
                        slot =>
                            item!.Equals(slot.GetContent()) &&
                            slot.Amount == amount - (amount - stackSize)
                    )
                );
            });
        }
    }
}
