using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Room
{
    public List<Entity> entities;
    public enum RoomSide
    {
        CLOSED, DOOR, OPEN
    }

    //right, left, up, down (same order as Position.directions)
    public RoomSide[] roomSides;
    public int distanceFromStart;
    public RoomLogic roomLogic;

    public Position RoomPosition { get; set; }

    private List<Entity> doors;
    private List<Position> roomLocks;




    public Room(Position roomPos)
    {
        entities = new List<Entity>();
        roomSides = new RoomSide[4];
        doors = new List<Entity>();
        roomLocks = new List<Position>();
        RoomPosition = roomPos;
    }

    public void AddDoor(Entity door, Position pos)
    {
        door.SetPositionInRoom(pos, null, true);
        doors.Add(door);
    }
    private void AddRoomLock(World world, Position roomThatLocked)
    {
        roomLocks.Add(roomThatLocked);
        if(roomLocks.Count == 1)
            Lock(world);
    }

    private void RemoveRoomLock(World world,  Position roomThatLocked)
    {
        roomLocks.Remove(roomThatLocked);
        if (roomLocks.Count == 0)
            Unlock(world);
    }


    private void Lock(World world)
    {
        foreach (Entity door in doors)
        {
            world.AddEntity(door, RoomPosition, door.PositionInRoom);
        }
    }

    private void Unlock(World world)
    {
        foreach (Entity door in doors)
        {
            world.RemoveEntity(door.Id);
        }
    }

    public void OnRoomEnter(World world, Character character)
    {
        roomLogic?.OnRoomEnter(world, this, character);
    }

    public void AddRoomLockToMultRoom(World world, Position roomThatLocked)
    {
        if (!roomLocks.Contains(roomThatLocked))
        {
            AddRoomLock(world, roomThatLocked);
            for (int i = 0; i < roomSides.Length; i++)
            {
                if (roomSides[i] == RoomSide.OPEN)
                    world.GetRoom(RoomPosition + Position.directions[i]).AddRoomLockToMultRoom(world, roomThatLocked);
            }
        }
        
    }

    public void RemoveRoomLockFromMultRoom(World world, Position roomThatLocked)
    {
        if (roomLocks.Contains(roomThatLocked))
        {
            RemoveRoomLock(world, roomThatLocked);
            for (int i = 0; i < roomSides.Length; i++)
            {
                if (roomSides[i] == RoomSide.OPEN)
                    world.GetRoom(RoomPosition + Position.directions[i]).RemoveRoomLockFromMultRoom(world, roomThatLocked);
            }
        }
    }
}

