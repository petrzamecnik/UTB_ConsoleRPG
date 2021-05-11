


namespace ConsoleRPG.Items
{
    public class Item
    {
        private static string Name { get; set; }
        private int Cost { get; set; }


        public Item(string name, int cost)
        {
            Name = name;
            Cost = cost;
        }

        public static string ReturnItemName()
        {
            return Name;
        }
    }
}