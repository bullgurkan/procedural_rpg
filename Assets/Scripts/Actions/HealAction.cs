using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Effect;
using static EntityLiving;

public class HealAction : Action
{

    Stat healingScaling;
    public HealAction(Effect source, Stat healingScaling) : base(source)
    {
        this.healingScaling = healingScaling;
    }
    public override void OnActivation(World world, EntityLiving caster, EntityLiving reciver, Position room, Position positionInRoom, List<EventType> usedEventTypes)
    {
        caster.Heal(world, reciver, caster.GetStat(healingScaling), usedEventTypes);
    }

    public override string ToString() => $"HealAction(Scaling:{healingScaling})";
}

