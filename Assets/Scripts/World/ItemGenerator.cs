using System;
using static EntityLiving;
using static Character;
using System.Collections.Generic;

public class ItemGenerator
{
    Random random;
    int powerLevel, maxEffectLayers;
    List<Stat> offensiveStats, defenseiveStats;

    public ItemGenerator(Random random, int powerLevel, int maxEffectLayers)
    {
        this.random = random;
        this.powerLevel = powerLevel;
        this.maxEffectLayers = maxEffectLayers;
        offensiveStats = new List<Stat>()
        {
            Stat.ATTACK_POWER, Stat.ATTACK_SPEED, Stat.LIFESTEAL, Stat.MOVEMENT_SPEED, Stat.LUCK
        };
        defenseiveStats = new List<Stat>()
        {
            Stat.ARMOR, Stat.DARK_RESITANCE, Stat.EARTH_RESITANCE, Stat.FIRE_RESITANCE, Stat.LIGHT_RESITANCE, Stat.MAX_HEALTH
        };
    }
    public Item GenerateItem()
    {
        Enum.GetValues(typeof(Slot));
        Item item = new Item((Slot)random.Next(Enum.GetValues(typeof(Slot)).Length));

        switch (item.Slot)
        {
            case Slot.CHEST: GiveRandomDefensiveStats(item, 3); break;
            case Slot.LEG: GiveRandomDefensiveStats(item, 3); break;
            case Slot.FEET: GiveRandomDefensiveStats(item, 3); break;
            case Slot.HEAD: GiveRandomDefensiveStats(item, 3); break;
            case Slot.RING: GiveRandomOffensiveStats(item, 3); break;
            case Slot.AMULET: GiveRandomOffensiveStats(item, 3); break;
            case Slot.WEAPON: item.stats[Stat.ATTACK_POWER] += random.Next(powerLevel) * 4; GiveRandomOffensiveStats(item, 1); break;
            case Slot.CAPE: GiveRandomOffensiveStats(item, 3); break;
            default: break;
        }


        return item;
    }

    private void GiveRandomStats(Item item, int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            AddStat(item, (Stat)random.Next(Enum.GetValues(typeof(Stat)).Length), random.Next(powerLevel));
        }
    }

    private void GiveRandomDefensiveStats(Item item, int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            AddStat(item, defenseiveStats[random.Next(defenseiveStats.Count)], random.Next(powerLevel));
        }
    }

    private void GiveRandomOffensiveStats(Item item, int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            AddStat(item, offensiveStats[random.Next(offensiveStats.Count)], random.Next(powerLevel));
        }
    }

    private void AddStat(Item item, Stat stat, int amount)
    {
        if (item.stats.ContainsKey(stat))
            item.stats[stat] += amount;
        else
            item.stats.Add(stat, amount);
    }
}

