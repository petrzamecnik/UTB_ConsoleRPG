using System;
using System.Collections.Generic;
using ConsoleRPG.Items;

namespace ConsoleRPG.Characters
{
    
    [Serializable]
    public abstract class Character : IComparable<Character>
    {
        public Inventory Inventory { get; set; }
        
        
        protected string Name { get; set; }
        protected int Level { get; set; }
        protected internal int Health { get; set; }
        protected int MaxHealth { get; set; }
        protected int Attack { get; set; }
        protected int Defense { get; set; }
        protected bool IsPlayer { get; set; }
        
        public int StunnedForXTurns { get; set; }

        protected Rand Rand = Rand.Instant;

        protected Character(string name, int level, int health, int maxHealth, int attack, int defense, bool isPlayer,
            int stunnedForXTurns, Inventory inventory)
        {
            Name = name;
            Level = level;
            Health = health;
            MaxHealth = maxHealth;
            Attack = attack;
            Defense = defense;
            IsPlayer = isPlayer;
            StunnedForXTurns = stunnedForXTurns;
            Inventory = inventory;
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

        public abstract void BasicAttack(Character target);

        protected virtual void HeavyBlow(Character target, Rand rand)
        {
            var hitChance = rand.Next(0, 100);
            if (hitChance <= 20)
            {
                Console.WriteLine($"{target.ReturnCharacterName()} has dodged the attack!");
            }
            else
            {
                var damage = (Attack * 140) / 100;
                target.Health -= damage;
                Console.WriteLine($"{Name} attacked for {damage} damage.");
            }
        }

        public abstract void Fireball(Player player, Character target, Random rnd);
        public abstract void Freeze(Player player, Character target, Random rnd);
        
        
        public int ReturnCharacterMaxHealth()
        {
            return MaxHealth;
        }

        public int ReturnCharacterAttack()
        {
            return Attack;
        }

        public int ReturnCharacterDefense()
        {
            return Defense;
        }

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
        
        public int CompareTo(Character other)
        {
            return other == null ? 1 : CalculatePower().CompareTo(other.CalculatePower());
        }

        public double CalculatePower()
        {
            return ((Attack * 0.4) + (MaxHealth * 0.2) + (Defense * 0.4));
        }

    }
}