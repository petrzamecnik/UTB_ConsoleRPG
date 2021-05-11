using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using ConsoleRPG.Characters;
using ConsoleRPG.Items;


namespace ConsoleRPG
{
    internal class Program
    {

        
        // TODO: CLEAN THE CODE!!
        // TODO: Implement inventory system for player
        // TODO: Implement more actions for player ( shop, crafting? )
        // TODO: Implement enemy item drop
        // TODO: Implement enemy battle logic
        public static event Action<Character, Character> EnemyChanged;

        public static void Main(string[] args)
        {
            SetupConsole();

            
            var rand = Rand.Instant;
            Monster enemy = null;
            ConsoleHelper.SetCurrentFont(default, 20);
            
            
            Center("Welcome to ConsoleRPG");
            CenterWrite("Choose name for your character: ");
            //var heroName = Console.ReadLine();
            const string heroName = "Eric";


            var testItem = new Item("testItem", 10);
            var testWeapon = new Weapon("testWeapon", 10, 10);
            var testShield = new Shield("testShield", 10, 10);
            var testHelmet = new Helmet("testHelmet", 10, 10);
            var testArmor = new Armor("testArmor", 10, 10);
            var testHealthPotion = new Potion("testHealthPotion", 10, 10, 0);
            var testManaPotion = new Potion("testManaPotion", 10, 0, 10);
            


            // Creation of player's character
            var player = new Player(heroName, 1, 120, 120, 20, 10, true,
                0, new Inventory(new List<Item>()), 100, 100, 0,
                100, testWeapon, testShield, testArmor, testHelmet, false);
            
            
            var monsters = CreateMonsterList(player, rand);
/*
            player.Inventory.AddItem(testItem);
            player.Inventory.AddItem(testWeapon);
            player.Inventory.AddItem(testShield);
            player.Inventory.AddItem(testHelmet);
            player.Inventory.AddItem(testArmor);
            player.Inventory.AddItem(testHealthPotion);
            player.Inventory.AddItem(testManaPotion);

            Console.WriteLine(player.Inventory.Items[0].ReturnItemName());
            Console.WriteLine(player.Inventory.Items[1].ReturnItemName());
            Console.WriteLine(player.Inventory.Items[2].ReturnItemName());
            Console.WriteLine(player.Inventory.Items[3].ReturnItemName());
            Console.WriteLine(player.Inventory.Items[4].ReturnItemName());
            Console.WriteLine(player.Inventory.Items[5].ReturnItemName());
            Console.WriteLine(player.Inventory.Items[6].ReturnItemName());
*/
            
            
            

 

          

            while (Console.ReadKey().Key != ConsoleKey.Enter) { }

            

            
            // game start
            gameStart:
            const int maxNumberOfActions = 6;
            Console.Clear();
            Console.WriteLine("1 - Battle");
            Console.WriteLine("2 - View character info");
            Console.WriteLine("3 - View available enemies");
            Console.WriteLine("4 - View all characters sorted by their power");
            Console.WriteLine("5 - View general info.");
            Console.WriteLine("6 - View inventory");
            Console.Write("Choose your next action! --> ");
            

            try
            {
                enemy = monsters[rand.Next(0, monsters.Count - 1)];

            }
            catch
            {
                monsters = RiseNewMonsters(player, rand);
                goto gameStart;
            }
            

            // switching actions
            var actionChoiceInput = Console.ReadLine();
            if (!int.TryParse(actionChoiceInput, out var actionChoice))
            {
                Console.WriteLine("*** Choice is not valid! ***");
                goto gameStart;
            }

            if (actionChoice > maxNumberOfActions || actionChoice < 0)
            {
                Console.WriteLine("*** Choice is not valid! ***");
                goto gameStart;
            }
            
            
            switch (actionChoice)
            {
                case 1:
                    player.SetRunningAwayState(false);
                    Console.WriteLine();
                    Console.WriteLine();
                    EnemyChanged = WriteNewMonster;
                    EnemyChanged?.Invoke(player, enemy);
                    Battle(player, enemy, rand, monsters);
                    goto gameStart;
                    
                case 2: 
                    TextNav.ViewCharacter(player);
                    goto gameStart;
                    
                case 3:
                    TextNav.ViewAvailableEnemies(monsters);
                    goto gameStart;
                    
                case 4:
                    TextNav.CompareCharacterWithEnemy(player, monsters);
                    goto gameStart;
                    
                case 5:
                    WriteGeneralInfo(player, monsters);
                    goto gameStart;
                    
                case 6:
                    TextNav.ViewInventory(player);
                    goto gameStart;

            }
        }

        private static void WriteNewMonster(Character player, Character enemy)
        {
            Console.WriteLine($"{player.ReturnCharacterName()} is facing a new monster -> {enemy.ReturnCharacterName()}");

        }

        private static List<Monster> RiseNewMonsters(Player player, Rand rand)
        {
            Console.Clear();
            Center("You have killed all monster nearby!");
            Center("...");
            System.Threading.Thread.Sleep(1000);
            Center("Tho you haven't won yet ");
            System.Threading.Thread.Sleep(1000);
            Center("...");
            System.Threading.Thread.Sleep(1000);
            Center("New monsters are rising. Good luck warrior!");
            return CreateMonsterList(player, rand);
        }


        // Method for battle
        private static void Battle(Player player, Monster actualEnemy, Random rand, List<Monster> monsters)
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
            // Ask player if he wants to continue or quit
            if (!player.IsAlive())
            {
                Console.Clear();
                Center(new string('*', 40));
                Center("You have died! :( ");
                Center(new string('*', 40));
                Console.WriteLine();
                CenterWrite("Do you wish to continue? --> Y/N   ");
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
                    Center("What a COWARD you are! ...");
                    System.Threading.Thread.Sleep(3000);
                    Environment.Exit(1);
                    
                }

            }
            // get exp, level up, remove monster from list
            if (!actualEnemy.IsAlive())
            {
                Console.Clear();
                Center("!CONGRATULION!");
                Center("Monster has been slain!");
                player.GainExperience(Convert.ToInt32(Math.Floor(actualEnemy.ReturnCharacterMaxHealth() * 0.6)));
                Center($"Experience gained: {Convert.ToInt32(Math.Floor(actualEnemy.ReturnCharacterMaxHealth() * 0.6))}");
                player.TryToLevelUp();
                monsters.Remove(actualEnemy);
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
                monsterDefence, false, 0, new Inventory(new List<Item>()));
            
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
                monsterNamesFile = File.ReadAllText("./monster_names.txt");
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

        public static List<Character> ReturnAllCharacters(Player player, List<Monster> monsters)
        {
            var allCharacters = new List<Character> {player};
            allCharacters.AddRange(monsters.Cast<Character>());

            return allCharacters;
        }
        
        private static void WriteGeneralInfo(Player player, List<Monster> monsters)
        {
            var averageCharacterPower = ReturnAllCharacters(player, monsters)
                .Average(ch => ch.CalculatePower());
            
            var characterWithLowestPower = ReturnAllCharacters(player, monsters)
                .Min(ch => ch.CalculatePower());
            
            var characterWithHighestPower = ReturnAllCharacters(player, monsters)
                .Max(ch => ch.CalculatePower());
            
            var characterWithPowerHigherThanAverage = ReturnAllCharacters(player, monsters)
                .FindAll(ch => ch.CalculatePower() > averageCharacterPower);
            
            var lastCharacterWithPowerLowerThanAverage = ReturnAllCharacters(player, monsters)
                .FindLast(ch => ch.CalculatePower() < averageCharacterPower);
            
            var onlyHero = ReturnAllCharacters(player, monsters).FindAll(ch => ch is Player);
            
            var averageDamage = ReturnAllCharacters(player, monsters).Average(ch => ch.ReturnCharacterAttack());

            var averageDefence = ReturnAllCharacters(player, monsters).Average(ch => ch.ReturnCharacterDefense());

            var higherThanOneQuarter = ReturnAllCharacters(player, monsters)
                .FindAll(ch => ch.ReturnCharacterDefense() > (averageDefence / 4));
            
            
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine();
            Center(new string('*', 30));
            Center("Press [ENTER] to return back.");
            Center(new string('*', 30));
            Console.WriteLine();
            
            Center($"Average power of characters: {Math.Round(averageCharacterPower, 2)}");
            Center(new string('-', 60));
            
            // !! Zeptat se jestli tohle jde dát na více řádků !!
            Center($"Weakest character: {ReturnAllCharacters(player, monsters).Find(ch => Math.Abs(ch.CalculatePower() - characterWithLowestPower) < 0.3).ReturnCharacterName()} - " +
                   $"{characterWithLowestPower}");
            Center(new string('-', 60));

            Center($"Strongest character: {ReturnAllCharacters(player, monsters).Find(ch => Math.Abs(ch.CalculatePower() - characterWithHighestPower) < 0.3).ReturnCharacterName()} - " +
                   $"{characterWithHighestPower}");
            Center(new string('-', 60));

            Center($"Characters with higher power than average: ");
            foreach (var ch in characterWithPowerHigherThanAverage)
            {
                Center($"{ch.ReturnCharacterName()} -> {ch.CalculatePower()}");
            }
            Center(new string('-', 60));

            
            Center($"Last listed character with power value lower than average: ");
            Center($"{lastCharacterWithPowerLowerThanAverage.ReturnCharacterName()} - {lastCharacterWithPowerLowerThanAverage.CalculatePower()}");
            Center(new string('-', 60));

            
            Center($"Player's character: {player.ReturnCharacterName()} - {player.CalculatePower()}");
            Center(new string('-', 60));

            
            // characters that remain after removing all characters,
            // that have maximum damage lower than half of the average
            // nelze :(
            
            Center("All character's that have lower defence then one quarter of average: ");
            foreach (var ch in higherThanOneQuarter)
            {
                Center($"{ch.ReturnCharacterName()} - {ch.ReturnCharacterDefense()}");
            }

            while (Console.ReadKey().Key != ConsoleKey.Enter) { }
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
        private static void CenterWrite(string message)
        {
            var screenWidth = Console.WindowWidth;
            var stringWidth = message.Length;
            var spaces = (screenWidth / 2) + (stringWidth / 2);
            Console.Write(message.PadLeft(spaces));
        }
        public static void Col(string input,string colorName)
        {
            Console.ForegroundColor = colorName switch
            {
                    "Yellow" => ConsoleColor.Yellow,
                    "DYellow"=>ConsoleColor.DarkYellow,
                    "Red" => ConsoleColor.Red,
                    "DGreen" => ConsoleColor.DarkGreen,
                    "Blue" => ConsoleColor.Blue,
                    "Cyan" => ConsoleColor.Cyan,
                    "White" => ConsoleColor.White,
                    _ => Console.ForegroundColor
                    
            };
            Console.WriteLine(input);
            Console.ResetColor();
        }

        public static void CenterCol(string input, string colorName)
        {
            var screenWidth = Console.WindowWidth;
            var stringWidth = input.Length;
            var spaces = (screenWidth / 2) + (stringWidth / 2);
            
            Console.ForegroundColor = colorName switch
            {
                "Yellow" => ConsoleColor.Yellow,
                "DYellow"=>ConsoleColor.DarkYellow,
                "Red" => ConsoleColor.Red,
                "DGreen" => ConsoleColor.DarkGreen,
                "Blue" => ConsoleColor.Blue,
                "Cyan" => ConsoleColor.Cyan,
                "White" => ConsoleColor.White,
                _ => Console.ForegroundColor
                    
            };
            Console.WriteLine(input.PadLeft(spaces));
            Console.ResetColor();
        }
        
        public static void ColWrite(string input,string colorName)
        {
            Console.ForegroundColor = colorName switch
            {
                "Yellow" => ConsoleColor.Yellow,
                "DYellow"=>ConsoleColor.DarkYellow,
                "Red" => ConsoleColor.Red,
                "DGreen" => ConsoleColor.DarkGreen,
                "Blue" => ConsoleColor.Blue,
                "Cyan" => ConsoleColor.Cyan,
                "White" => ConsoleColor.White,
                _ => Console.ForegroundColor
                    
            };
            Console.Write(input);
            Console.ResetColor();
        }
    }
}