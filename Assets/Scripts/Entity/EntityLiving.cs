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

    public int Damage(World world, EntityLiving caster, int amount, Stat? resitanceStat, List<EventType> usedEventTypes)
    { 
        if (amount < 0)
        {
            
            return -Heal(world, caster, -amount, usedEventTypes);
        }
        int resistance = resitanceStat != null ? stats[(Stat)resitanceStat] : 0;
        int postMitigationDamage = (int)(amount * Math.Pow(0.5, (double)resistance / 100));
        health -= postMitigationDamage;
        OnDamage(world, caster, usedEventTypes);

        if (health <= 0)
        {
            OnDeath(world, caster, usedEventTypes);
        }
        return postMitigationDamage;
    }

    public void AddEffect(Effect effect)
    {
        effects.Add(effect);
        if(effects.Find(x => x.source == effect.source) == null)
        {
            RecalculateStats();
            OnStatChange();
        }
        
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

    public int Heal(World world, EntityLiving caster, int amount, List<EventType> usedEventTypes)
    {
        if (amount < 0)
        {
            
            return -Damage(world, caster, -amount, Stat.LIGHT_RESITANCE, usedEventTypes);
        }
        health += amount;
        if (health > GetStat(Stat.MAX_HEALTH))
            health = GetStat(Stat.MAX_HEALTH);
        OnHeal(world, caster, usedEventTypes);
        return amount;
    }

    private void TriggerEffectEvents(EventType e, EntityLiving causer, World world, List<EventType> usedEventTypes)
    {
        foreach (Effect effect in effects)
        {
            effect.OnEvent(e, world, this, causer, causer.CurrentRoom, causer.PositionInRoom, usedEventTypes);
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


    protected virtual void OnDamage(World world, EntityLiving causer, List<EventType> usedEventTypes)
    { TriggerEffectEvents(EventType.ON_DAMAGE, causer, world, usedEventTypes); }
    protected virtual void OnHeal(World world, EntityLiving causer, List<EventType> usedEventTypes)
    { TriggerEffectEvents(EventType.ON_HEAL, causer, world, usedEventTypes); }
    protected virtual void OnDeath(World world, EntityLiving causer, List<EventType> usedEventTypes)
    { TriggerEffectEvents(EventType.ON_DEATH, causer, world, usedEventTypes); }
    public virtual void OnEnemyKill(World world, EntityLiving causer, List<EventType> usedEventTypes)
    { TriggerEffectEvents(EventType.ON_ENEMY_KILL, causer, world, usedEventTypes); }
    public virtual void OnEnemyHit(World world, EntityLiving causer, List<EventType> usedEventTypes)
    { TriggerEffectEvents(EventType.ON_ENEMY_HIT, causer, world, usedEventTypes); }
    protected virtual void OnStatChange() {}

    public enum Stat
    {
        MOVEMENT_SPEED, ATTACK_POWER, ATTACK_SPEED, ARMOR, FIRE_RESITANCE, WATER_RESITANCE, DARK_RESITANCE, EARTH_RESITANCE, LIGHT_RESITANCE, LUCK, LIFESTEAL, MAX_HEALTH
    }

}

