using TheChest.Tests.Common.Extensions.Containers;
using TheChest.Tests.Common.Attributes;

namespace TheChest.Inventories.Tests.Containers.Inventory
{
    public partial class InventoryTests<T>
    {
        [TestCase(-1)]
        [TestCase(MAX_SIZE_TEST)]
        public void AddAt_InvalidSlotIndex_ThrowsArgumentOutOfRangeException(int index)
        {
            var size = this.GenerateRandomSize();
            var inventory = this.inventoryFactory.EmptyContainer(size);

            Assert.That(
                () => inventory.AddAt(this.itemFactory.CreateDefault(), index),
                Throws.Exception.TypeOf<ArgumentOutOfRangeException>().With.Property("ParamName").EqualTo("index")
            );
        }

        [Test]
        [IgnoreIfValueType]
        public void AddAt_NullItem_ThrowsArgumentNullException()
        {
            var size = this.GenerateRandomSize();
            var inventory = this.inventoryFactory.EmptyContainer(size);
            Assert.That(
                () => inventory.AddAt(default!, 0), 
                Throws.ArgumentNullException.With.Property("ParamName").EqualTo("item")
            );
        }

        [Test]
        public void AddAt_FullSelectedSlot_ThrowsInvalidOperationException()
        {
            var size = this.GenerateRandomSize();
            var oldItem = this.itemFactory.CreateDefault();
            var inventory = this.inventoryFactory.FullContainer(size, oldItem);

            var randomIndex = this.random.Next(0, size);
            var item = this.itemFactory.CreateRandom();
            Assert.That(
                () => inventory.AddAt(item, randomIndex), 
                Throws.InvalidOperationException.With.Message.EqualTo("The slot is already full.")
            );
        }

        [Test]
        public void AddAt_FullSelectedSlot_DoesNotAddTheItem()
        {
            var size = this.GenerateRandomSize();
            var oldItem = this.itemFactory.CreateDefault();
            var inventory = this.inventoryFactory.FullContainer(size, oldItem);

            var randomIndex = this.random.Next(0, size);
            var item = this.itemFactory.CreateRandom();
            Assert.That(() => inventory.AddAt(item, randomIndex), Throws.InvalidOperationException);

            Assert.Multiple(() =>
            {
                var randomItem = inventory.GetItem(randomIndex);
                Assert.That(randomItem, Is.EqualTo(oldItem));
                Assert.That(randomItem, Is.Not.EqualTo(item));
            });
        }

        [Test]
        public void AddAt_FullSelectedSlot_DoesNotCallOnAddEvent()
        {
            var size = this.GenerateRandomSize();
            var oldItem = this.itemFactory.CreateDefault();
            var inventory = this.inventoryFactory.FullContainer(size, oldItem);

            inventory.OnAdd += (sender, args) => Assert.Fail("OnAdd should not be called if inventory is full");

            var randomIndex = this.random.Next(0, size);
            var item = this.itemFactory.CreateRandom();

            Assert.That(() => inventory.AddAt(item, randomIndex), Throws.InvalidOperationException);
        }

        [Test]
        public void AddAt_EmptySelectedSlot_AddsItem()
        {
            var size = this.GenerateRandomSize();
            var inventory = this.inventoryFactory.EmptyContainer(size);

            var randomIndex = this.random.Next(0, size);
            var item = this.itemFactory.CreateDefault();
            inventory.AddAt(item, randomIndex);

            Assert.That(inventory.GetItem(randomIndex), Is.EqualTo(item));
        }

        [Test]
        public void AddAt_EmptySelectedSlot_CallsOnAddEvent()
        {
            var size = this.GenerateRandomSize();
            var inventory = this.inventoryFactory.EmptyContainer(size);

            var randomIndex = this.random.Next(0, size);
            var item = this.itemFactory.CreateDefault();

            var raised = false;
            inventory.OnAdd += (sender, args) =>
            {
                Assert.Multiple(() =>
                {
                    Assert.That(sender, Is.EqualTo(inventory));
                    Assert.That(args.Data.Select(x => x.Item), Has.All.EqualTo(item));
                });
                raised = true;
            };

            inventory.AddAt(item, randomIndex);

            Assert.That(raised, Is.True, "OnAdd event was not raised");
        }

        [Test]
        public void AddAt_EmptySlot_ReturnsTrue()
        {
            var size = this.GenerateRandomSize();
            var inventory = this.inventoryFactory.EmptyContainer(size);

            var randomIndex = this.random.Next(0, size);
            var item = this.itemFactory.CreateDefault();
            var result = inventory.AddAt(item, randomIndex);

            Assert.That(result, Is.True);
        }
    }
}
