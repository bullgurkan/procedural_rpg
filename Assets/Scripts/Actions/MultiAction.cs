using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Effect;

public class MultiAction : Action
{
    List<Action> actions;
    public MultiAction(Action action1, Action action2)
    {
        actions = new List<Action>();
        actions.Add(action1);
        actions.Add(action2);
    }

    public void AddAction(Action action)
    {
        actions.Add(action);
    }
    public override void OnActivation(World world, EntityLiving caster, EntityLiving reciver, Position room, Position positionInRoom, Effect source, List<EventType> usedEventTypes)
    {
        foreach (Action action in actions)
        {
            action.OnActivation(world, caster, reciver, room, positionInRoom, source, usedEventTypes);
        }
    }

    public override string ToString()
    {

        string s = "MultiAction(Actions:)";
        for (int i = 0; i < actions.Count; i++)
        {
            s += actions[i];
            if (i < actions.Count - 1)
                s += ",";
        }

        return s;
    }
}

