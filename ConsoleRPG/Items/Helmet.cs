namespace ConsoleRPG.Items
{
    public class Helmet : Item
    {
        private static int BonusDefense { get; set; }

        public Helmet(string name, int cost, int bonusDefense) : base(name, cost)
        {
            BonusDefense = bonusDefense;
        }
        
        public static int ReturnBonusDefense()
        {
            return BonusDefense;
        }
    }
}