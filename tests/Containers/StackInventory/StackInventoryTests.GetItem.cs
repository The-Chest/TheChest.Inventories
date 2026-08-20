using TheChest.Tests.Common.Extensions.Containers;

using TheChest.Tests.Common.Attributes;

namespace TheChest.Inventories.Tests.Containers.StackInventory
{
    public partial class StackInventoryTests<T>
    {
        [Test]
        [IgnoreIfValueType]
        public void GetItem_NullItem_ThrowsArgumentNullException()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var inventory = this.inventoryFactory.EmptyContainer(size, stackSize);

            Assert.That(
                () => inventory.Get(default(T)!),
                Throws.ArgumentNullException.With.Property("ParamName").EqualTo("item")
            );
        }

        [Test]
        public void GetItem_EmptyInventory_ThrowsInvalidOperationException()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var item = this.itemFactory.CreateDefault();
            var inventory = this.inventoryFactory.EmptyContainer(size, stackSize);

            Assert.That(() => inventory.Get(item), Throws.InvalidOperationException);
        }

        [Test]
        public void GetItem_InventoryWithDifferentItems_ThrowsInvalidOperationException()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var slotItem = this.itemFactory.CreateRandom();
            var inventory = this.inventoryFactory.FullContainer(size, stackSize, slotItem);

            var item = this.itemFactory.CreateDefault();

            Assert.That(() => inventory.Get(item), Throws.InvalidOperationException);
        }

        [Test]
        [IgnoreIfReferenceType]
        public void GetItem_ValueType_DefaultItem_ThrowsInvalidOperationException()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var inventory = this.inventoryFactory.EmptyContainer(size, stackSize);

            Assert.That(() => inventory.Get((T)default!), Throws.InvalidOperationException);
        }

        [Test]
        public void GetItem_EmptyInventory_DoesNotCallOnGetEvent()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var inventory = this.inventoryFactory.EmptyContainer(size, stackSize);

            inventory.OnGet += (sender, args) => Assert.Fail("OnGet event should not be called when no item is found");

            var item = this.itemFactory.CreateDefault();

            Assert.That(() => inventory.Get(item), Throws.InvalidOperationException);
        }

        [Test]
        public void GetItem_InventoryWithDifferentItems_DoesNotCallOnGetEvent()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var slotItem = this.itemFactory.CreateRandom();
            var inventory = this.inventoryFactory.FullContainer(size, stackSize, slotItem);

            inventory.OnGet += (sender, args) => Assert.Fail("OnGet event should not be called when no item is found");

            var item = this.itemFactory.CreateDefault();

            Assert.That(() => inventory.Get(item), Throws.InvalidOperationException);
        }

        [Test]
        public void GetItem_InventoryWithItems_RemovesItemFromFirstFoundSlot()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var slotItem = this.itemFactory.CreateRandom();
            var inventory = this.inventoryFactory.FullContainer(size, stackSize, slotItem);

            inventory.Get(slotItem);

            Assert.That(inventory.GetSlot(0)!.Amount, Is.EqualTo(stackSize - 1));
        }

        [Test]
        public void GetItem_InventoryWithItems_CallsOnGetEvent()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var slotItem = this.itemFactory.CreateRandom();
            var inventory = this.inventoryFactory.FullContainer(size, stackSize, slotItem);

            var raised = false;
            inventory.OnGet += (sender, args) =>
            {
                Assert.Multiple(() =>
                {
                    var firstEvent = args.Data.FirstOrDefault();
                    Assert.That(args.Data, Has.Count.EqualTo(1));
                    Assert.That(firstEvent.Items, Has.All.EqualTo(slotItem));
                    Assert.That(firstEvent.Index, Is.EqualTo(0));
                });
                raised = true;
            };

            inventory.Get(slotItem);

            Assert.That(raised, Is.True, "OnGet event was not raised");
        }

        [Test]
        public void GetItem_InventoryWithItems_ReturnsItem()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var slotItem = this.itemFactory.CreateRandom();
            var inventory = this.inventoryFactory.FullContainer(size, stackSize, slotItem);

            var item = inventory.Get(slotItem);

            Assert.That(item, Is.EqualTo(slotItem));
        }
    }
}
