﻿namespace TheChest.Inventories.Tests.Slots
{
    public partial class IInventoryStackSlotTests<T>
    {
        [Test]
        public void AddItems_EmptySlot_WithLessItemsThanMaxAmount_AddsAllItems()
        {
            var randomSize = this.random.Next(1,20);
            var slot = this.slotFactory.EmptySlot(randomSize);

            var addingItems = this.itemFactory.CreateManyRandom(randomSize);
            slot.Add(addingItems);

            Assert.That(slot.StackAmount, Is.EqualTo(randomSize));
        }

        [Test]
        public void AddItems_EmptySlot_WithMoreItemsThanMaxAmount_AddsAllItems()
        {
            var maxAmount = this.random.Next(1, 20);
            var slot = this.slotFactory.EmptySlot(maxAmount);

            var randomSize = this.random.Next(maxAmount + 1, 20);
            var addingItems = this.itemFactory.CreateManyRandom(randomSize);
            var expectedItems = (T[])addingItems[0..maxAmount].Clone();
            slot.Add(addingItems);

            Assert.That(slot.Content, Is.EqualTo(expectedItems));
        }

        [Test]
        public void AddItems_FullSlot_WithAnyAmountOfItems_DoNotAdd()
        {
            var randomSize = this.random.Next(1, 20);
            var items = this.itemFactory.CreateMany(randomSize);
            var slot = this.slotFactory.FullSlot(items);

            var addingItems = this.itemFactory.CreateMany(randomSize);
            slot.Add(addingItems);

            Assert.That(slot.Content, Is.EqualTo(items));
        }

        [Test]
        public void AddItems_AddingDifferentItems_ThrowsArgumentException()
        {
            var slot = this.slotFactory.EmptySlot(20);

            var randomSize = this.random.Next(1, 20);
            var addingItems = this.itemFactory
                .CreateManyRandom(randomSize)
                .Append(this.itemFactory.CreateRandom())
                .ToArray();

            Assert.That(() => slot.Add(addingItems), Throws.ArgumentException);
        }

        [Test]
        public void AddItems_DifferentItemsFromSlot_ThrowsArgumentException()
        {
            var slot = this.slotFactory.WithItem(this.itemFactory.CreateDefault());

            var randomSize = this.random.Next(2, 10);
            var addingItems = this.itemFactory.CreateManyRandom(randomSize);

            Assert.That(() => slot.Add(addingItems), Throws.ArgumentException);
        }

        [Test]
        public void AddItems_OneItemDifferentFromSlot_ThrowsArgumentException()
        {
            var slot = this.slotFactory.WithItem(this.itemFactory.CreateDefault());

            var randomSize = this.random.Next(1, 10);
            var addingItems = this.itemFactory.CreateMany(randomSize)
                .Append(this.itemFactory.CreateRandom())
                .ToArray();

            Assert.That(() => slot.Add(addingItems), Throws.ArgumentException);
        }

        [Test]
        public void AddItems_AddingNoItems_ThrowsArgumentException()
        {
            var slot = this.slotFactory.EmptySlot();

            var addingItems = Array.Empty<T>();

            Assert.That(() => slot.Add(addingItems), Throws.ArgumentException);
        }
    }
}
