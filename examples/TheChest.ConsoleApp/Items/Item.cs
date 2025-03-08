﻿namespace TheChest.ConsoleApp.Items
{
    public class Item
    {
        public string Id { get; }
        public string Name { get; }
        public string Description { get; }

        public Item(string id, string name, string description)
        {
            Id = id;
            Name = name;
            Description = description;
        }

        public Item()
        {
            Id = "";
            Name = "";
            Description = "";
        }
        public override bool Equals(object? obj)
        {
            if (obj == null) return false;
            if (obj is not Item) return false;
            var item = obj as Item;
            return item.Id == this.Id;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Name, Description);
        }
    }
}