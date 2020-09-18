using System;
using System.Collections.Generic;
using System.Diagnostics;
using static Effect;
public class Character : EntityLiving
{
    Dictionary<Slot, Item> items;
    List<Effect> effects;

    Dictionary<Stat, int> baseStats;

    private bool triggeredRoomEnter;
    private int wallWidth;


    public enum Slot
    {
        CHEST, LEG, FEET, HEAD, RING, AMULET, WEAPON, CAPE
    }


    public Character(Position size, int wallWidth, string spriteId = null, string name = null) : base(size, spriteId, name)
    {
        stats = new Dictionary<Stat, int>();
        effects = new List<Effect>();
        items = new Dictionary<Slot, Item>();
        this.wallWidth = wallWidth;
        Tag = TagType.PLAYER;

        GenerateBaseStats();
        foreach (var stat in baseStats)
        {
            stats.Add(stat.Key, stat.Value);
        }
    }

    private void GenerateBaseStats()
    {
        baseStats = new Dictionary<Stat, int>();

        foreach (Stat stat in Enum.GetValues(typeof(Stat)))
        {
            baseStats.Add(stat, 1);
        }
        baseStats[Stat.ATTACK_POWER] = 10;

    }
    public void ChangeEquipment(Item item)
    {
        items[item.Slot] = item;
        RecalculateStats();
    }

    public void RecalculateStats()
    {
        
        foreach (var stat in baseStats.Keys)
        {
            stats[stat] = baseStats[stat];
        }
        
        foreach (var item in items)
        {
            
            item.Value.ModifyStats(this);
        }

        foreach (var effect in effects)
        {
            effect.ModifyStats(this);
        }

    }

    public override void OnRoomChange(World world)
    {
        triggeredRoomEnter = false;
    }
    public override void OnPositionChange(World world)
    {
        if (!triggeredRoomEnter && PositionInRoom.x < world.RoomSize / 2 - (Size.x + wallWidth) && PositionInRoom.x > (Size.x + wallWidth) - world.RoomSize / 2 && PositionInRoom.y < world.RoomSize / 2 - (Size.y + wallWidth) && PositionInRoom.y > (Size.y + wallWidth) - world.RoomSize / 2)
        {
            world.GetRoom(CurrentRoom).OnRoomEnter(world, this);
            triggeredRoomEnter = true;
        }
    }

    public override void Tick(World world)
    {

    }

    private void TriggerItemEvents(EventType e, World world)
    {
        foreach (Item item in items.Values)
        {
            item.OnEvent(e, world, this, CurrentRoom, PositionInRoom);
        }
    }

    public void Activate(World world, Position activationPos)
    {
        foreach (Item item in items.Values)
        {
            item.OnEvent(EventType.ON_ACTIVATION, world, this, CurrentRoom, activationPos);
        }
    }

    protected override void OnDamage(World world)
    { TriggerItemEvents(EventType.ON_DAMAGE, world);}
    protected override void OnHeal(World world)
    { TriggerItemEvents(EventType.ON_HEAL, world);}
    protected override void OnDeath(World world)
    { TriggerItemEvents(EventType.ON_DEATH, world); }
}
