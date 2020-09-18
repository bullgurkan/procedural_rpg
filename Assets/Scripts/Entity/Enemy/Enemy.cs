


public abstract class Enemy : EntityLiving
{
    int timeToSpawn;
    public Enemy(Position pos, int spawnTime, string spriteId, string name, Position size) : base(Position.zero, spriteId, name, size)
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
