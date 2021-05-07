using System;
using ConsoleRPG.Characters;

namespace ConsoleRPG
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Welcome to Hero RPG!");
            Console.Write("Choose name of your character: ");
            var heroName = Console.ReadLine();
            
            // Creation of player's character
            var player = new Player(heroName, 1, 100, 100, 10, 0, true);
        }
    }
}