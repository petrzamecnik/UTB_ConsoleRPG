using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using ConsoleRPG.Characters;
using CONNECTDATA = System.Runtime.InteropServices.ComTypes.CONNECTDATA;

namespace ConsoleRPG
{
    internal class Program
    {
        // TODO: Implement inventory system for player
        // TODO: Implement more actions for player ( shop, crafting? )
        // TODO: Implement enemy item drop
        // TODO: Implement enemy battle logic
        
        
        public static void Main(string[] args)
        {
            var rand = new Random();
            
            Console.WriteLine("Welcome to Hero RPG!");
            Console.Write("Choose name of your character: \n");
            //var heroName = Console.ReadLine();
            var heroName = "Crowans";
            
            // Creation of player's character
            var player = new Player(heroName, 1, 100, 100, 10, 0, true,
                0, 100,100, 0, 100);

            var monsters = CreateMonsterList(player, rand);
            var enemy = monsters[rand.Next(0, monsters.Count - 1)];
            

            Battle(player, enemy);
            
        }

        private static void Battle(Player player, Monster actualEnemy)
        {
            for (var i = 0; player.IsAlive() && actualEnemy.IsAlive(); i++)
            {
                Console.WriteLine(new string('-', 20));
                Console.WriteLine($"Round: {i}");
                Console.WriteLine($"{player.ReturnCharacterName()}'s health: {player.ReturnCharacterHealth()}" +
                                  $"  ||  " +
                                  $"{player.ReturnCharacterName()}'s mana: {player.ReturnPlayerMana()}");
                Console.WriteLine($"{actualEnemy.ReturnCharacterName()}'s health: {actualEnemy.ReturnCharacterHealth()}");
                Console.WriteLine(new string('-', 20));

                if (player.IsAlive() && !player.IsStunned())
                {
                    
                    PlayerBattleActions(player, actualEnemy);
                }
                else if (player.IsAlive() && player.IsStunned())
                {
                    player.StunnedForXTurns -= 1;
                    Console.WriteLine("You are stunned!");
                    Console.WriteLine($"You will be stunned for {player.StunnedForXTurns} more rounds.");
                }
                

                if (actualEnemy.IsAlive() && !actualEnemy.IsStunned())
                {
                    actualEnemy.BasicAttack(player);
                }
                else if (actualEnemy.IsAlive() && actualEnemy.IsStunned())
                {
                    actualEnemy.StunnedForXTurns -= 1;
                    Console.WriteLine($"{actualEnemy.ReturnCharacterName()} is stunned!");
                    Console.WriteLine($"{actualEnemy.ReturnCharacterName()} will be stunned for {actualEnemy.StunnedForXTurns} more rounds.");
                }

                if (!player.IsAlive())
                {
                    Console.WriteLine(new string('*', 20));
                    Console.WriteLine("You have died! :( ");
                    Console.WriteLine(new string('*', 20));

                }

                if (!actualEnemy.IsAlive())
                {
                    Console.WriteLine("Monster has been slain!");
                    player.GainExperience(Convert.ToInt32(Math.Floor(actualEnemy.Health * 0.4)));
                    player.TryToLevelUp();
                    
                }
            }
        }

        private static void PlayerBattleActions(Player player, Monster actualEnemy)
        {
            Console.WriteLine("It's your turn! Choose your action.");
            Console.WriteLine("1 - Attack");
            Console.WriteLine("2 - Cast Spell");
            Console.WriteLine("3 - Use Potion");
            Console.WriteLine("4 - Run");
            var actionChoice = Convert.ToInt32(Console.ReadLine());
            Console.Clear();
            Console.Write($"You choosed: {actionChoice} \n");

            switch (actionChoice)
            {
                case 1: 
                    player.AttackAction(actualEnemy);
                    break;
                
                case 2: 
                    player.CastSpellAction(player, actualEnemy);
                    break;
                    
                case 3: 
                    player.UsePotionAction();
                    break;
                
                case 4: 
                    player.Run();
                    break;
            }
        }


        // Method to create new monster
        private static Monster CreateNewMonster(Player player, Random rand)
        {
            // get list of monster names
            var nameList = ReturnNameOfNewMonster();

            // choose name for monster
            var monsterName = nameList[rand.Next(1, nameList.Count)].Trim();
            var playerLevel = player.ReturnCharacterLevel();

            var monsterLevel = playerLevel + rand.Next(1, 3);
            var monsterHealth = 50 + (20 * playerLevel) * Convert.ToInt32(rand.Next(1,300 +1) / 100);
            var monsterMaxHealth = monsterHealth;
            var monsterAttack = 10 + (5 * playerLevel) * Convert.ToInt32(rand.Next(1, 300 + 1) / 100);
            var monsterDefence = 5 + (5 * playerLevel) * Convert.ToInt32(rand.Next(1,300 +1) / 100);

            var monster = new Monster(monsterName, monsterLevel, monsterHealth, monsterMaxHealth, monsterAttack,
                monsterDefence, false, 0);
            
            return monster;
        }

        // Method to make list of monster names 
        private static List<string> ReturnNameOfNewMonster()
        {
            var monsterNamesFile = "";
            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                monsterNamesFile = File.ReadAllText("/Users/petrzamecnik/RiderProjects/Ukoly/hrdina_a_drak/jmena_monster.txt");
            }
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            { 
                monsterNamesFile = File.ReadAllText("C:\\Users\\petrz\\RiderProjects\\UTB\\hrdina_a_drak\\jmena_monster.txt");
            }
            var names = monsterNamesFile.Split(',').ToList();
            
            return names;
        }

        // Method to create new list of monsters
        private static List<Monster> CreateMonsterList(Player player, Random rand)
        {
            var monster1 = CreateNewMonster(player, rand);
            var monster2 = CreateNewMonster(player, rand);
            var monster3 = CreateNewMonster(player, rand);
            var monster4 = CreateNewMonster(player, rand);
            var monster5 = CreateNewMonster(player, rand);

            var monsterList = new List<Monster>()
            {
                monster1, monster2, monster3, monster4, monster5
            };

            return monsterList;
        }
    }
}