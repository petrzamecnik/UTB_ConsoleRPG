using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using ConsoleRPG.Characters;
using ConsoleRPG.Items;
using static ConsoleRPG.Program;


namespace ConsoleRPG
{
    public class TextNav
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

            while (Console.ReadKey().Key != ConsoleKey.Enter) { }
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

            WriteOutWeapons(player);
            WriteOutShields(player);
            WriteOutHelmets(player);
            WriteOUtArmors(player);
            WriteOutPotions(player);

            while (Console.ReadKey().Key != ConsoleKey.Enter) { }
        }
        
        public static void ChangeEquipment(Player player)
        {
            
            changeEquipment:
            Console.Clear();
            Center("Choose what item to change [1, 2, 3, 4] or [0] to return back");
            Center("0 - Return back");
            Center("1 - Weapon");
            Center("2 - Shield");
            Center("3 - Helmet");
            Center("4 - Armor");
            var actionChoiceInput = Console.ReadLine();

            if (!int.TryParse(actionChoiceInput, out var actionChoice))
            {
                Center("*** Choice is not valid! ***");
                goto changeEquipment;
            }

            if (actionChoice > 4 || actionChoice < 0 )
            {
                Center("*** Choice is not valid! ***");
                goto changeEquipment;
            }

            if (actionChoice == 0)
            {
                return;
            }
            

            switch (actionChoice)
            {
                case 1:
                    EquipWeapon(player);
                    return;
                    

                case 2:
                    EquipShield(player);
                    return;
                    
                case 3:
                    WriteOutHelmets(player);
                    goto changeEquipment;
                    
                case 4: WriteOUtArmors(player);
                    goto changeEquipment;
            }
            
 
            while (Console.ReadKey().Key != ConsoleKey.Enter) { }

        }

        private static void EquipWeapon(Player player)
        {
            equipWeapon:
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine();
            var weapons = player.Inventory.Items.OfType<Weapon>().ToList();
            var itemIndex = 1;
            Center($"0 -> return back");
            foreach (var weapon in weapons)
            {
                Center($"{itemIndex} -> {weapon.ReturnItemName()} with {weapon.ReturnBonusAttack()} bonus damage");
                itemIndex++;
            }
                    
            var actionChoiceInput = Console.ReadLine();

            if (!int.TryParse(actionChoiceInput, out var actionChoice))
            {
                
                Center("*** Choice is not valid! ***");
                goto equipWeapon;
            }

            if (actionChoice >= itemIndex || actionChoice< 0 )
            {
                Center("*** Choice is not valid! ***");
                goto equipWeapon;
            }

            if (actionChoice == 0)
            {
                return;
            }
                    
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine();
            Center(new string('*', 30));
            Center("Press [ENTER] to return back.");
            Center(new string('*', 30));
            Console.WriteLine();
            Console.WriteLine();

            player.EquipNewWeapon(weapons[actionChoice - 1]);
            Center($"You have successfully equiped {weapons[actionChoice - 1].ReturnItemName()}! ");
            while (Console.ReadKey().Key != ConsoleKey.Enter) { }
        }
        
        private static void EquipShield(Player player)
        {
            equipShield:
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine();
            var shields = player.Inventory.Items.OfType<Shield>().ToList();
            var itemIndex = 1;
            Center($"0 -> return back");
            foreach (var shield in shields)
            {
                Center($"{itemIndex} -> {shield.ReturnItemName()} with {shield.ReturnBonusDefense()} bonus defense");
                itemIndex++;
            }
                    
            var actionChoiceInput = Console.ReadLine();

            if (!int.TryParse(actionChoiceInput, out var actionChoice))
            {
                
                Center("*** Choice is not valid! ***");
                goto equipShield;
            }

            if (actionChoice >= itemIndex || actionChoice< 0 )
            {
                Center("*** Choice is not valid! ***");
                goto equipShield;
            }

            if (actionChoice == 0)
            {
                return;
            }
                    
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine();
            Center(new string('*', 30));
            Center("Press [ENTER] to return back.");
            Center(new string('*', 30));
            Console.WriteLine();
            Console.WriteLine();

            player.EquipNewShield(shields[actionChoice - 1]);
            Center($"You have successfully equiped {shields[actionChoice - 1].ReturnItemName()}! ");
            while (Console.ReadKey().Key != ConsoleKey.Enter) { }
        }



        
        private static void WriteOutWeapons(Player player)
        {
            var weapons = player.Inventory.Items.OfType<Weapon>().ToList();
            foreach (var weapon in weapons)
            {
                Center($"{weapon.ReturnItemName()}");
                Center($"Bonus attack: {weapon.ReturnBonusAttack()}");
                Center(new string('-', 30));
            }
        }
        
        private static void WriteOutPotions(Player player)
        {
            var potions = player.Inventory.Items.OfType<Potion>().ToList();
            foreach (var potion in potions)
            {
                Center($"{potion.ReturnItemName()}");
                Center($"Health regen: {potion.ReturnHealthRegenAmount()}");
                Center($"Mana regen: {potion.ReturnManaRegenAmount()}");
                Center(new string('-', 30));
            }
        }

        private static void WriteOUtArmors(Player player)
        {
            var armors = player.Inventory.Items.OfType<Armor>().ToList();
            foreach (var armor in armors)
            {
                Center($"{armor.ReturnItemName()}");
                Center($"Bonus attack: {armor.ReturnBonusDefense()}");
                Center(new string('-', 30));
            }
        }

        private static void WriteOutHelmets(Player player)
        {
            var helmets = player.Inventory.Items.OfType<Helmet>().ToList();
            foreach (var helmet in helmets)
            {
                Center($"{helmet.ReturnItemName()}");
                Center($"Bonus attack: {helmet.ReturnBonusDefense()}");
                Center(new string('-', 30));
            }
        }

        private static void WriteOutShields(Player player)
        {
            var shields = player.Inventory.Items.OfType<Shield>().ToList();
            foreach (var shield in shields)
            {
                Center($"{shield.ReturnItemName()}");
                Center($"Bonus attack: {shield.ReturnBonusDefense()}");
                Center(new string('-', 30));
            }
        }

        

        
    }
}