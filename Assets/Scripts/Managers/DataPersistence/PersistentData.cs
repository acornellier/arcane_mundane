using System;
using System.Collections.Generic;

[Serializable]
public class PersistentData
{
    public Player.Data player;
    public ItemStack.Data[] stacks = Array.Empty<ItemStack.Data>();
    public QuestPersistence.Data questPersistence = new();
    public Dictionary<string, bool> bools = new();
}