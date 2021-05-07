namespace ConsoleRPG.Characters
{
    public class Player: Character
    {
        public Player(string name, int level, int health, int maxHealth, int attack, int defence, bool isPlayer)
            : base(name, level, health, maxHealth, attack, defence, isPlayer)
        {
            
        }
    }
}