using TheChest.Core.Slots.Interfaces;
using TheChest.Tests.Common.Extensions.Containers;
using TheChest.Tests.Common.Extensions.Slots;

namespace TheChest.Inventories.Tests.Containers.StackInventory
{
    public partial class StackInventoryTests<T>
    {
        [Test]
        public void AddItems_AddingEmptyArray_ThrowsArgumentException()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var inventory = this.inventoryFactory.EmptyContainer(size, stackSize);

            var items = Array.Empty<T>();
            Assert.That(() => inventory.Add(items), Throws.ArgumentException);
        }

        [Test]
        [TheChest.Tests.Common.Attributes.IgnoreIfValueType]
        public void AddItems_NullItems_ThrowsArgumentNullException()
        {
            var inventory = this.inventoryFactory.EmptyContainer();
            Assert.That(() => inventory.Add(null!), Throws.ArgumentNullException);
        }

        [Test]
        [TheChest.Tests.Common.Attributes.IgnoreIfValueType]
        public void AddItems_ItemsContainsNull_ThrowsArgumentNullException()
        {
            var inventory = this.inventoryFactory.EmptyContainer(5, 5);
            var validItem = this.itemFactory.CreateDefault();
            var items = new T[] { validItem, default!, validItem };

            Assert.That(() => inventory.Add(items), Throws.ArgumentNullException);
        }

        [Test]
        public void AddItems_EmptyInventory_AddsToFirstSlot()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var inventory = this.inventoryFactory.EmptyContainer(size, stackSize);

            var amount = this.random.Next(1, stackSize);
            var items = this.itemFactory.CreateMany(amount);
            inventory.Add(items);

            Assert.That(inventory.GetItems(0).Where(x => x is not null), Is.EquivalentTo(items));
        }

        [Test]
        public void AddItems_EmptyInventory_CallsOnAddEvent()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var inventory = this.inventoryFactory.EmptyContainer(size, stackSize);

            var items = this.itemFactory.CreateMany(stackSize);
            var raised = false;
            inventory.OnAdd += (sender, args) =>
            {
                Assert.Multiple(() =>
                {
                    var firstEvent = args.Data.First();
                    Assert.That(args.Data, Has.Count.EqualTo(1));
                    Assert.That(firstEvent.Items, Has.Length.EqualTo(items.Length).And.EquivalentTo(items));
                    Assert.That(firstEvent.Index, Is.EqualTo(0));
                });
                raised = true;
            };
            inventory.Add(items);

            Assert.That(raised, Is.True, "OnAdd event was not raised");
        }

        [Test]
        public void AddItems_SlotWithSameItem_AddsToSlotWithItemFirst()
        {
            var item = this.itemFactory.CreateDefault();
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var inventory = this.inventoryFactory.ShuffledItemsContainer(size, stackSize, item);
            inventory.Get(item, stackSize - 1);

            var amount = stackSize;
            var items = this.itemFactory.CreateMany(amount);
            inventory.Add(items);

            Assert.That(
                inventory.GetSlots(),
                Has.One.Matches<IStackSlot<T>>(
                    x => x.GetContents()!.Contains(item) && x.Amount == stackSize
                )
            );
        }

        [Test]
        public void AddItems_SlotWithSameItem_CallsOnAddEvent()
        {
            var item = this.itemFactory.CreateDefault();
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var inventory = this.inventoryFactory.ShuffledItemsContainer(size, stackSize, item);
            inventory.Get(item, stackSize - 1);

            var amount = stackSize;
            var items = this.itemFactory.CreateMany(amount);

            var raised = false;
            inventory.OnAdd += (sender, args) =>
            {
                Assert.That(args.Data, Has.Count.EqualTo(2));
                Assert.Multiple(() =>
                {
                    var firstEvent = args.Data.First();
                    Assert.That(firstEvent.Items, Has.Length.EqualTo(stackSize - 1).And.EqualTo(items[..^1]));
                });
                Assert.Multiple(() =>
                {
                    var secondEvent = args.Data.Skip(1).First();
                    Assert.That(secondEvent.Items, Has.Length.EqualTo(1).And.All.EqualTo(items[^1]));
                });
                raised = true;
            };
            inventory.Add(items);

            Assert.That(raised, Is.True, "OnAdd event was not raised");
        }

        [Test]
        public void AddItems_FullSlotWithSameItem_AddsToSlotWithItemFirst()
        {
            var item = this.itemFactory.CreateDefault();
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var inventory = this.inventoryFactory.ShuffledItemsContainer(size, stackSize, item);

            var amount = this.random.Next(1, stackSize);
            var items = this.itemFactory.CreateMany(stackSize + amount);
            inventory.Add(items);

            var inventorySlots = inventory.GetSlots();
            Assert.That(
                inventorySlots,
                Has.Exactly(2).Matches<IStackSlot<T>>(
                    x => x.Amount == stackSize && x.GetContents().Contains(item)
                )
            );
            Assert.That(
                inventorySlots,
                Has.Exactly(1).Matches<IStackSlot<T>>(
                    x => x.Amount == amount && x.GetContents().Contains(item)
                )
            );
        }

        [Test]
        public void AddItems_SlotWithSameItem_AddsToFirstAvailableSlot()
        {
            var item = this.itemFactory.CreateDefault();
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var inventory = this.inventoryFactory.ShuffledItemsContainer(size, stackSize, item);
            inventory.Get(item, stackSize - 1);

            var amount = stackSize;
            var items = this.itemFactory.CreateMany(amount);
            inventory.Add(items);

            Assert.That(
                inventory.GetSlots(),
                Has.One.Matches<IStackSlot<T>>(
                    x => x.Amount == 1 && x.GetContents<T>()!.Contains(item)
                )
            );
        }

        [Test]
        public void AddItems_EmptyInventory_BiggerAmountThanSlotSize_CallsOnAddEventOnTwoSlots()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var inventory = this.inventoryFactory.EmptyContainer(size, stackSize);

            var items = this.itemFactory.CreateMany(stackSize * 2);
            var raised = false;
            inventory.OnAdd += (sender, args) =>
            {
                Assert.That(args.Data, Has.Count.EqualTo(2));

                Assert.Multiple(() =>
                {
                    var firstEvent = args.Data.First();
                    Assert.That(firstEvent.Items, Has.Length.EqualTo(stackSize).And.EquivalentTo(items.Take(stackSize)));
                    Assert.That(firstEvent.Index, Is.EqualTo(0));
                });

                Assert.Multiple(() =>
                {
                    var secondEvent = args.Data.Skip(1).First();
                    Assert.That(secondEvent.Items, Has.Length.EqualTo(stackSize).And.EquivalentTo(items.Skip(stackSize)));
                    Assert.That(secondEvent.Index, Is.EqualTo(1));
                });

                raised = true;
            };

            inventory.Add(items);

            Assert.That(raised, Is.True, "OnAdd event was not raised");
        }

        [Test]
        public void AddItems_EmptyInventory_BiggerAmountThanSlotSize_AddsToTwoAvailableSlots()
        {
            var items = this.itemFactory.CreateMany(20);
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var inventory = this.inventoryFactory.EmptyContainer(size);

            inventory.Add(items);

            Assert.Multiple(() =>
            {
                Assert.That(inventory.GetItems(0), Is.EqualTo(items.Take(10)));
                Assert.That(inventory.GetItems(1), Is.EqualTo(items.Skip(10)));
            });
        }

        [Test]
        public void AddItems_FullInventory_ThrowsInvalidOperationException()
        {
            var slotItem = this.itemFactory.CreateRandom();
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var items = this.itemFactory.CreateMany(size);
            var inventory = this.inventoryFactory.FullContainer(size, 2, slotItem);
            Assert.That(() => inventory.Add(items), Throws.InvalidOperationException);
        }

        [Test]
        public void AddItems_FullInventory_ThrowsAndDoNotCallOnAddEvent()
        {
            var slotItem = this.itemFactory.CreateRandom();
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var items = this.itemFactory.CreateMany(size);
            var inventory = this.inventoryFactory.FullContainer(size, 2, slotItem);

            inventory.OnAdd += (sender, args) => Assert.Fail("OnAdd event should not be called when item is not possible to add");

            Assert.That(() => inventory.Add(items), Throws.InvalidOperationException);
        }
    }
}
