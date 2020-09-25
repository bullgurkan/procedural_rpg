using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Room
{
    public List<int> entityIds;
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
    bool hasBeenEntered = false;


    public Room(Position roomPos)
    {
        entityIds = new List<int>();
        roomSides = new RoomSide[4];
        doors = new List<Entity>();
        roomLocks = new List<Position>();
        RoomPosition = roomPos;
    }

    public void AddDoor(Entity door, Position pos)
    {
        door.SetPositionInRoom(pos, null, true);
        door.SetCurrentRoom(RoomPosition, null);
        doors.Add(door);
    }
    private void AddRoomLock(World world, Position roomThatLocked)
    {
        roomLocks.Add(roomThatLocked);

        if (roomLocks.Count == 1)
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
            world.AddEntity(door, door.CurrentRoom, door.PositionInRoom);
        }
    }

    private void Unlock(World world)
    {
        foreach (Entity door in doors)
        {
            world.QueueEntityRemoval(door.Id);
        }
    }

    public void OnRoomEnter(World world, Character character)
    {

        if (!hasBeenEntered)
        {
            hasBeenEntered = true;
            roomLogic?.OnRoomEnter(world, this, character);
            for (int i = 0; i < roomSides.Length; i++)
            {
                if (roomSides[i] == RoomSide.OPEN)
                {
                    world.GetRoom(RoomPosition + Position.directions[i]).OnRoomEnter(world, character);
                }

            }
        }
        
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

