using TheChest.Tests.Common.Extensions.Containers;
using TheChest.Tests.Common.Extensions.Slots;

namespace TheChest.Inventories.Tests.Containers.StackInventory
{
    public partial class StackInventoryTests<T>
    {
        [Test]
        public void GetItems_WhenAmountIsZeroOrSmaller_ThrowsArgumentOutOfRangeException()
        {
            var inventory = this.inventoryFactory.EmptyContainer();
            var item = this.itemFactory.CreateDefault();

            Assert.That(
                () => inventory.Get(item, 0), 
                Throws.TypeOf(typeof(ArgumentOutOfRangeException)).With.Property("ParamName").EqualTo("amount")
            );
        }

        [Test]
        public void GetItems_InvalidItem_ThrowsArgumentNullException()
        {
            var inventory = this.inventoryFactory.EmptyContainer();

            Assert.That(
                () => inventory.Get(default(T)!, 1), 
                Throws.ArgumentNullException.With.Property("ParamName").EqualTo("item")
            );
        }

        [Test]
        public void GetItems_ItemNotFound_DoesNotCallOnGetEvent()
        {
            var inventory = this.inventoryFactory.EmptyContainer();
            var item = this.itemFactory.CreateDefault();

            inventory.OnGet += (sender, args) => Assert.Fail("OnGet event should not be called when no item is found");
            
            inventory.Get(item, 1);
        }

        [Test]
        public void GetItems_ItemsFound_RemovesItems()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var item = this.itemFactory.CreateDefault();
            var inventoryItems = this.itemFactory.CreateManyRandom(size - 1)
                .Append(item)
                .ToList();
            var inventory = this.inventoryFactory.ShuffledItemsContainer(size, stackSize, inventoryItems.ToArray());

            inventory.Get(item, stackSize);

            Assert.That(inventory.GetSlots().Any(x => x.GetContents()?.Contains(item) ?? false), Is.False);
        }

        [Test]
        public void GetItems_FoundItems_CallsOnGetEvent()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var item = this.itemFactory.CreateDefault();
            var inventoryItems = this.itemFactory.CreateManyRandom(size - 1)
                .Append(item)
                .ToList();
            var inventory = this.inventoryFactory.ShuffledItemsContainer(size, stackSize, inventoryItems.ToArray());

            var raised = false;
            inventory.OnGet += (sender, args) =>
            {
                Assert.Multiple(() =>
                {
                    var firstEvent = args.Data.FirstOrDefault();
                    Assert.That(args.Data, Has.Count.EqualTo(1));
                    Assert.That(firstEvent.Items, Has.Length.EqualTo(stackSize).And.All.EqualTo(item));
                });
                raised = true;
            };

            inventory.Get(item, stackSize);

            Assert.That(raised, Is.True, "OnGet event was not raised");
        }

        [Test]
        public void GetItems_AmoutBiggerThanItemsInInventory_CallsOnGetEvent()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var item = this.itemFactory.CreateDefault();
            var inventoryItems = this.itemFactory.CreateManyRandom(size - 1)
                .Append(item)
                .ToList();

            var inventory = this.inventoryFactory.ShuffledItemsContainer(size, stackSize, inventoryItems.ToArray());
            inventory.OnGet += (sender, args) =>
            {
                Assert.Multiple(() =>
                {
                    Assert.That(args.Data, Has.Count.EqualTo(1));
                    Assert.That(args.Data.First().Items, Has.All.EqualTo(item));
                });
            };
            inventory.Get(item, 100);
        }


        [Test]
        public void GetItems_ItemNotFound_ReturnsEmptyArray()
        {
            var item = this.itemFactory.CreateDefault();
            var inventory = this.inventoryFactory.EmptyContainer();

            var items = inventory.Get(item, 1);

            Assert.That(items, Is.Empty);
        }

        [Test]
        public void GetItems_ItemsFound_ReturnsItems()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var items = this.itemFactory.CreateManyRandom(size / 2);
            var inventoryItems = this.itemFactory
                .CreateManyRandom(size / 2)
                .ToList();
            inventoryItems.AddRange(items);

            var inventory = this.inventoryFactory.ShuffledItemsContainer(size, stackSize, inventoryItems.ToArray());

            var expectedItem = items[0];
            var amount = this.random.Next(1, stackSize);
            var result = inventory.Get(expectedItem, amount);

            Assert.That(result, Has.Length.EqualTo(amount));
            Assert.That(result, Has.All.EqualTo(expectedItem));
        }

        [Test]
        public void GetItems_AmoutBiggerThanItemsInInventory_ReturnsAllItemsFound()
        {
            var size = this.random.Next(MIN_SIZE_TEST, MAX_SIZE_TEST);
            var stackSize = this.random.Next(MIN_STACK_SIZE_TEST, MAX_STACK_SIZE_TEST);
            var item = this.itemFactory.CreateDefault();
            var inventoryItems = this.itemFactory.CreateManyRandom(size - 1)
                .Append(item)
                .ToList();

            var inventory = this.inventoryFactory.ShuffledItemsContainer(size, stackSize, inventoryItems.ToArray());
            var result = inventory.Get(item, 100);

            Assert.That(result, Has.Length.EqualTo(stackSize));
            Assert.That(result, Has.All.EqualTo(item));
        }
    }
}
