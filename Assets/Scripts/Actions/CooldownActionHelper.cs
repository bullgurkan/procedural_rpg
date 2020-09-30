using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Effect;


public class CooldownHelperAction : Action
{
    public CooldownHelperAction(Effect source) : base(source)
    {

    }
    public override void OnActivation(World world, EntityLiving caster, EntityLiving reciver, Position room, Position positionInRoom, List<EventType> usedEventTypes)
    {
        if (source.effectData.ContainsKey(EffectData.COOLDOWN))
        {
            if ((int)source.effectData[EffectData.COOLDOWN] > 0)
                source.effectData[EffectData.COOLDOWN] = (int)source.effectData[EffectData.COOLDOWN] - 1;
        }
    }

    public override string ToString() => "CooldownHelperAction";
}

