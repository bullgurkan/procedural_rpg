using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public abstract class EntityLiving : Entity, ITickable
{
    protected Dictionary<Stat, int> stats;
    protected int health;

    public EntityLiving(Position size, string spriteId, string name, Position? renderSize = null) : base(size, renderSize, spriteId, name)
    {

    }
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

    protected abstract void OnDamage(World world);
    protected abstract void OnHeal(World world);
    protected abstract void OnDeath(World world);

    public enum Stat
    {
        MOVEMENT_SPEED, ATTACK_POWER, ATTACK_SPEED, ARMOR, FIRE_RESITANCE, WATER_RESITANCE, DARK_RESITANCE, EARTH_RESITANCE, LIGHT_RESITANCE, LUCK, LIFESTEAL, MAX_HEALTH
    }

}

