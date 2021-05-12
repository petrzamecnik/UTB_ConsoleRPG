using System;

namespace ConsoleRPG.Items
{
    [Serializable]
    public class Helmet : Item
    {
        private int BonusDefense { get; set; }

        public Helmet(string name, int cost, int bonusDefense) : base(name, cost)
        {
            BonusDefense = bonusDefense;
        }
        
        public int ReturnBonusDefense()
        {
            return BonusDefense;
        }
    }
}