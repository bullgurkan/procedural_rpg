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
    bool damageCaster;
    public DamageAction(Effect source, Stat damageScaling, Stat resitance, bool damageCaster = false) : base(source)
    {
        this.damageScaling = damageScaling;
        this.resitance = resitance;
    }
    public override void OnActivation(World world, EntityLiving caster, EntityLiving reciver, Position room, Position positionInRoom, List<EventType> usedEventTypes)
    {
        if (damageCaster)
            reciver = caster;
        if (reciver != null)
        {
            int lifestealAmount = reciver.Damage(world, caster, caster.GetStat(damageScaling), resitance, usedEventTypes) * caster.GetStat(Stat.LIFESTEAL) / 100;
            if (lifestealAmount > 0)
                caster.Heal(world, reciver, lifestealAmount, usedEventTypes);
        }

    }

    public override string ToString() => $"DamageAction(Scaling:{damageScaling}, AttackType:{resitance}, DamageCaster:{damageCaster})";

}

