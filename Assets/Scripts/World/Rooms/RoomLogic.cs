using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public abstract class RoomLogic
{
    public abstract void OnRoomEnter(World world, Character player);

    public void Generate(World world, Random rand, List<Room> emptyRooms, int difficulty)
    {
        OnGeneration(world, PickRoomPosition(world, rand, emptyRooms), difficulty);
    }
    protected abstract void OnGeneration(World world, Room roomToGenerate, int difficulty);
    protected virtual Room PickRoomPosition(World world, Random rand, List<Room> emptyRooms)
    {
        int index = rand.Next(emptyRooms.Count);
        Room room = emptyRooms[index];
        emptyRooms.RemoveAt(index);
        return room;
    }
}

