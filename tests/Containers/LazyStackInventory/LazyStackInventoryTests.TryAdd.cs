using TheChest.Tests.Common.Extensions.Containers;
using TheChest.Tests.Common.Extensions.Slots;

namespace TheChest.Inventories.Tests.Containers.LazyStackInventory
{
    public partial class LazyStackInventoryTests<T>
    {
        [Test]
        public void TryAdd_NullItem_ThrowsArgumentNullException()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var inventory = this.inventoryFactory.EmptyContainer(size, stackSize);

            Assert.That(
                () => inventory.TryAdd(default!, 1),
                Throws.ArgumentNullException.With.Property("ParamName").EqualTo("item")
            );
        }

        [TestCase(0)]
        [TestCase(-1)]
        public void TryAdd_ZeroOrLessAmount_ThrowsArgumentOutOfRangeException(int amount)
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var inventory = this.inventoryFactory.EmptyContainer(size, stackSize);
            var item = this.itemFactory.CreateDefault();

            Assert.That(
                () => inventory.TryAdd(item, amount),
                Throws.TypeOf<ArgumentOutOfRangeException>().With.Property("ParamName").EqualTo("amount")
            );
        }

        [Test]
        public void TryAdd_NotEnoughSpace_ReturnsFalse()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var randomItems = this.itemFactory.CreateManyRandom(size - 1);
            var inventory = this.inventoryFactory.ShuffledItemsContainer(size, stackSize, randomItems);

            var item = this.itemFactory.CreateDefault();
            var amount = stackSize + this.random.Next(1, 5);

            var result = inventory.TryAdd(item, amount);

            Assert.That(result, Is.False);
        }

        [Test]
        public void TryAdd_NotEnoughSpace_DoesNotAddItem()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var randomItems = this.itemFactory.CreateManyRandom(size - 1);
            var inventory = this.inventoryFactory.ShuffledItemsContainer(size, stackSize, randomItems);

            var initialSlots = inventory.GetSlots().Select(slot => (slot.GetContent(), slot.Amount)).ToArray();
            var item = this.itemFactory.CreateDefault();
            var amount = stackSize + this.random.Next(1, 5);

            inventory.TryAdd(item, amount);

            Assert.That(
                inventory.GetSlots().Select(slot => (slot.GetContent(), slot.Amount)).ToArray(),
                Is.EqualTo(initialSlots)
            );
        }

        [Test]
        public void TryAdd_NotEnoughSpace_DoesNotCallOnAddEvent()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var randomItems = this.itemFactory.CreateManyRandom(size - 1);
            var inventory = this.inventoryFactory.ShuffledItemsContainer(size, stackSize, randomItems);
            var item = this.itemFactory.CreateDefault();
            var amount = stackSize + this.random.Next(1, 5);

            inventory.OnAdd += (sender, args) => Assert.Fail("OnAdd should not be called when TryAdd returns false.");

            inventory.TryAdd(item, amount);
        }

        [Test]
        public void TryAdd_EnoughSpace_ReturnsTrue()
        {
            var (size, stackSize) = this.GenerateRandomSizeAndStackSize();
            var inventory = this.inventoryFactory.EmptyContainer(size, stackSize);
            var item = this.itemFactory.CreateDefault();
            var amount = this.random.Next(1, stackSize);

            var result = inventory.TryAdd(item, amount);

            Assert.That(result, Is.True);
        }
    }
}
