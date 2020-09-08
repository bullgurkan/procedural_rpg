using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public struct Collision
{
    Entity movingObj, staticObj;
    Position collisionPos, movingObjPosUnderCollision;

    public Collision(Entity movingObj, Entity staticObj, Position collisionPos, Position movingObjPosUnderCollision)
    {
        this.movingObj = movingObj;
        this.staticObj = staticObj;
        this.collisionPos = collisionPos;
        this.movingObjPosUnderCollision = movingObjPosUnderCollision;
    }
}

