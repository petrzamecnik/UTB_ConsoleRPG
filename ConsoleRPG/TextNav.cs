using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ConsoleRPG.Characters;
using ConsoleRPG.Items;
using static ConsoleRPG.Program;


namespace ConsoleRPG
{
    public static class TextNav
    {

        public static void ViewCharacter(Player player)
        {
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Center(new string('*', 30));
            Center("Press [ENTER] to return back.");
            Center(new string('*', 30));
            Console.WriteLine();
            Console.WriteLine();
            Center(new string('-', 30));
            Console.WriteLine();
            Center($"Level: {player.ReturnCharacterLevel()}");
            Center($"Maximum health: {player.ReturnCharacterMaxHealth()}");
            Center($"Actual Health: {player.ReturnCharacterHealth()}");
            Center($"Mana: {player.ReturnPlayerMana()} / {player.ReturnPlayerMaxMana()}");
            Console.WriteLine();
            Center(new string('-', 30));
            Console.WriteLine();
            Center($"Attack: {player.ReturnCharacterAttack()}");
            Center($"Defense: {player.ReturnCharacterDefense()}");
            Console.WriteLine();
            Center(new string('-', 30));
            Console.WriteLine();
            Center($"Experience: {player.ReturnExperience()}");
            Center($"Experience needed: {player.ReturnMaxExperience()}");
            Console.WriteLine();
            Center(new string('-', 30));

            while (Console.ReadKey().Key != ConsoleKey.Enter)
            {
            }
        }

        public static void ViewAvailableEnemies(List<Monster> monsters)
        {
            Center($"Total available enemies: {monsters.Count}");

            foreach (var enemy in monsters)
            {
                Console.Clear();
                Console.WriteLine();
                Console.WriteLine();
                Center(new string('*', 30));
                Center("Press [E] to continue to next enemy or [Q] to return back.");
                Center(new string('*', 30));
                Console.WriteLine();
                Console.WriteLine();
                Center(new string('-', 40));
                Center($"{enemy.ReturnCharacterName()}");
                Center($"{enemy.ReturnCharacterName()} has {enemy.ReturnCharacterHealth()} health");
                Center($"{enemy.ReturnCharacterName()} has {enemy.ReturnCharacterAttack()} attack");
                Center($"{enemy.ReturnCharacterName()} has {enemy.ReturnCharacterDefense()} defense");
                Center($"{enemy.ReturnCharacterName()} has {enemy.CalculatePower()} power");
                Center(new string('-', 40));

                if (Console.ReadKey().Key == ConsoleKey.E)
                {
                    continue;
                }

                break;
            }
        }

        public static void CompareCharacterWithEnemy(Player player, List<Monster> monsters)
        {
            var allCharacters = ReturnAllCharacters(player, monsters);
            allCharacters.Sort();
            allCharacters.Reverse();

            Console.Clear();
            Console.WriteLine();
            Console.WriteLine();
            Center(new string('*', 30));
            Center("Press [ENTER] to return back.");
            Center(new string('*', 30));
            Console.WriteLine();
            Center("All characters sorted by power: ");
            Console.WriteLine();
            Console.WriteLine();
            foreach (var character in allCharacters)
            {
                Center($"{character.ReturnCharacterName()} has {character.CalculatePower()} power points.");
                Console.WriteLine();
            }

            while (Console.ReadKey().Key != ConsoleKey.Enter) { }
        }



        public static void RainbowWorld()
        {
            Col("Lorem ipsum", "Yellow");
            Col("Lorem ipsum", "DYellow");
            Col("Lorem ipsum", "Red");
            Col("Lorem ipsum", "DGreen");
            Col("Lorem ipsum", "Blue");
            Col("Lorem ipsum", "Cyan");
            Col("Lorem ipsum", "White");

            Console.ReadKey();
            Console.Clear();
        }

        public static void ViewInventory(Player player)
        {
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine();
            Center(new string('*', 30));
            Center("Press [ENTER] to return back.");
            Center(new string('*', 30));
            Console.WriteLine();
            Console.WriteLine();
            Center(new string('-', 30));



            
           /*
            foreach (var item in player.Inventory.Items)
            {
                switch (item)
                {
                    case Weapon:
                        Center($"{Armor.ReturnItemName()}");
                        Center($"Bonus attack: {Weapon.ReturnBonusAttack()}");
                        Center(new string('-', 30));
                        break;
                    
                    case Shield:
                        Center($"{Armor.ReturnItemName()}");
                        Center($"Bonus defense: {Shield.ReturnBonusDefense()}");
                        Center(new string('-', 30));
                        break;
                    
                    case Helmet:
                        Center($"{Armor.ReturnItemName()}");
                        Center($"Bonus defense: {Helmet.ReturnBonusDefense()}");
                        Center(new string('-', 30));
                        break;
                    
                    case Armor:
                        Center($"{Armor.ReturnItemName()}");
                        Center($"Bonus defense: {Armor.ReturnBonusDefense()}");
                        Center(new string('-', 30));
                        break;

                        
                }
            }
            */
            while (Console.ReadKey().Key != ConsoleKey.Enter) { }
        }

        public static void EquipItem(Player player)
        {
            ViewInventory(player);
            
            
        }
    }
}