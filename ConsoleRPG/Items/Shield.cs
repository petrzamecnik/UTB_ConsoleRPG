namespace ConsoleRPG.Items
{
    public class Shield : Item
    {
        private static int BonusDefense { get; set; }

        public Shield(string name, int cost, int bonusDefense) : base(name, cost)
        {
            BonusDefense = bonusDefense;
        }

        public static int ReturnBonusDefense()
        {
            return BonusDefense;
        }
    }
}