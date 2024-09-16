﻿using TheChest.ConsoleApp.Inventories;
using TheChest.ConsoleApp.Items;

var slots = new Slot[10];
for (int i = 0; i < slots.Length; i++)
{
   slots[i] = new Slot();
}
var inventory = new Inventory(slots);
inventory.AddItem(new Item("123","Item", "..."));
inventory.AddItem(new Item("123","Item", "..."));

var items = inventory.GetAll(new Item("123", "Item", "..."));
Console.WriteLine(items);
