using System;
using ConsoleRPG.Items;
using static System.Environment;

namespace ConsoleRPG.Characters
{
    public class Player: Character
    {
        private Inventory inventory = new Inventory();
        
        
        private Random rnd = new Random();
        private int Mana { get; set; }
        private int MaxMana { get; set; }
        private int Experience { get; set; }
        private int ExperienceToNextLevel { get; set; }

        public Player(string name, int level, int health, int maxHealth, int attack, int defence, bool isPlayer, int stunnedForXTurns, int mana, int maxMana, int experience, int experienceToNextLevel) : base(name, level, health, maxHealth, attack, defence, isPlayer, stunnedForXTurns)
        {
            Mana = mana;
            MaxMana = maxMana;
            Experience = experience;
            ExperienceToNextLevel = experienceToNextLevel;
        }

        public void AttackAction(Character target)
        {
            //Console.WriteLine(new string('*', 60));
            Console.WriteLine("1 - Quick attack [always hit, normal damage]");
            Console.WriteLine("2 - Heavy blow   [chance to miss, higher damage]");
            Console.Write("Choose type of attack --> ");
            var actionChoice = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            //Console.WriteLine(new string('*', 60));

            switch (actionChoice)
            {
                case 1: 
                    BasicAttack(target);
                    break;
                
                case 2:
                    HeavyBlow(target, rnd);
                    break;
            }

        }

        public void CastSpellAction(Player player, Character target)
        {
            //Console.WriteLine(new string('*', 60));
            Console.WriteLine("1 - Heal     [heal yourself (30 mana)]");
            Console.WriteLine("2 - Fireball [chance to miss, high damange (50 mana)]");
            Console.WriteLine("3 - Freeze   [chance to freeze enemy for 2-3 turns [(80 mana)]");
            Console.Write("Choose type of spell --> ");
            var actionChoice = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            //Console.WriteLine(new string('*', 60));

            switch (actionChoice)
            {
                case 1:
                    Heal(player);
                    break;
                
                case 2:
                    Fireball(player, target, rnd);
                    break;
                
                case 3:
                    Freeze(player, target, rnd);
                    break;
            }
        }

        public void UsePotionAction()
        {
            throw new NotImplementedException();
        }

        public void Run()
        {
            throw new NotImplementedException();
        }

        /* Attacks here */
        public override void BasicAttack(Character target)
        {
            target.Health -= Attack;
            Console.WriteLine($"{Name} attacked for {Attack} damage");
        }
        
        /* Spells here */
        private void Heal(Player player)
        {
            const int manaCost = 30;
            if (player.Mana > manaCost)
            {
                player.Mana -= manaCost;
                var healAmount = Convert.ToInt32(Math.Round(MaxHealth * 0.2));

                Health += healAmount;
                Console.WriteLine($"{player.ReturnCharacterName()} has healed himself for {healAmount} health");
            }
            
            if (Health > MaxHealth)
                Health = MaxHealth;
        }

        public override void Fireball(Player player, Character target, Random rnd)
        {
            const int manaCost = 50;
            if (player.Mana > manaCost)
            {
                player.Mana -= manaCost;
                if (rnd.Next(0, 100) > 10)
                {
                    var damage = Convert.ToInt32(Math.Round(player.Attack * 1.8));
                    target.Health -= damage;
                    Console.WriteLine($"{player.ReturnCharacterName()} has used Fireball and hit enemy for {damage} damage");
                }
                else
                {
                    Console.WriteLine($"{target.ReturnCharacterName()} has dodged the attack!");
                    
                    
                }
            }
        }

        public override void Freeze(Player player, Character target, Random rnd)
        {
            const int manaCost = 80;
            if (player.Mana > manaCost)
            {
                player.Mana -= manaCost;
                if (rnd.Next(0, 100) > 30)
                {
                    target.StunnedForXTurns += rnd.Next(2, 3);
                }
            }
        }

        public void GainExperience(int amount)
        {
            Experience += amount;
        }

     
        public void TryToLevelUp()
        {
            if (Experience > ExperienceToNextLevel)
            {
                var nextLevelExperience = Experience - ExperienceToNextLevel;
                if (nextLevelExperience > 0)
                {
                    Experience = 0;
                    Experience += nextLevelExperience;
                    Level += 1;
                    ExperienceToNextLevel = Convert.ToInt32(Math.Floor(ExperienceToNextLevel * 1.2));

                    OnLevelUp();
                    
                }
            }
        }

        private void OnLevelUp()
        {
            MaxHealth += 30;
            MaxMana += 20;
            Attack += 10;
            Defence += 5;

            Mana = MaxMana;
            
            Health += 20;
            if (Health > MaxHealth)
            {
                Health = MaxHealth;
            }
            
            
        }

        public int ReturnPlayerMana()
        {
            return Mana;
        }


        public int ReturnPlayerMaxMana()
        {
            return MaxMana;
        }

        public int ReturnMaxExperience()
        {
            return ExperienceToNextLevel;
        }

        public int ReturnExperience()
        {
            return Experience;
        }
    }
}