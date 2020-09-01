using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public abstract class EntityLiving : Entity, ITickable
{
    protected Dictionary<Stat, float> stats;

    public abstract void Tick(World world);

    public enum Stat
    {
        MovementSpeed, AttackPower, AttackSpeed, Armor, FireResistance, WaterResistance, DarkResistance, EarthResistance, LightResistance, Luck, LifeSteal
    }
}

