using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Entity
{
    public Position CurrentRoom { get; private set; }

    //size and pos in room will be scaled by world accuracy
    public Position PositionInRoom { get; private set; }
    public Position Size { get; private set; }
    public int Id { get; set; }
    public int SpriteId;

    public Entity(Position unScaledSize)
    {
        Size = unScaledSize;
    }

    public void MoveInLine(Position direction, int distance, World world)
    {
        if (world.BoxCastinLine(CurrentRoom, PositionInRoom, direction, distance, Size).Length == 0)
        {
            SetPositionInRoom(PositionInRoom/world.Accuracy + direction * distance, world, true);
        }
        else
        {
            Console.WriteLine("fail");
        }
    }
    public void SetPositionInRoom(Position unScaledPos, World world, bool force)
    {
        Position scaledPos = unScaledPos * world.Accuracy;
        if (force || world.BoxCast(CurrentRoom, scaledPos, Size) == null)
        {
            PositionInRoom = scaledPos;

            Position roomDelta = world.RoomDeltaIfOutsideOfRoom(PositionInRoom);
            if (roomDelta != Position.zero)
                SetCurrentRoom(CurrentRoom + roomDelta, world);
        }
    }

    public void SetCurrentRoom(Position room, World world)
    {
        world.GetRoom(CurrentRoom).entities.Remove(this);
        world.GetRoom(room).entities.Add(this);
        PositionInRoom = world.ConvertPositionBetweenRooms(PositionInRoom, CurrentRoom, room);
        CurrentRoom = room;
    }

    public void SetSize(Position size, World world, bool force)
    {
        if (force || world.BoxCast(CurrentRoom, PositionInRoom, size) == null)
        {
            Size = size * world.Accuracy;
        }
    }



}

