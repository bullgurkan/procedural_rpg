using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Effect;
using static EntityLiving;

public class DamageAction : Action
{

    Stat damageScaling, resitance;
    public DamageAction(Stat damageScaling, Stat resitance)
    {
        this.damageScaling = damageScaling;
        this.resitance = resitance;
    }
    public override void OnActivation(World world, EntityLiving caster, EntityLiving reciver, Position room, Position positionInRoom, Dictionary<EffectData, Object> effectData)
    {
        caster.Heal(world, caster, reciver.Damage(world, caster, caster.GetStat(damageScaling), resitance) * 100/caster.GetStat(Stat.LIFESTEAL));
    }

    public override string ToString() => $"DamageAction(Scaling:{damageScaling}, AttackType:{resitance})";
}

