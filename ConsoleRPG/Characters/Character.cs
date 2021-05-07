namespace ConsoleRPG.Characters
{
    public class Character
    {
        protected string Name { get; set; }
        protected int Level { get; set; }
        protected int Health { get; set; }
        protected int MaxHealth { get; set; }
        protected int Attack { get; set; }
        protected int Defence { get; set; }
        protected bool IsPlayer { get; set; }

        public Character(string name, int level, int health, int maxHealth, int attack, int defence, bool isPlayer)
        {
            Name = name;
            Level = level;
            Health = health;
            MaxHealth = maxHealth;
            Attack = attack;
            Defence = defence;
            IsPlayer = isPlayer;
        }
    }
}