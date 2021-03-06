﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Effect;

public class Projectile : Entity, ITickable
{
    Position direction;
    Action actionToUseOnHit;
    EntityLiving caster;
    List<EventType> usedEventTypes;

    public Projectile(Position size, Position directionAndSpeed, Action actionToUseOnHit, EntityLiving caster, List<EventType> usedEventTypes, Position? renderSize = null, string spriteId = null, string name = null, bool passable = true, List<TagType> tagsToIgnore = null) : base(size, renderSize, spriteId, name, renderPriority: WorldRenderer.RenderPriority.PROJECTILE, tag: passable ? TagType.PROJECTILE_PASSABLE : TagType.PROJECTILE_SOLID, tagsToIgnore:tagsToIgnore, isTrigger:true)
    {
        direction = directionAndSpeed;
        this.actionToUseOnHit = actionToUseOnHit;
        this.caster = caster;
        this.usedEventTypes = usedEventTypes;
    }

    public void Tick(World world)
    {
        MoveInLine(direction, 2, world, false);
    }

    public override void OnCollision(World world, Entity collidingEntiy, bool isTheMovingEntity)
    {

        if (collidingEntiy is EntityLiving)
        {
            actionToUseOnHit.OnActivation(world, caster, collidingEntiy as EntityLiving, collidingEntiy.CurrentRoom, collidingEntiy.PositionInRoom, usedEventTypes);
            if (collidingEntiy is Enemy)
                caster.OnEnemyHit(world, collidingEntiy as EntityLiving, usedEventTypes);
        }
            

        world.QueueEntityRemoval(Id);
    }
}

