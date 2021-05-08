namespace ConsoleRPG.Characters
{
    public class Monster : Enemy
    {
        public Monster(string name, int level, int health, int maxHealth, int attack, int defence, bool isPlayer, int stunnedForXTurns) : base(name, level, health, maxHealth, attack, defence, isPlayer, stunnedForXTurns)
        {
        }
        
    }
}