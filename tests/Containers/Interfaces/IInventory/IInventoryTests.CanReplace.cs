namespace TheChest.Inventories.Tests.Containers.Interfaces
{
    public partial class IInventoryTests<T>
    {
        [Test]
        public void CanReplace_EmptySlot_ReturnsTrue()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var inventory = this.inventoryFactory.EmptyContainer(size);
            var randomIndex = this.random.Next(0, size);

            var result = inventory.CanReplace(this.itemFactory.CreateDefault(), randomIndex);

            Assert.That(result, Is.True);
        }

        [Test]
        public void CanReplace_FullSlot_ReturnsTrue()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var inventory = this.inventoryFactory.FullContainer(size, this.itemFactory.CreateDefault());
            var randomIndex = this.random.Next(0, size);

            var result = inventory.CanReplace(this.itemFactory.CreateRandom(), randomIndex);

            Assert.That(result, Is.True);
        }
    }
}
