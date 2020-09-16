using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public abstract class Action
{
    public abstract void OnActivation(World world, Character character, Position room, Position positionInRoom);
}

