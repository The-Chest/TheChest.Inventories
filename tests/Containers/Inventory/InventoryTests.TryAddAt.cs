using TheChest.Tests.Common.Attributes;
using TheChest.Tests.Common.Extensions.Containers;

namespace TheChest.Inventories.Tests.Containers.Inventory
{
    public partial class InventoryTests<T>
    {
        [Test]
        [IgnoreIfValueType]
        public void TryAddAt_NullItem_ThrowsArgumentNullException()
        {
            var inventory = this.inventoryFactory.EmptyContainer(this.GenerateRandomSize());
            Assert.That(
                () => inventory.TryAddAt(default!, 0),
                Throws.ArgumentNullException.With.Property("ParamName").EqualTo("item")
            );
        }

        [Test]
        [IgnoreIfReferenceType]
        public void TryAddAt_DefaultItem_ThrowsNothing()
        {
            var size = this.GenerateRandomSize();
            var inventory = this.inventoryFactory.EmptyContainer(size);

            Assert.That(() => inventory.TryAddAt(default!, 0), Throws.Nothing);
        }

        [Test]
        [IgnoreIfReferenceType]
        public void TryAddAt_DefaultValue_EmptySlot_ReturnsTrue()
        {
            var size = this.GenerateRandomSize();
            var inventory = this.inventoryFactory.EmptyContainer(size);

            var index = this.random.Next(0, size);
            var result = inventory.TryAddAt(default!, index);

            Assert.That(result, Is.True);
        }

        [Test]
        [IgnoreIfReferenceType]
        public void TryAddAt_DefaultValue_EmptySlot_AddsToSlot()
        {
            var size = this.GenerateRandomSize();
            var inventory = this.inventoryFactory.EmptyContainer(size);

            var index = this.random.Next(0, size);
            var item = default(T);
            inventory.TryAddAt(item!, index);
            
            Assert.That(inventory.GetItem(index), Is.EqualTo(item));
        }

        [Test]
        [IgnoreIfReferenceType]
        public void TryAddAt_DefaultValue_FullSlot_ReturnsFalse()
        {
            var size = this.GenerateRandomSize();
            var inventory = this.inventoryFactory.FullContainer(size, default!);

            var index = this.random.Next(0, size);
            var result = inventory.TryAddAt(default!, index);

            Assert.That(result, Is.False);
        }

        [Test]
        [IgnoreIfReferenceType]
        public void TryAddAt_DefaultValue_FullSlot_DoesNotAddToSlot()
        {
            var size = this.GenerateRandomSize();
            var inventory = this.inventoryFactory.FullContainer(size, default!);

            var index = this.random.Next(0, size);
            var item = default(T);
            inventory.TryAddAt(item!, index);

            Assert.That(inventory.GetItem(index), Is.EqualTo(item));
        }

        [TestCase(-1)]
        [TestCase(MAX_SIZE_TEST)]
        public void TryAddAt_NegativeIndex_ThrowsArgumentOutOfRangeException(int index)
        {
            var inventory = this.inventoryFactory.EmptyContainer(this.GenerateRandomSize());
            var item = this.itemFactory.CreateRandom();
            Assert.That(
                () => inventory.TryAddAt(item, index),
                Throws.InstanceOf<ArgumentOutOfRangeException>().With.Property("ParamName").EqualTo("index")
            );
        }

        [Test]
        public void TryAddAt_TargetEqualToSize_ThrowsArgumentOutOfRangeException()
        {
            var size = this.GenerateRandomSize();
            var inventory = this.inventoryFactory.EmptyContainer(size);

            var item = this.itemFactory.CreateDefault();
            Assert.That(
                () => inventory.TryAddAt(item, size),
                Throws.TypeOf<ArgumentOutOfRangeException>().With.Property("ParamName").EqualTo("index")
            );
        }

        [Test]
        public void TryAddAt_ValidIndexAndItem_EmptySlot_ReturnsTrue()
        {
            var size = this.GenerateRandomSize();
            var inventory = this.inventoryFactory.EmptyContainer(size);
            var item = this.itemFactory.CreateRandom();

            var index = this.random.Next(0, size);
            var result = inventory.TryAddAt(item, index);

            Assert.That(result, Is.True);
        }

        [Test]
        public void TryAddAt_ValidIndexAndItem_EmptySlot_AddsToSlot()
        {
            var size = this.GenerateRandomSize();
            var inventory = this.inventoryFactory.EmptyContainer(size);
            var item = this.itemFactory.CreateRandom();

            var index = this.random.Next(0, size);
            inventory.TryAddAt(item, index);

            Assert.That(inventory.GetItem(index), Is.EqualTo(item));
        }

        [Test]
        public void TryAddAt_ValidIndexAndItem_EmptySlot_CallsOnAddEvent()
        {
            var size = this.GenerateRandomSize();
            var inventory = this.inventoryFactory.EmptyContainer(size);
            var item = this.itemFactory.CreateRandom();

            var called = false;
            var index = this.random.Next(0, size);
            inventory.OnAdd += (sender, e) => {
                Assert.That(sender, Is.EqualTo(inventory));
                var firstData = e.Data.First();
                
                Assert.Multiple(() =>
                {
                    Assert.That(firstData.Item, Is.EqualTo(item));
                    Assert.That(firstData.Index, Is.EqualTo(index));
                });
                called = true;
            };
            inventory.TryAddAt(item, index);

            Assert.That(called, Is.True, "OnAdd event was not called.");
        }

        [Test]
        public void TryAddAt_ValidIndexAndItem_FullSlot_ReturnsFalse()
        {
            var size = this.GenerateRandomSize();
            var inventoryItem = this.itemFactory.CreateRandom();
            var inventory = this.inventoryFactory.FullContainer(size, inventoryItem);

            var index = this.random.Next(0, size);
            var item = this.itemFactory.CreateDefault();
            var result = inventory.TryAddAt(item, index);

            Assert.That(result, Is.False);
        }

        [Test]
        public void TryAddAt_ValidIndexAndItem_FullSlot_DoesNotChangeSlot()
        {
            var size = this.GenerateRandomSize();
            var inventoryItem = this.itemFactory.CreateRandom();
            var inventory = this.inventoryFactory.FullContainer(size, inventoryItem);

            var index = this.random.Next(0, size);
            var item = this.itemFactory.CreateDefault();
            inventory.TryAddAt(item, index);

            Assert.That(inventory.GetItem(index), Is.EqualTo(inventoryItem));
        }

        [Test]
        public void TryAddAt_ValidIndexAndItem_FullSlot_DoesNotCallOnAddEvent()
        {
            var size = this.GenerateRandomSize();
            var inventoryItem = this.itemFactory.CreateRandom();
            var inventory = this.inventoryFactory.FullContainer(size, inventoryItem);

            var called = false;
            inventory.OnAdd += (sender, e) => called = true;

            var item = this.itemFactory.CreateDefault();
            var index = this.random.Next(0, size);
            inventory.TryAddAt(item, index);

            Assert.That(called, Is.False, "OnAdd event was called.");
        }
    }
}
