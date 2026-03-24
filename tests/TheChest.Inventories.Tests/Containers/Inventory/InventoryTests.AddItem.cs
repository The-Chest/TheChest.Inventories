using TheChest.Inventories.Slots.Interfaces;
using TheChest.Tests.Common.Extensions;

namespace TheChest.Inventories.Tests.Containers
{
    public partial class InventoryTests<T>
    {
        [Test]
        public void AddItem_NullItem_ThrowsArgumentNullException()
        {
            var inventory = this.containerFactory.EmptyContainer();
            Assert.That(() => inventory.Add(item: default!), Throws.ArgumentNullException);
        }

        [Test]
        public void AddItem_EmptyInventory_AddsToFirstSlot()
        {
            var inventory = this.containerFactory.EmptyContainer();

            var item = this.itemFactory.CreateDefault();
            inventory.Add(item);

            Assert.Multiple(() =>
            {
                var firstSlot = inventory.GetSlot<T>(0);

                Assert.That(firstSlot, Is.Not.Null);
                Assert.That(firstSlot!.IsEmpty, Is.False);
                Assert.That(firstSlot.GetContent<T>(), Is.EqualTo(item));
            });
        }

        [Test]
        public void AddItem_EmptyInventory_CallsOnAddEvent()
        {
            var inventory = this.containerFactory.EmptyContainer();

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
            var size = this.random.Next(10, 20);
            var items = this.itemFactory.CreateMany(size / 2);
            var inventory = this.containerFactory.ShuffledItemsContainer(size, items);
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
            var size = this.random.Next(10, 20);
            var items = this.itemFactory.CreateDefault();
            var inventory = this.containerFactory.FullContainer(size, items);

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
            var size = this.random.Next(10, 20);
            var items = this.itemFactory.CreateDefault();
            var inventory = this.containerFactory.FullContainer(size, items);

            inventory.OnAdd += (sender, args) => Assert.Fail("OnAdd should not be called if inventory is full");
            
            var item = this.itemFactory.CreateRandom();
            inventory.Add(item);
        }
    }
}
