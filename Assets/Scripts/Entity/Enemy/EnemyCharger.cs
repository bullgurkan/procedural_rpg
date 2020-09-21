using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static EntityLiving;

public class EnemyCharger : Enemy
{
    Position charagingDir;
    bool isCharging = false;
    int speed;
    int accuracy = 10;
    int chargeCooldownMax;
    int chargeCooldownleft;

    Action actionOnHit;
    public EnemyCharger(Position pos, int speed, int chargeCooldown) : base(pos, 10, "", "Charger", new Position(400, 400))
    {
        this.speed = speed / accuracy;
        actionOnHit = new DamageAction(10, Stat.ARMOR);
        this.chargeCooldownMax = chargeCooldown;

    }
    public override void EnemyTick(World world)
    {
        if (isCharging)
        {

            if (MoveInLine(charagingDir, speed, world, false) < speed)
            {
                
                chargeCooldownleft = chargeCooldownMax;
                isCharging = false;
            }
        }
        else
        {

        
            if(chargeCooldownleft <= 0)
            {
                isCharging = true;
                double minMag = int.MaxValue;
                foreach (var player in world.players)
                {
                    Position delta = world.ConvertPositionBetweenRooms(player.PositionInRoom, player.CurrentRoom, CurrentRoom) - PositionInRoom;
                    if (delta.Magnitude < minMag)
                    {
                        minMag = delta.Magnitude;
                        charagingDir = delta * accuracy / delta.Magnitude;
                    }

                }
            }
            else
            {
                chargeCooldownleft--;
            }
            
        }
        
    }

    public override void OnCollision(World world, Entity collidingEntiy, bool isTheMovingEntity)
    {
        if (collidingEntiy is EntityLiving && isTheMovingEntity)
        {
            actionOnHit.OnActivation(world, collidingEntiy as EntityLiving, collidingEntiy.CurrentRoom, collidingEntiy.PositionInRoom);
        }
            
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

