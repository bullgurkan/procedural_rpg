


using System;
using System.Collections.Generic;
using static Effect;

public class RoomLogicTrapAmbush : RoomLogic
{
    bool triggered = false;
    List<Enemy> enemies;
    Room room;
    

    public RoomLogicTrapAmbush()
    {
        enemies = new List<Enemy>();
    }

    public override RoomLogic CreateNew()
    {
        return new RoomLogicTrapAmbush();
    }
    public override void OnRoomEnter(World world, Room room, Character player)
    {
        
        if (!triggered)
        {
            this.room = room;
            room.AddRoomLockToMultRoom(world, room.RoomPosition);

            

            foreach(Enemy enemy in enemies)
            {
                Effect effect = new Effect();
                effect.actions.Add(EventType.ON_DEATH, new UnlockRoomAction(this));
                enemy.AddEffect(effect);
                world.AddEntity(enemy, room.RoomPosition, enemy.PositionInRoom);
            }
            
            triggered = true;
        }
            
    }

    protected override void OnGeneration(World world, Room room, WorldGenerator worldGen, int difficulty)
    {
        enemies.Add(worldGen.enemyGen.GenerateEnemy(Position.zero, difficulty));
    }

    public void EntityDied(World world, Enemy enemy)
    {
        enemies.Remove(enemy);
        if (enemies.Count == 0)
            room.RemoveRoomLockFromMultRoom(world, room.RoomPosition);
    }

}
    

