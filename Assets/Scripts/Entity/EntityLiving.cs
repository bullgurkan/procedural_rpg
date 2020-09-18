using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Effect;

public abstract class EntityLiving : Entity, ITickable
{
    protected Dictionary<Stat, int> stats;
    Dictionary<Stat, int> baseStats;
    protected int health;
    List<Effect> effects;

    public EntityLiving(Position size, string spriteId, string name, Position? renderSize = null) : base(size, renderSize, spriteId, name)
    {
        stats = new Dictionary<Stat, int>();
        effects = new List<Effect>();
        baseStats = GenerateBaseStats();

        foreach (var stat in baseStats)
        {
            stats.Add(stat.Key, stat.Value);
        }

        health = stats[Stat.MAX_HEALTH];
    }
    public abstract Dictionary<Stat, int> GenerateBaseStats();
    public abstract void Tick(World world);

    public void Damage(int amount, Stat? resitanceStat, World world)
    {
        if (amount < 0)
        {
            Heal(-amount, world);
            return;
        }
        int resistance = resitanceStat != null ? stats[(Stat)resitanceStat] : 0;
        health -= (int)(amount * Math.Pow(0.5, (double)resistance/100));
        OnDamage(world);

        if (health <= 0)
        {
            OnDeath(world);
        }
    }

    public void AddEffect(Effect effectToPutOnHit)
    {
        effects.Add(effectToPutOnHit);
    }

    public virtual void RecalculateStats()
    {

        foreach (var stat in baseStats.Keys)
        {
            stats[stat] = baseStats[stat];
        }

        foreach (var effect in effects)
        {
            effect.ModifyStats(this);
        }

    }

    public void Heal(int amount, World world)
    {
        if (amount < 0)
        {
            Damage(-amount, Stat.LIGHT_RESITANCE, world);
            return;
        }
        health += amount;
        OnHeal(world);
    }

    private void TriggerEffectEvents(EventType e, World world)
    {
        foreach (Effect effect in effects)
        {
            effect.OnEvent(e, world, this, CurrentRoom, PositionInRoom);
        }
    }

    protected virtual void OnDamage(World world)
    { TriggerEffectEvents(EventType.ON_DAMAGE, world); }
    protected virtual void OnHeal(World world)
    { TriggerEffectEvents(EventType.ON_HEAL, world); }
    protected virtual void OnDeath(World world)
    { TriggerEffectEvents(EventType.ON_DEATH, world); }

    public enum Stat
    {
        MOVEMENT_SPEED, ATTACK_POWER, ATTACK_SPEED, ARMOR, FIRE_RESITANCE, WATER_RESITANCE, DARK_RESITANCE, EARTH_RESITANCE, LIGHT_RESITANCE, LUCK, LIFESTEAL, MAX_HEALTH
    }

}

