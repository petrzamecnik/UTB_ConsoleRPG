using System;

namespace ConsoleRPG.Items
{
    
    [Serializable]
    public class Potion : Item
    {
        private int HealthRegenAmount { get; set; }
        private int ManaRegenAmount { get; set; }

        public Potion(string name, int cost, int healthRegenAmount, int manaRegenAmount) : base(name, cost)
        {
            HealthRegenAmount = healthRegenAmount;
            ManaRegenAmount = manaRegenAmount;
        }

        public int ReturnHealthRegenAmount()
        {
            return HealthRegenAmount;
        }
        public int ReturnManaRegenAmount()
        {
            return ManaRegenAmount;
        }
    }
}