using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Effect;
using static EntityLiving;

public class EnemyCharger : Enemy
{
    Position charagingDir;
    bool isCharging = false;
    static int accuracy = 10;
    int chargeCooldownMax;
    int chargeCooldownleft;

    DamageAction actionOnHit;
    public EnemyCharger(Position pos, int spawnTime, int difficulty) : base(pos, spawnTime, "enemy_charger", "Charger", new Position(400, 400), GenerateBaseStats(difficulty))
    {
        actionOnHit = new DamageAction(null, Stat.ATTACK_POWER, Stat.ARMOR);

        chargeCooldownMax = (int)(50/Math.Sqrt(difficulty));

    }
    public override void EnemyTick(World world)
    {
        if (isCharging)
        {

            if (MoveInLine(charagingDir, GetStat(Stat.MOVEMENT_SPEED), world, false) < GetStat(Stat.MOVEMENT_SPEED))
            {

                chargeCooldownleft = chargeCooldownMax;
                isCharging = false;
            }
        }
        else
        {


            if (chargeCooldownleft <= 0)
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
        if (collidingEntiy is Character && isTheMovingEntity)
        {
            actionOnHit.OnActivation(world, this, collidingEntiy as EntityLiving, collidingEntiy.CurrentRoom, collidingEntiy.PositionInRoom, new List<EventType>());
        }

    }

    public static Dictionary<Stat, int> GenerateBaseStats(int difficulty)
    {
        Dictionary<Stat, int> baseStats = new Dictionary<Stat, int>();

        foreach (Stat stat in Enum.GetValues(typeof(Stat)))
        {
            baseStats.Add(stat, 1);
        }
        baseStats[Stat.MAX_HEALTH] = difficulty * difficulty;
        baseStats[Stat.ATTACK_POWER] = 2 * difficulty;
        baseStats[Stat.MOVEMENT_SPEED] = (int)(20 * Math.Sqrt(difficulty) / accuracy);

        return baseStats;
    }

}

