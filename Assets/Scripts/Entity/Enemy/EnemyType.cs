using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public abstract class EnemyType
{
    public Position size;
    public string spriteId;
    public string name;
    public EnemyType(Position size, string spriteId, string name)
    {
        this.size = size;
        this.spriteId = spriteId;
        this.name = name;
    }

    public abstract void Tick(World world, Enemy enemy);
}

