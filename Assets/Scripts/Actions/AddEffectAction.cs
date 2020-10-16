using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Effect;

public class AddEffectAction : Action
{
    Effect effectToApply;
    bool applyToCaster;
    public AddEffectAction(Effect source, Effect effectToApply, bool applyToCaster) : base(source)
    {
        this.effectToApply = effectToApply;
        this.applyToCaster = applyToCaster;
    }
    public override void OnActivation(World world, EntityLiving caster, EntityLiving reciver, Position room, Position positionInRoom, List<EventType> usedEventTypes)
    {
        if (applyToCaster)
            reciver = caster;
        reciver.AddEffect(effectToApply, world.WorldRenderer);
    }

    public override string ToString() => $"AddEffectAction(EffectToApply:{effectToApply}, ApplyToCaster:{applyToCaster})";
}


