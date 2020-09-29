using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Effect;

public abstract class Action
{
    public abstract void OnActivation(World world, EntityLiving caster, EntityLiving reciver, Position room, Position positionInRoom, Effect source, List<EventType> usedEventTypes);
    public abstract override string ToString();
}

