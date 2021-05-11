using System;

namespace ConsoleRPG.Items
{
    public class Weapon : Item
    {
        private static int BonusAttack { get; set; }

        public Weapon(string name, int cost, int bonusAttack) : base(name, cost)
        {
            BonusAttack = bonusAttack;
        }

        public static int ReturnBonusAttack()
        {
            return BonusAttack;
        }
    }
}