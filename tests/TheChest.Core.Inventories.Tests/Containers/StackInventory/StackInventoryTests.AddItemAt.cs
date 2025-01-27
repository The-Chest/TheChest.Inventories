namespace TheChest.Core.Inventories.Tests.Containers
{
    //TODO: find a way to create a complex inventory for this test 
    // with multiple items in each slot
    public partial class StackInventoryTests<T>
    {
        [TestCase(-1)]
        [TestCase(100)]
        public void AddItemAt_InvalidIndex_ThrowsArgumentException(int index)
        {
            var item = this.itemFactory.CreateDefault();
            var inventory = this.containerFactory.EmptyContainer(20);

            Assert.That(() => inventory.AddAt(item, index), Throws.ArgumentException);
        }

        [Test]
        public void AddItemAt_InvalidItem_ThrowsArgumentException()
        {
            var inventory = this.containerFactory.EmptyContainer(20);

            Assert.That(() => inventory.AddAt((T)default!, 0), Throws.ArgumentException);
        }

        [Test]
        public void AddItemAt_EmptySlot_ReplaceEnabled_AddsToStack()
        {
            var index = this.random.Next(0, 20);
            var inventory = this.containerFactory.EmptyContainer(20);

            var item = this.itemFactory.CreateDefault();
            inventory.AddAt(item, index, replace: true);

            Assert.That(inventory[index].Content, Has.One.EqualTo(item));
        }

        [Test]
        public void AddItemAt_EmptySlot_ReplaceEnabled_ReturnsEmpty()
        {
            var index = this.random.Next(0, 20);
            var inventory = this.containerFactory.EmptyContainer(20);

            var item = this.itemFactory.CreateDefault();
            var result = inventory.AddAt(item, index, replace: true);

            Assert.That(result, Is.Empty);
        }

        [Test]
        [Ignore("This test is not working due Inventory creation")]
        public void AddItemAt_SlotWithDifferentItem_ReplaceDisabled_DoNotReplaceItem()
        {
            var index = this.random.Next(0, 20);
            var items = this.itemFactory.CreateManyRandom(20);
            var inventory = this.containerFactory.ShuffledItemsContainer(20, 5, items);

            var item = this.itemFactory.CreateDefault();
            inventory.AddAt(item, index, replace: false);

            Assert.That(inventory[index].Content, Has.No.AnyOf(item));
        }

        [Test]
        public void AddItemAt_SlotWithDifferentItem_ReplaceDisabled_ReturnsItem()
        {
            var index = this.random.Next(0, 20);
            var items = this.itemFactory.CreateManyRandom(20);
            //TODO: find a way to create a complex inventory
            var inventory = this.containerFactory.ShuffledItemsContainer(20, 5, items);

            var item = this.itemFactory.CreateDefault();
            var result = inventory.AddAt(item, index, replace: false);

            Assert.That(result, Has.One.AnyOf(item));
        }

        [Test]
        public void AddItemAt_SlotWithDifferentItem_ReplaceEnabled_ReturnsItemFromSlot()
        {
            var index = this.random.Next(0, 20);
            var slotItem = this.itemFactory.CreateRandom();
            var inventory = this.containerFactory.FullContainer(20, 5, slotItem);

            var item = this.itemFactory.CreateDefault();
            var result = inventory.AddAt(item, index, replace: true);

            Assert.That(result, Has.One.AnyOf(slotItem));
        }

        [Test]
        [Ignore("This test is not working due Inventory creation")]
        public void AddItemAt_SlotWithDifferentItem_ReplaceEnabled_ReplacesItem()
        {
            var index = this.random.Next(0, 20);
            var slotItem = this.itemFactory.CreateRandom();
            var inventory = this.containerFactory.FullContainer(20, 5, slotItem);

            var item = this.itemFactory.CreateDefault();
            inventory.AddAt(item, index, replace: true);

            Assert.That(inventory[index].Content, Has.One.AnyOf(item));
        }

        [Test]
        public void AddItemAt_SlotWithSameItem_ReplaceEnabled_AddsToStack()
        {
            var index = this.random.Next(0, 20);
            var slotItem = this.itemFactory.CreateDefault();
            var inventory = this.containerFactory.FullContainer(20, 5, slotItem);

            var stackSize = inventory[index].StackAmount;
            var item = this.itemFactory.CreateDefault();
            inventory.AddAt(item, index, replace: true);

            Assert.That(inventory[index].StackAmount, Is.EqualTo(stackSize + 1));
        }

        [Test]
        public void AddItemAt_SlotWithSameItem_ReplaceEnabled_ReturnsEmptyArray()
        {
            var index = this.random.Next(0, 20);
            var slotItem = this.itemFactory.CreateDefault();

            var amount = this.random.Next(1, 10);
            var inventory = this.containerFactory.FullContainer(20, amount, slotItem);
            inventory.Get(index);

            var item = this.itemFactory.CreateDefault();
            var result = inventory.AddAt(item, index, replace: true);

            Assert.That(result, Is.Empty);
        }

        [Test]
        public void AddItemAt_SlotWithSameItem_ReplaceDisabled_AddsToStack()
        {
            var index = this.random.Next(0, 20);
            var slotItem = this.itemFactory.CreateDefault();
            var inventory = this.containerFactory.FullContainer(20, 5, slotItem);

            var stackSize = inventory[index].StackAmount;
            var item = this.itemFactory.CreateDefault();
            inventory.AddAt(item, index, replace: false);

            Assert.That(inventory[index].StackAmount, Is.EqualTo(stackSize + 1));
        }

        [Test]
        public void AddItemAt_SlotWithSameItem_ReplaceDisabled_ReturnsEmptyArray()
        {
            var index = this.random.Next(0, 20);
            var slotItem = this.itemFactory.CreateDefault();

            var amount = this.random.Next(1, 10);
            var inventory = this.containerFactory.FullContainer(20, amount, slotItem);
            inventory.Get(index);

            var item = this.itemFactory.CreateDefault();
            var result = inventory.AddAt(item, index, replace: false);

            Assert.That(result, Is.Empty);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void AddItemAt_FullSlotWithSameItem_DoNotAddsToStack(bool replace)
        {
            var index = this.random.Next(0, 20);
            var slotItem = this.itemFactory.CreateDefault();

            var amount = this.random.Next(1, 10);
            var inventory = this.containerFactory.FullContainer(20, amount, slotItem);
            inventory.Get(index);

            var item = this.itemFactory.CreateDefault();
            inventory.AddAt(item, index, replace);

            Assert.That(inventory[index].Content, Has.No.AnyOf(item));
        }

        [TestCase(true)]
        [TestCase(false)]
        public void AddItemAt_FullSlotWithSameItem_ReturnsNotAddedItems(bool replace)
        {
            var index = this.random.Next(0, 20);
            var slotItem = this.itemFactory.CreateDefault();

            var inventory = this.containerFactory.FullContainer(20, 5, slotItem);

            var item = this.itemFactory.CreateDefault();
            var result = inventory.AddAt(item, index, replace);

            Assert.That(result, Is.Not.Empty.And.Contains(item));
        }
    }
}
