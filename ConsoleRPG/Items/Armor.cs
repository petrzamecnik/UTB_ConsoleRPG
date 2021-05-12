using System;

namespace ConsoleRPG.Items
{
    [Serializable]
    public class Armor : Item
    {
        private int BonusDefense { get; set; }

        public Armor(string name, int cost, int bonusDefense) : base(name, cost)
        {
            BonusDefense = bonusDefense;
        }
        
        public int ReturnBonusDefense()
        {
            return BonusDefense;
        }
    }
}