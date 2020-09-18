using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static EntityLiving;

public class Effect
{
    public Dictionary<EventType, Action> actions;
    public Dictionary<Stat, float> stats;

    public enum EventType
    {
        ON_DAMAGE, ON_ENEMY_HIT, ON_HEAL, ON_ENEMY_KILL, ON_ACTIVATION, ON_DEATH
    }

    public Effect()
    {
        actions = new Dictionary<EventType, Action>();
        stats = new Dictionary<Stat, float>();
    }
    public void OnEvent(EventType eventType, World world, EntityLiving entityLiving, Position room, Position positionInRoom)
    {
        if(actions.ContainsKey(eventType))
            actions[eventType].OnActivation(world, entityLiving, room, positionInRoom);
    }

    public void ModifyStats(EntityLiving entity)
    {
        
    }
}

