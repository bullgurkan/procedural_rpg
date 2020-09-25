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
    public override void OnActivation(World world, EntityLiving caster, EntityLiving reciver, Position room, Position positionInRoom, Dictionary<EffectData, Object> effectData)
    {
        Position delta = world.ConvertPositionBetweenRooms(positionInRoom, room, caster.CurrentRoom) - caster.PositionInRoom;

        List<TagType> tagsToIgnore = new List<TagType>() { TagType.PLAYER, TagType.PROJECTILE_PASSABLE, TagType.PICKUP};
        world.AddEntity(new Projectile(size, delta * speed / delta.Magnitude, actionToUseOnProjectileHit, caster, tagsToIgnore:tagsToIgnore), caster.CurrentRoom, caster.PositionInRoom + delta * caster.Size.x / delta.Magnitude);
    }

    public override string ToString() => $"SpawnProjectileAction(Size:{size}, Speed:{speed}, ActionToUseOnProjectileHit:{actionToUseOnProjectileHit})";
}

