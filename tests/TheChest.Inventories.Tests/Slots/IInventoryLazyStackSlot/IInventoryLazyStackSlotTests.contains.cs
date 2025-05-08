namespace TheChest.Inventories.Tests.Slots
{
    public partial class IInventoryLazyStackSlotTests<T>
    {
        [Test]
        public void Contains_EmptySlot_ReturnsFalse()
        {
            var slot = this.slotFactory.EmptySlot();
            var item = this.itemFactory.CreateDefault();

            var result = slot.Contains(item);

            Assert.That(result, Is.False);
        }

        [Test]
        public void Contains_ItemDoesNotMatchingContent_ReturnsFalse()
        {
            var item = this.itemFactory.CreateDefault();
            var slot = this.slotFactory.WithItem(item, 1, 10);

            var checkItem = this.itemFactory.CreateRandom();
            var result = slot.Contains(checkItem);

            Assert.That(result, Is.False);
        }

        [Test]
        public void Contains_ShouldReturnTrue_WhenItemMatchesContent()
        {
            var item = this.itemFactory.CreateDefault();
            var slot = this.slotFactory.WithItem(item, 1, 10);

            var checkItem = this.itemFactory.CreateDefault();
            var result = slot.Contains(checkItem);

            Assert.That(result, Is.True);
        }
    }
}
