using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGenerator
{
    public Enemy GenerateEnemy(Position pos, int difficulty)
    {
        return new EnemyCharger(pos, 20, difficulty);
    }
}
