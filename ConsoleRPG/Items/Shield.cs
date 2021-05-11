namespace ConsoleRPG.Items
{
    public class Shield : Item
    {
        private int BonusDefense { get; set; }

        public Shield(string name, int cost, int bonusDefense) : base(name, cost)
        {
            BonusDefense = bonusDefense;
        }

        public int ReturnBonusDefense()
        {
            return BonusDefense;
        }
    }
}