﻿using System;
using System.Collections.Generic;

namespace ConsoleRPG.Items
{
    [Serializable]
    public class Inventory
    {
        
        public List<Item> Items { get; set; }

        public Inventory(List<Item> items)
        {
            Items = items;
        }

        public void AddItem(Item item)
        {
            Items?.Add(item);
        }
        
    }
    
}