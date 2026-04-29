using TheChest.Tests.Common.Extensions.Containers;

using TheChest.Tests.Common.Attributes;
namespace TheChest.Inventories.Tests.Containers.Inventory
{
    public partial class InventoryTests<T>
    {
        [TestCase(-1)]
        [TestCase(MAX_SIZE_TEST + 1)]
        public void Replace_InvalidSlotIndex_ThrowsArgumentOutOfRangeException(int index)
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var inventory = this.inventoryFactory.EmptyContainer(size);

            Assert.That(
                () => inventory.Replace(this.itemFactory.CreateDefault(), index),
                Throws.Exception.TypeOf<ArgumentOutOfRangeException>()
            );
        }

        [Test]
        [IgnoreIfValueType]
        public void Replace_NullItem_ThrowsArgumentNullException()
        {
            var inventory = this.inventoryFactory.EmptyContainer();
            Assert.That(() => inventory.Replace(default!, 0), Throws.ArgumentNullException);
        }

        [Test]
        public void Replace_EmptySlot_AddsItemToSlot()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var inventory = this.inventoryFactory.EmptyContainer(size);

            var randomIndex = this.random.Next(0, size);
            var item = this.itemFactory.CreateDefault();
            inventory.Replace(item, randomIndex);

            Assert.That(inventory.GetItem<T>(randomIndex), Is.EqualTo(item));
        }

        [Test]
        public void Replace_EmptySlot_CallsOnAddEventWithEmptyOldItem()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var inventory = this.inventoryFactory.EmptyContainer(size);

            var randomIndex = this.random.Next(0, size);
            var item = this.itemFactory.CreateDefault();

            var raised = false;
            inventory.OnReplace += (sender, args) =>
            {
                Assert.Multiple(() =>
                {
                    Assert.That(sender, Is.EqualTo(inventory));
                    Assert.That(args.Data.Select(x => x.OldItem), Has.All.Null);
                    Assert.That(args.Data.Select(x => x.NewItem), Has.All.EqualTo(item));
                });
                raised = true;
            };

            inventory.Replace(item, randomIndex);

            Assert.That(raised, Is.True, "OnReplace event was not raised");
        }

        [Test]
        public void Replace_FullSlot_ReplacesItemInSlot()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var initialItem = this.itemFactory.CreateDefault();
            var inventory = this.inventoryFactory.FullContainer(size, initialItem);

            var randomIndex = this.random.Next(0, size);
            var newItem = this.itemFactory.CreateRandom();
            inventory.Replace(newItem, randomIndex);

            Assert.That(inventory.GetItem(randomIndex), Is.EqualTo(newItem));
        }

        [Test]
        public void Replace_FullSlot_CallsOnReplaceEvent()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var initialItem = this.itemFactory.CreateDefault();
            var inventory = this.inventoryFactory.FullContainer(size, initialItem);

            var randomIndex = this.random.Next(0, size);
            var newItem = this.itemFactory.CreateRandom();

            var raised = false;
            inventory.OnReplace += (sender, args) =>
            {
                Assert.Multiple(() =>
                {
                    Assert.That(sender, Is.EqualTo(inventory));
                    Assert.That(args.Data.Select(x => x.OldItem), Has.All.EqualTo(initialItem));
                    Assert.That(args.Data.Select(x => x.NewItem), Has.All.EqualTo(newItem));
                });
                raised = true;
            };
            inventory.Replace(newItem, randomIndex);

            Assert.That(raised, Is.True, "OnReplace event was not raised");
        }
    }
}
