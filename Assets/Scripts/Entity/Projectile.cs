using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Projectile : Entity, ITickable
{
    Position direction;
    int shooterId;
    bool canDamageShooter;
    List<TagType> tagsToIgnore;
    public Projectile(Position size, Position directionAndSpeed, Entity shooter, bool canDamageShooter, Position? renderSize = null, string spriteId = null, string name = null) : base(size, renderSize, spriteId, name)
    {
        direction = directionAndSpeed;
        shooterId = shooter.Id;
        this.canDamageShooter = canDamageShooter;
        tagsToIgnore = new List<TagType>();
        if (!canDamageShooter)
            tagsToIgnore.Add(shooter.Tag);
    }

    public void Tick(World world)
    {
        MoveInLine(direction, 2, world, false, tagsToIgnore);
    }

    public override void OnCollision(World world, Entity collidingEntiy)
    {
        world.QueueEntityRemoval(Id);
    }
}

