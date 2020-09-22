using System;
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
    public SpawnProjectileAction(Position size, int speed, Action actionToUseOnProjectileHit)
    {
        this.size = size;
        this.speed = speed;
        this.actionToUseOnProjectileHit = actionToUseOnProjectileHit;
    }
    public override void OnActivation(World world, EntityLiving entityLiving, Position room, Position positionInRoom, Dictionary<EffectData, Object> effectData)
    {
        Position delta = world.ConvertPositionBetweenRooms(positionInRoom, room, entityLiving.CurrentRoom) - entityLiving.PositionInRoom;

        List<TagType> tagsToIgnore = new List<TagType>() { TagType.PLAYER, TagType.PROJECTILE_PASSABLE};
        world.AddEntity(new Projectile(size, delta * speed / delta.Magnitude, actionToUseOnProjectileHit, tagsToIgnore:tagsToIgnore), entityLiving.CurrentRoom, entityLiving.PositionInRoom + delta * entityLiving.Size.x / delta.Magnitude);
    }
}

