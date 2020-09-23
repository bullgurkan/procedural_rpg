

using System.Collections.Generic;

public class Exit : Entity
{


    public Exit(Position size, Position? renderSize = null, string spriteId = null, string name = null) : base(size, renderSize, spriteId, name, tag: TagType.TERRAIN, renderPriority: WorldRenderer.RenderPriority.EXIT, isTrigger: true)
    {
   
    }

    public override void OnCollision(World world, Entity collidingEntiy, bool isTheMovingEntity)
    {
        if (collidingEntiy is Character)
            world.GenerateWorld();

        //world.QueueEntityRemoval(Id);
    }
}
