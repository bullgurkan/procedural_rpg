using System;
using static EntityLiving;
using static Character;
using System.Collections.Generic;
using static Effect;

public class ItemGenerator
{
    Random random;
    int powerLevel, maxDepth;
    List<Stat> offensiveStats, resistanceStats;

    public ItemGenerator(Random random, int powerLevel, int maxEffectLayers)
    {
        this.random = random;
        this.powerLevel = powerLevel;
        this.maxDepth = maxEffectLayers;
        offensiveStats = new List<Stat>()
        {
            Stat.ATTACK_POWER, Stat.ATTACK_SPEED, Stat.LIFESTEAL, Stat.MOVEMENT_SPEED, Stat.LUCK
        };
        resistanceStats = new List<Stat>()
        {
            Stat.ARMOR, Stat.DARK_RESITANCE, Stat.EARTH_RESITANCE, Stat.FIRE_RESITANCE, Stat.LIGHT_RESITANCE
        };
    }
    public Item GenerateItem()
    {
        int itemStatBudget = powerLevel;


        Item item = new Item((Slot)random.Next(Enum.GetValues(typeof(Slot)).Length));


        if (item.Slot == Slot.WEAPON)
            itemStatBudget = GiveRandomAction(item, EventType.ON_ACTIVATION, itemStatBudget);
        else //if(random.Next(0, 100/powerLevel) == 0)
            itemStatBudget = GiveRandomAction(item, null, itemStatBudget);



        switch (item.Slot)
        {
            case Slot.CHEST: GiveRandomDefensiveStats(item, 3, itemStatBudget); break;
            case Slot.LEG: GiveRandomDefensiveStats(item, 3, itemStatBudget); break;
            case Slot.FEET: GiveRandomDefensiveStats(item, 3, itemStatBudget); break;
            case Slot.HEAD: GiveRandomDefensiveStats(item, 3, itemStatBudget); break;
            case Slot.RING: GiveRandomOffensiveStats(item, 3, itemStatBudget); break;
            case Slot.AMULET: GiveRandomOffensiveStats(item, 3, itemStatBudget); break;
            case Slot.WEAPON: AddStat(item, Stat.ATTACK_POWER, itemStatBudget * 3 / 4); GiveRandomOffensiveStats(item, 1, itemStatBudget / 4); break;
            case Slot.CAPE: GiveRandomOffensiveStats(item, 3, itemStatBudget); break;
            default: break;
        }



        return item;
    }

    private void GiveRandomStats(Item item, int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            AddStat(item, (Stat)random.Next(Enum.GetValues(typeof(Stat)).Length), random.Next(1, powerLevel));
        }
    }

    private void GiveRandomDefensiveStats(Item item, int amount, int powerBudget)
    {
        for (int i = 0; i < amount; i++)
        {
            if (powerBudget < 1)
            {
                UnityEngine.Debug.Log(powerBudget);
                return;
            }
            int power = random.Next(1, powerBudget);

            if (i == amount - 1)
            {
                power = powerBudget;
            }

            powerBudget -= power;

            if (random.Next(6) == 0)
                AddStat(item, Stat.MAX_HEALTH, power);
            else
                AddStat(item, resistanceStats[random.Next(resistanceStats.Count)], power);
        }
    }

    private void GiveRandomOffensiveStats(Item item, int amount, int powerBudget)
    {
        for (int i = 0; i < amount; i++)
        {
            if (powerBudget < 1)
            {
                UnityEngine.Debug.Log(powerBudget);
                return;
            }

            int power = random.Next(1, powerBudget);

            if (i == amount - 1)
            {
                power = powerBudget;
            }

            powerBudget -= power;

            AddStat(item, offensiveStats[random.Next(offensiveStats.Count)], random.Next(1, power));
        }
    }
    private int GiveRandomAction(Item item, EventType? eventType, int itemEffectBudget)
    {
        EventType eventTypeSafe = eventType ?? (EventType)random.Next(1, Enum.GetValues(typeof(EventType)).Length);
        Action action = null;
        if (eventTypeSafe == EventType.ON_TICK)
        {
            int cooldown = random.Next(1, 1000);
            itemEffectBudget += itemEffectBudget * (cooldown - 500) / 100;
            action = new CooldownAction(GenerateRandomAction(0, ref itemEffectBudget), item, cooldown);
        }

        else if (eventTypeSafe == EventType.ON_ACTIVATION)
            action = new CooldownAction(GenerateRandomAction(0, ref itemEffectBudget), item);
        else
            action = GenerateRandomAction(0, ref itemEffectBudget);
        if (item.actions.ContainsKey(eventTypeSafe))
        {
            item.actions[eventTypeSafe] = new MultiAction(action, item.actions[eventTypeSafe]);
        }
        else
            item.actions.Add(eventTypeSafe, action);


        return itemEffectBudget;
    }

    private Action GenerateRandomAction(int depth, ref int itemActionBudget)
    {
        switch (random.Next(10))
        {
            case 0:
                if (depth > 0)
                {
                    if (random.Next(4) == 0)
                        return new DamageAction(offensiveStats[random.Next(offensiveStats.Count)], resistanceStats[random.Next(resistanceStats.Count)]);
                    else
                        return new DamageAction(Stat.ATTACK_POWER, resistanceStats[random.Next(resistanceStats.Count)]);
                }
                else
                {
                    goto case 1;
                }
            case 1:
                if (depth < maxDepth)
                {
                    itemActionBudget -= itemActionBudget / 10;
                    return new SpawnProjectileAction(Position.one * random.Next(100, (100 + powerLevel * 5) * 2) * 2, random.Next(1, powerLevel) * 10, GenerateRandomAction(depth + 1, ref itemActionBudget));
                }

                goto case 0;
            default:
                goto case 0;
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

