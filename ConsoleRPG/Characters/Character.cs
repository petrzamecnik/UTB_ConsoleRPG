using System;

namespace ConsoleRPG.Characters
{
    public abstract class Character
    {
        protected string Name { get; set; }
        protected int Level { get; set; }
        protected internal int Health { get; set; }
        protected int MaxHealth { get; set; }
        protected int Attack { get; set; }
        protected int Defence { get; set; }
        protected bool IsPlayer { get; set; }
        
        public int StunnedForXTurns { get; set; }

        private Random rnd = new Random();

        protected Character(string name, int level, int health, int maxHealth, int attack, int defence, bool isPlayer, int stunnedForXTurns)
        {
            Name = name;
            Level = level;
            Health = health;
            MaxHealth = maxHealth;
            Attack = attack;
            Defence = defence;
            IsPlayer = isPlayer;
            StunnedForXTurns = stunnedForXTurns;
        }

        public int ReturnCharacterLevel()
        {
            return Level;
        }

        public bool IsStunned()
        {
            if (StunnedForXTurns > 0)
            {
                return true;
            }

            return false;
        }

        public void WriteCharacetrInfo()
        {
            Console.WriteLine(Name);
            Console.WriteLine(Health);
            Console.WriteLine(Attack);
            Console.WriteLine(Defence);
        }

        public abstract void BasicAttack(Character target);

        protected virtual void HeavyBlow(Character target, Random rnd)
        {
            var hitChance = rnd.Next(0, 100);
            if (hitChance <= 20)
            {
                Console.WriteLine($"{target.ReturnCharacterName()} has dodged the attack!");
            }
            else
            {
                target.Health -= (Attack * 140) / 100;
            }
        }

        public abstract void Fireball(Player player, Character target, Random rnd);
        public abstract void Freeze(Player player, Character target, Random rnd);
        
        



        public string ReturnCharacterName()
        {
            return Name;
        }

        public int ReturnCharacterHealth()
        {
            return Health;
        }

        public bool IsAlive()
        {
            return Health > 0;
        }
        
    }
}