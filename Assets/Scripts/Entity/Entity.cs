using System;
using UnityEngine;
using static WorldRenderer;

public class Entity
{
    public Position CurrentRoom { get; private set; }

    //size and pos in room will be scaled by world accuracy
    public Position PositionInRoom { get; private set; }
    public Position Size { get; private set; }

    public Position RenderSize { get; private set; }
    public Position TileSize { get; private set; }

    private int id = -1;
    public int Id { get { return id; } set {if(id == -1) id = value; } }
    public string SpriteId { get; private set; }

    public string Name { get; private set; }

    public RenderPriority RenderPrio { get; private set; }

    public Entity(Position size, Position? renderSize = null, string spriteId = null, string name = null, Position? tileSize = null, RenderPriority renderPriority = RenderPriority.DEFAULT)
    {
        Size = size;
        SpriteId = spriteId;
        RenderSize = renderSize ?? size;
        Name = name;
        TileSize = tileSize ?? Position.zero;
        RenderPrio = renderPriority;
    }

    public int MoveInLine(Position direction, int distance, World world, bool shouldSlide)
    {

        if (world.BoxCastinLine(Id, this, direction, distance).Length == 0)
        {
            SetPositionInRoom(PositionInRoom + direction * distance, world, true);
            world.WorldRenderer?.UpdateEntityPosition(this, world);
            return distance;
        }
        else
        {
            
            int distToMove = 0;
            for (int i = 2; i < distance; i+=2)
            {
                if (world.BoxCastinLine(Id, this, direction, i).Length == 0)
                {
                    distToMove = i;
                }
            }
            SetPositionInRoom(PositionInRoom + direction * distToMove, world, true);

            int distanceLeft = distance - distToMove;

            if (shouldSlide && direction.x != 0 && direction.y != 0)
            {
                distanceLeft -=  MoveInLine(new Position(direction.x, 0), distanceLeft, world, false);
                distanceLeft -= MoveInLine(new Position(0, direction.y), distanceLeft, world, false);
            }


            world.WorldRenderer?.UpdateEntityPosition(this, world);

            return distance - distanceLeft;
        }
    }
    public void SetPositionInRoom(Position pos, World world, bool force)
    {
        Position scaledPos = pos;
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
        if (world.GetRoom(room) != null)
        {
            world.GetRoom(CurrentRoom).entities.Remove(this);
            world.GetRoom(room).entities.Add(this);
            PositionInRoom = world.ConvertPositionBetweenRooms(PositionInRoom, CurrentRoom, room);
            CurrentRoom = room;
        }

    }

    public void SetSize(Position size, World world, bool force)
    {
        if (force || world.BoxCast(CurrentRoom, PositionInRoom, size) == null)
        {
            if (size.x % 2 == 1 || size.x % 2 == 1)
                throw new Exception("Size has to be dividable by 2");
            Size = size;
        }
    }



}

