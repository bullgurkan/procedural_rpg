using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Entity
{
    public Position CurrentRoom { get; private set; }

    //size and pos in room will be scaled by world accuracy
    public  Position PositionInRoom { get; private set; }
    public Position Size { get; private set; }
    public int Id { get; set; }
    public int SpriteId;


    public void MoveInLine(Position direction, int length, World world)
    {
        if (world.BoxCast(CurrentRoom, pos, Size) == null)
        {
            PositionInRoom = pos;
            Position roomDelta = world.RoomDeltaIfOutsideOfRoom(PositionInRoom);
            if (roomDelta != Position.zero)
                SetCurrentRoom(CurrentRoom + roomDelta, world);

        }
    }
    public void SetPositionInRoom(Position pos, World world, bool force)
    {
        if (force || world.BoxCast(CurrentRoom, pos, Size) == null)
        {
            PositionInRoom = pos;
            Position roomDelta = world.RoomDeltaIfOutsideOfRoom(PositionInRoom);
            if (roomDelta != Position.zero)
                SetCurrentRoom(CurrentRoom + roomDelta, world);

        }
    }

    public void SetCurrentRoom(Position room, World world)
    {
        world.GetRoom(room).entities.Add(this);
        world.GetRoom(CurrentRoom).entities.Remove(this);
        PositionInRoom = world.ConvertPositionBetweenRooms(PositionInRoom, CurrentRoom, room);
        CurrentRoom = room;
    }



}

