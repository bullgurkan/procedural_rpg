using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static EntityLiving;
using static Effect;

public class CooldownAction : Action
{
    Action action;
    int cooldown;
    public CooldownAction(Effect source, Action action, Effect effect, int cooldown = -1) : base(source)
    {
        this.action = action;
        this.cooldown = cooldown;
        effect.actions.Add(EventType.ON_TICK, new CooldownHelperAction(source));
    }
    public override void OnActivation(World world, EntityLiving caster, EntityLiving reciver, Position room, Position positionInRoom, List<EventType> usedEventTypes)
    {

        if (source.effectData.ContainsKey(EffectData.COOLDOWN))
        {
            if ((int)source.effectData[EffectData.COOLDOWN] == 0)
            {
                if(caster.GetStat(Stat.ATTACK_SPEED) > 0)
                {
                    action.OnActivation(world, caster, reciver, room, positionInRoom, usedEventTypes);
                    if (cooldown > 0)
                    {
                        source.effectData[EffectData.COOLDOWN] = cooldown;
                    }
                    else
                    {
                        if (caster.GetStat(Stat.ATTACK_SPEED) > 0)
                            source.effectData[EffectData.COOLDOWN] = 200/ caster.GetStat(Stat.ATTACK_SPEED);
                    }
                }
                
            }
            
        }
        else
        {
            source.effectData.Add(EffectData.COOLDOWN, 0);
            OnActivation(world, caster, reciver, room, positionInRoom, usedEventTypes);
        }   
        
    }

    public override string ToString() => $"CooldownAction(Action:{action})";
}

