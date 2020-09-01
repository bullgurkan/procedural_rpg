using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Character;


public class Effect
{
    public Dictionary<EventType, Action> actions;
    public Dictionary<Stat, float> stats;

    public enum EventType
    {
        OnDamage, OnEnemyHit, OnHeal, OnEnemyKill, OnActivation
    }
    public void OnEvent(EventType eventType, Character character)
    {
        actions[eventType].OnActivation();
    }

    public void ModifyStats(Character character)
    {

    }
}

