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
    bool healCaster;
    public HealAction(Effect source, Stat healingScaling, bool healCaster = false) : base(source)
    {
        this.healingScaling = healingScaling;
        this.healCaster = healCaster;
    }
    public override void OnActivation(World world, EntityLiving caster, EntityLiving reciver, Position room, Position positionInRoom, List<EventType> usedEventTypes)
    {
        if (healCaster)
            reciver = caster;
        reciver.Heal(world, caster, caster.GetStat(healingScaling), usedEventTypes);
    }

    public override string ToString() => $"HealAction(Scaling:{healingScaling}, HealCaster:{healCaster})";
}

