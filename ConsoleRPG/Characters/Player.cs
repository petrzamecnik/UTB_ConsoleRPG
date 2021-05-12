using System;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using ConsoleRPG.Items;
using static System.Environment;
using static ConsoleRPG.BetterConsole;

namespace ConsoleRPG.Characters
{
    
    [Serializable]
    public class Player: Character 
    {

        //private Inventory Inventory = new Inventory();
        
        
        protected Rand Rand = Rand.Instant;
        private int Mana { get; set; }
        private int MaxMana { get; set; }
        private int Experience { get; set; }
        private int ExperienceToNextLevel { get; set; }
        
        private Weapon Weapon { get; set; }
        
        private Shield Shield { get; set; }
        
        private Armor Armor { get; set; }
        
        private Helmet Helmet { get; set; }
        
        private bool RunningAway { get; set; }

        public Player(string name, int level, int health, int maxHealth, int attack, int defense, bool isPlayer,
            int stunnedForXTurns, Inventory inventory, int mana, int maxMana, int experience, int experienceToNextLevel,
            Weapon weapon, Shield shield, Armor armor, Helmet helmet, bool runningAway)
            : base(name, level, health, maxHealth, attack, defense, isPlayer, stunnedForXTurns, inventory)
        {
            Mana = mana;
            MaxMana = maxMana;
            Experience = experience;
            ExperienceToNextLevel = experienceToNextLevel;
            Weapon = weapon;
            Shield = shield;
            Armor = armor;
            Helmet = helmet;
            RunningAway = runningAway;

            Attack += Weapon.ReturnBonusAttack();
            Defense += Shield.ReturnBonusDefense() + Helmet.ReturnBonusDefense() + Armor.ReturnBonusDefense();
        }

        public (Weapon, Shield, Helmet, Armor) ReturnEquippedItems()
        {
            return (Weapon, Shield, Helmet, Armor);
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
                    HeavyBlow(target, Rand);
                    break;
            }

        }

        public void CastSpellAction(Player player, Monster target)
        {
            castSpellAction:
            //Console.WriteLine(new string('*', 60));
            Console.WriteLine("0 - return");
            Console.WriteLine("1 - Heal     [heal yourself (30 mana)]");
            Console.WriteLine("2 - Fireball [chance to miss, high damange (50 mana)]");
            Console.WriteLine("3 - Freeze   [chance to freeze enemy for 2-3 turns [(80 mana)]");
            Console.Write("Choose type of spell --> ");
            //Console.WriteLine(new string('*', 60));
            
            var actionChoiceInput = Console.ReadLine();

            if (!int.TryParse(actionChoiceInput, out var actionChoice))
            {
                Col("*** Choice is not valid! ***", "red");

                goto castSpellAction;
            }

            if (actionChoice > 4 || actionChoice < 0 )
            {
                Col("*** Choice is not valid! ***", "red");
                goto castSpellAction;
            }

            switch (actionChoice)
            {
                case 0:
                    Program.PlayerBattleActions(player, target);
                    break;
                
                case 1:
                    Heal(player);
                    break;
                
                case 2:
                    Fireball(player, target, Rand);
                    break;
                
                case 3:
                    Freeze(player, target, Rand);
                    break;
            }
        }

        public void UsePotionAction(Player player)
        {
            choosePotion:
            var potions = player.Inventory.Items.OfType<Potion>().ToList();
            var potionIndex = 1;
            
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine();
            Center("Available potions:");
            foreach (var potion in potions)
            {
                Center($"{potionIndex} -> {potion.ReturnItemName()} - " +
                       $"Regenerate {potion.ReturnHealthRegenAmount()} health & {potion.ReturnManaRegenAmount()} mana.");
                potionIndex++;
            }
            
            var actionChoiceInput = Console.ReadLine();
            Console.Clear();
            if (!int.TryParse(actionChoiceInput, out var actionChoice))
            {
                
                Center("*** Choice is not valid! ***");
                goto choosePotion;
            }

            if (actionChoice >= potionIndex || actionChoice < 0 )
            {
                Center("*** Choice is not valid! ***");
                goto choosePotion;
            }

            if (actionChoice == 0)
            {
                return;
            }
            player.UsePotion(potions[actionChoice - 1]);
        }

        private void UsePotion(Potion potion)
        {
            Health += potion.ReturnHealthRegenAmount();
            Mana += potion.ReturnManaRegenAmount();
            Inventory.Items.Remove(potion);
            Col($"{Name} has used potion and regenerated: {potion.ReturnHealthRegenAmount()}" +
                              $" health & {potion.ReturnManaRegenAmount()} mana", "darkgreen");
        }

        public void Run(Player player)
        {
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine();
            CenterCol($"{player.ReturnCharacterName()} has run!", "red");
            CenterCol($"{player.ReturnCharacterName()} has been penalized by -{10}% maximum health", "red");
            player.ReduceMaximumHealth(10);
            player.RunningAway = true;
            System.Threading.Thread.Sleep(5000);
        }

        private void ReduceMaximumHealth(int amount)
        {
            MaxHealth -= amount;
        }

        /* Attacks here */
        public override void BasicAttack(Character target)
        {
            target.Health -= Attack;
            Col($"{Name} attacked for {Attack} damage", "yellow");
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
                Col($"{player.ReturnCharacterName()} has healed himself for {healAmount} health", "darkgreen");
            }
            else
            {
                Col($"{player.ReturnCharacterName()} had not enough mana and failed!", "red");
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
                    Col($"{player.ReturnCharacterName()} has used Fireball and hit enemy for {damage} damage", "blue");
                }
                
            }
            else
            {
                Col($"{player.ReturnCharacterName()} had not enough mana and failed!", "red");
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
                    var freezeTime = rnd.Next(2, 3);
                    target.StunnedForXTurns += freezeTime;
                    Col($"{player.ReturnCharacterName()} has succesfuly freezed the enemy for {freezeTime} rounds! ", "blue");
                }
                else
                {
                    Col($"{player.ReturnCharacterName()} has failed to hit the freeze spell!", "red");
                }
            }
            else
            {
                Col($"{player.ReturnCharacterName()} had not enough mana and failed!", "red");
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
                OnLevelUp();
                var bonusExperience = Math.Abs(ExperienceToNextLevel - Experience);
                Experience = bonusExperience;
            }

            if (Experience > ExperienceToNextLevel)
            {
                var nextLevelExperience = Math.Abs(ExperienceToNextLevel - Experience);
                if (nextLevelExperience > 0)
                {
                    Experience = 0;
                    Experience += nextLevelExperience;
                    OnLevelUp();
                }
            }
            
        }

        private void OnLevelUp()
        {
            MaxHealth += 30;
            MaxMana += 20;
            Attack += 10;
            Defense += 5;
            Level++;
            Mana = MaxMana;
            Health = MaxHealth;
            ExperienceToNextLevel = Convert.ToInt32(ExperienceToNextLevel * 1.4);
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

        public bool ReturnRunState()
        {
            return RunningAway;
        }

        public void SetRunningAwayState(bool b)
        {
            RunningAway = b;
        }

        public void EquipNewWeapon(Weapon newWeapon)
        {
            Attack -= Weapon.ReturnBonusAttack();
            Attack += newWeapon.ReturnBonusAttack();
            Weapon = newWeapon;
        }

        public void EquipNewShield(Shield newShield)
        {
            Defense -= Shield.ReturnBonusDefense();
            Defense += newShield.ReturnBonusDefense();
            Shield = newShield;
        }

        public void EquipNewHelmet(Helmet newHelmet)
        {
            Defense -= Helmet.ReturnBonusDefense();
            Defense += newHelmet.ReturnBonusDefense();
            Helmet = newHelmet;
        }

        public void EquipNewArmor(Armor newArmor)
        {
            Defense -= Armor.ReturnBonusDefense();
            Defense += newArmor.ReturnBonusDefense();
            Armor = newArmor;
        }
    }
}