


namespace ConsoleRPG.Items
{
    public class Item
    {
        public string Name { get; set; }
        public int MaximumQuantity { get; set; }

        public Item(string name, int maximumQuantity)
        {
            Name = name;
            MaximumQuantity = maximumQuantity;
        }
    }

    public class Weapon : Item
    {
        public Weapon(string name, int maximumQuantity) : base(name, maximumQuantity)
        {
        }
    }

    public class Armor : Item
    {
        public Armor(string name, int maximumQuantity) : base(name, maximumQuantity)
        {
        }
    }

    public class Potion : Item
    {
        public Potion(string name, int maximumQuantity) : base(name, maximumQuantity)
        {
            MaximumQuantity = 10;
        }
    }
}