

using UnityEngine;

public class RoomLogicTrapAmbush : RoomLogic
{
    public override void OnRoomEnter(World world, Character player)
    {
        
    }

    protected override void OnGeneration(World world, Room roomToGenerate, int difficulty)
    {
        Debug.Log(roomToGenerate.RoomPosition);
        roomToGenerate.AddRoomLock(world, roomToGenerate.RoomPosition);
    }
}
    

