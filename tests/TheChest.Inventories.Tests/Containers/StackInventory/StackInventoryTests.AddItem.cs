﻿using TheChest.Core.Slots.Interfaces;

namespace TheChest.Inventories.Tests.Containers
{
    public partial class StackInventoryTests<T>
    {
        [Test]
        public void Add_EmptyInventory_AddsToFirstEmptySlot()
        {
            var item = this.itemFactory.CreateDefault();
            var inventory = this.containerFactory.EmptyContainer();

            inventory.Add(item);
            
            Assert.Multiple(() =>
            {
                Assert.That(inventory[0].Content, Has.One.EqualTo(item));
                Assert.That(inventory[0].StackAmount, Is.EqualTo(1));
            });
        }

        [Test]
        public void Add_EmptyInventory_ReturnsTrue()
        {
            var item = this.itemFactory.CreateDefault();
            var inventory = this.containerFactory.EmptyContainer();

            var result = inventory.Add(item);

            Assert.That(result, Is.True);
        }

        [Test]
        public void Add_InventoryWithItems_AddsToAvailableSlot()
        {
            var item = this.itemFactory.CreateDefault();
            var inventory = this.containerFactory.ShuffledItemsContainer(20, 10, this.itemFactory.CreateManyRandom(10));

            inventory.Add(item);
            
            Assert.That(inventory.Slots.Any(x => x.Content?.Contains(item) ?? false), Is.True);
        }

        [Test]
        public void Add_InventoryWithSameItem_AddsToAvailableSlotWithSameItem()
        {
            var item = this.itemFactory.CreateDefault();
            var amount = this.random.Next(2, 10);
            var inventory = this.containerFactory.ShuffledItemsContainer(20, amount, item);
            inventory.Get(item, amount - 1);

            inventory.Add(item);

            Assert.That(inventory.Slots, Has.One.Matches<IStackSlot<T>>(x => x.StackAmount == 2 && x.Content!.Contains(item)));
        }

        [Test]
        public void Add_InventoryWithFullSlotWithSameItem_AddsToFirstAvailableSlot()
        {
            var item = this.itemFactory.CreateDefault();
            var amount = this.random.Next(2, 10);
            var inventory = this.containerFactory.ShuffledItemsContainer(20, amount, item);

            inventory.Add(item);

            Assert.Multiple(() =>
            {
                Assert.That(inventory[0].Content, Has.One.EqualTo(item));
                Assert.That(inventory[0].StackAmount, Is.EqualTo(1));
            });
        }

        [Test]
        public void Add_InventoryWithSameItem_AddsToSlotWithItem()
        {
            var item = this.itemFactory.CreateDefault();
            var items = this.itemFactory.CreateManyRandom(19)
                .Append(this.itemFactory.CreateDefault())
                .ToArray();
            var inventory = this.containerFactory.ShuffledItemsContainer(20, 10, items);
            var slotIndex = Array.IndexOf(inventory.Slots.ToArray(), inventory.Slots.First(x => x.Content?.Contains(item) ?? false));
            inventory.Get(slotIndex, 9);

            inventory.Add(item);

            Assert.Multiple(() =>
            {
                Assert.That(inventory[slotIndex].Content, Has.All.EqualTo(item));
                Assert.That(inventory[slotIndex].StackAmount, Is.EqualTo(2));
            });
        }

        [Test]
        public void Add_FullInventory_DoesNotAddToSlot()
        {
            var item = this.itemFactory.CreateDefault();
            var inventory = this.containerFactory.FullContainer(10,10, this.itemFactory.CreateRandom());

            inventory.Add(item);

            Assert.That(inventory.GetCount(item), Is.Zero);
        }

        [Test]
        public void Add_FullInventory_ReturnsFalse()
        {
            var item = this.itemFactory.CreateDefault();
            var inventory = this.containerFactory.FullContainer(10, 10, this.itemFactory.CreateRandom());

            var result = inventory.Add(item);

            Assert.That(result, Is.False);
        }
    }
}
