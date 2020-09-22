using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Effect;
using static EntityLiving;

public class DamageAction : Action
{
    public int amount;
    Stat resitance;
    public DamageAction(int amount, Stat resitance)
    {
        this.amount = amount;
        this.resitance = resitance;
    }
    public override void OnActivation(World world, EntityLiving entityLiving, Position room, Position positionInRoom, Dictionary<EffectData, Object> effectData)
    {
        entityLiving.Damage(amount, resitance, world);
    }
}

