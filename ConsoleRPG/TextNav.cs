using System;
using ConsoleRPG.Characters;
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
            Center(new string('*' ,30));
            Center("Press [ENTER] to return back.");
            Center(new string('*' ,30));
            Console.WriteLine();
            Console.WriteLine();
            Center(new string('-' ,30));
            Console.WriteLine();
            Center($"Level: {player.ReturnCharacterLevel()}");
            Center($"Maximum health: {player.ReturnCharacterMaxHealth()}");
            Center($"Actual Health: {player.ReturnCharacterHealth()}");
            Center($"Mana: {player.ReturnPlayerMana()} / {player.ReturnPlayerMaxMana()}");
            Console.WriteLine();
            Center(new string('-' ,30));
            Console.WriteLine();
            Center($"Attack: {player.ReturnCharacterAttack()}");
            Center($"Defense: {player.ReturnCharacterDefense()}");
            Console.WriteLine();
            Center(new string('-' ,30));
            Console.WriteLine();
            Center($"Experience: {player.ReturnExperience()}");
            Center($"Experience needed: {player.ReturnMaxExperience() - player.ReturnExperience()}");
            Console.WriteLine();
            Center(new string('-' ,30));


            while (Console.ReadKey().Key != ConsoleKey.Enter){}


        }
    }
}