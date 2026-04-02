using NUnit.Framework.Internal;
using TheChest.Core.Slots.Interfaces;
using TheChest.Tests.Common.Extensions.Containers;
using TheChest.Tests.Common.Extensions.Slots;

namespace TheChest.Inventories.Tests.Containers.LazyStackInventory
{
    public partial class LazyStackInventoryTests<T>
    {
        [Test]
        public void Clear_EmptyInventory_CallsOnGetEvent()
        {
            var inventory = this.inventoryFactory.EmptyContainer();
            
            inventory.OnGet += (sender, args) => Assert.Fail("OnGet event should not be called for empty inventory");
            
            inventory.Clear();
        }

        [Test]
        public void Clear_InventoryWithItems_CallsOnGetEvent()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var item = this.itemFactory.CreateRandom();
            var inventory = this.inventoryFactory.FullContainer(size, stackSize, item);

            var raised = false;
            inventory.OnGet += (sender, args) => {
                Assert.That(args.Data, Has.Count.EqualTo(size));
                Assert.Multiple(() =>
                {
                    Assert.That(args.Data.Select(x => x.Item), Has.All.EqualTo(item));
                    Assert.That(args.Data.Select(x => x.Amount), Has.All.EqualTo(stackSize));
                });
                raised = true;
            };
            inventory.Clear();

            Assert.That(raised, Is.True, "OnGet event was not raised");
        }

        [Test]
        public void Clear_InventoryWithItems_RemoveItemsFromAllSlots()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var item = this.itemFactory.CreateDefault();
            var inventory = this.inventoryFactory.FullContainer(size, stackSize, item);

            inventory.Clear();

            Assert.Multiple(() =>
            {
                Assert.That(inventory.GetSlots(),
                    Has.All.Matches<ILazyStackSlot<T>>(
                        slot => slot.GetContent() is null && slot.Amount == 0
                    )
                );
                Assert.That(inventory.IsEmpty, Is.True);
            });
        }
    }
}
