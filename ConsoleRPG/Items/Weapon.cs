using System;

namespace ConsoleRPG.Items
{
    [Serializable]
    public class Weapon : Item
    {
        private int BonusAttack { get; set; }

        public Weapon(string name, int cost, int bonusAttack) : base(name, cost)
        {
            BonusAttack = bonusAttack;
        }

        public int ReturnBonusAttack()
        {
            return BonusAttack;
        }
    }
}