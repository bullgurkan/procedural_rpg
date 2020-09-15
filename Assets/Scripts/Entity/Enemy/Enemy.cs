


public class Enemy : EntityLiving
{
    EnemyType type;
    int timeToSpawn;
    public Enemy(EnemyType enemyType, Position pos, int spawnTime) : base(Position.zero, enemyType.spriteId, enemyType.name, enemyType.size)
    {
        type = enemyType;
        timeToSpawn = spawnTime;
        PositionInRoom = pos;
    }
    public override void Tick(World world)
    {
        if(timeToSpawn < 0)
        {
            type.Tick(world, this);
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

}
