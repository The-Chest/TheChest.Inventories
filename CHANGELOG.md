# v0.12.2

## What's Fixed
* `StackInventory.Move` 
  * Improvements
    * Now it doesn't try to move items when the source and destination index are the same
    * The amount of items from the source slot needs to be equal the slot target amount and vice versa
  * Unit tests fixed to use the correct parameters

## Known issues
* [#110](https://github.com/The-Chest/TheChest.Inventories/issues/110) - Interface unit tests are in the same class as implementation unit tests
* [#129](https://github.com/The-Chest/TheChest.Inventories/issues/129) - No Add/Replace/Move validations on Containers
* Unit tests are getting complex and need a refactor
* Index in Inventory/Container properties might be removed or become obsolete
* `StackInventory<T>.Move` method is too complex and needs a refactor
* Event system will need an improvement on creation/dispatch
  * The new Event API is being planned

## What's next
* [#136](https://github.com/The-Chest/TheChest.Inventories/issues/136) - Improve library's packing
* [#144](https://github.com/The-Chest/TheChest.Inventories/issues/144) - Update to `TheChest.Core v0.14.0`

**Full Changelog**: https://github.com/The-Chest/TheChest.Inventories/compare/v0.12.1...v0.12.2

# v0.12.1

## What's Fixed
* `StackInventory.Replace(T[] items, int index)` now throws 
  * `ArgumentNullException` when `items` is null
  * `ArgumentException` when `items` length is zero
  * `ArgumentOutOfRangeException` when `index` is bigger than the Inventory size or smaller than zero

## Known issues
* [#110](https://github.com/The-Chest/TheChest.Inventories/issues/110) - Interface unit tests are in the same class as implementation unit tests
* [#129](https://github.com/The-Chest/TheChest.Inventories/issues/129) - No Add/Replace/Move validations
* Index in Inventory/Container properties might be removed or become obsolete
* Event system may need an improvement on creation/dispatch
  * No changes to the Event API is being planned yet

**Full Changelog**: https://github.com/The-Chest/TheChest.Inventories/compare/v0.12.0...v0.12.1

# v0.12.0

## What's Changed
* Project now using `TheChest.Core v0.12.1`
* `InventoryLazyStackSlot` now implements `ILazyStackSlot`
* `LazyStackInventory` now implements `ILazyStackContainer`

## Known issues
* [#110](https://github.com/The-Chest/TheChest.Inventories/issues/110) - Interface unit tests are in the same class as implementation unit tests
* [#129](https://github.com/The-Chest/TheChest.Inventories/issues/129) - No Add/Replace/Move validations
* Index in Inventory properties might be removed or become obsolete
* Event system may need an improvement on creation/dispatch
  * No changes to the Event API is being planned

**Full Changelog**: https://github.com/The-Chest/TheChest.Inventories/compare/v0.11.1...v0.12.0

# v0.11.1

## What's Changed
* `AddAt` methods are now `virtual`
  * `Inventory`
  * `StackInventory`
  * `LazyStackInventory`

## Known issues
* [#110](https://github.com/The-Chest/TheChest.Inventories/issues/110) - Interface unit tests are in the same class as implementation unit tests
* [#129](https://github.com/The-Chest/TheChest.Inventories/issues/129) - No Add/Replace/Move validations
* Index in Inventory properties might be removed or become obsolete
* Event system may need an improvement on creation/dispatch
  * No changes to the Event API is being planned

**Full Changelog**: https://github.com/The-Chest/TheChest.Inventories/compare/v0.11.0...v0.11.1

# v0.11.0

## What's Added
* Replace method to every Inventory Type class
   * `Inventory` : `Replace(T item, int index): T?` 
     * Throws `ArgumentNullException` when the param `item` is null
     * Throws `ArgumentOutOfRangeException` when the param `index` is invalid
     * Replaces an item in a specific slot
     * Dispatch `OnReplace` event if successful
  * `StackInventory` : `Replace(T[] items, int index): T[]` 
     * Does nothing the param `items` is empty
     * Throws `ArgumentOutOfRangeException` when the param `index` is invalid
     * Replaces one or more items in a specific slot
     * Dispatch `OnReplace` event if successful
  * `LazyStackInventory` : `Replace(T item, int index, int amount): T[]` 
     * Throws `ArgumentNullException` when the param `item` is null
     * Throws `ArgumentOutOfRangeException` when the param `index` is invalid
     * Throws `ArgumentOutOfRangeException` when the param `amount` is invalid
     * Replaces one or more items in a specific slot
     * Dispatch `OnReplace` event if successful

## What's Removed
* Obsolete method `AddAt` with "replace" param from every Slot type class

## Known issues
* [#37](https://github.com/The-Chest/TheChest.Inventories/issues/37) - `InventoryLazyStackSlot.Replace` needs to receive an array of item instead of an item and the amount number
* [#110](https://github.com/The-Chest/TheChest.Inventories/issues/110) - Interface unit tests are in the same class as implementation unit tests
* [#129](https://github.com/The-Chest/TheChest.Inventories/issues/129) - No Add/Replace/Move validations
* Index in Inventory properties might be removed or become obsolete
* Event system may need an improvement on creation/dispatch
  * No changes to the Event API is being planned

**Full Changelog**: https://github.com/The-Chest/TheChest.Inventories/compare/v0.10.1...v0.11.0

# v0.10.0

## What's Added
* New `protected` methods to get from the `content` field while decreasing the field `amount` value.
  * `GetItem<T>()`
  * `GetItems<T[]>()`
* The protected methods `AddItem` and `AddItems` now increases the value of the field `amount`

## What's Changed
* The project is now using `TheChest.Core v0.10.0` so it uses its new resources like
  * `StackAmount` is now `Amount`
  * `MaxStackAmount` is not `MaxAmount`
  * `protected` fields added for the properties `Amount` and `MaxAmount`
  * More info on : https://github.com/The-Chest/TheChest.Core/releases/tag/v0.10.0

## Known issues
* [#38](https://github.com/The-Chest/TheChest.Inventories/issues/38) - Inventories doesn't have `Replace` methods
* [#37](https://github.com/The-Chest/TheChest.Inventories/issues/37) - `InventoryLazyStackSlot.Replace` needs to receive an array of item instead of an item and the amount number
* [#110](https://github.com/The-Chest/TheChest.Inventories/issues/110) - Interface unit tests are in the same class as implementation unit tests
* Index in Inventory properties might be removed or become obsolete
* The project technical debt is increasing... (#87, #88)
* `InventoryStackSlot` might have a BBC (big breaking changes) in the future (not a issue, more of a disclaimer)

**Full Changelog**: https://github.com/The-Chest/TheChest.Inventories/compare/v0.9.0...v0.10.0

# v0.9.0

## What's Added
* `ISlotsExtensions` to get the protected field `content` using 
  * `GetContent<T>()` - if it is stored as T
  * `GetContents<T>()` - if it is stored as an array of T
* `IInventoryExtensions` to get the protected field `content` using 
  * `GetSlots<T>()` - for each Inventory Type

## What's Changed
* The project is now using `TheChest.Core`[v0.9.1](https://github.com/The-Chest/TheChest.Core/releases/tag/v0.9.1)

## What's Fixed
* Fixes in summary docs when had `<inheritdocs />` but with a different return description

## What's Removed
* Property `Content` override removed from
  * `InventorySlot<T>`
  * `InventoryStackSlot<T>`
  * `InventoryLazyStackSlot<T>`
* Property `Slots` removed from 
  * `Inventory`
  * `StackInventory`
  * `LazyStackInventory`

## Known issues
* [#38](https://github.com/The-Chest/TheChest.Inventories/issues/38) - Inventories doesn't have `Replace` methods
* [#37](https://github.com/The-Chest/TheChest.Inventories/issues/37) - `InventoryLazyStackSlot.Replace` needs to receive an array of item instead of an item and the amount number
* [#110](https://github.com/The-Chest/TheChest.Inventories/issues/110) - Interface unit tests are in the same class as implementation unit tests
* Index in Inventory properties might be removed or become obsolete
* The project technical debt is increasing... ([#87](https://github.com/The-Chest/TheChest.Inventories/issues/87), [#88](https://github.com/The-Chest/TheChest.Inventories/issues/88))

**Full Changelog**: https://github.com/The-Chest/TheChest.Inventories/compare/v0.8.1...v0.9.0

# v0.8.1

## What's Fixed
* Remove `IInventoryStackSlot` methods using ref parameters 
  * `Add(ref T item);`
  * `Add(ref T[] items);`
  * `Replace(ref T[] items);`
  * `Replace(ref T item);`
* Fixes `StackInventoryAddEventArgs` and `StackInventoryGetEventArgs` constructor instantiation

## Known issues
* [#38](https://github.com/The-Chest/TheChest.Inventories/issues/38) - Inventories doesn't have `Replace` methods
* [#37](https://github.com/The-Chest/TheChest.Inventories/issues/37) - `InventoryLazyStackSlot.Replace` needs to receive an array of item instead of an item and the amount number
* Index in Inventory properties might be removed or become obsolete

**Full Changelog**: https://github.com/The-Chest/TheChest.Inventories/compare/v0.8.0...v0.8.1

# v0.8.0

## What's Changed
* The project framework target is now `.net standard 2.1` meaning that it increased its compatibility over .net versions at the cost of not being compatible with any other version of `TheChest.Core` over `v0.8.0`
* `T` is not nullable on the interface's contract 

## Known issues
* [#42](https://github.com/The-Chest/TheChest.Inventories/issues/42) - `StackInventory.Add(T[] items)` does not support adding distinct items
* [#38](https://github.com/The-Chest/TheChest.Inventories/issues/38) - Inventories doesn't have `Replace` methods
* [#37](https://github.com/The-Chest/TheChest.Inventories/issues/37) - `InventoryLazyStackSlot.Replace` needs to receive an array of item instead of an item and the amount number
* Index in Inventory properties might be removed or become obsolete

**Full Changelog**: https://github.com/The-Chest/TheChest.Inventories/compare/v0.7.1...v0.8.0

# v0.7.1

## What's Added
* Now the project has summary docs when used as a NuGet package

## What's Changed
* `AddAt` Methods using the `replace` parameter are now `Obsolete` and requires the `replace` parameter value to be set, otherwise it'll use the newer version that doesn't have the replace feature. Methods affected:
  * `IInventory<T>.AddAt(T item, int index, bool replace)`
  * `IStackInventory<T>.AddAt(T item, int index, bool replace)`
  * `IStackInventory<T>.AddAt(T[] items, int index, bool replace)`
  * It also applies to each implementation of its interfaces

## What's fixed
* `IStackInventory<T>.AddAt(T[] items, int index)` does not throw `ArgumentException` when items are empty 

## Known issues
* [#42](https://github.com/The-Chest/TheChest.Inventories/issues/42) - `StackInventory.Add(T[] items)` does not support adding distinct items
* [#38](https://github.com/The-Chest/TheChest.Inventories/issues/38) - Inventories doesn't have `Replace` methods
* [#37](https://github.com/The-Chest/TheChest.Inventories/issues/37) - `InventoryLazyStackSlot.Replace` needs to receive an array of item instead of an item and the amount number


**Full Changelog**: https://github.com/The-Chest/TheChest.Inventories/compare/v0.7.0...v0.7.1

# v0.7.0

## What's Added
* Events published in the classes `Inventory<T>`, `StackInventory<T>` and `LazyStackInventory<T>`:
  * `OnGet` : When an item or items are returned from an index
  * `OnAdd` : When an item or items are added to an index
  * `OnReplace` : When an item or items is moved from an index to other

## What's Changed
* Documentation improvements
  * Now the folder are organized in
    * `Inventory<T>`, `StackInventory<T>` and `LazyStackInventory<T>`
      * `class_diagram.md` - The class diagrams of the class type
      * `event.md` - The events that the class type fires
      * `extending.md` - Examples how to extend the built-in class
      * `implementing` - Examples how to implement the interface type
  * Readme.md how have reference for the new docs added
* `LazyStackInventory.AddAtAddAt(T item, int index, int amount, bool replace)` is now Obsolete 
* `InventoryStackSlot` methods with ref now have Obsolete in it and a version without ref
  * `Add(ref T item)`
  * `Add(ref T[] items)`
  * `Replace(ref T[] items)`
  * `Replace(ref T item)`

## What's Fixed
* Flaky Tests in `StackInventoryTests.AddItem`
* Flaky Tests in `InventoryTests.AddItems`

## Known issues
* [#89](https://github.com/The-Chest/TheChest.Inventories/issues/89) - `AddAt` method does not fire `OnReplace` event
* [#42](https://github.com/The-Chest/TheChest.Inventories/issues/42) - `StackInventory.Add(T[] items)` does not support adding distinct items
* [#38](https://github.com/The-Chest/TheChest.Inventories/issues/38) - Inventories doesn't have `Replace` methods
* [#37](https://github.com/The-Chest/TheChest.Inventories/issues/37) - `InventoryLazyStackSlot.Replace` needs to receive an array of item instead of an item and the amount number
* [#74](https://github.com/The-Chest/TheChest.Inventories/issues/74) - `StackInventory<T>.AddAt(T[] items)` throws `ArgumentException` instead of returning an empty array

**Full Changelog**: https://github.com/The-Chest/TheChest.Inventories/compare/v0.6.0...v0.7.0

# v0.6.0

## What's Changed
* The project is now using `TheChest.Core` [v0.7.0](https://www.nuget.org/packages/TheChest.Core/0.7.0) dependency
* Now, `Inventory<T>.Slots` is a `ReadOnlyArray`, avoid using indexing on it, use `this[int index]` 
* Now, `StackSlots<T>.Content` is a `ReadOnlyArray`, avoid using indexing on it, use `this[int index]`  

**Full Changelog**: https://github.com/The-Chest/TheChest.Inventories/compare/v0.5.0...v0.6.0

# v0.5.0

## What's Changed
* The `Contains` method now comes from `TheChest.Core`
* The project is now using the version [0.5.0](https://github.com/The-Chest/TheChest.Core/releases/tag/v0.5.0) from `TheChest.Core`

## What's Removed
* `Contains` method removed from
  * `InventoryLazyStackSlot<T>`
  * `InventorySlot<T>`
  * `InventoryStackSlot<T>` 

## What's Fixed
* Flaky test in `InventoryLazyStackSlot.Add` fixed

## Known Issues
* [#42](https://github.com/The-Chest/TheChest.Inventories/issues/42) - `StackInventory.Add(T[] items)` does not support adding distinct items
* [#62](https://github.com/The-Chest/TheChest.Inventories/issues/62) - Flaky test in `InventoryTests.Add`
* The project does not support the version [0.6.0](https://github.com/The-Chest/TheChest.Core/releases/tag/v0.6.0) of `TheChest.Core`
* The project does not have a support chart

**Full Changelog**: https://github.com/The-Chest/TheChest.Inventories/compare/v0.4.5...v0.5.0

# v0.4.5

## What's Changed
* `Inventory<T>` methods using the parameter `index` now throws `ArgumentOutOfRangeException` instead of `IndexOutOfRangeException`
  * `AddAt(T item, int index, bool replace = true)`
  * `Get(int index)` 
* `StackInventory<T>` methods using the parameter `index` now throws `ArgumentOutOfRangeException` instead of `IndexOutOfRangeException`
  * `AddAt(T item, int index, bool replace = true)`
  * `AddAt(T[] items, int index, bool replace = true)`
  * `GetAll(int index)`
  * `Get(int index)`
  * `Get(int index, int amount)`

## Known Issues
* [#42](https://github.com/The-Chest/TheChest.Inventories/issues/42) - `StackInventory.Add(T[] items)` does not support adding distinct items
* [#52](https://github.com/The-Chest/TheChest.Inventories/issues/52) - Flaky test in `InventoryLazyStackSlot.Add`
* `LazyStackInventory` and `InventoryLazyStackSlot` is using Stack Behavior (Will be fixed in the Core project first: [#58](https://github.com/The-Chest/TheChest.Core/issues/58))

**Full Changelog**: https://github.com/The-Chest/TheChest.Inventories/compare/v0.4.4...v0.4.5

# v0.4.4

## What's Changed
* `Inventory.Add(T item)` now checks if the param `item` is null
* `Inventory.AddAt(T item, int index, bool replace = true)` now checks if the param `item` is null
* `Inventory.Add(params T[] items)` now adds only not null items to its slots

## Known Issues
* [#42](https://github.com/The-Chest/TheChest.Inventories/issues/42) - `StackInventory.Add(T[] items)` does not support adding distinct items
* [#52](https://github.com/The-Chest/TheChest.Inventories/issues/52) - Flaky test in `InventoryLazyStackSlot.Add`
* `LazyStackInventory` and `InventoryLazyStackSlot` is using Stack Behavior (Will be fixed in the Core project first: [#58](https://github.com/The-Chest/TheChest.Core/issues/58))

**Full Changelog**: https://github.com/The-Chest/TheChest.Inventories/compare/v0.4.3...v0.4.4

# v0.4.3

## What's Changed
* The methods `Get(T item)` , `Get(T item, int amount)`, `GetAll(T item)` and `GetCount(T item)`  from the class `Inventory<T>` now throws `ArgumentNullException` when the parameter item is null

## Known Issues
* [#42](https://github.com/The-Chest/TheChest.Inventories/issues/42) - `StackInventory.Add(T[] items)` does not support adding distinct items
* [#52](https://github.com/The-Chest/TheChest.Inventories/issues/52) - Flaky test in `InventoryLazyStackSlot.Add`
* [#54](https://github.com/The-Chest/TheChest.Inventories/issues/54) - No validation to `item` param in `Inventory.Add` methods
* `LazyStackInventory` and `InventoryLazyStackSlot` is using Stack Behavior (Will be fixed in the Core project first: [#58](https://github.com/The-Chest/TheChest.Core/issues/58))

**Full Changelog**: https://github.com/The-Chest/TheChest.Inventories/compare/v0.4.2...v0.4.3

# v0.4.2

## What's Changed
* `ArgumentException` removed from `Inventory.Add(params T[] items)`
* `ArgumentException` removed from `StackInventory.Add(params T[] items)`

## Known Issues
* [#11](https://github.com/The-Chest/TheChest.Inventories/issues/11) - No validation to item param in `Inventory.Get` methods
* [#42](https://github.com/The-Chest/TheChest.Inventories/issues/42) - `StackInventory.Add(T[] items)` does not support adding distinct items
* [#52](https://github.com/The-Chest/TheChest.Inventories/issues/52) - Flaky test in `InventoryLazyStackSlot.Add`
* `LazyStackInventory` and `InventoryLazyStackSlot` is using Stack Behavior (Will be fixed in the Core project first: [#58](https://github.com/The-Chest/TheChest.Core/issues/58))

**Full Changelog**: https://github.com/The-Chest/TheChest.Inventories/compare/v0.4.1...v0.4.2

# v0.4.1

## What's Changed
* `StackInventory.Add` now prioritizes adding to slots that already contains the same item type as the item to be added

## What's Fixed
* `StackInventory.Replace` now returns an Empty array instead of null

## Known Issues
* [#10](https://github.com/The-Chest/TheChest.Inventories/issues/10) - `InventorySlot.Add(T[] items)` throwing `ArgumentException` when items param is empty
* [#11](https://github.com/The-Chest/TheChest.Inventories/issues/11) - No validation to item param in `Inventory.Get` methods
* [#42](https://github.com/The-Chest/TheChest.Inventories/issues/42) - `StackInventory.Add(T[] items)` does not support adding distinct items
* [#52](https://github.com/The-Chest/TheChest.Inventories/issues/52) - Flaky test in `InventoryLazyStackSlot.Add`
* `LazyStackInventory` and `InventoryLazyStackSlot` is using Stack Behavior (Will be fixed in the Core project first: [#58](https://github.com/The-Chest/TheChest.Core/issues/58))

**Full Changelog**: https://github.com/The-Chest/TheChest.Inventories/compare/v0.4.0...v0.4.1

# v0.4.0

## What's Added
* Abstract classes
  * `LazyStackInventory<T>` - Generic Inventory with `ILazyStackInventory<T>` implementation
  * `InventoryLazyStackSlot<T>` - Generic Slot Inventory with `IInventoryLazyStackSlot<T>` implementation
* Interfaces 
  * `InventoryLazyStackSlot<T>` - Interface with methods for a basic Inventory Stackable Slot with only one copy of the item
  * `ILazyStackInventory<T>` - Interface with methods for interaction with a Stack Container

## Known Issues
* [#10](https://github.com/The-Chest/TheChest.Inventories/issues/10) - `InventorySlot.Add(T[] items)` throwing `ArgumentException` when items param is empty
* [#11](https://github.com/The-Chest/TheChest.Inventories/issues/11) - No validation to item param in `Inventory.Get` methods
* [#12](https://github.com/The-Chest/TheChest.Inventories/issues/12) - `StackInventory.Replace` is returning default when slot is empty 
* [#28](https://github.com/The-Chest/TheChest.Inventories/issues/28) - `StackInventory.Add` is adding item to the first available/empty slot instead of searching for slots with items to stack
* Lazy Stack Inventory and InventoryLazyStackSlot is using Stack Behavior

**Full Changelog**: https://github.com/The-Chest/TheChest.Inventories/compare/v0.3.0...v0.4.0

# v0.3.0

## What's Added
* Basic documentation to the project 

## What's Changed
* `Base` name removed from Base classes
* Base classes are not abstract anymore
* `TheChest.Core.Inventory` namespaces are now `TheChest.Inventory`

## What's Removed
* `TheChest.Core` code from the solution

## Known Issue
* [#10](https://github.com/The-Chest/TheChest.Inventories/issues/10) - `InventorySlot.Add(T[] items)` throwing `ArgumentException` when items param is empty
* [#11](https://github.com/The-Chest/TheChest.Inventories/issues/11) - No validation to item param in `Inventory.Get` methods
* [#12](https://github.com/The-Chest/TheChest.Inventories/issues/12) - `StackInventory.Replace` is returning default when slot is empty 
* [#28](https://github.com/The-Chest/TheChest.Inventories/issues/28) - `StackInventory.Add` is adding item to the first available/empty slot instead of searching for slots with items to stack

**Full Changelog**: https://github.com/The-Chest/TheChest.Inventories/compare/v0.2.0...v0.3.0

# v0.2.0

## What's Added
* Summary documentation to Inventories and Slots
* Unit test project
 
## What's Changed
* `Slot<T>` is now called `InventorySlot<T>`
* `InventorySlots` methods are now virtual

## Known Issues
* `StackInventorySlot<T>` has no methods 
* `IStackInventory<T>` interface has no implementation and all its methods are not guaranteed to have the correct contract

**Full Changelog**: https://github.com/The-Chest/TheChest.Inventories/compare/v0.1.0...v0.2.0

# v0.1.0

## What's Added
* Base project architecture migrated from [TheChest.Core](https://github.com/The-Chest/TheChest.Core)
  * Base classes
    * `BaseInventory<T>` - Generic Inventory with `IInventory<T>` implementation
    * `BaseStackInventory<T>` - Generic Inventory with `IStackInventory<T>` implementation
    * `BaseInventorySlot<T>` - Generic Slot Inventory with `IInventorySlot<T>` implementation
    * `BaseInventoryStackSlot<T>` - Generic Slot Inventory with `IStackInventorySlot<T>` implementation
  * Interfaces 
    * `IInteractiveContainer<T>` - Interface with methods for interaction with the Container
    * `IInventory<T>` - Interface with methods for interaction with the Inventory with `IInventorySlot<T>` slots
    * `IStackInventory<T>` - Interface with methods for interaction with the Inventory using `IInventoryStackSlot<T>` slots
    * `IInventorySlot<T>` - Interface with methods for a basic Inventory Slot
    * `IInventoryStackSlot<T>` - Interface with methods for a basic Inventory Stackable Slot 

## Known Issues
* No documentation yet
* InventorySlots interfaces and abstract classes are in the wrong namespaces/folder
* No unit tests yet

**Full Changelog**: https://github.com/The-Chest/TheChest.Inventories/commits/v0.1.0