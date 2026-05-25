using TheChest.Tests.Common.Extensions.Containers;

namespace TheChest.Inventories.Tests.Containers.StackInventory
{
    public partial class StackInventoryTests<T>
    {
        [Test]
        public void TryAddItemAt_NullItem_ThrowsArgumentNullException()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var inventory = this.inventoryFactory.EmptyContainer(size, stackSize);

            Assert.That(
                () => inventory.TryAddAt(default!, 0),
                Throws.ArgumentNullException.With.Property("ParamName").EqualTo("item")
            );
        }

        [TestCase(-1)]
        [TestCase(MAX_SIZE_TEST)]
        public void TryAddItemAt_InvalidIndex_ThrowsArgumentOutOfRangeException(int index)
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var inventory = this.inventoryFactory.EmptyContainer(size, stackSize);
            var item = this.itemFactory.CreateDefault();

            Assert.That(
                () => inventory.TryAddAt(item, index),
                Throws.TypeOf<ArgumentOutOfRangeException>().With.Property("ParamName").EqualTo("index")
            );
        }

        [Test]
        public void TryAddItemAt_FullSlot_ReturnsFalse()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var slotItem = this.itemFactory.CreateDefault();
            var inventory = this.inventoryFactory.FullContainer(size, stackSize, slotItem);

            var result = inventory.TryAddAt(slotItem, 0);

            Assert.That(result, Is.False);
        }

        [Test]
        public void TryAddItemAt_FullSlot_DoesNotCallOnAddEvent()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var slotItem = this.itemFactory.CreateDefault();
            var inventory = this.inventoryFactory.FullContainer(size, stackSize, slotItem);

            inventory.OnAdd += (_, _) => Assert.Fail("OnAdd event should not be called when TryAddAt returns false");
            inventory.TryAddAt(slotItem, 0);
        }

        [Test]
        public void TryAddItemAt_EmptySlot_ReturnsTrue()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var inventory = this.inventoryFactory.EmptyContainer(size, stackSize);
            var item = this.itemFactory.CreateDefault();

            var result = inventory.TryAddAt(item, 0);

            Assert.That(result, Is.True);
        }

        [Test]
        public void TryAddItemAt_EmptySlot_CallsOnAddEvent()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var inventory = this.inventoryFactory.EmptyContainer(size, stackSize);
            var item = this.itemFactory.CreateDefault();

            var raised = false;
            inventory.OnAdd += (_, args) =>
            {
                Assert.That(args.Data, Has.Count.EqualTo(1));
                raised = true;
            };

            inventory.TryAddAt(item, 0);

            Assert.That(raised, Is.True);
        }
    }
}
