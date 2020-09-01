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
        OnDamage, OnEnemyHit, OnHeal, OnEnemyKill, OnActivation
    }

    public Effect()
    {
        actions = new Dictionary<EventType, Action>();
        stats = new Dictionary<Stat, float>();
    }
    public void OnEvent(EventType eventType, Character character)
    {
        actions[eventType].OnActivation(character);
    }

    public void ModifyStats(EntityLiving entity)
    {
        
    }
}

