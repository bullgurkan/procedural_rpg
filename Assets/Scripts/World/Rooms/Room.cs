using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


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
    public void AddRoomLock(World world, Position roomThatLocked)
    {
        roomLocks.Add(roomThatLocked);
        if(roomLocks.Count > 0)
            Lock(world);
    }

    public void RemoveRoomLock(World world,  Position roomThatLocked)
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
}

