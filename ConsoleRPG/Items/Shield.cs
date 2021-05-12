using System;

namespace ConsoleRPG.Items
{
    [Serializable]
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