using System;
using static EntityLiving;
using static Character;
using System.Collections.Generic;
using static Effect;
using Unity.Collections;

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
    private int GiveRandomAction(Effect item, EventType? eventType, int itemEffectBudget, bool isPositive = true, bool isAffectingGoodEntity = true)
    {
        EventType eventTypeSafe = eventType ?? (EventType)random.Next(1, Enum.GetValues(typeof(EventType)).Length);
        Action action = null;
        if (eventTypeSafe == EventType.ON_TICK)
        {
            int cooldown = random.Next(1, 1000);
            itemEffectBudget += itemEffectBudget * (cooldown - 500) / 500;
            action = new CooldownAction(item, GenerateRandomAction(item, 0, ref itemEffectBudget, isPositive, isAffectingGoodEntity), item, cooldown);
        }

        else if (eventTypeSafe == EventType.ON_ACTIVATION)
            action = new CooldownAction(item, GenerateRandomAction(item, 0, ref itemEffectBudget, isPositive, isAffectingGoodEntity), item);
        else
            action = GenerateRandomAction(item, 0, ref itemEffectBudget, isPositive, isAffectingGoodEntity);
        if (item.actions.ContainsKey(eventTypeSafe))
        {
            item.actions[eventTypeSafe] = new MultiAction(item, action, item.actions[eventTypeSafe]);
        }
        else
            item.actions.Add(eventTypeSafe, action);


        return itemEffectBudget;
    }

    private Action GenerateRandomAction(Effect item, int depth, ref int itemActionBudget, bool isPositive, bool isAffectingGoodEntity)
    {
        switch (random.Next(5))
        {
            case 0:
                itemActionBudget -= powerLevel / 2 * (isPositive ? 1 : -1);
                if (random.Next(4) == 0)
                    return new DamageAction(item, offensiveStats[random.Next(offensiveStats.Count)], resistanceStats[random.Next(resistanceStats.Count)], !isAffectingGoodEntity);
                else
                    return new DamageAction(item, Stat.ATTACK_POWER, resistanceStats[random.Next(resistanceStats.Count)], !isAffectingGoodEntity);
            case 1:
                if (depth < maxDepth)
                {
                    itemActionBudget += itemActionBudget/2 < 300 ? 300 : itemActionBudget/2;
                    int speed = random.Next(1, itemActionBudget > 200 ? 200 : itemActionBudget);
                    itemActionBudget -= speed;
                    int size = random.Next(100, (100 + itemActionBudget > 200 ? 200 : itemActionBudget) * 2) * 2;
                    itemActionBudget -= size / 100;
                    return new SpawnProjectileAction(item, Position.one * size, speed * 10, GenerateRandomAction(item, depth + 1, ref itemActionBudget, isPositive, !isAffectingGoodEntity));
                }
                goto case 0;
            case 2:
                {
                    Effect effect = new Effect(item);
                    bool shouldEffectBePositive = random.Next(2) == 0;
                    if (random.Next(0, 2) == 0)
                    {
                        int statPower = random.Next(1, itemActionBudget);
                        itemActionBudget -= statPower * (shouldEffectBePositive ? 1 : -1);
                        AddStat(effect, (Stat)random.Next(Enum.GetValues(typeof(Stat)).Length), statPower * (isAffectingGoodEntity ? 1 : -1) * (shouldEffectBePositive ? 1 : -1));
                    }
                    else
                    {
                        itemActionBudget = GiveRandomAction(effect, null, itemActionBudget, shouldEffectBePositive, shouldEffectBePositive ? isAffectingGoodEntity : !isAffectingGoodEntity);
                    }
                       
                    return new AddEffectAction(item, effect, shouldEffectBePositive);
                }

            case 3:
                itemActionBudget -= powerLevel / 2 * (isPositive ? 1 : -1);
                return new HealAction(item, (Stat)random.Next(Enum.GetValues(typeof(Stat)).Length), isAffectingGoodEntity);
            case 4:
                return new MultiAction(item, GenerateRandomAction(item, depth + 1, ref itemActionBudget, isAffectingGoodEntity, isPositive), GenerateRandomAction(item, depth + 1, ref itemActionBudget, isPositive, isAffectingGoodEntity));

            default:
                goto case 0;
        }
    }

    private void AddStat(Effect item, Stat stat, int amount)
    {
        if (item.stats.ContainsKey(stat))
            item.stats[stat] += amount;
        else
            item.stats.Add(stat, amount);
    }
}

