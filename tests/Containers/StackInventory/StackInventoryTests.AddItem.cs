using TheChest.Core.Slots.Interfaces;
using TheChest.Tests.Common.Extensions.Containers;
using TheChest.Tests.Common.Extensions.Slots;

namespace TheChest.Inventories.Tests.Containers.StackInventory
{
    public partial class StackInventoryTests<T>
    {
        [Test]
        public void AddItem_FullInventory_ThrowsInvalidOperationException()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var randomItem = this.itemFactory.CreateRandom();
            var inventory = this.inventoryFactory.FullContainer(size, stackSize, randomItem);

            var item = this.itemFactory.CreateDefault();
            Assert.That(
                () => inventory.Add(item),
                Throws.InvalidOperationException.With.Message.EqualTo("The inventory is full")
            );
        }
        [Test]
        public void AddItem_NullItem_ThrowsArgumentNullException()
        {
            var inventory = this.inventoryFactory.EmptyContainer();
            Assert.That(
                () => inventory.Add(default(T)!),
                Throws.ArgumentNullException.With.Property("ParamName").EqualTo("item")
            );
        }

        [Test]
        public void AddItem_EmptyInventory_AddsToFirstEmptySlot()
        {
            var item = this.itemFactory.CreateDefault();
            var inventory = this.inventoryFactory.EmptyContainer();

            inventory.Add(item);

            Assert.Multiple(() =>
            {
                var firstSlot = inventory.GetSlot(0);
                Assert.That(firstSlot!.GetContents(), Has.One.EqualTo(item));
                Assert.That(firstSlot!.Amount, Is.EqualTo(1));
            });
        }
        [Test]
        public void AddItem_EmptyInventory_CallsOnAddEvent()
        {
            var item = this.itemFactory.CreateDefault();
            var inventory = this.inventoryFactory.EmptyContainer();

            var raised = false;
            inventory.OnAdd += (sender, args) =>
            {
                Assert.Multiple(() =>
                {
                    var firstEvent = args.Data.First();
                    Assert.That(args.Data, Has.Count.EqualTo(1));
                    Assert.That(firstEvent.Items, Has.Length.EqualTo(1).And.All.EqualTo(item));
                    Assert.That(firstEvent.Index, Is.EqualTo(0));
                });
                raised = true;
            };
            inventory.Add(item);

            Assert.That(raised, Is.True, "OnAdd event was not raised");
        }

        [Test]
        public void AddItem_InventoryWithDifferentItems_AddsToAvailableSlot()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var items = this.itemFactory.CreateManyRandom(stackSize);
            var inventory = this.inventoryFactory.ShuffledItemsContainer(size, stackSize, items);

            var item = this.itemFactory.CreateDefault();
            inventory.Add(item);

            Assert.That(
                inventory.GetSlots()?.Any(x => x.GetContents()?.Contains(item) ?? false),
                Is.True
            );
        }
        [Test]
        public void AddItem_InventoryWithDifferentItems_CallsOnAddEvent()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var item = this.itemFactory.CreateDefault();
            var inventory = this.inventoryFactory.FullContainer(size, stackSize, item);

            var expectedIndex = this.random.Next(0, size);
            inventory.GetAll(expectedIndex);

            var raised = false;
            inventory.OnAdd += (sender, args) =>
            {
                Assert.That(args.Data, Has.Count.EqualTo(1));
                Assert.Multiple(() =>
                {
                    var firstEvent = args.Data.First();
                    Assert.That(firstEvent.Items, Has.Length.EqualTo(1).And.All.EqualTo(item));
                    Assert.That(firstEvent.Index, Is.EqualTo(expectedIndex));
                });
                raised = true;
            };
            inventory.Add(item);

            Assert.That(raised, Is.True);
        }

        [Test]
        public void AddItem_InventoryWithSameItem_AddsToAvailableSlotWithSameItem()
        {
            var item = this.itemFactory.CreateDefault();
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var inventory = this.inventoryFactory.ShuffledItemsContainer(size, stackSize, item);
            inventory.Get(item, stackSize - 1);

            inventory.Add(item);

            Assert.That(
                inventory.GetSlots(),
                Has.One.Matches<IStackSlot<T>>(
                    x => x.Amount == 2 && x.GetContents()!.Contains(item)
                )
             );
        }
        [Test]
        public void AddItem_InventoryWithSameItem_CallsOnAddEvent()
        {
            var item = this.itemFactory.CreateDefault();
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var inventory = this.inventoryFactory.ShuffledItemsContainer(size, stackSize, item);

            var slots = inventory.GetSlots();
            var slotIndex = Array.IndexOf(slots, slots.First(x => x.GetContents()?.Contains(item) ?? false));
            inventory.Get(slotIndex, stackSize - 1);
            
            var raised = false;
            inventory.OnAdd += (sender, args) =>
            {
                Assert.That(args.Data, Has.Count.EqualTo(1));
                Assert.Multiple(() =>
                {
                    var firstEvent = args.Data.First();
                    Assert.That(firstEvent.Items, Has.Length.EqualTo(1).And.All.EqualTo(item));
                    Assert.That(firstEvent.Index, Is.EqualTo(slotIndex));
                });
                raised = true;
            };
            
            inventory.Add(item);
            
            Assert.That(raised, Is.True);
        }

        [Test]
        public void AddItem_InventoryWithFullSlotWithSameItem_AddsToFirstAvailableSlot()
        {
            var item = this.itemFactory.CreateDefault();
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var inventory = this.inventoryFactory.FullContainer(size, stackSize, item);
            var expectedIndex = this.random.Next(0, size);
            inventory.GetAll(expectedIndex);

            inventory.Add(item);

            Assert.Multiple(() =>
            {
                var slot = inventory.GetSlot(expectedIndex);
                Assert.That(slot.GetContents(), Has.One.EqualTo(item));
                Assert.That(slot.Amount, Is.EqualTo(1));
            });
        }
        [Test]
        public void AddItem_InventoryWithFullSlotWithSameItem_CallsOnAddEvent()
        {
            var item = this.itemFactory.CreateDefault();
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var inventory = this.inventoryFactory.FullContainer(size, stackSize, item);
            var expectedIndex = this.random.Next(0, size);
            inventory.GetAll(expectedIndex);
            
            var raised = false;
            inventory.OnAdd += (sender, args) =>
            {
                Assert.That(args.Data, Has.Count.EqualTo(1));
                Assert.Multiple(() =>
                {
                    var firstEvent = args.Data.First();
                    Assert.That(firstEvent.Items, Has.Length.EqualTo(1).And.All.EqualTo(item));
                    Assert.That(firstEvent.Index, Is.EqualTo(expectedIndex));
                });
                raised = true;
            };
            
            inventory.Add(item);
            
            Assert.That(raised, Is.True);
        }
    }
}
