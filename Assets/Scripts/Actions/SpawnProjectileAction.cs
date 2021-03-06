﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using static Effect;
using static Entity;

public class SpawnProjectileAction : Action
{
    Position size;
    int speed;
    Action actionToUseOnProjectileHit;
    public SpawnProjectileAction(Effect source, Position size, int speed, Action actionToUseOnProjectileHit): base(source)
    {
        this.size = size;
        this.speed = speed;
        this.actionToUseOnProjectileHit = actionToUseOnProjectileHit;
    }
    public override void OnActivation(World world, EntityLiving caster, EntityLiving reciver, Position room, Position positionInRoom, List<EventType> usedEventTypes)
    {
            Position delta = world.ConvertPositionBetweenRooms(positionInRoom, room, caster.CurrentRoom) - caster.PositionInRoom;
            if (delta.Magnitude != 0)
            {

                List<TagType> tagsToIgnore = new List<TagType>() { TagType.PLAYER, TagType.PROJECTILE_PASSABLE, TagType.PICKUP };

                world.AddEntity(new Projectile(size, delta * speed / delta.Magnitude, actionToUseOnProjectileHit, caster, usedEventTypes, tagsToIgnore: tagsToIgnore), caster.CurrentRoom, caster.PositionInRoom + delta * caster.Size.x / delta.Magnitude);
            }
            else
            {
                UnityEngine.Debug.Log("spawn no proj");
            }
        
    }

    public override string ToString() => $"SpawnProjectileAction(Size:{size}, Speed:{speed}, ActionToUseOnProjectileHit:{actionToUseOnProjectileHit})";
}

