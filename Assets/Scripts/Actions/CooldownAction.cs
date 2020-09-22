using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Effect;


public class CooldownHelperAction : Action
{
    public override void OnActivation(World world, EntityLiving entityLiving, Position room, Position positionInRoom, Dictionary<Effect.EffectData, object> effectData)
    {
        if (effectData.ContainsKey(EffectData.COOLDOWN))
        {
            if ((int)effectData[EffectData.COOLDOWN] > 0)
                effectData[EffectData.COOLDOWN] = (int)effectData[EffectData.COOLDOWN] - 1;

        }

    }
}

