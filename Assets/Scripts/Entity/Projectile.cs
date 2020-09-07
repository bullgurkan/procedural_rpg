using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Projectile : Entity, ITickable
{
    Position direction;
    public Projectile(Position size, Position directionAndSpeed, Position? renderSize = null, string spriteId = null, string name = null) : base(size, renderSize, spriteId, name)
    {
        this.direction = directionAndSpeed;
    }

    public void Tick(World world)
    {
        MoveInLine(direction, 2, world, true);
    }
}

