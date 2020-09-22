using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Effect;

public abstract class Action
{
    public abstract void OnActivation(World world, EntityLiving entityLiving, Position room, Position positionInRoom, Dictionary<EffectData, Object> effectData);
}

