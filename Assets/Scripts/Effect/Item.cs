using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class Item : Effect
{
    public Character.Slot Slot { get; private set; }
    public string spriteId;

    public Item(Character.Slot slot)
    {
        Slot = slot;
    }

    public override string ToString() => $"Slot:{Slot}, {base.ToString()}";
}

