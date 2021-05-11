namespace ConsoleRPG.Items
{
    public class Armor : Item
    {
        private static int BonusDefense { get; set; }

        public Armor(string name, int cost, int bonusDefense) : base(name, cost)
        {
            BonusDefense = bonusDefense;
        }
        
        public static int ReturnBonusDefense()
        {
            return BonusDefense;
        }
    }
}