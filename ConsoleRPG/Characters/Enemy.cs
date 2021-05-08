using System;
using ConsoleRPG.Items;

namespace ConsoleRPG.Characters
{
    public class Enemy : Character
    {
        public Enemy(string name, int level, int health, int maxHealth, int attack, int defence, bool isPlayer,
            int stunnedForXTurns) 
            : base(name, level, health, maxHealth, attack, defence, isPlayer, stunnedForXTurns, new Inventory())
        {
        }
        
        
        public override void BasicAttack(Character target)
        {
            target.Health -= Attack;
            Console.WriteLine($"{Name} attacked for {Attack} damage");
        }

        public override void Fireball(Player player, Character target, Random rnd)
        {
            throw new NotImplementedException();
        }

        public override void Freeze(Player player, Character target, Random rnd)
        {
            throw new NotImplementedException();
        }

    }
}