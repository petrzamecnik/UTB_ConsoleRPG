﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Runtime.InteropServices;
using ConsoleRPG.Characters;
using ConsoleRPG.Items;


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
            SetupConsole();
            
            var rand = new Random();

            ConsoleHelper.SetCurrentFont(default, 18);
            Center("Welcome to ConsoleRPG");
            CenterWrite("Choose name for your character: ");

            //var heroName = Console.ReadLine();
           
            var heroName = "Crowans";
            
            // Creation of player's character
            var player = new Player(heroName, 1, 10, 100, 10, 0, true,
                0, 100,100, 0, 100, new Inventory(), false);

            var monsters = CreateMonsterList(player, rand);
            var enemy = monsters[rand.Next(0, monsters.Count - 1)];
            
            // game start
            gameStart:
            Console.Clear();
            Console.WriteLine("1 - Battle");
            Console.WriteLine("2 - View character info");
            Console.Write("Choose your next action! --> ");
            
            // switching actions
            var actionChoice = Convert.ToInt32(Console.ReadLine());
            switch (actionChoice)
            {
                case 1:
                    player.SetRunningAwayState(false);
                    Battle(player, enemy, rand);
                    goto gameStart;
                    
                case 2: 
                    TextNav.ViewCharacter(player);
                    goto gameStart;
                        
            }
        }

        // Method for battle
        private static void Battle(Player player, Monster actualEnemy, Random rand)
        {
            for (var i = 0; player.IsAlive() && actualEnemy.IsAlive() && !player.ReturnRunState(); i++)
            {
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine(new string('*', 60));
                Console.WriteLine($"Round: {i}");
                Console.WriteLine($"{player.ReturnCharacterName()}'s health: {player.ReturnCharacterHealth()}" +
                                  $"  ||  " +
                                  $"{player.ReturnCharacterName()}'s mana: {player.ReturnPlayerMana()}");
                Console.WriteLine($"{actualEnemy.ReturnCharacterName()}'s health: {actualEnemy.ReturnCharacterHealth()}");
                Console.WriteLine(new string('*', 60));
                

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
                else if (player.IsAlive() && !actualEnemy.IsAlive())
                {
                    Console.WriteLine("Monster has been slain!");
                    player.GainExperience(Convert.ToInt32(Math.Floor(actualEnemy.ReturnCharacterMaxHealth() * 0.4)));
                    player.TryToLevelUp();
                }
                

                if (actualEnemy.IsAlive() && !actualEnemy.IsStunned())
                {
                    // give chance to run away, if that fails, then fight
                    RunAway(actualEnemy, rand);
                    actualEnemy.BasicAttack(player);
                }
                else if (actualEnemy.IsAlive() && actualEnemy.IsStunned())
                {
                    actualEnemy.StunnedForXTurns -= 1;
                    Console.WriteLine($"{actualEnemy.ReturnCharacterName()} is stunned!");
                    Console.WriteLine($"{actualEnemy.ReturnCharacterName()} will be stunned for {actualEnemy.StunnedForXTurns} more rounds.");
                }


                
            }
            if (!player.IsAlive())
            {
                Console.Clear();
                Center(new string('*', 40));
                Center("You have died! :( ");
                Center(new string('*', 40));
                Console.WriteLine();
                CenterWrite("Do you wish to continue? --> Y/N");
                var continueOrExit = Console.ReadLine()?.ToLower();
                if (continueOrExit == "y")
                {
                    Console.Clear();
                    Center("Starting a new game!");
                    System.Threading.Thread.Sleep(3000);
                    Main(null);
                }
                else
                {
                    Console.Clear();
                    Center("What a shame! ...");
                    System.Threading.Thread.Sleep(3000);
                    Environment.Exit(1);
                    
                }

            }
            if (!actualEnemy.IsAlive())
            {
                Console.Clear();
                Console.WriteLine("Monster has been slain!");
                player.GainExperience(Convert.ToInt32(Math.Floor(actualEnemy.Health * 0.4)));
                player.TryToLevelUp();
                System.Threading.Thread.Sleep(3000);
                
            }
        }

        // Action for player in battle
        private static void PlayerBattleActions(Player player, Monster actualEnemy)
        {
            Console.WriteLine("It's your turn!");
            Console.WriteLine("1 - Attack");
            Console.WriteLine("2 - Cast Spell");
            Console.WriteLine("3 - Use Potion");
            Console.WriteLine("4 - Run");
            Console.Write("Choose your action --> ");
            var actionChoice = Convert.ToInt32(Console.ReadLine());

            if (actionChoice > 4 || actionChoice <= 0)
            {
                Console.WriteLine("*** Not valid choice ***");
                PlayerBattleActions(player, actualEnemy);
            }

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
                    player.Run(player);
                    break;
            }

            Console.WriteLine("SWITCH HAS ENDED");
            
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
        
        // Method to give monster chance to run away
        private static void RunAway(Character character, Random rnd)
        {
            if (character.Health < character.ReturnCharacterMaxHealth() * 0.2)
            {
                if (rnd.Next(0, 100) > 90)
                {
                    Console.WriteLine($"{character.ReturnCharacterName()} has run away!");
                    
                }
            }
        }

        // Setup Console
        private static void SetupConsole()
        {
            Console.WindowWidth = 120;
            Console.WindowHeight = 40;
            Console.ForegroundColor = ConsoleColor.Yellow;

        }

        // Method to center string
        public static void Center(string message)
        {
            var screenWidth = Console.WindowWidth;
            var stringWidth = message.Length;
            //var consoleHeight = Console.WindowHeight;
            var spaces = (screenWidth / 2) + (stringWidth / 2);
            Console.WriteLine(message.PadLeft(spaces));
        }
        
        // Method to center string ( write )
        public static void CenterWrite(string message)
        {
            var screenWidth = Console.WindowWidth;
            var stringWidth = message.Length;
            var spaces = (screenWidth / 2) + (stringWidth / 2);
            Console.Write(message.PadLeft(spaces));
        }
    }
}