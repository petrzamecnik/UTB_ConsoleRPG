using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using ConsoleRPG.Characters;
using ConsoleRPG.Items;
using static ConsoleRPG.BetterConsole;
using CONNECTDATA = System.Runtime.InteropServices.ComTypes.CONNECTDATA;


namespace ConsoleRPG
{
    internal class Program
    {
        
        
        
        // TODO: Tidy up code
        // TODO: Implement enemy battle logic
        // TODO: Make own exceptions
        // TODO: Log Battles
        // TODO: Implement save / load
        // TODO: Implement more actions for player ( shop, crafting? )
        
        public static event Action<Character, Character> EnemyChanged;

        public static void Main(string[] args)
        {
            SetupConsole();
            // weapons
            var placeHolderWeapon = new Weapon("Empty", 0, 0);
            var woodenSword = new Weapon("Wooden Sword", 0, 10);
            var stoneSword = new Weapon("Stone Sword", 10, 15);
            var woodenStickOfDoom = new Weapon("Wooden Stick of Doom", 30, 30);
            var ironSword = new Weapon("Iron Sword", 50, 40);
            var brutalBattleAxe = new Weapon("Brutal Battle Axe", 50, 45);
            var fieryRapier = new Weapon("Fiery Rapier", 80, 60);
            
            // shields
            var placeHolderShield = new Shield("Empty", 0, 0);
            var woodenShield = new Shield("Wooden Shield", 0, 10);
            var ironShield = new Shield("Iron Shield", 30, 30);
            var ironDome = new Shield("Iron Dome", 100, 50);
            var hugeLifeSavingBarrier = new Shield("Huge Life Saving Barrier", 150, 60);
            
            // helmets
            var placeHolderHelmet = new Helmet("Empty", 0, 0);
            var leatherHelmet = new Helmet("Leather Helmet", 0, 5);
            var ironHelmet = new Helmet("Iron Helmet", 30, 10);
            var reinforcedIronHelmet = new Helmet("Reinforced Iron Helmet", 50, 20);
            var helmetOfGreatProtector = new Helmet("Helmet of Great Protector", 100, 30);
            var dragonScaleHelmet = new Helmet("Dragon Scale Helmet", 300, 50);

            // armors
            var placeHolderArmor = new Armor("Empty", 0, 0);
            var leatherArmor = new Armor("Leather Armor", 0, 5);
            var ironArmor = new Armor("Iron Armor", 30, 10);
            var reinforcedIronArmor = new Armor("Reinforced Iron Armor", 50, 20);
            var armorOfGreatProtector = new Armor("Armor of Great Protector", 100, 30);
            var dragonScaleArmor = new Armor("Dragon Scale Armor", 300, 50);

            // potions
            var smallHealthPotion = new Potion("Small Health Potion", 20, 30, 0);
            var mediumHealthPotion = new Potion("Medium Health Potion", 30, 60, 0);
            var largeHealthPotion = new Potion("Large Health Potion", 50, 90, 0);

            var smallManaPotion = new Potion("Small Mana Potion", 20, 0, 30);
            var mediumManaPotion = new Potion("Medium Health Potion", 30, 0, 60);
            var largeManaPotion = new Potion("Large Health Potion", 50, 0, 90);

            var smallHybridPotion = new Potion("Small Hybrid Potion", 40, 30, 30);
            var mediumHybridPotion = new Potion("Medium Hybrid Potion", 60, 60, 60);
            var largeHybridPotion = new Potion("Large Hybrid Potion", 90, 90, 90);

            var items1To5 = new List<Item>
            {
                stoneSword, woodenStickOfDoom,
                woodenShield,
                leatherHelmet,
                leatherArmor
                
            };

            var items6To10 = new List<Item>
            {
                ironSword,
                ironShield,
                ironHelmet, reinforcedIronHelmet,
                ironArmor, reinforcedIronArmor
            };

            var items11To20 = new List<Item>
            {
                ironSword, brutalBattleAxe, fieryRapier,
                ironShield, ironDome, hugeLifeSavingBarrier,
                helmetOfGreatProtector, dragonScaleHelmet,
                armorOfGreatProtector, dragonScaleArmor
            };
            

            
            var rand = Rand.Instant;
            ConsoleHelper.SetCurrentFont(default, 20);
            Monster enemy = null;
            string heroName = null;
            
            Center("Welcome to ConsoleRPG");
            CenterWrite("Choose name for your character: ");

            try
            {
                heroName = Console.ReadLine();
            }
            catch
            {
                throw new InvalidCharacternameException("error");
            }

            
            
            

            // Creation of player's character
            var player = new Player(heroName, 1, 120, 120, 20, 10, true,
                0, new Inventory(new List<Item>()), 100, 100, 0,
                100, placeHolderWeapon, placeHolderShield, placeHolderArmor, placeHolderHelmet, false);
            
            player.Inventory.AddItem(woodenSword);
            player.Inventory.AddItem(smallHealthPotion);
            player.Inventory.AddItem(smallManaPotion);
            
            

            var monsters = CreateMonsterList(player, rand, items1To5, items6To10, items11To20);
            
            
            // game start
            gameStart:
            const int maxNumberOfActions = 9;
            Console.Clear();
            Console.WriteLine("1 - Battle");
            Console.WriteLine("2 - View character info");
            Console.WriteLine("3 - View available enemies");
            Console.WriteLine("4 - View all characters sorted by their power");
            Console.WriteLine("5 - View general info.");
            Console.WriteLine("6 - View inventory");
            Console.WriteLine("7 - Change Equipment");
            Console.WriteLine("8 - Load Game");
            Console.WriteLine("9 - Save Game");
            Console.Write("Choose your next action! --> ");
            

            try
            {
                enemy = monsters[rand.Next(0, monsters.Count - 1)];

            }
            catch
            {
                monsters = RiseNewMonsters(player, rand, items1To5, items6To10, items11To20);
                goto gameStart;
            }
            

            // switching actions
            var actionChoiceInput = Console.ReadLine();
            if (!int.TryParse(actionChoiceInput, out var actionChoice))
            {
                Console.WriteLine("*** Choice is not valid! ***");
                goto gameStart;
            }

            if (actionChoice > maxNumberOfActions || actionChoice <= 0)
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
                    TextNav.WriteGeneralInfo(player, monsters);
                    goto gameStart;
                    
                case 6:
                    TextNav.ViewInventory(player);
                    goto gameStart;
                    
                case 7:
                    TextNav.ChangeEquipment(player);
                    goto gameStart;
                    
                case 8:
                    player = LoadGame();
                    goto gameStart;
                    
                case 9:
                    SaveGame(player);
                    goto gameStart;
                    

            }
        }

        private static void SaveGame(Player player)
        {
            Console.Clear();
            saveGame:

            CenterColWrite("Enter save name: ", "yellow");
            var saveName = Console.ReadLine();
            saveName = "save - " + saveName + ".txt";

            if (File.Exists(saveName))
            {
                CenterCol("Save already exists!", "red");
                goto saveGame;
            }

            File.Create(saveName).Close();
            
            
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream("C:\\Users\\petrz\\RiderProjects\\ConsoleRPG\\ConsoleRPG\\bin\\Debug\\" + saveName,
                FileMode.Create, FileAccess.Write);


            
            formatter.Serialize(stream, player);
            stream.Close();

            
            CenterCol("Game successfully saved!", "yellow");
            Thread.Sleep(2000);
        }
        
        private static Player LoadGame()
        {
            loadGame:
            Console.Clear();
            var filePaths = Directory.GetFiles("C:\\Users\\petrz\\RiderProjects\\ConsoleRPG\\ConsoleRPG\\bin\\Debug\\",
                "save - *.txt");

            var fileNames = filePaths.Select(filePath => Path.GetFileName(filePath)).ToList();

            var fileIndex = 0;
            foreach (var fileName in fileNames)
            {
                Center($"{fileIndex + 1} - {fileName}");
                fileIndex++;
            }

            CenterCol("Choose which save to load [1, 2, 3 ... ], [0] to return", "yellow");
            var actionChoice = Convert.ToInt32(Console.ReadLine());

            if (actionChoice == 0)
            {
                //return null;
            }

            try
            {
                IFormatter formatter = new BinaryFormatter();
                Stream stream = new FileStream(filePaths[actionChoice - 1],
                    FileMode.Open, FileAccess.Read);

                var player = (Player)formatter.Deserialize(stream);
                stream.Close();

                CenterCol($"Loading save: {fileNames[actionChoice]}", "cyan");
                Center(player.ReturnCharacterName());
                Thread.Sleep(2000);
                return player;
            }
            catch
            {
                throw new SaveNotLoadedProperlyException();
            }
            
        }

        private static void GameSetupController(Player player)
        {
            gameSetupController:
            CenterCol("Welcome to ConsoleRPG", "yellow");
            CenterCol("[1] Start New Game", "yellow");
            CenterCol("[2] Load Game", "yellow");
            CenterCol("[0] Exit Game", "yellow");

            var actionChoiceInput = Console.ReadLine();

            if (!int.TryParse(actionChoiceInput, out var actionChoice))
            {
                
                CenterCol("*** Choice is not valid! ***", "red");
                goto gameSetupController;
            }
            switch (actionChoice)
            {
                case 0:
                    Environment.Exit(0);
                    break;
                case 1:
                    return;
                case 2:
                    player = LoadGame();
                    return;
                
                default:
                    CenterCol("*** Choice is not valid! ***", "red");
                    goto gameSetupController;
                    
                
            }
        }
        
        

        

        public static Exception InvalidCharacternameException { get; set; }

        private static void WriteNewMonster(Character player, Character enemy)
        {
            Col($"{player.ReturnCharacterName()} is facing a new monster -> {enemy.ReturnCharacterName()}", "cyan");

        }

        private static List<Monster> RiseNewMonsters(Player player, Rand rand, List<Item> items1To5, List<Item> items6To10, List<Item> items11To20)
        {
            Console.Clear();
            Center("You have killed all monster nearby!");
            Center("...");
            Thread.Sleep(1000);
            Center("Tho you haven't won yet ");
            Thread.Sleep(1000);
            Center("...");
            Thread.Sleep(1000);
            Center("New monsters are rising. Good luck warrior!");
            return CreateMonsterList(player, rand, items1To5, items6To10, items11To20);
        }


        // Method for battle
        private static void Battle(Player player, Monster actualEnemy, Rand rand, List<Monster> monsters)
        {
            for (var i = 0; player.IsAlive() && actualEnemy.IsAlive() && !player.ReturnRunState(); i++)
            {
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine(new string('*', 60));
                Console.WriteLine($"Round: {i}");
                Col($"{player.ReturnCharacterName()}'s health: {player.ReturnCharacterHealth()}", "darkgreen");
                Col($"{player.ReturnCharacterName()}'s mana: {player.ReturnPlayerMana()}", "blue");
                Col($"{actualEnemy.ReturnCharacterName()}'s health: {actualEnemy.ReturnCharacterHealth()}", "darkyellow");
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
                    RunAway(actualEnemy, rand, actualEnemy, monsters);
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
                System.Threading.Thread.Sleep(1500);
                Console.Clear();
                CenterCol("!CONGRATULION!", "cyan");
                CenterCol("Monster has been slain!", "cyan");
                GetItem(player, actualEnemy, rand);
                player.GainExperience(Convert.ToInt32(Math.Floor(actualEnemy.ReturnCharacterMaxHealth() * 0.6)));
                Center($"Experience gained: {Convert.ToInt32(Math.Floor(actualEnemy.ReturnCharacterMaxHealth() * 0.6))}");
                player.TryToLevelUp();
                monsters.Remove(actualEnemy);
                System.Threading.Thread.Sleep(3000);
                
            }
        }

        private static void GetItem(Player player, Monster enemy, Rand rand)
        {
            var itemIndex = rand.Next(0, enemy.Inventory.Items.Count);
            if (enemy.Inventory.Items.Count == 0) return;
            
            player.Inventory.AddItem(enemy.Inventory.Items[itemIndex]);
            CenterCol($"{player.ReturnCharacterName()} has acquired a new {enemy.Inventory.Items[itemIndex].ReturnItemName()}", "darkyellow");
            
        }

        // Action for player in battle
        public static void PlayerBattleActions(Player player, Monster actualEnemy)
        {
            battleActionChoice:
            Console.WriteLine("It's your turn!");
            Console.WriteLine("1 - Attack");
            Console.WriteLine("2 - Cast Spell");
            Console.WriteLine("3 - Use Potion");
            Console.WriteLine("4 - Run");
            Console.Write("Choose your action --> ");
            var actionChoiceInput = Console.ReadLine();

            if (!int.TryParse(actionChoiceInput, out var actionChoice))
            {
                Col("*** Choice is not valid! ***", "red");
                PlayerBattleActions(player, actualEnemy);
            }

            if (actionChoice > 4 || actionChoice <= 0)
            {
                Col("*** Choice is not valid! ***", "red");
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
                    player.UsePotionAction(player);
                    goto battleActionChoice;
                
                case 4: 
                    player.Run(player);
                    break;
            }
        }

        // Method to create new monster
        private static Monster CreateNewMonster(Player player, Random rand, List<Item> items1To5, List<Item> items6To10, List<Item> items11To20)
        {
            // get list of monster names
            var nameList = ReturnNameOfNewMonster();
            List<Item> itemList = new List<Item>();

            // choose name for monster
            var monsterName = nameList[rand.Next(1, nameList.Count)].Trim();
            var playerLevel = player.ReturnCharacterLevel();

            var monsterLevel = playerLevel + rand.Next(1, 3);
            var monsterHealth = 50 + (20 * playerLevel) * Convert.ToInt32(rand.Next(1,300 +1) / 100);
            var monsterMaxHealth = monsterHealth;
            var monsterAttack = 10 + (5 * playerLevel) * Convert.ToInt32(rand.Next(1, 300 + 1) / 100);
            var monsterDefence = 5 + (5 * playerLevel) * Convert.ToInt32(rand.Next(1,300 +1) / 100);

            if (playerLevel >= 1 && playerLevel <= 5)
            {
                itemList = items1To5;
            }
            else if (playerLevel >= 6 && playerLevel <= 10)
            {
                itemList = items6To10;
            }
            else if (playerLevel >= 11 && playerLevel <= 20)
            {
                itemList = items11To20;
            }

            var monster = new Monster(monsterName, monsterLevel, monsterHealth, monsterMaxHealth, monsterAttack,
                monsterDefence, false, 0, new Inventory(new List<Item>()));
            
            if (rand.Next(1, 100) > 20)
            {
                monster.Inventory.AddItem(itemList[rand.Next(0, itemList.Count)]);
            }

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
        private static List<Monster> CreateMonsterList(Player player, Random rand, List<Item> items1To5, List<Item> items6To10, List<Item> items11To20)
        {
            var monster1 = CreateNewMonster(player, rand, items1To5, items6To10, items11To20);
            var monster2 = CreateNewMonster(player, rand, items1To5, items6To10, items11To20);
            var monster3 = CreateNewMonster(player, rand, items1To5, items6To10, items11To20);
            var monster4 = CreateNewMonster(player, rand, items1To5, items6To10, items11To20);
            var monster5 = CreateNewMonster(player, rand, items1To5, items6To10, items11To20);

            var monsterList = new List<Monster>()
            {
                monster1, monster2, monster3, monster4, monster5
            };

            return monsterList;
        }
        
        // Method to give monster chance to run away
        private static void RunAway(Character character, Random rnd, Monster enemy, List<Monster> monsters)
        {
            if (character.Health < character.ReturnCharacterMaxHealth() * 0.2)
            {
                if (rnd.Next(0, 100) > 90)
                {
                    Console.WriteLine($"{character.ReturnCharacterName()} has run away!");
                    monsters.Remove(enemy);

                }
            }
        }

        public static List<Character> ReturnAllCharacters(Player player, List<Monster> monsters)
        {
            var allCharacters = new List<Character> {player};
            allCharacters.AddRange(monsters.Cast<Character>());

            return allCharacters;
        }
    }

    internal sealed class InvalidCharacternameException : Exception
    {
        public InvalidCharacternameException()
        {
            
        }

        public InvalidCharacternameException(string ex)
            : base(ex)
        {
            CenterWrite("Not valid character name!");
        }
    }
    
    internal sealed class InvalidFilePath : Exception
    {
        public InvalidFilePath()
        {
            
        }

        public InvalidFilePath(string ex)
            : base(ex)
        {
            CenterWrite("Invalid File Path!");
        }
    }
    
    internal sealed class SaveNotLoadedProperlyException : Exception
    {
        public SaveNotLoadedProperlyException()
        {
            
        }

        public SaveNotLoadedProperlyException(string ex)
            : base(ex)
        {
            CenterWrite("Player not loaded properly...");
        }
    }
}