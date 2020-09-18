using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static EntityLiving;

public class UnlockRoomAction : Action
{

    RoomLogicTrapAmbush ambushLogic;
    public UnlockRoomAction(RoomLogicTrapAmbush ambushLogic)
    {
        this.ambushLogic = ambushLogic;
    }
    public override void OnActivation(World world, EntityLiving entityLiving, Position room, Position positionInRoom)
    {
        ambushLogic.EntityDied(world, entityLiving as Enemy);
    }
}

