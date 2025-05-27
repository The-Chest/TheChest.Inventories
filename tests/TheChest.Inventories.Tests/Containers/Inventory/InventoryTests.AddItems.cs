namespace TheChest.Inventories.Tests.Containers
{
    public partial class InventoryTests<T>
    {
        [Test]
        public void AddItems_NoItems_ReturnsEmptyArray()
        {
            var size = this.random.Next(10, 20);
            var inventory = this.containerFactory.EmptyContainer(size);

            var items = this.itemFactory.CreateMany(0);

            var result = inventory.Add(items);

            Assert.That(result, Is.Empty);
        }

        [Test]
        public void AddItems_ArrayWithOnlyNullItems_ReturnsEmptyArray()
        {
            var size = this.random.Next(10, 20);
            var inventory = this.containerFactory.EmptyContainer(size);

            var items = new T[10];
            var result = inventory.Add(items);

            Assert.That(result, Is.Empty);
        }

        [Test]
        public void AddItems_ArrayWithOnlyNullItems_DoesNotAddToAnySlot()
        {
            var size = this.random.Next(10, 20);
            var inventory = this.containerFactory.EmptyContainer(size);

            var items = new T[10];
            inventory.Add(items);

            Assert.That(inventory.Slots.All(slot => slot.IsEmpty), Is.True);
        }

        [Test]
        public void AddItems_ArrayContainingNullItems_ReturnsEmptyArray()
        {
            var size = this.random.Next(10, 20);
            var inventory = this.containerFactory.EmptyContainer(size);

            var items = this.itemFactory
                .CreateManyRandom(10)
                .Append(default!)
                .Reverse()
                .ToArray();
            var result = inventory.Add(items);

            Assert.That(result, Is.Empty);
        }

        [Test]
        public void AddItems_ArrayContainingNullItems_DoesNotAddToSlot()
        {
            var size = this.random.Next(10, 20);
            var inventory = this.containerFactory.EmptyContainer(size);

            var randomItemSize = this.random.Next(2, size);
            var items = this.itemFactory
                .CreateManyRandom(randomItemSize)
                .Append(default!)
                .Reverse()
                .ToArray();
            inventory.Add(items);

            Assert.Multiple(() =>
            {
                Assert.That(inventory.Slots[0].Content, Is.EqualTo(items[1]));
                Assert.That(inventory.Slots[randomItemSize - 1].Content, Is.EqualTo(items[randomItemSize - 2]));
            });
        }

        [Test]
        public void AddItems_EmptySlots_AddsAllItems()
        {
            var size = this.random.Next(10, 20);
            var inventory = this.containerFactory.EmptyContainer(size);

            var randomSize = this.random.Next(1, size);
            var items = this.itemFactory.CreateMany(randomSize);
            inventory.Add(items);

            Assert.That(inventory.Slots[0..randomSize].Select(x => x.Content), Is.EqualTo(items));
        }

        [Test]
        public void AddItems_SuccessAdding_ReturnsEmptyArray()
        {
            var size = this.random.Next(10, 20);
            var inventory = this.containerFactory.EmptyContainer(size);

            var randomSize = this.random.Next(1, size);
            var items = this.itemFactory.CreateMany(randomSize);
            var result = inventory.Add(items);

            Assert.That(result, Is.Empty);
        }

        [Test]
        public void AddItems_NotAvailabeSlotsForAllItems_AddsSomeItems()
        {
            var size = this.random.Next(10, 20);
            var emptySlotsSize = this.random.Next(1, size);
            var itemSize = size - emptySlotsSize;
            var items = this.itemFactory.CreateMany(itemSize);
            var inventory = this.containerFactory.ShuffledItemsContainer(size, items);

            var addSize = emptySlotsSize + 1;
            var manyAdded = this.itemFactory.CreateManyRandom(addSize);
            inventory.Add(manyAdded);

            Assert.That(inventory.GetCount(manyAdded[0]), Is.EqualTo(emptySlotsSize));
        }

        [Test]
        public void AddItems_NotAvailabeSlotsForAllItems_ReturnsNotAddedItems()
        {
            var size = this.random.Next(10, 20);
            var emptySlotsSize = this.random.Next(1, size);
            var itemSize = size - emptySlotsSize;
            var items = this.itemFactory.CreateMany(itemSize);
            var inventory = this.containerFactory.ShuffledItemsContainer(size, items);

            var manyAdded = this.itemFactory.CreateManyRandom(emptySlotsSize + 1);
            var result = inventory.Add(manyAdded);

            Assert.That(result.Length, Is.EqualTo(1));
        }

        [Test]
        public void AddItems_FullInventory_DoNotAddItems()
        {
            var size = this.random.Next(10, 20);
            var item = this.itemFactory.CreateDefault();
            var inventory = this.containerFactory.FullContainer(size, item);

            var items = this.itemFactory.CreateManyRandom(size);
            inventory.Add(items);

            Assert.That(inventory.GetCount(items[0]), Is.EqualTo(0));
        }

        [Test]
        public void AddItems_FullInventory_ReturnsAllNotAddedItems()
        {
            var size = this.random.Next(10, 20);
            var item = this.itemFactory.CreateDefault();
            var inventory = this.containerFactory.FullContainer(size, item);

            var items = this.itemFactory.CreateMany(size);
            var result = inventory.Add(items);

            Assert.That(result, Is.EqualTo(items));
        }
    }
}
