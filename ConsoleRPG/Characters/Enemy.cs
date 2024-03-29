﻿using System;
using ConsoleRPG.Items;
using static ConsoleRPG.BetterConsole;

namespace ConsoleRPG.Characters
{
    public class Enemy : Character
    {
        public Enemy(string name, int level, int health, int maxHealth, int attack, int defense, bool isPlayer, int stunnedForXTurns, Inventory inventory) : base(name, level, health, maxHealth, attack, defense, isPlayer, stunnedForXTurns, inventory)
        {
        }


        public override void BasicAttack(Character target)
        {
            target.Health -= Attack;
            Col($"{Name} attacked for {Attack} damage", "yellow");
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