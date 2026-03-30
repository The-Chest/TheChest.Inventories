using TheChest.Core.Slots.Interfaces;
using TheChest.Inventories.Tests.Common.Extensions.Containers;
using TheChest.Tests.Common.Extensions;

namespace TheChest.Inventories.Tests.Containers
{
    public partial class StackInventoryTests<T>
    {
        [Test]
        public void AddItem_EmptyInventory_AddsToFirstEmptySlot()
        {
            var item = this.itemFactory.CreateDefault();
            var inventory = this.inventoryFactory.EmptyContainer();

            inventory.Add(item);

            Assert.Multiple(() =>
            {
                var firstSlot = inventory.GetSlot(0);
                Assert.That(firstSlot!.GetContents<T>(), Has.One.EqualTo(item));
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
        public void AddItem_InventoryWithItems_AddsToAvailableSlot()
        {
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var items = this.itemFactory.CreateManyRandom(stackSize);
            var inventory = this.inventoryFactory.ShuffledItemsContainer(size, stackSize, items);

            var item = this.itemFactory.CreateDefault();
            inventory.Add(item);

            Assert.That(
                inventory.GetSlots()?.Any(x => x.GetContents<T>()?.Contains(item) ?? false),
                Is.True
            );
        }

        [Test]
        public void AddItem_InventoryWithItems_CallsOnAddEvent()
        {
            var item = this.itemFactory.CreateDefault();
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
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
            var amount = this.random.Next(2, 10);
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var inventory = this.inventoryFactory.ShuffledItemsContainer(size, amount, item);
            inventory.Get(item, amount - 1);

            inventory.Add(item);

            Assert.That(
                inventory.GetSlots(),
                Has.One.Matches<IStackSlot<T>>(
                    x => x.Amount == 2 && x.GetContents<T>()!.Contains(item)
                )
             );
        }

        [Test]
        public void AddItem_InventoryWithFullSlotWithSameItem_AddsToFirstAvailableSlot()
        {
            var item = this.itemFactory.CreateDefault();
            var amount = this.random.Next(2, 10);
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var inventory = this.inventoryFactory.FullContainer(size, amount, item);
            var expectedIndex = this.random.Next(0, size);
            inventory.GetAll(expectedIndex);

            inventory.Add(item);

            Assert.Multiple(() =>
            {
                var slot = inventory.GetSlot(expectedIndex);
                Assert.That(slot.GetContents<T>(), Has.One.EqualTo(item));
                Assert.That(slot.Amount, Is.EqualTo(1));
            });
        }

        [Test]
        public void AddItem_InventoryWithSameItem_AddsToSlotWithItem()
        {
            var item = this.itemFactory.CreateDefault();
            var items = this.itemFactory.CreateManyRandom(19)
                .Append(this.itemFactory.CreateDefault())
                .ToArray();
            var inventory = this.inventoryFactory.ShuffledItemsContainer(20, 10, items);
            var slots = inventory.GetSlots()!;
            var slotIndex = Array.IndexOf(slots, slots.First(x => x.GetContents<T>()?.Contains(item) ?? false));
            inventory.Get(slotIndex, 9);

            inventory.Add(item);

            Assert.Multiple(() =>
            {
                var slot = inventory.GetSlot(slotIndex);
                Assert.That(slot.GetContents<T>(), Has.Exactly(2).EqualTo(item));
                Assert.That(slot.Amount, Is.EqualTo(2));
            });
        }

        [Test]
        public void AddItem_FullInventory_DoesNotCallOnAddEvent()
        {
            var item = this.itemFactory.CreateDefault();
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var inventory = this.inventoryFactory.FullContainer(size, size, this.itemFactory.CreateRandom());

            inventory.OnAdd += (sender, args) => Assert.Fail("OnAdd event should not be called when item is not possible to add");

            inventory.Add(item);
        }
    }
}
