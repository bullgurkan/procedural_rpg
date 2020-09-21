using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Projectile : Entity, ITickable
{
    Position direction;
    Action actionToUseOnHit;

    public Projectile(Position size, Position directionAndSpeed, Action actionToUseOnHit, Position? renderSize = null, string spriteId = null, string name = null, bool passable = true, List<TagType> tagsToIgnore = null) : base(size, renderSize, spriteId, name, tag: passable ? TagType.PROJECTILE_PASSABLE : TagType.PROJECTILE_SOLID, tagsToIgnore:tagsToIgnore)
    {
        direction = directionAndSpeed;
        this.actionToUseOnHit = actionToUseOnHit;

    }

    public void Tick(World world)
    {
        MoveInLine(direction, 2, world, false);
    }

    public override void OnCollision(World world, Entity collidingEntiy)
    {
        if (collidingEntiy is EntityLiving)
            actionToUseOnHit.OnActivation(world, collidingEntiy as EntityLiving, collidingEntiy.CurrentRoom, collidingEntiy.PositionInRoom);

        world.QueueEntityRemoval(Id);
    }
}

