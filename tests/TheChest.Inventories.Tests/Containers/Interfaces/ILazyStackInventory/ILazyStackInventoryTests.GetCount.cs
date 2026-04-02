namespace TheChest.Inventories.Tests.Containers.Interfaces
{
	public partial class ILazyStackInventoryTests<T>
	{
		[Test]
		public void GetCount_NotFoundItem_ReturnsZero()
		{
			var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
			var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
			var wrongItem = this.itemFactory.CreateRandom();
			var inventory = this.inventoryFactory.ShuffledItemsContainer(size, stackSize, wrongItem);

			var item = this.itemFactory.CreateDefault();
			var count = inventory.GetCount(item);

			Assert.That(count, Is.Zero);
		}

		[Test]
		public void GetCount_ExistingItem_ReturnsCorrectCount()
		{
			var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
			var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
			var item = this.itemFactory.CreateDefault();
			var inventory = this.inventoryFactory.FullContainer(size, stackSize, item);

			var count = size * stackSize;
			var result = inventory.GetCount(item);

			Assert.That(result, Is.EqualTo(count));
		}
	}
}
