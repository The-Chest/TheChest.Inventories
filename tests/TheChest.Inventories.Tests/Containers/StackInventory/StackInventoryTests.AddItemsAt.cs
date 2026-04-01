using TheChest.Tests.Common.Extensions.Containers;

namespace TheChest.Inventories.Tests.Containers
{
    public partial class StackInventoryTests<T>
    {
        [TestCase(-1)]
        [TestCase(100)]
        public void AddItemsAt_InvalidIndex_ThrowsArgumentOutOfRangeException(int index)
        {
            var items = this.itemFactory.CreateMany(20);
            var inventory = this.inventoryFactory.EmptyContainer(20);

            Assert.That(() => inventory.AddAt(items, index), Throws.TypeOf<ArgumentOutOfRangeException>());
        }

        [Test]
        public void AddItemsAt_EmptySlot_AddsToStack()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var stackSize = this.random.Next(1, 10);
            var inventory = this.inventoryFactory.EmptyContainer(size, stackSize);

            var items = this.itemFactory.CreateMany(10);
            var index = this.random.Next(0, size);
            inventory.AddAt(items, index);

            Assert.That(inventory.GetItems(index), Is.EqualTo(items));
        }

        [Test]
        public void AddItemsAt_EmptySlot_CallsOnAddEvent()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var stackSize = this.random.Next(1, 10);
            var inventory = this.inventoryFactory.EmptyContainer(size, stackSize);

            var items = this.itemFactory.CreateMany(10);
            var index = this.random.Next(0, size);

            var raised = false;
            inventory.OnAdd += (sender, e) => {
                Assert.That(e.Data, Has.Count.EqualTo(1));
                Assert.Multiple(() =>
                {
                    var firstEvent = e.Data.First();
                    Assert.That(firstEvent.Items, Is.EqualTo(items));
                    Assert.That(firstEvent.Index, Is.EqualTo(index));
                });
                raised = true;
            };

            inventory.AddAt(items, index);

            Assert.That(raised, Is.True, "OnAdd event was not raised");
        }

        [Test]
        public void AddItemsAt_SlotWithDifferentItem_DoesNotCallOnAddEvent()
        {
            var index = this.random.Next(0, MAX_SIZE_TEST);
            var slotItem = this.itemFactory.CreateRandom();
            var amount = this.random.Next(1, 10);
            var inventory = this.inventoryFactory.FullContainer(20, amount, slotItem);

            var items = this.itemFactory.CreateMany(amount);
            inventory.OnAdd += (sender, e) => Assert.Fail("OnAdd event should not be called when is not possible to Add.");

            inventory.AddAt(items, index);
        }
    }
}
