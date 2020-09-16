


using System;
using System.Collections.Generic;

public class RoomLogicTrapAmbush : RoomLogic
{
    bool triggered = false;
    List<EntityLiving> enemies;
    

    public RoomLogicTrapAmbush()
    {
        enemies = new List<EntityLiving>();
    }

    public override RoomLogic CreateNew()
    {
        return new RoomLogicTrapAmbush();
    }
    public override void OnRoomEnter(World world, Room room, Character player)
    {
        if (!triggered)
        {
            room.AddRoomLockToMultRoom(world, room.RoomPosition);

            foreach(EntityLiving enemy in enemies)
            {
                world.AddEntity(enemy, room.RoomPosition, enemy.PositionInRoom);
            }
            
            triggered = true;
        }
            
    }

    protected override void OnGeneration(World world, Room room, int difficulty)
    {
        enemies.Add(new Enemy(new EnemyTypeCharger(Position.one * 400, "Wall", "Charger"), Position.zero, 60));
    }
}
    

