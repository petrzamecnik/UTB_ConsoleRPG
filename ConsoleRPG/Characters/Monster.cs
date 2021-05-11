using ConsoleRPG.Items;

namespace ConsoleRPG.Characters
{
    public class Monster : Enemy
    {
        public Monster(string name, int level, int health, int maxHealth, int attack, int defense, bool isPlayer, int stunnedForXTurns, Inventory inventory) : base(name, level, health, maxHealth, attack, defense, isPlayer, stunnedForXTurns, inventory)
        {
        }
    }
}