using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Effect;

public abstract class Action
{
    protected Effect source;

    public Action(Effect source)
    {
        this.source = source;
    }
    public abstract void OnActivation(World world, EntityLiving caster, EntityLiving reciver, Position room, Position positionInRoom, List<EventType> usedEventTypes);
    public abstract override string ToString();
}

