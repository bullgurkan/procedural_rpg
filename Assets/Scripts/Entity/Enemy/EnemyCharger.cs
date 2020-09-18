using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static EntityLiving;

public class EnemyCharger : Enemy
{

    public EnemyCharger(Position pos) : base(pos, 10, "", "Charger", new Position(400, 400))
    {

    }
    public override void EnemyTick(World world)
    {
        
    }

    public override Dictionary<Stat, int> GenerateBaseStats()
    {
        Dictionary<Stat, int> baseStats = new Dictionary<Stat, int>();

        foreach (Stat stat in Enum.GetValues(typeof(Stat)))
        {
            baseStats.Add(stat, 1);
        }
        baseStats[Stat.MAX_HEALTH] = 100;
        baseStats[Stat.ATTACK_POWER] = 10;

        return baseStats;
    }
}

