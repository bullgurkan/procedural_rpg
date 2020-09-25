using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Effect;

public abstract class EntityLiving : Entity, ITickable
{
    private Dictionary<Stat, int> stats;
    protected Dictionary<Stat, int> baseStats;
    protected int health;
    List<Effect> effects;

    public EntityLiving(Position size, string spriteId, string name, Dictionary<Stat, int> baseStats, Position? renderSize = null, TagType tag = TagType.ENEMY) : base(size, renderSize, spriteId, name, tag: tag)
    {
        stats = new Dictionary<Stat, int>();
        effects = new List<Effect>();
        this.baseStats = baseStats;

        foreach (var stat in baseStats)
        {
            stats.Add(stat.Key, stat.Value);
        }

        health = stats[Stat.MAX_HEALTH];
    }
    public abstract void Tick(World world);

    public int Damage(World world, EntityLiving caster, int amount, Stat? resitanceStat)
    {
        if (amount < 0)
        {
            
            return -Heal(world, caster, -amount);
        }
        int resistance = resitanceStat != null ? stats[(Stat)resitanceStat] : 0;
        int postMitigationDamage = (int)(amount * Math.Pow(0.5, (double)resistance / 100));
        health -= postMitigationDamage;
        OnDamage(world, caster);

        if (health <= 0)
        {
            OnDeath(world, caster);
        }
        return postMitigationDamage;
    }

    public void AddEffect(Effect effect)
    {
        effects.Add(effect);
        RecalculateStats();
        OnStatChange();
    }

    public virtual void RecalculateStats()
    {
        int prevMaxHealth = stats[Stat.MAX_HEALTH];
        foreach (var stat in baseStats.Keys)
        {
            stats[stat] = baseStats[stat];
        }

        foreach (var effect in effects)
        {
            effect.ModifyStats(this);
        }

        health += stats[Stat.MAX_HEALTH] - prevMaxHealth;
    }

    public int Heal(World world, EntityLiving caster, int amount)
    {
        if (amount < 0)
        {
            
            return -Damage(world, caster, -amount, Stat.LIGHT_RESITANCE);
        }
        health += amount;
        OnHeal(world, caster);
        return amount;
    }

    private void TriggerEffectEvents(EventType e, EntityLiving causer, World world)
    {
        foreach (Effect effect in effects)
        {
            effect.OnEvent(e, world, this, causer, CurrentRoom, PositionInRoom);
        }
    }

    public int GetStat(Stat stat)
    {
        if(stats.ContainsKey(stat))
            return stats[stat];
        return 0;
    }
    public void SetStat(Stat stat, int amount)
    {
        if (stats.ContainsKey(stat))
            stats[stat] = amount;
        else
            stats.Add(stat, amount);
    }


    protected virtual void OnDamage(World world, EntityLiving causer)
    { TriggerEffectEvents(EventType.ON_DAMAGE, causer, world); }
    protected virtual void OnHeal(World world, EntityLiving causer)
    { TriggerEffectEvents(EventType.ON_HEAL, causer, world); }
    protected virtual void OnDeath(World world, EntityLiving causer)
    { TriggerEffectEvents(EventType.ON_DEATH, causer, world); }
    protected virtual void OnStatChange() {}

    public enum Stat
    {
        MOVEMENT_SPEED, ATTACK_POWER, ATTACK_SPEED, ARMOR, FIRE_RESITANCE, WATER_RESITANCE, DARK_RESITANCE, EARTH_RESITANCE, LIGHT_RESITANCE, LUCK, LIFESTEAL, MAX_HEALTH
    }

}

