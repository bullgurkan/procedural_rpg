using System;
using System.Collections;
using System.Collections.Generic;

public class Character : EntityLiving
{
    Dictionary<Slot, Item> items;
    List<Effect> effects;

    Dictionary<Stat, float> baseStats;


    public enum Slot
    {
        Chest, Leg, Feet, Head, Ring, Amulet, Weapon, Cape
    }


    public Character(Position size, string spriteId) : base(size, spriteId)
    {
        stats = new Dictionary<Stat, float>();
        effects = new List<Effect>();
        items = new Dictionary<Slot, Item>();


        GenerateBaseStats();
        foreach (var stat in baseStats)
        {
            stats.Add(stat.Key, stat.Value);
        }
    }

    private void GenerateBaseStats()
    {
        baseStats = new Dictionary<Stat, float>();

        foreach (Stat stat in Enum.GetValues(typeof(Stat)))
        {
            baseStats.Add(stat, 1);
        }
        baseStats[Stat.AttackPower] = 10;

    }
    public void ChangeEquipment(Item item)
    {
        items[item.Slot] = item;
        RecalculateStats();
    }

    public void RecalculateStats()
    {

        foreach (var item in stats)
        {
            stats[item.Key] = baseStats[item.Key];
        }

        foreach (var item in items)
        {
            item.Value.ModifyStats(this);
        }

        foreach (var effect in effects)
        {
            effect.ModifyStats(this);
        }

    }

    public override void Tick(World world)
    {
        throw new System.NotImplementedException();
    }
}
