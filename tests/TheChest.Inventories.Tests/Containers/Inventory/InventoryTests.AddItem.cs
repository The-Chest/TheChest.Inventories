using TheChest.Inventories.Slots.Interfaces;

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
                var firstSlot = inventory[0];

                Assert.That(firstSlot.IsEmpty, Is.False);
                Assert.That(firstSlot.GetContent(), Is.EqualTo(item));
            });
        }

        [Test]
        public void AddItem_EmptyInventory_CallsOnAddEvent()
        {
            var inventory = this.containerFactory.EmptyContainer();

            var item = this.itemFactory.CreateDefault();
            inventory.OnAdd += (sender, args) =>
            {
                Assert.Multiple(() =>
                {
                    Assert.That(sender, Is.EqualTo(inventory));
                    Assert.That(args.Data.Select(x => x.Item), Has.All.EqualTo(item));
                });
            }; 
            inventory.Add(item);
        }

        [Test]
        public void AddItem_EmptyInventory_ReturnsTrue()
        {
            var inventory = this.containerFactory.EmptyContainer();

            var item = this.itemFactory.CreateDefault();
            var result = inventory.Add(item);

            Assert.That(result, Is.True);
        }

        [Test]
        public void AddItem_InventoryWithItems_AddsToFirstAvailableSlot()
        {
            var size = this.random.Next(10, 20);
            var items = this.itemFactory.CreateMany(size / 2);
            var inventory = this.containerFactory.ShuffledItemsContainer(size, items);
            var firstAvailableSlot = inventory.GetSlots()?.First(slot => slot.IsEmpty);

            var item = this.itemFactory.CreateDefault();
            inventory.Add(item);

            Assert.That(firstAvailableSlot, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(firstAvailableSlot.IsEmpty, Is.False);
                Assert.That(firstAvailableSlot.GetContent(), Is.EqualTo(item));
            });
        }

        [Test]
        public void AddItem_FullInventory_DoNotAddItem()
        {
            var size = this.random.Next(10, 20);
            var items = this.itemFactory.CreateDefault();
            var inventory = this.containerFactory.FullContainer(size, items);

            var item = this.itemFactory.CreateRandom();
            inventory.Add(item);

            Assert.That(
                inventory.GetSlots(), 
                Is.All.Matches<IInventorySlot<T>>(
                    x => x.IsFull && !x.GetContent()!.Equals(item)
                )
            );
        }

        [Test]
        public void AddItem_FullInventory_DoesNotCallOnAddEvent()
        {
            var size = this.random.Next(10, 20);
            var items = this.itemFactory.CreateDefault();
            var inventory = this.containerFactory.FullContainer(size, items);

            var item = this.itemFactory.CreateRandom();
            inventory.OnAdd += (sender, args) => Assert.Fail("OnAdd should not be called if inventory is full");
            inventory.Add(item);
        }

        [Test]
        public void AddItem_FullInventory_ReturnsFalse()
        {
            var size = this.random.Next(10, 20);
            var items = this.itemFactory.CreateDefault();
            var inventory = this.containerFactory.FullContainer(size, items);

            var item = this.itemFactory.CreateDefault();

            var result = inventory.Add(item);
        
            Assert.That(result, Is.False);
        }
    }
}
