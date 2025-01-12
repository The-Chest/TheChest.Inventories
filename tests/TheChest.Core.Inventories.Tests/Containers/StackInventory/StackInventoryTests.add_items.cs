namespace TheChest.Core.Inventories.Tests.Containers
{
    public partial class StackInventoryTests<T>
    {
        [Test]
        public void AddItems_AddingEmptyArray_ThrowsArgumentException()
        {
            var items = Array.Empty<T>();

            var inventory = this.containerFactory.EmptyContainer();

            Assert.That(
                () => inventory.Add(items), 
                Throws.ArgumentException.
                With.Message.EqualTo("No items to add")
            );
        }

        [Test]
        public void AddItems_EmptyInventory_ReturnsEmptyArray()
        {
            var items = this.itemFactory.CreateMany(20);
            var inventory = this.containerFactory.EmptyContainer();

            var result = inventory.Add(items);

            Assert.That(result, Is.Empty);
        }

        [Test]
        public void AddItems_EmptyInventory_AddingDifferentItems_AddsEachInDifferentSlots()
        {
            var items = this.itemFactory.CreateManyRandom(20);
            var inventory = this.containerFactory.EmptyContainer();

            inventory.Add(items);

            Assert.That(inventory.Slots.SelectMany(x => x.Content), Is.EqualTo(items));
        }

        [Test]
        [Ignore("Find a better way to instantiate an inventory")]
        public void AddItems_EmptyInventory_AddsToFirstSlot()
        {
            var items = this.itemFactory.CreateMany(20);
            var inventory = this.containerFactory.EmptyContainer();

            inventory.Add(items);

            Assert.That(inventory[0].Content, Is.EqualTo(items));
        }

        [Test]
        [Ignore("Find a better way to instantiate an inventory")]
        public void AddItems_EmptyInventory_BiggerAmountThanSlotSize_AddsToAvailableSlots()
        {
            var items = this.itemFactory.CreateMany(20);
            var inventory = this.containerFactory.EmptyContainer();

            inventory.Add(items);

            Assert.Multiple(() =>
            {
                Assert.That(inventory[0].Content, Is.EqualTo(items.Take(10)));
                Assert.That(inventory[1].Content, Is.EqualTo(items.Skip(10)));
            });
        }

        [Test]
        public void AddItems_FullInventory_DoNotAddsItems()
        {
            var slotItem = this.itemFactory.CreateRandom();
            var items = this.itemFactory.CreateMany(20);
            var expectedItems = items.ToArray();//TODO: clone array
            var inventory = this.containerFactory.FullContainer(20, 2, slotItem);

            inventory.Add(items);

            Assert.That(items, Is.EqualTo(expectedItems));
        }

        [Test]
        public void AddItems_FullInventory_ReturnsNotAddedItems()
        {
            var slotItem = this.itemFactory.CreateRandom();
            var items = this.itemFactory.CreateMany(20);
            var inventory = this.containerFactory.FullContainer(20, 2, slotItem);

            var result = inventory.Add(items);

            Assert.That(result, Is.EqualTo(items));
        }
    }
}
