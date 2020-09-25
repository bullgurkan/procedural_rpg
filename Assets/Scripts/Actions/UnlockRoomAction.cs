using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Effect;
using static EntityLiving;

public class UnlockRoomAction : Action
{

    RoomLogicTrapAmbush ambushLogic;
    public UnlockRoomAction(RoomLogicTrapAmbush ambushLogic)
    {
        this.ambushLogic = ambushLogic;
    }
    public override void OnActivation(World world, EntityLiving caster, EntityLiving reciver, Position room, Position positionInRoom, Dictionary<EffectData, Object> effectData)
    {
        ambushLogic.EntityDied(world, caster as Enemy);
    }

    public override string ToString() => $"UnlockRoomAction(LinkedRoomLogicToUnlock:{ambushLogic})";
}

