using TheChest.Inventories.Slots.Interfaces;
using TheChest.Tests.Common.Extensions.Containers;
using TheChest.Tests.Common.Extensions.Slots;

namespace TheChest.Inventories.Tests.Containers.Inventory
{
    public partial class InventoryTests<T>
    {
        [Test]
        public void AddItem_NullItem_ThrowsArgumentNullException()
        {
            var inventory = this.inventoryFactory.EmptyContainer();
            Assert.That(() => inventory.Add(item: default!), Throws.ArgumentNullException);
        }

        [Test]
        public void AddItem_EmptyInventory_AddsToFirstSlot()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var inventory = this.inventoryFactory.EmptyContainer(size);

            var item = this.itemFactory.CreateDefault();
            inventory.Add(item);

            Assert.Multiple(() =>
            {
                var randomIndex = this.random.Next(0, size);
                var firstSlot = inventory.GetSlot<T>(randomIndex);

                Assert.That(firstSlot, Is.Not.Null);
                Assert.That(firstSlot!.IsEmpty, Is.False);
                Assert.That(firstSlot.GetContent<T>(), Is.EqualTo(item));
            });
        }

        [Test]
        public void AddItem_EmptyInventory_CallsOnAddEvent()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var inventory = this.inventoryFactory.EmptyContainer(size);

            var item = this.itemFactory.CreateDefault();

            var raised = false;
            inventory.OnAdd += (sender, args) =>
            {
                Assert.Multiple(() =>
                {
                    Assert.That(sender, Is.EqualTo(inventory));
                    Assert.That(args.Data.Select(x => x.Item), Has.All.EqualTo(item));
                });
                raised = true;
            }; 

            inventory.Add(item);

            Assert.That(raised, Is.True, "OnAdd event was not raised");
        }

        [Test]
        public void AddItem_InventoryWithItems_AddsToFirstAvailableSlot()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var items = this.itemFactory.CreateMany(size / 2);
            var inventory = this.inventoryFactory.ShuffledItemsContainer(size, items);
            var firstAvailableSlot = inventory.GetSlots<T>()?.First(slot => slot.IsEmpty);

            var item = this.itemFactory.CreateDefault();
            inventory.Add(item);

            Assert.That(firstAvailableSlot, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(firstAvailableSlot.IsEmpty, Is.False);
                Assert.That(firstAvailableSlot.GetContent<T>(), Is.EqualTo(item));
            });
        }

        [Test]
        public void AddItem_FullInventory_DoesNotAddItem()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var items = this.itemFactory.CreateDefault();
            var inventory = this.inventoryFactory.FullContainer(size, items);

            var item = this.itemFactory.CreateRandom();
            inventory.Add(item);

            Assert.That(
                inventory.GetSlots<T>(), 
                Is.All.Matches<IInventorySlot<T>>(
                    x => x.IsFull && !x.GetContent<T>()!.Equals(item)
                )
            );
        }

        [Test]
        public void AddItem_FullInventory_DoesNotCallOnAddEvent()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var items = this.itemFactory.CreateDefault();
            var inventory = this.inventoryFactory.FullContainer(size, items);

            inventory.OnAdd += (sender, args) => Assert.Fail("OnAdd should not be called if inventory is full");
            
            var item = this.itemFactory.CreateRandom();
            inventory.Add(item);
        }
    }
}
