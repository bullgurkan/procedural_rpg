using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : EntityLiving
{
    Dictionary<Slot, Item> items;
    List<Effect> effects;

    Dictionary<Stat, float> baseStats;


    public enum Slot
    {
        Chest, Leg, Feet, Head, Ring, Amulet, Weapon, Cape
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
