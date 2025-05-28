namespace TheChest.Inventories.Tests.Containers
{
    public partial class StackInventoryTests<T>
    {
        [TestCase(-1)]
        [TestCase(100)]
        public void AddItemsAt_InvalidIndex_ThrowsArgumentOutOfRangeException(int index)
        {
            var items = this.itemFactory.CreateMany(20);
            var inventory = this.containerFactory.EmptyContainer(20);

            Assert.That(() => inventory.AddAt(items, index), Throws.TypeOf<ArgumentOutOfRangeException>());
        }

        [Test]
        public void AddItemsAt_InvalidItem_ThrowsArgumentException()
        {
            var inventory = this.containerFactory.EmptyContainer(20);

            Assert.That(() => inventory.AddAt(Array.Empty<T>(), 0), Throws.ArgumentException);
        }

        [Test]
        public void AddItemsAt_EmptySlot_ReplaceEnabled_AddsToStack()
        {
            var index = this.random.Next(0, 20);
            var inventory = this.containerFactory.EmptyContainer(20);

            var items = this.itemFactory.CreateMany(10);
            inventory.AddAt(items, index, replace: true);

            Assert.That(inventory[index].Content, Is.EqualTo(items));
        }

        [Test]
        public void AddItemsAt_EmptySlot_ReplaceEnabled_ReturnsEmpty()
        {
            var index = this.random.Next(0, 20);
            var inventory = this.containerFactory.EmptyContainer(20);

            var items = this.itemFactory.CreateMany(10);
            var result = inventory.AddAt(items, index, replace: true);

            Assert.That(result, Is.Empty);
        }

        [Test]
        public void AddItemsAt_SlotWithDifferentItem_ReplaceDisabled_DoNotReplaceItemsFromSlot()
        {
            var index = this.random.Next(0, 20);
            var slotItem = this.itemFactory.CreateRandom();
            var amount = this.random.Next(1, 10);
            var inventory = this.containerFactory.FullContainer(20, amount, slotItem);

            var items = this.itemFactory.CreateMany(amount);
            inventory.AddAt(items, index, replace: false);

            Assert.That(inventory[index].Content, Has.All.EqualTo(slotItem));
        }

        [Test]
        public void AddItemsAt_SlotWithDifferentItem_ReplaceDisabled_ReturnsItemsFromParams()
        {
            var index = this.random.Next(0, 20);
            var slotItem = this.itemFactory.CreateRandom();
            var amount = this.random.Next(1, 10);
            var inventory = this.containerFactory.FullContainer(20, amount, slotItem);

            var items = this.itemFactory.CreateMany(amount);
            var result = inventory.AddAt(items, index, replace: false);

            Assert.That(result, Is.EqualTo(items));
        }

        [Test]
        public void AddItemsAt_SlotWithDifferentItem_ReplaceEnabled_ReturnsItemsFromSlot()
        {
            var index = this.random.Next(0, 20);
            var slotItem = this.itemFactory.CreateRandom();
            var amount = this.random.Next(1, 10);
            var inventory = this.containerFactory.FullContainer(20, amount, slotItem);

            var items = this.itemFactory.CreateMany(amount);
            var result = inventory.AddAt(items, index, replace: true);

            Assert.That(result, Has.All.EqualTo(slotItem));
        }

        [Test]
        public void AddItemsAt_SlotWithDifferentItems_ReplaceEnabled_ReplacesItems()
        {
            var index = this.random.Next(0, 20);
            var slotItem = this.itemFactory.CreateRandom();
            var amount = this.random.Next(1, 10);
            var inventory = this.containerFactory.FullContainer(20, amount, slotItem);

            var items = this.itemFactory.CreateMany(amount);
            inventory.AddAt(items, index, replace: true);

            Assert.That(inventory[index].Content, Is.EqualTo(items));
        }

        [Test]
        public void AddItemsAt_SlotWithSameItem_ReplaceEnabled_AddsToStack()
        {
            var index = this.random.Next(0, 20);
            var slotItem = this.itemFactory.CreateDefault();

            var amount = this.random.Next(5, 10);
            var inventory = this.containerFactory.FullContainer(20, amount, slotItem);
            inventory.Get(index, 2); 

            var items = this.itemFactory.CreateMany(2);
            inventory.AddAt(items, index, replace: true);

            Assert.Multiple(() =>
            {
                Assert.That(inventory[index].IsFull, Is.True);
                Assert.That(inventory[index].StackAmount, Is.EqualTo(amount));
                Assert.That(inventory[index].Content[^2..], Is.EqualTo(items));
            });
        }

        [Test]
        public void AddItemsAt_FullSlotSlotWithSameItem_ReplaceEnabled_ReturnsParamItems()
        {
            var index = this.random.Next(0, 20);
            var slotItem = this.itemFactory.CreateDefault();

            var amount = this.random.Next(1, 10);
            var inventory = this.containerFactory.FullContainer(20, amount, slotItem);

            var items = this.itemFactory.CreateMany(amount);
            var result = inventory.AddAt(items, index, replace: true);

            Assert.That(result, Is.EqualTo(items));
        }

        [Test]
        public void AddItemsAt_SlotWithSameItem_ReplaceDisabled_AddsToStack()
        {
            var index = this.random.Next(0, 20);
            var slotItem = this.itemFactory.CreateDefault();

            var amount = this.random.Next(5, 10);
            var inventory = this.containerFactory.FullContainer(20, amount, slotItem);
            inventory.Get(index, 2);

            var items = this.itemFactory.CreateMany(2);
            inventory.AddAt(items, index, replace: false);

            Assert.Multiple(() =>
            {
                Assert.That(inventory[index].IsFull, Is.True);
                Assert.That(inventory[index].StackAmount, Is.EqualTo(amount));
                Assert.That(inventory[index].Content[^2..], Is.EqualTo(items));
            });
        }

        [Test]
        public void AddItemsAt_FullSlotWithSameItem_ReplaceDisabled_ReturnsParamItems()
        {
            var index = this.random.Next(0, 20);
            var slotItem = this.itemFactory.CreateDefault();

            var amount = this.random.Next(1, 10);
            var inventory = this.containerFactory.FullContainer(20, amount, slotItem);

            var items = this.itemFactory.CreateMany(10);
            var result = inventory.AddAt(items, index, replace: false);

            Assert.That(result, Is.EqualTo(items));
        }

        [TestCase(true)]
        [TestCase(false)]
        public void AddItemsAt_FullSlotWithSameItem_DoNotAddsToStack(bool replace)
        {
            var index = this.random.Next(0, 20);
            var slotItem = this.itemFactory.CreateDefault();

            var amount = this.random.Next(1, 10);
            var inventory = this.containerFactory.FullContainer(20, amount, slotItem);
            inventory.Get(index);

            var items = this.itemFactory.CreateMany(10);
            inventory.AddAt(items, index, replace);

            Assert.That(inventory[index].Content, Has.No.AnyOf(items));
        }

        [TestCase(true)]
        [TestCase(false)]
        public void AddItemsAt_FullSlotWithSameItem_ReturnsNotAddedItems(bool replace)
        {
            var index = this.random.Next(0, 20);
            var slotItem = this.itemFactory.CreateDefault();

            var inventory = this.containerFactory.FullContainer(20, 5, slotItem);

            var items = this.itemFactory.CreateMany(10);
            var result = inventory.AddAt(items, index, replace);

            Assert.That(result, Is.Not.Empty.And.EqualTo(items));
        }
    }
}
