namespace TheChest.Inventories.Tests.Containers
{
    public partial class StackInventoryTests<T>
    {
        [Test]
        public void GetAllItems_InvalidItem_ThrowsArgumentNullException()
        {
            var inventory = this.containerFactory.EmptyContainer();
            Assert.That(() => inventory.GetAll(default(T)!), Throws.ArgumentNullException);
        }

        [Test]
        public void GetAllItems_EmptyInventory_ReturnsEmptyArray()
        {
            var item = this.itemFactory.CreateRandom();
            var inventory = this.containerFactory.EmptyContainer();
            var items = inventory.GetAll(item);
            Assert.That(items, Is.Empty);
        }

        [Test]
        public void GetAllItems_InventoryWithItems_ReturnSearchedItems()
        {
            var inventorySize = this.random.Next(10, 20);
            var stackSize = this.random.Next(1, 20);
            var item = this.itemFactory.CreateDefault();
            var inventoryItems = this.itemFactory.CreateManyRandom(inventorySize / 2)
                .Append(item)
                .Append(item)
                .ToArray();
            var inventory = this.containerFactory.ShuffledItemsContainer(20, stackSize, inventoryItems);

            var items = inventory.GetAll(item);

            Assert.Multiple(() =>
            {
                Assert.That(items, Has.Length.EqualTo(stackSize * 2));
                Assert.That(items, Has.All.EqualTo(item));
            });
        }

        [Test]
        public void GetAllItems_InventoryWithItems_RemovesItemsFromInventory()
        {
            var inventorySize = this.random.Next(10, 20);
            var stackSize = this.random.Next(1, 20);
            var slotItems = this.itemFactory.CreateMany(inventorySize / 2);
            var randomItems = this.itemFactory.CreateManyRandom(inventorySize / 2);
            var inventoryItems = slotItems.Concat(randomItems).ToArray();
            var inventory = this.containerFactory.ShuffledItemsContainer(20, stackSize, inventoryItems);

            inventory.GetAll(slotItems[0]);

            Assert.That(inventory.Slots.Any(x => x.Content?.Contains(slotItems[0]) ?? false), Is.False);
        }
    }
}
