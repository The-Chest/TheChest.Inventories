using TheChest.Tests.Common.Extensions.Containers;

using TheChest.Tests.Common.Attributes;

namespace TheChest.Inventories.Tests.Containers.StackInventory
{
    public partial class StackInventoryTests<T>
    {
        [Test]
        [IgnoreIfValueType]
        public void AddItemAt_NullItem_ThrowsArgumentException()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var inventory = this.inventoryFactory.EmptyContainer(size, stackSize);

            Assert.That(
                () => inventory.AddAt(default(T)!, 0),
                Throws.ArgumentNullException.With.Property("ParamName").EqualTo("item")
            );
        }

        [TestCase(-1)]
        [TestCase(MAX_SIZE_TEST)]
        public void AddItemAt_InvalidIndex_ThrowsArgumentOutOfRangeException(int index)
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var inventory = this.inventoryFactory.EmptyContainer(size, stackSize);

            var item = this.itemFactory.CreateDefault();
            Assert.That(
                () => inventory.AddAt(item, index),
                Throws.TypeOf<ArgumentOutOfRangeException>()
            );
        }

        [Test]
        public void AddItemAt_SlotWithDifferentItem_ThrowsInvalidOperationException()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var inventoryItem = this.itemFactory.CreateRandom();
            var inventory = this.inventoryFactory.FullContainer(size, stackSize, inventoryItem);

            var index = this.random.Next(0, size);
            var item = this.itemFactory.CreateDefault();

            Assert.That(() => inventory.AddAt(item, index), Throws.InvalidOperationException);
        }

        [Test]
        public void AddItemAt_FullSlotWithSameItem_ThrowsInvalidOperationException()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var slotItem = this.itemFactory.CreateDefault();
            var inventory = this.inventoryFactory.FullContainer(size, stackSize, slotItem);

            var item = this.itemFactory.CreateDefault();
            var index = this.random.Next(0, size);

            Assert.That(() => inventory.AddAt(item, index), Throws.InvalidOperationException);
        }

        [Test]
        public void AddItemAt_SlotWithDifferentItem_DoesNotAddItem()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var items = this.itemFactory.CreateManyRandom(size);
            var inventory = this.inventoryFactory.ShuffledItemsContainer(size, stackSize, items);

            var index = this.random.Next(0, size);
            var item = this.itemFactory.CreateDefault();

            Assert.That(() => inventory.AddAt(item, index), Throws.InvalidOperationException);
            Assert.That(inventory.GetItems(index), Has.No.AnyOf(item));
        }

        [Test]
        public void AddItemAt_SlotWithDifferentItem_DoesNotCallOnAddEvent()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var containerItem = this.itemFactory.CreateRandom();
            var inventory = this.inventoryFactory.FullContainer(size, stackSize, containerItem);

            var index = this.random.Next(0, size);
            var item = this.itemFactory.CreateDefault();
            inventory.OnAdd += (sender, args) => Assert.Fail("OnAdd event should not be called when item is not possible to add");

            Assert.That(() => inventory.AddAt(item, index), Throws.InvalidOperationException);
        }

        [Test]
        public void AddItemAt_FullSlotWithSameItem_DoesNotCallOnAddEvent()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var containerItem = this.itemFactory.CreateDefault();
            var inventory = this.inventoryFactory.FullContainer(size, stackSize, containerItem);

            var index = this.random.Next(0, size);
            var item = this.itemFactory.CreateDefault();

            inventory.OnAdd += (sender, args) => Assert.Fail("OnAdd event should not be called when item is not possible to add");

            Assert.That(() => inventory.AddAt(item, index), Throws.InvalidOperationException);
        }

        [Test]
        public void AddItemAt_EmptySlot_AddsToStack()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var inventory = this.inventoryFactory.EmptyContainer(size, stackSize);

            var index = this.random.Next(0, size);
            var item = this.itemFactory.CreateDefault();
            inventory.AddAt(item, index);

            Assert.That(inventory.GetItems(index), Has.One.EqualTo(item));
        }

        [Test]
        public void AddItemAt_SlotWithSameItem_AddsToStack()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var containerItem = this.itemFactory.CreateDefault();
            var inventory = this.inventoryFactory.FullContainer(size, stackSize, containerItem);

            var index = this.random.Next(0, size);
            inventory.Get(index);

            var expectedStackSize = inventory.GetSlot(index).Amount + 1;
            var item = this.itemFactory.CreateDefault();
            inventory.AddAt(item, index);

            Assert.That(inventory.GetSlot(index).Amount, Is.EqualTo(expectedStackSize));
        }

        [Test]
        public void AddItemAt_EmptySlot_CallsOnAddEvent()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var inventory = this.inventoryFactory.EmptyContainer(size, stackSize);

            var item = this.itemFactory.CreateDefault();
            var index = this.random.Next(0, size);

            var raised = false;
            inventory.OnAdd += (sender, args) =>
            {
                Assert.Multiple(() =>
                {
                    var firstEvent = args.Data.First();
                    Assert.That(args.Data, Has.Count.EqualTo(1));
                    Assert.That(firstEvent.Items, Has.Length.EqualTo(1).And.All.EqualTo(item));
                    Assert.That(firstEvent.Index, Is.EqualTo(index));
                });
                raised = true;
            };

            inventory.AddAt(item, index);

            Assert.That(raised, Is.True, "OnAdd event was not raised");
        }

        [Test]
        public void AddItemAt_SlotWithSameItem_CallsOnAddEvent()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var containerItem = this.itemFactory.CreateDefault();
            var inventory = this.inventoryFactory.FullContainer(size, stackSize, containerItem);

            var index = this.random.Next(0, size - 1);
            inventory.Get(index);

            var item = this.itemFactory.CreateDefault();

            var raised = false;
            inventory.OnAdd += (sender, args) =>
            {
                Assert.Multiple(() =>
                {
                    var firstEvent = args.Data.First();
                    Assert.That(args.Data, Has.Count.EqualTo(1));
                    Assert.That(firstEvent.Items, Has.Length.EqualTo(1).And.All.EqualTo(item));
                    Assert.That(firstEvent.Index, Is.EqualTo(index));
                });
                raised = true;
            };
            inventory.AddAt(item, index);

            Assert.That(raised, Is.True, "OnAdd event was not raised");
        }

        [Test]
        public void AddItemAt_EmptySlot_ReturnsTrue()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var inventory = this.inventoryFactory.EmptyContainer(size, stackSize);

            var item = this.itemFactory.CreateDefault();
            var index = this.random.Next(0, size);
            var result = inventory.AddAt(item, index);

            Assert.That(result, Is.True);
        }

        [Test]
        public void AddItemAt_SlotWithSameItem_ReturnsTrue()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var inventoryItem = this.itemFactory.CreateDefault();
            var inventory = this.inventoryFactory.FullContainer(size, stackSize, inventoryItem);

            var index = this.random.Next(0, size);
            inventory.Get(index);

            var item = this.itemFactory.CreateDefault();
            var result = inventory.AddAt(item, index);

            Assert.That(result, Is.True);
        }

        [Test]
        [IgnoreIfReferenceType]
        public void AddItemAt_ValueType_DefaultItem_ReturnsTrue()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var inventory = this.inventoryFactory.EmptyContainer(size, stackSize);

            var added = inventory.AddAt((T)default!, 0);

            Assert.That(added, Is.True);
        }
    }
}
