using TheChest.Tests.Common.Attributes;
using TheChest.Tests.Common.Extensions.Containers;

namespace TheChest.Inventories.Tests.Containers.Inventory
{
    public partial class InventoryTests<T>
    {
        #region Invalid Parameters
        [Test]
        [IgnoreIfValueType]
        public void TryReplace_NullItem_ThrowsArgumentNullException()
        {
            var size = this.GenerateRandomSize();
            var inventory = this.inventoryFactory.EmptyContainer(size);

            Assert.That(
                () => inventory.TryReplace(default!, 0, out _),
                Throws.ArgumentNullException.With.Property("ParamName").EqualTo("item")
            );
        }

        [TestCase(-1)]
        [TestCase(MAX_SIZE_TEST)]
        public void TryReplace_InvalidSlotIndex_ThrowsArgumentOutOfRangeException(int index)
        {
            var size = this.GenerateRandomSize();
            var inventory = this.inventoryFactory.EmptyContainer(size);

            Assert.That(
                () => inventory.TryReplace(this.itemFactory.CreateDefault(), index, out _),
                Throws.Exception.TypeOf<ArgumentOutOfRangeException>().With.Property("ParamName").EqualTo("index")
            );
        }

        [Test]
        public void TryReplace_IndexEqualToSize_ThrowsArgumentOutOfRangeException()
        {
            var size = this.GenerateRandomSize();
            var inventory = this.inventoryFactory.EmptyContainer(size);

            Assert.That(
                () => inventory.TryReplace(this.itemFactory.CreateDefault(), size, out _),
                Throws.Exception.TypeOf<ArgumentOutOfRangeException>().With.Property("ParamName").EqualTo("index")
            );
        }
        #endregion

        #region Empty Slot
        [Test]
        public void TryReplace_EmptySlot_ReturnsFalse()
        {
            var size = this.GenerateRandomSize();
            var inventory = this.inventoryFactory.EmptyContainer(size);

            var item = this.itemFactory.CreateRandom();
            var randomIndex = this.random.Next(0, size);
            var result = inventory.TryReplace(item, randomIndex, out _);

            Assert.That(result, Is.False);
        }

        [Test]
        public void TryReplace_EmptySlot_DoesNotAddsItemToSlot()
        {
            var size = this.GenerateRandomSize();
            var inventory = this.inventoryFactory.EmptyContainer(size);

            var item = this.itemFactory.CreateRandom();
            var randomIndex = this.random.Next(0, size);
            inventory.TryReplace(item, randomIndex, out _);

            Assert.That(inventory.GetItem(randomIndex), Is.Not.EqualTo(item));
        }

        [Test]
        public void TryReplace_EmptySlot_DoesNotCallsOnReplaceEvent()
        {
            var size = this.GenerateRandomSize();
            var inventory = this.inventoryFactory.EmptyContainer(size);

            var raised = false;
            inventory.OnReplace += (sender, args) => raised = true;

            var randomIndex = this.random.Next(0, size);
            var item = this.itemFactory.CreateDefault();
            inventory.TryReplace(item, randomIndex, out _);

            Assert.That(raised, Is.False, "OnReplace event should not be raised for empty slot.");
        }

        [Test]
        public void TryReplace_EmptySlot_SetsOldItemToDefault()
        {
            var size = this.GenerateRandomSize();
            var inventory = this.inventoryFactory.EmptyContainer(size);

            var item = this.itemFactory.CreateDefault();
            var randomIndex = this.random.Next(0, size);
            inventory.TryReplace(item, randomIndex, out var oldItem);

            Assert.That(oldItem, Is.EqualTo(default(T)));
        }
        #endregion

        #region Full Slot
        [Test]
        [IgnoreIfReferenceType]
        public void TryReplace_DefaultValue_ReplacesItemInSlot()
        {
            var size = this.GenerateRandomSize();
            var initialItem = this.itemFactory.CreateRandom();
            var inventory = this.inventoryFactory.FullContainer(size, initialItem);

            var randomIndex = this.random.Next(0, size);
            inventory.TryReplace(default!, randomIndex, out _);

            Assert.That(inventory.GetItem(randomIndex), Is.EqualTo(default(T)));
        }

        [Test]
        [IgnoreIfReferenceType]
        public void TryReplace_DefaultValue_CallsOnReplaceEvent()
        {
            var size = this.GenerateRandomSize();
            var initialItem = this.itemFactory.CreateRandom();
            var inventory = this.inventoryFactory.FullContainer(size, initialItem);

            var raised = false;
            var randomIndex = this.random.Next(0, size);

            inventory.OnReplace += (sender, args) =>
            {
                Assert.Multiple(() =>
                {
                    Assert.That(sender, Is.EqualTo(inventory));
                    Assert.That(args.Data.Select(x => x.Index), Has.All.EqualTo(randomIndex));
                    Assert.That(args.Data.Select(x => x.OldItem), Has.All.EqualTo(initialItem));
                    Assert.That(args.Data.Select(x => x.NewItem), Has.All.EqualTo(default(T)));
                });
                raised = true;
            };
            inventory.TryReplace(default!, randomIndex, out _);

            Assert.That(raised, Is.True, "OnReplace event was not raised");
        }

        [Test]
        [IgnoreIfReferenceType]
        public void TryReplace_DefaultValue_SetsOldItemToPreviousItem()
        {
            var size = this.GenerateRandomSize();
            var initialItem = this.itemFactory.CreateRandom();
            var inventory = this.inventoryFactory.FullContainer(size, initialItem);

            var randomIndex = this.random.Next(0, size);
            inventory.TryReplace(default!, randomIndex, out var oldItem);

            Assert.That(oldItem, Is.EqualTo(initialItem));
        }

        [Test]
        [IgnoreIfReferenceType]
        public void TryReplace_DefaultValue_ReturnsTrue()
        {
            var size = this.GenerateRandomSize();
            var initialItem = this.itemFactory.CreateRandom();
            var inventory = this.inventoryFactory.FullContainer(size, initialItem);

            var randomIndex = this.random.Next(0, size);
            var result = inventory.TryReplace(default!, randomIndex, out _);

            Assert.That(result, Is.True);
        }

        [Test]
        public void TryReplace_FullSlot_ReplacesItemInSlot()
        {
            var size = this.GenerateRandomSize();
            var initialItem = this.itemFactory.CreateDefault();
            var inventory = this.inventoryFactory.FullContainer(size, initialItem);

            var newItem = this.itemFactory.CreateRandom();   
            var randomIndex = this.random.Next(0, size);
            inventory.TryReplace(newItem, randomIndex, out _);

            Assert.That(inventory.GetItem(randomIndex), Is.EqualTo(newItem));
        }

        [Test]
        public void TryReplace_FullSlot_CallsOnReplaceEvent()
        {
            var size = this.GenerateRandomSize();
            var initialItem = this.itemFactory.CreateDefault();
            var inventory = this.inventoryFactory.FullContainer(size, initialItem);

            var raised = false;

            var randomIndex = this.random.Next(0, size);
            var newItem = this.itemFactory.CreateRandom();

            inventory.OnReplace += (sender, args) =>
            {
                Assert.Multiple(() =>
                {
                    Assert.That(sender, Is.EqualTo(inventory));
                    Assert.That(args.Data.Select(x => x.Index), Has.All.EqualTo(randomIndex));
                    Assert.That(args.Data.Select(x => x.OldItem), Has.All.EqualTo(initialItem));
                    Assert.That(args.Data.Select(x => x.NewItem), Has.All.EqualTo(newItem));
                });
                raised = true;
            };

            inventory.TryReplace(newItem, randomIndex, out _);

            Assert.That(raised, Is.True, "OnReplace event was not raised");
        }

        [Test]
        public void TryReplace_FullSlot_SetsOldItemToPreviousItem()
        {
            var size = this.GenerateRandomSize();
            var initialItem = this.itemFactory.CreateDefault();
            var inventory = this.inventoryFactory.FullContainer(size, initialItem);

            var newItem = this.itemFactory.CreateRandom();
            var randomIndex = this.random.Next(0, size);
            inventory.TryReplace(newItem, randomIndex, out var oldItem);

            Assert.That(oldItem, Is.EqualTo(initialItem));
        }

        [Test]
        public void TryReplace_FullSlot_ReturnsTrue()
        {
            var size = this.GenerateRandomSize();
            var initialItem = this.itemFactory.CreateDefault();
            var inventory = this.inventoryFactory.FullContainer(size, initialItem);

            var newItem = this.itemFactory.CreateRandom();
            var randomIndex = this.random.Next(0, size);
            var result = inventory.TryReplace(newItem, randomIndex, out _);

            Assert.That(result, Is.True);
        }
        #endregion
    }
}
