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
    public Effect source;

    public enum EventType
    {
        ON_ACTIVATION ,ON_DAMAGE, ON_ENEMY_HIT, ON_HEAL, ON_ENEMY_KILL, ON_DEATH, ON_TICK
    }

    public enum EffectData
    {
        COOLDOWN, DURATION_LEFT
    }

    public Effect(Effect source)
    {
        actions = new Dictionary<EventType, Action>();
        stats = new Dictionary<Stat, int>();
        effectData = new Dictionary<EffectData, object>();
        this.source = source;

    }

    public Effect()
    {
        actions = new Dictionary<EventType, Action>();
        stats = new Dictionary<Stat, int>();
        effectData = new Dictionary<EffectData, object>();
        source = this;
    }

    public void OnEvent(EventType eventType, World world, EntityLiving caster, EntityLiving reciver, Position room, Position positionInRoom, List<EventType> usedEventTypes)
    {
        if (actions.ContainsKey(eventType) && !usedEventTypes.Contains(eventType))
        {
            usedEventTypes.Add(eventType);
            actions[eventType].OnActivation(world, caster, reciver, room, positionInRoom, usedEventTypes);
        }
           
    }

    public void ModifyStats(EntityLiving entity)
    {
        foreach (Stat stat in stats.Keys)
        {
            entity.SetStat(stat, entity.GetStat(stat) + stats[stat]);
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

