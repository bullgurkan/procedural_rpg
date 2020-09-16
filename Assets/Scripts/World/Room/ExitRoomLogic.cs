using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class ExitRoomLogic : RoomLogic
{
    public override RoomLogic CreateNew()
    {
        return new ExitRoomLogic();
    }

    public override void OnRoomEnter(World world, Room room, Character player)
    {
        
    }

    protected override void OnGeneration(World world, Room room, int difficulty)
    {
        world.AddEntity(new Entity(Position.zero, Position.one * 500, null, "exit"), room.RoomPosition, Position.zero);

    }

    protected override Room PickRoomPosition(World world, Random rand, List<Room> emptyRooms)
    {
        int furthestDistanceFromStart = 0;
        Room roomToPick = null;

        foreach (Room room in emptyRooms)
        {


            if (room.distanceFromStart > furthestDistanceFromStart)
            {
                roomToPick = room;
                furthestDistanceFromStart = room.distanceFromStart;
            }
                
        }

        emptyRooms.Remove(roomToPick);
        return roomToPick;
    }
}

