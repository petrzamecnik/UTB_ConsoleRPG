


namespace ConsoleRPG.Items
{
    public class Item
    {
        protected string Name { get; set; }
        private int Cost { get; set; }


        public Item(string name, int cost)
        {
            Name = name;
            Cost = cost;
        }

        public string ReturnItemName()
        {
            return Name;
        }
    }
}