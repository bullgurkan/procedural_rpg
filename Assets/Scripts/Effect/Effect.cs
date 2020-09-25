using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static EntityLiving;

public class Effect
{
    public Dictionary<EventType, Action> actions;
    public Dictionary<Stat, int> stats;
    public Dictionary<EffectData, object> effectData;

    public enum EventType
    {
        ON_DAMAGE, ON_ENEMY_HIT, ON_HEAL, ON_ENEMY_KILL, ON_ACTIVATION, ON_DEATH, ON_TICK
    }

    public enum EffectData
    {
        COOLDOWN, DURATION_LEFT
    }

    public Effect()
    {
        actions = new Dictionary<EventType, Action>();
        stats = new Dictionary<Stat, int>();
        effectData = new Dictionary<EffectData, object>();
    }
    public void OnEvent(EventType eventType, World world, EntityLiving entityLiving, Position room, Position positionInRoom)
    {
        if(actions.ContainsKey(eventType))
            actions[eventType].OnActivation(world, entityLiving, room, positionInRoom, effectData);
    }

    public void ModifyStats(EntityLiving entity)
    {
        foreach (Stat stat in stats.Keys)
        {
            entity.SetStat(stat, entity.GetStat(stat) + stats[stat]);
            UnityEngine.Debug.Log(stat + " " + entity.GetStat(stat));
        }

    }

    public override string ToString()
    {
        string s = "Stats:(";
        int i = 0;
        foreach (var stat in stats)
        {
            i++;
            s += stat.Key + ":" + stat.Value;
            if (i < stats.Count)
                s += ",";
        }
        s += ")";

        s += "Actions:(";
        i = 0;
        foreach (var action in actions)
        {
            i++;
            s += action.Key + ":" + action.Value;
            if (i < actions.Count)
                s += ",";
        }
        s += ")";

        return s;
    }
}

