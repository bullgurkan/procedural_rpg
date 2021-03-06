﻿


using System;
using System.Collections.Generic;
using static Effect;

public abstract class Enemy : EntityLiving
{
    int timeToSpawn;
    string spriteId;
    public Enemy(Position pos, int spawnTime, string spriteId, string name, Position size, Dictionary<Stat, int> baseStats) : base(Position.zero, "enemy_portal", name, baseStats, size)
    {
        timeToSpawn = spawnTime;
        PositionInRoom = pos;
        this.spriteId = spriteId;
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
                SetSprite(world, spriteId);
            }
            timeToSpawn--;
        }
       
    }


    public abstract void EnemyTick(World world);

    protected override void OnDeath(World world, EntityLiving causer, List<EventType> usedEventTypes) { base.OnDeath(world, causer, usedEventTypes); causer.OnEnemyKill(world, this, usedEventTypes); world.QueueEntityRemoval(Id); }

}
