using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class StartRoomLogic : RoomLogic
{
    public override RoomLogic CreateNew()
    {
        return new StartRoomLogic();
    }

    public override void OnRoomEnter(World world, Room room, Character player)
    {
       
    }

    protected override void OnGeneration(World world, Room room, WorldGenerator worldGen, int difficulty)
    {
        int currentPos = 0;
        foreach (Character player in world.players)
        {
            world.AddEntity(player, room.RoomPosition, Position.right * currentPos, true);
            currentPos += player.Size.x;
        }
        
    }


    protected override Room PickRoomPosition(World world, Random rand, List<Room> emptyRooms)
    {
        Room roomToPick = null;

        foreach (Room room in emptyRooms)
        {

            if (room.distanceFromStart == 0)
            {
                roomToPick = room;
                break;
            }

        }

        emptyRooms.Remove(roomToPick);
        return roomToPick;
    }
}

