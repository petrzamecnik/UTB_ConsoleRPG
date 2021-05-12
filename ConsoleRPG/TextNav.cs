using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using ConsoleRPG.Characters;
using ConsoleRPG.Items;
using static ConsoleRPG.BetterConsole;
using static ConsoleRPG.Program;


namespace ConsoleRPG
{
    public class TextNav
    {

        public static void ViewCharacter(Player player)
        {
            var (actualWeapon, actualShield, actualHelmet, actualArmor) = player.ReturnEquippedItems();

            
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
            CenterCol($"Level: {player.ReturnCharacterLevel()}", "cyan");
            CenterCol($"Health: {player.ReturnCharacterHealth()} / {player.ReturnCharacterMaxHealth()}", "darkgreen");
            CenterCol($"Mana: {player.ReturnPlayerMana()} / {player.ReturnPlayerMaxMana()}", "blue");
            Console.WriteLine();
            Center(new string('-', 30));
            Console.WriteLine();
            CenterCol($"Attack: {player.ReturnCharacterAttack()}", "yellow");
            CenterCol($"Defense: {player.ReturnCharacterDefense()}", "yellow");
            Console.WriteLine();
            Center(new string('-', 30));
            Console.WriteLine();
            Center($"Experience: {player.ReturnExperience()}");
            Center($"Experience needed: {player.ReturnMaxExperience()}");
            Console.WriteLine();
            Center(new string('-', 40));    
            Center($"Weapon -> {actualWeapon.ReturnItemName()}  - {actualWeapon.ReturnBonusAttack()}  bonus damage");
            Center($"Shield -> {actualShield.ReturnItemName()}  - {actualShield.ReturnBonusDefense()} bonus defense");
            Center($"Helmet -> {actualHelmet.ReturnItemName()}  - {actualHelmet.ReturnBonusDefense()} bonus defense");
            Center($"Armor  -> {actualArmor.ReturnItemName()}   - {actualArmor.ReturnBonusDefense()}  bonus defense");
            Center(new string('-', 40));    


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
                Center(new string('*', 60));
                Center("Press [E] to continue to next enemy or [Q] to return back.");
                Center(new string('*', 60));
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine();
                Console.WriteLine();
                Center(new string('-', 40));
                Center($"{enemy.ReturnCharacterName()}");
                Center($"{enemy.ReturnCharacterName()} has {enemy.ReturnCharacterHealth()} health");
                Center($"{enemy.ReturnCharacterName()} has {enemy.ReturnCharacterAttack()} attack");
                Center($"{enemy.ReturnCharacterName()} has {enemy.ReturnCharacterDefense()} defense");
                Center($"{enemy.ReturnCharacterName()} has {enemy.CalculatePower()} power");
                Center(new string('-', 40));
                Console.ForegroundColor = ConsoleColor.White;

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
            CenterCol("All characters sorted by power: ", "darkyellow");
            Console.WriteLine();
            Console.WriteLine();
            foreach (var character in allCharacters)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;

                CenterCol($"{character.ReturnCharacterName()} has {character.CalculatePower()} power points.", "yellow");
                Console.WriteLine();
            }

            while (Console.ReadKey().Key != ConsoleKey.Enter) { }
        }
        


        public static void WriteGeneralInfo(Player player, List<Monster> monsters)
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
            Console.ForegroundColor = ConsoleColor.White;
            Center(new string('*', 30));
            Center("Press [ENTER] to return back.");
            Center(new string('*', 30));
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Yellow;

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

            Console.ForegroundColor = ConsoleColor.White;
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
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Center("1 - Weapon");
            Center("2 - Shield");
            Center("3 - Helmet");
            Center("4 - Armor");
            Console.WriteLine();
            Center("Choose what item to change [1, 2, 3, 4] or [0] to return back");
            CenterWrite(" --> ");
            var actionChoiceInput = Console.ReadLine();

            if (!int.TryParse(actionChoiceInput, out var actionChoice))
            {
                CenterCol("*** Choice is not valid! ***", "red");

                goto changeEquipment;
            }

            if (actionChoice > 4 || actionChoice < 0 )
            {
                CenterCol("*** Choice is not valid! ***", "red");

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
                    EquipHelmet(player);
                    return;
                    
                case 4: 
                    EquipArmor(player);
                    return;
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
        
        private static void EquipHelmet(Player player)
        {
            equipHelmet:
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine();
            var helmets = player.Inventory.Items.OfType<Helmet>().ToList();
            var itemIndex = 1;
            Center($"0 -> return back");
            foreach (var helmet in helmets)
            {
                Center($"{itemIndex} -> {helmet.ReturnItemName()} with {helmet.ReturnBonusDefense()} bonus defense");
                itemIndex++;
            }
                    
            var actionChoiceInput = Console.ReadLine();

            if (!int.TryParse(actionChoiceInput, out var actionChoice))
            {
                
                Center("*** Choice is not valid! ***");
                goto equipHelmet;
            }

            if (actionChoice >= itemIndex || actionChoice< 0 )
            {
                Center("*** Choice is not valid! ***");
                goto equipHelmet;
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

            player.EquipNewHelmet(helmets[actionChoice - 1]);
            Center($"You have successfully equiped {helmets[actionChoice - 1].ReturnItemName()}! ");
            while (Console.ReadKey().Key != ConsoleKey.Enter) { }
        }
        
        private static void EquipArmor(Player player)
        {
            equipArmor:
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine();
            var armors = player.Inventory.Items.OfType<Armor>().ToList();
            var itemIndex = 1;
            Center($"0 -> return back");
            foreach (var armor in armors)
            {
                Center($"{itemIndex} -> {armor.ReturnItemName()} with {armor.ReturnBonusDefense()} bonus defense");
                itemIndex++;
            }
                    
            var actionChoiceInput = Console.ReadLine();

            if (!int.TryParse(actionChoiceInput, out var actionChoice))
            {
                
                Center("*** Choice is not valid! ***");
                goto equipArmor;
            }

            if (actionChoice >= itemIndex || actionChoice< 0 )
            {
                Center("*** Choice is not valid! ***");
                goto equipArmor;
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

            player.EquipNewArmor(armors[actionChoice - 1]);
            Center($"You have successfully equiped {armors[actionChoice - 1].ReturnItemName()}! ");
            while (Console.ReadKey().Key != ConsoleKey.Enter) { }
        }



        
        private static void WriteOutWeapons(Player player)
        {
            var weapons = player.Inventory.Items.OfType<Weapon>().ToList();
            foreach (var weapon in weapons)
            {
                CenterCol($"{weapon.ReturnItemName()}", "yellow");
                CenterCol($"Bonus attack: {weapon.ReturnBonusAttack()}", "darkyellow");
                Center(new string('-', 30));
            }
        }

        public static void WriteOutPotions(Player player)
        {
            var potions = player.Inventory.Items.OfType<Potion>().ToList();
            foreach (var potion in potions)
            {
                CenterCol($"{potion.ReturnItemName()}", "yellow");
                CenterCol($"Health regen: {potion.ReturnHealthRegenAmount()}", "darkgreen");
                CenterCol($"Mana regen: {potion.ReturnManaRegenAmount()}", "blue");
                Center(new string('-', 30));
            }
        }

        private static void WriteOUtArmors(Player player)
        {
            var armors = player.Inventory.Items.OfType<Armor>().ToList();
            foreach (var armor in armors)
            {
                CenterCol($"{armor.ReturnItemName()}", "yellow");
                CenterCol($"Bonus armor: {armor.ReturnBonusDefense()}", "darkyellow");
                Center(new string('-', 30));
            }
        }

        private static void WriteOutHelmets(Player player)
        {
            var helmets = player.Inventory.Items.OfType<Helmet>().ToList();
            foreach (var helmet in helmets)
            {
                CenterCol($"{helmet.ReturnItemName()}", "yellow");
                CenterCol($"Bonus armor: {helmet.ReturnBonusDefense()}", "darkyellow");
                Center(new string('-', 30));
            }
        }

        private static void WriteOutShields(Player player)
        {
            var shields = player.Inventory.Items.OfType<Shield>().ToList();
            foreach (var shield in shields)
            {
                CenterCol($"{shield.ReturnItemName()}", "yellow");
                CenterCol($"Bonus armor: {shield.ReturnBonusDefense()}", "darkyellow");
                Center(new string('-', 30));
            }
        }
    }
}