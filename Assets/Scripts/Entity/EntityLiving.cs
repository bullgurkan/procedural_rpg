using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public abstract class EntityLiving : Entity, ITickable
{
    protected Dictionary<Stat, float> stats;

    public EntityLiving(Position size, string spriteId) : base(size)
    {

    }
    public abstract void Tick(World world);

    public enum Stat
    {
        MOVEMENT_SPEED, ATTACK_POWER, ATTACK_SPEED, ARMOR, FIRE_RESITANCE, WATER_RESITANCE, DARK_RESITANCE, EARTH_RESITANCE, LIGHT_RESITANCE, LUCK, LIFESTEAL
    }
}

