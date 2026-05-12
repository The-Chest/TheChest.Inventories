using TheChest.Tests.Common.Attributes;
using TheChest.Tests.Common.Extensions;

namespace TheChest.Inventories.Tests.Containers.StackInventory
{
    public partial class StackInventoryTests<T>
    {
        [Test]
        [IgnoreIfValueType]
        public void CanAddItems_NullItem_ThrowsArgumentNullException()
        {
            var inventory = this.inventoryFactory.EmptyContainer();
            Assert.That(() => inventory.CanAdd(items: default!), Throws.ArgumentNullException);
        }

        [Test]
        [IgnoreIfValueType]
        public void CanAddItems_ArrayContainingNullItem_ThrowsArgumentNullException()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var inventory = this.inventoryFactory.EmptyContainer(size, stackSize);
            
            var items = this.itemFactory.CreateMany(stackSize - 1)
                .Append(default!)
                .ToArray();
            items.Shuffle();
            Assert.That(() => inventory.CanAdd(items), Throws.ArgumentNullException);
        }

        [Test]
        public void CanAddItems_ArrayContainingDifferentItemType_ThrowsArgumentException()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var inventory = this.inventoryFactory.EmptyContainer(size, stackSize);

            var items = this.itemFactory.CreateMany(stackSize - 1)
                .Append(this.itemFactory.CreateRandom())
                .ToArray();
            items.Shuffle();
            Assert.That(() => inventory.CanAdd(items.ToArray()), Throws.ArgumentException); 
        }
    }
}
