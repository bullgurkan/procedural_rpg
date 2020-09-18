using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static EntityLiving;

public class DamageAction : Action
{
    int amount;
    Stat resitance;
    public DamageAction(int amount, Stat resitance)
    {
        this.amount = amount;
        this.resitance = resitance;
    }
    public override void OnActivation(World world, EntityLiving entityLiving, Position room, Position positionInRoom)
    {
        entityLiving.Damage(amount, resitance, world);
    }
}

