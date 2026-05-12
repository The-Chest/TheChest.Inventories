using TheChest.Core.Slots.Interfaces;
using TheChest.Tests.Common.Extensions.Containers;
using TheChest.Tests.Common.Extensions.Slots;

namespace TheChest.Inventories.Tests.Containers.LazyStackInventory
{
    public partial class LazyStackInventoryTests<T>
    {
        [Test]
        public void Add_NullItem_ThrowsArgumentNullException()
        {
            var inventory = this.inventoryFactory.EmptyContainer();
            Assert.That(
                () => inventory.Add(default!), 
                Throws.ArgumentNullException.With.Property("ParamName").EqualTo("item")
            );
        }

        [Test]
        public void Add_FullInventory_ThrowsInvalidOperationException()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var items = this.itemFactory.CreateRandom();
            var inventory = this.inventoryFactory.FullContainer(size, stackSize, items);

            var item = this.itemFactory.CreateDefault();

            Assert.That(
                () => inventory.Add(item),
                Throws.InvalidOperationException.With.Message.EqualTo("The inventory is full")
            );
        }

        [Test]
        public void Add_FullInventory_DoesNotCallOnAddEvent()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var items = this.itemFactory.CreateRandom();
            var inventory = this.inventoryFactory.FullContainer(size, stackSize, items);

            inventory.OnAdd += (sender, args) => Assert.Fail("OnAdd event should not be called when inventory is full.");

            var item = this.itemFactory.CreateDefault();
            Assert.Throws<InvalidOperationException>(() => inventory.Add(item));
        }

        [Test]
        public void Add_EmptyInventory_AddsToFirstAvailableSlot()
        {
            var inventory = this.inventoryFactory.EmptyContainer();

            var item = this.itemFactory.CreateDefault();
            inventory.Add(item);
            
            Assert.Multiple(() =>
            {
                var slot = inventory.GetSlot(0);

                Assert.That(slot!.GetContent(), Is.EqualTo(item));
                Assert.That(slot!.IsEmpty, Is.False);
                Assert.That(slot!.Amount, Is.EqualTo(1));
            });
        }

        [Test]
        public void Add_EmptyInventory_CallsOnAddEvent()
        {
            var inventory = this.inventoryFactory.EmptyContainer();
            var item = this.itemFactory.CreateDefault();

            var raised = false;
            inventory.OnAdd += (sender, args) =>
            {
                Assert.That(args.Data, Has.Count.EqualTo(1));
                Assert.Multiple(() =>
                {
                    var firstEvent = args.Data.FirstOrDefault();
                    Assert.That(firstEvent.Item, Is.EqualTo(item));
                    Assert.That(firstEvent.Index, Is.EqualTo(0));
                    Assert.That(firstEvent.Amount, Is.EqualTo(1));
                });
                raised = true;
            };  

            inventory.Add(item);

            Assert.That(raised, Is.True, "OnAdd event was not raised");
        }

        [Test]
        public void Add_InventoryWithDifferentItems_AddsToFirstAvailableSlot()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var items = this.itemFactory.CreateManyRandom(size - 1);
            var inventory = this.inventoryFactory.ShuffledItemsContainer(size, stackSize, items);

            var item = this.itemFactory.CreateDefault();
            inventory.Add(item);

            Assert.Multiple(() =>
            {
                Assert.That(
                    inventory.GetSlots(),
                    Has.One.Matches<ILazyStackSlot<T>>(
                        slot =>
                            item!.Equals(slot.GetContent()) && 
                            slot.Amount == 1
                    )
                );
            });
        }

        [Test]
        public void Add_InventoryWithDifferentItems_CallsOnAddEvent()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var items = this.itemFactory.CreateManyRandom(size - 1);
            var inventory = this.inventoryFactory.ShuffledItemsContainer(size, stackSize, items);

            var emptySlotIndex = Array.IndexOf(
                inventory.GetSlots(),
                inventory.GetSlots().First(slot => slot.IsEmpty)
            );
            var item = this.itemFactory.CreateDefault();
            var raised = false;
            inventory.OnAdd += (sender, args) =>
            {
                Assert.That(args.Data, Has.Count.EqualTo(1));
                Assert.Multiple(() =>
                {
                    var firstEvent = args.Data.FirstOrDefault();
                    Assert.That(firstEvent.Item, Is.EqualTo(item));
                    Assert.That(firstEvent.Index, Is.EqualTo(emptySlotIndex));
                    Assert.That(firstEvent.Amount, Is.EqualTo(1));
                });
                raised = true;
            };
            inventory.Add(item);
            Assert.That(raised, Is.True, "OnAdd event was not raised");
        }
    }
}
