using TheChest.Tests.Common.Attributes;
using TheChest.Tests.Common.Extensions.Containers;

namespace TheChest.Inventories.Tests.Containers.Inventory
{
    public partial class InventoryTests<T>
    {
        #region Invalid parameter
        [Test]
        [IgnoreIfValueType]
        public void Replace_NullItem_ThrowsArgumentNullException()
        {
            var size = this.GenerateRandomSize();
            var inventory = this.inventoryFactory.EmptyContainer(size);

            Assert.That(
                () => inventory.Replace(default!, 0),
                Throws.ArgumentNullException.With.Property("ParamName").EqualTo("item")
            );
        }

        [TestCase(-1)]
        [TestCase(MAX_SIZE_TEST)]
        public void Replace_InvalidSlotIndex_ThrowsArgumentOutOfRangeException(int index)
        {
            var size = this.GenerateRandomSize();
            var inventory = this.inventoryFactory.EmptyContainer(size);

            Assert.That(
                () => inventory.Replace(this.itemFactory.CreateDefault(), index),
                Throws.Exception.TypeOf<ArgumentOutOfRangeException>().With.Property("ParamName").EqualTo("index")
            );
        }

        [Test]
        public void Replace_IndexEqualToSize_ThrowsArgumentOutOfRangeException()
        {
            var size = this.GenerateRandomSize();
            var inventory = this.inventoryFactory.EmptyContainer(size);

            Assert.That(
                () => inventory.Replace(this.itemFactory.CreateDefault(), size),
                Throws.Exception.TypeOf<ArgumentOutOfRangeException>().With.Property("ParamName").EqualTo("index")
            );
        }
        #endregion

        #region Empty slot
        [Test]
        public void Replace_EmptySlot_ThrowsInvalidOperationException()
        {
            var size = this.GenerateRandomSize();
            var inventory = this.inventoryFactory.EmptyContainer(size);

            var item = this.itemFactory.CreateDefault();
            var randomIndex = this.random.Next(0, size);
            Assert.That(
                () => inventory.Replace(item, randomIndex),
                Throws.Exception.TypeOf<InvalidOperationException>().With.Message.EqualTo("The slot is empty.")
            );
        }

        [Test]
        public void Replace_EmptySlot_DoesNotCallsOnReplace()
        {
            var size = this.GenerateRandomSize();
            var inventory = this.inventoryFactory.EmptyContainer(size);

            var raised = false;
            inventory.OnReplace += (_,_) => raised = true;

            Assert.Multiple(() =>
            {
                var item = this.itemFactory.CreateDefault();
                var randomIndex = this.random.Next(0, size);
                Assert.That(() => inventory.Replace(item, randomIndex), Throws.Exception);
                Assert.That(raised, Is.False, "OnReplace event was raised");
            });
        }
        #endregion

        #region Full slot
        [Test]
        [IgnoreIfReferenceType]
        public void Replace_DefaultValue_FullSlot_ReturnsOldItem()
        {
            var size = this.GenerateRandomSize();
            var initialItem = this.itemFactory.CreateDefault();
            var inventory = this.inventoryFactory.FullContainer(size, initialItem);

            var newItem = default(T);
            var randomIndex = this.random.Next(0, size);
            var result = inventory.Replace(newItem, randomIndex);

            Assert.That(result, Is.EqualTo(initialItem));
        }

        [Test]
        [IgnoreIfReferenceType]
        public void Replace_DefaultValue_FullSlot_ReplacesItemInSlot()
        {
            var size = this.GenerateRandomSize();
            var initialItem = this.itemFactory.CreateDefault();
            var inventory = this.inventoryFactory.FullContainer(size, initialItem);

            var newItem = default(T);
            var randomIndex = this.random.Next(0, size);
            inventory.Replace(newItem, randomIndex);

            Assert.That(inventory.GetItem(randomIndex), Is.EqualTo(newItem));
        }

        [Test]
        [IgnoreIfReferenceType]
        public void Replace_DefaultValue_FullSlot_CallsOnReplaceEvent()
        {
            var size = this.GenerateRandomSize();
            var initialItem = this.itemFactory.CreateDefault();
            var inventory = this.inventoryFactory.FullContainer(size, initialItem);

            var raised = false;
            var randomIndex = this.random.Next(0, size);
            var newItem = default(T);

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

            inventory.Replace(newItem, randomIndex);

            Assert.That(raised, Is.True, "OnReplace event was not raised");
        }

        [Test]
        public void Replace_FullSlot_ReplacesItemInSlot()
        {
            var size = this.GenerateRandomSize();
            var initialItem = this.itemFactory.CreateDefault();
            var inventory = this.inventoryFactory.FullContainer(size, initialItem);

            var newItem = this.itemFactory.CreateRandom();
            var randomIndex = this.random.Next(0, size);
            inventory.Replace(newItem, randomIndex);

            Assert.That(inventory.GetItem(randomIndex), Is.EqualTo(newItem));
        }

        [Test]
        public void Replace_FullSlot_CallsOnReplaceEvent()
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

            inventory.Replace(newItem, randomIndex);

            Assert.That(raised, Is.True, "OnReplace event was not raised");
        }

        [Test]
        public void Replace_FullSlot_ReturnsOldItemFromSlot()
        {
            var size = this.GenerateRandomSize();
            var initialItem = this.itemFactory.CreateDefault();
            var inventory = this.inventoryFactory.FullContainer(size, initialItem);

            var randomIndex = this.random.Next(0, size);
            var newItem = this.itemFactory.CreateRandom();
            var result = inventory.Replace(newItem, randomIndex);

            Assert.That(result, Is.EqualTo(initialItem));
        }
        #endregion
    }
}
