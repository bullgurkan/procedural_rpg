using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Room;

public abstract class RoomLogic
{

    public abstract void OnRoomEnter(World world, Room room, Character player);

    public void Generate(World world, Random rand, List<Room> emptyRooms, WorldGenerator worldGen, int difficulty)
    {
        Room room = PickRoomPosition(world, rand, emptyRooms);
        OnGeneration(world, room, worldGen, difficulty);
        room.roomLogic = this;
    }
    protected abstract void OnGeneration(World world, Room roomToGenerate, WorldGenerator worldGen, int difficulty);
    protected virtual Room PickRoomPosition(World world, Random rand, List<Room> emptyRooms)
    {
        int index = rand.Next(emptyRooms.Count);
        Room room = emptyRooms[index];
        emptyRooms.RemoveAt(index);
        return room;
    }

    public abstract RoomLogic CreateNew();
}

