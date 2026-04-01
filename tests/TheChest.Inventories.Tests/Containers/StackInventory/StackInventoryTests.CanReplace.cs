using TheChest.Tests.Common.Extensions.Containers;

namespace TheChest.Inventories.Tests.Containers
{
    public partial class StackInventoryTests<T>
    {
        [Test]
        public void Replace_SlotWithItems_ReplacesItemsInSlot()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var item = this.itemFactory.CreateDefault();
            var inventory = this.inventoryFactory.FullContainer(size, stackSize, item);

            var randomIndex = this.random.Next(0, size);
            var newItems = this.itemFactory.CreateManyRandom(stackSize);
            inventory.Replace(newItems, randomIndex);

            Assert.That(inventory.GetItems(randomIndex), Is.EqualTo(newItems));
        }

        [Test]
        public void Replace_SlotWithItems_ReturnsOldItems()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var item = this.itemFactory.CreateDefault();
            var inventory = this.inventoryFactory.FullContainer(size, stackSize, item);

            var randomIndex = this.random.Next(0, size);
            var newStackSize = this.random.Next(1, stackSize - 1);
            var newItems = this.itemFactory.CreateManyRandom(newStackSize);

            var result = inventory.Replace(newItems, randomIndex);

            Assert.That(result, Has.Length.EqualTo(stackSize));
            Assert.That(result, Has.All.EqualTo(item));
        }

        [Test]
        public void Replace_SlotWithItems_CallsOnReplaceEvent()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var item = this.itemFactory.CreateDefault();
            var inventory = this.inventoryFactory.FullContainer(size, stackSize, item);

            var randomIndex = this.random.Next(0, size);
            var newItems = this.itemFactory.CreateManyRandom(stackSize);
            inventory.OnReplace += (sender, args) =>
            {
                Assert.That(sender, Is.EqualTo(inventory));
                var data = args.Data.Single();
                Assert.Multiple(() =>
                {
                    Assert.That(data.Index, Is.EqualTo(randomIndex));
                    Assert.That(data.OldItems, Has.Length.EqualTo(stackSize).And.All.EqualTo(item));
                    Assert.That(data.NewItems, Is.EqualTo(newItems));
                });
            };
            inventory.Replace(newItems, randomIndex);
        }

        [Test]
        public void Replace_EmptySlot_ReplacesItemsInSlot()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var inventory = this.inventoryFactory.EmptyContainer(size, stackSize);

            var randomIndex = this.random.Next(0, size);
            var newItems = this.itemFactory.CreateManyRandom(stackSize);
            inventory.Replace(newItems, randomIndex);

            Assert.That(inventory.GetItems(randomIndex), Is.EqualTo(newItems));
        }

        [Test]
        public void Replace_EmptySlot_ReturnsEmptyArray()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var inventory = this.inventoryFactory.EmptyContainer(size, stackSize);

            var randomIndex = this.random.Next(0, size);
            var newItems = this.itemFactory.CreateManyRandom(this.random.Next(1, stackSize));
            var result = inventory.Replace(newItems, randomIndex);

            Assert.That(result, Is.Empty);
        }

        [Test]
        public void Replace_EmptySlot_CallsOnReplaceEvent()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var inventory = this.inventoryFactory.FullContainer(size, stackSize);

            var randomIndex = this.random.Next(0, size);
            var newItems = this.itemFactory.CreateManyRandom(stackSize);
            inventory.OnReplace += (sender, args) =>
            {
                Assert.That(sender, Is.EqualTo(inventory));
                var data = args.Data.Single();
                Assert.Multiple(() =>
                {
                    Assert.That(data.Index, Is.EqualTo(randomIndex));
                    Assert.That(data.OldItems, Is.Empty);
                    Assert.That(data.NewItems, Is.EqualTo(newItems));
                });
            };
            inventory.Replace(newItems, randomIndex);
        }
    }
}
