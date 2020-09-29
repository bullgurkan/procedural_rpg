using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Effect;

public class AddEffectAction : Action
{
    public override void OnActivation(World world, EntityLiving caster, EntityLiving reciver, Position room, Position positionInRoom, Effect source, List<EventType> usedEventTypes)
    {
        Effect effect = new Effect(source);
        reciver.AddEffect(effect);
    }

    public override string ToString()
    {
        throw new NotImplementedException();
    }
}

