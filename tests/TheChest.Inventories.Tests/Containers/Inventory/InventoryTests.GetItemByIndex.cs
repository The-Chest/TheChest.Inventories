using NUnit.Framework.Internal;
using TheChest.Tests.Common.Extensions;

namespace TheChest.Inventories.Tests.Containers
{
    public partial class InventoryTests<T>
    {
        [TestCase(-1)]
        [TestCase(30)]
        public void GetItemByIndex_InvalidIndex_ThrowsArgumentOutOfRangeException(int index)
        {
            var size = this.random.Next(10, 20);
            var item = this.itemFactory.CreateDefault();
            var inventory = this.containerFactory.FullContainer(size, item);

            Assert.That(
                () => inventory.Get(index), 
                Throws.Exception.TypeOf<ArgumentOutOfRangeException>()
            );
        }

        [Test]
        public void GetItemByIndex_ValidIndexEmptySlot_DoesNotCallOnGetEvent()
        {
            var size = this.random.Next(10, 20);
            var inventory = this.containerFactory.EmptyContainer();
            var index = this.random.Next(0, size);

            inventory.OnGet += (sender, args) => Assert.Fail("Get(int index) should not be called if no item is found");
            
            inventory.Get(index);
        }

        [Test]
        public void GetItemByIndex_ValidIndexFullSlot_RemovesItemFromSlot()
        {
            var size = this.random.Next(10, 20);
            var item = this.itemFactory.CreateDefault();
            var inventory = this.containerFactory.FullContainer(size, item);

            var randomIndex = this.random.Next(0, size);
            inventory.Get(randomIndex);

            Assert.That(inventory.GetSlot<T>(randomIndex).IsEmpty, Is.True);
        }

        [Test]
        public void GetItemByIndex_ExistingItemOnSlot_CallsOnGetEvent()
        {
            var size = this.random.Next(10, 20);
            var item = this.itemFactory.CreateDefault();
            var inventory = this.containerFactory.FullContainer(size, item);

            var randomIndex = this.random.Next(0, size);
            var raised = false;
            inventory.OnGet += (sender, args) => {
                Assert.That(args.Data, Has.Count.EqualTo(1));
                var eventData = args.Data.First();
                Assert.Multiple(() =>
                {
                    Assert.That(eventData.Item, Is.EqualTo(item));
                    Assert.That(eventData.Index, Is.EqualTo(randomIndex));
                });
                raised = true;
            };

            inventory.Get(randomIndex); 
            
            Assert.That(raised, Is.True, "OnGet event was not raised");
        }
    }
}
