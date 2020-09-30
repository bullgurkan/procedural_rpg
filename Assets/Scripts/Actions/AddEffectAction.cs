using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Effect;

public class AddEffectAction : Action
{
    Effect effectToApply;
    public AddEffectAction(Effect source, Effect effectToApply) : base(source)
    {
        this.effectToApply = effectToApply;
    }
    public override void OnActivation(World world, EntityLiving caster, EntityLiving reciver, Position room, Position positionInRoom, List<EventType> usedEventTypes)
    {
        reciver.AddEffect(effectToApply);
    }

    public override string ToString()
    {
        throw new NotImplementedException();
    }
}

