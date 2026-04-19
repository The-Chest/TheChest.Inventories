using TheChest.Tests.Common.Extensions.Slots;

namespace TheChest.Inventories.Tests.Slots.InventoryStackSlot
{
    public partial class InventoryStackSlotTests<T>
    {
        [Test]
        public void AddItems_AddingDifferentItems_ThrowsArgumentException()
        {
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var slot = this.slotFactory.EmptySlot(stackSize);

            var randomSize = this.random.Next(1, stackSize);
            var addingItems = this.itemFactory
                .CreateManyRandom(randomSize)
                .Append(this.itemFactory.CreateRandom())
                .ToArray();

            Assert.That(() => slot.Add(addingItems), Throws.ArgumentException);
        }

        [Test]
        public void AddItems_DifferentItemsFromSlot_ThrowsArgumentException()
        {
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var items = new T[1] {  this.itemFactory.CreateRandom() };
            var slot = this.slotFactory.WithItems(items, stackSize);

            var addingItems = this.itemFactory.CreateManyRandom(stackSize);

            Assert.That(() => slot.Add(addingItems), Throws.ArgumentException);
        }

        [Test]
        public void AddItems_OneItemDifferentFromSlot_ThrowsArgumentException()
        {
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var items = new T[1] { this.itemFactory.CreateRandom() };
            var slot = this.slotFactory.WithItems(items, stackSize);

            var addingItems = this.itemFactory.CreateMany(stackSize)
                .Append(this.itemFactory.CreateRandom())
                .ToArray();

            Assert.That(() => slot.Add(addingItems), Throws.ArgumentException);
        }

        [Test]
        public void AddItems_AddingNoItems_ThrowsArgumentException()
        {
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var slot = this.slotFactory.EmptySlot(stackSize);

            var addingItems = Array.Empty<T>();

            Assert.That(() => slot.Add(addingItems), Throws.ArgumentException);
        }

        [Test]
        public void AddItems_EmptySlot_WithMoreItemsThanMaxAmount_AddsAllItems()
        {
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var slot = this.slotFactory.EmptySlot(stackSize);

            var randomSize = this.random.Next(stackSize + 1, stackSize + 20);
            var addingItems = this.itemFactory.CreateManyRandom(randomSize);
            var expectedItems = (T[])addingItems[0..stackSize].Clone();
            slot.Add(addingItems);

            Assert.That(slot.GetContents(), Is.EqualTo(expectedItems));
        }


        [Test]
        public void Add_FullSlot_DoesNotAdd()
        {
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var items = this.itemFactory.CreateMany(stackSize);
            var slot = this.slotFactory.FullSlot(items);

            var addingItems = this.itemFactory.CreateMany(stackSize);
            slot.Add(addingItems);

            Assert.That(slot.GetContents(), Is.EqualTo(items));
        }
    }
}
