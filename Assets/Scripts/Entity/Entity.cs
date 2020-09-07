using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;

public class Entity
{
    public Position CurrentRoom { get; private set; }

    //size and pos in room will be scaled by world accuracy
    public Position PositionInRoom { get; private set; }
    public Position Size { get; private set; }

    public Position RenderSize { get; private set; }
    public int Id { get; set; }
    public string SpriteId { get; set; }

    public string Name { get; set; }

    public Entity(Position size, Position? renderSize = null, string spriteId = null, string name = null)
    {
        Size = size;
        SpriteId = spriteId;
        RenderSize = renderSize ?? size;
        Name = name;
    }

    public void MoveInLine(Position direction, int distance, World world, bool shouldSlide)
    {

        if (world.BoxCastinLine(Id, CurrentRoom, PositionInRoom, direction, distance, Size).Length == 0)
        {
            SetPositionInRoom(PositionInRoom + direction * distance, world, true);
            world.WorldRenderer?.UpdateEntityPosition(this, world);
        }
        else
        {
            if (shouldSlide && direction.x != 0 && direction.y != 0)
            {

                Position halfDir = new Position(direction.x, 0);
                if (world.BoxCastinLine(Id, CurrentRoom, PositionInRoom, halfDir, distance, Size).Length == 0)
                {
                    SetPositionInRoom(PositionInRoom + halfDir * distance, world, true);
                    world.WorldRenderer?.UpdateEntityPosition(this, world);
                }

                halfDir = new Position(0, direction.y);
                if (world.BoxCastinLine(Id, CurrentRoom, PositionInRoom, halfDir, distance, Size).Length == 0)
                {
                    SetPositionInRoom(PositionInRoom + halfDir * distance, world, true);
                    world.WorldRenderer?.UpdateEntityPosition(this, world);
                }
            }
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

