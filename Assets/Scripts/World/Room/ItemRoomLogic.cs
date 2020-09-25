using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class ItemRoomLogic : RoomLogic
{
    public override RoomLogic CreateNew()
    {
        return new ItemRoomLogic();
    }

    public override void OnRoomEnter(World world, Room room, Character player)
    {
        
    }

    protected override void OnGeneration(World world, Room roomToGenerate, WorldGenerator worldGen, int difficulty)
    {
        world.AddEntity(new ItemPickup(worldGen.itemGenerator.GenerateItem()), roomToGenerate.RoomPosition, Position.zero);
    }
}

