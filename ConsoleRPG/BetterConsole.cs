using System;

namespace ConsoleRPG
{
    public static class BetterConsole
    {
        
        // Setup Console
        public static void SetupConsole()
        {
            Console.WindowWidth = 120;
            Console.WindowHeight = 40;
            //Console.ForegroundColor = ConsoleColor.Yellow;


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
        public static void Col(string input,string colorName)
        {
            colorName = colorName.ToLower();
            Console.ForegroundColor = colorName switch
            {
                    "yellow" => ConsoleColor.Yellow,
                    "darkyellow"=>ConsoleColor.DarkYellow,
                    "red" => ConsoleColor.Red,
                    "darkgreen" => ConsoleColor.DarkGreen,
                    "blue" => ConsoleColor.Blue,
                    "cyan" => ConsoleColor.Cyan,
                    "white" => ConsoleColor.White,
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
            colorName = colorName.ToLower();

            Console.ForegroundColor = colorName switch
            {
                "yellow" => ConsoleColor.Yellow,
                "darkyellow"=>ConsoleColor.DarkYellow,
                "red" => ConsoleColor.Red,
                "darkgreen" => ConsoleColor.DarkGreen,
                "blue" => ConsoleColor.Blue,
                "cyan" => ConsoleColor.Cyan,
                "white" => ConsoleColor.White,
                _ => Console.ForegroundColor
                    
            };
            Console.WriteLine(input.PadLeft(spaces));
            Console.ResetColor();
            //Console.ForegroundColor = ConsoleColor.Yellow;
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