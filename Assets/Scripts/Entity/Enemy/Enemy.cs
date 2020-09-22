


using System;
using System.Collections.Generic;

public abstract class Enemy : EntityLiving
{
    int timeToSpawn;
    public Enemy(Position pos, int spawnTime, string spriteId, string name, Position size, Dictionary<Stat, int> baseStats) : base(Position.zero, spriteId, name, baseStats, size)
    {
        timeToSpawn = spawnTime;
        PositionInRoom = pos;
    }
    public override void Tick(World world)
    {
        if(timeToSpawn < 0)
        {
            EnemyTick(world);
        }
        else
        {
            if(timeToSpawn == 0)
            {
                SetSize(RenderSize, world, true);
            }
            timeToSpawn--;
        }
       
    }




    public abstract void EnemyTick(World world);

    protected override void OnDeath(World world) { base.OnDeath(world); world.QueueEntityRemoval(Id); }

}
