using System;
using System.Collections.Generic;
using System.Diagnostics;
using static Effect;
public class Character : EntityLiving
{
    Dictionary<Slot, Item> items;

    private bool triggeredRoomEnter;
    private int wallWidth;


    public enum Slot
    {
        CHEST, LEG, FEET, HEAD, RING, AMULET, WEAPON, CAPE
    }


    public Character(Position size, int wallWidth, string spriteId = null, string name = null) : base(size, spriteId, name, GenerateBaseStats(), tag: TagType.PLAYER)
    {
        items = new Dictionary<Slot, Item>();
        this.wallWidth = wallWidth;

    }
    public static Dictionary<Stat, int> GenerateBaseStats()
    {
        Dictionary<Stat, int> baseStats = new Dictionary<Stat, int>();

        foreach (Stat stat in Enum.GetValues(typeof(Stat)))
        {
            baseStats.Add(stat, 1);
        }
        baseStats[Stat.ATTACK_POWER] = 10;
        baseStats[Stat.MAX_HEALTH] = 20;
        baseStats[Stat.ATTACK_SPEED] = 5;
        return baseStats;
    }

    public Item ChangeEquipment(Item item)
    {
        Item prevItem = null;
        if (items.ContainsKey(item.Slot))
            prevItem = items[item.Slot];
        items[item.Slot] = item;
        RecalculateStats();
        return prevItem;
    }

    public override void RecalculateStats()
    {

        base.RecalculateStats();

        foreach (var item in items)
        {

            item.Value.ModifyStats(this);
        }
    }
    public override void OnRoomChange(World world)
    {
        triggeredRoomEnter = false;
    }
    public override void OnPositionChange(World world)
    {
        if (!triggeredRoomEnter && PositionInRoom.x < (world.RoomSize - (Size.x + wallWidth)) / 2 && PositionInRoom.x > ((Size.x + wallWidth) - world.RoomSize) / 2 && PositionInRoom.y < (world.RoomSize - (Size.y + wallWidth)) / 2 && PositionInRoom.y > ((Size.y + wallWidth) - world.RoomSize) / 2)
        {
            //UnityEngine.Debug.Log(world.GetRoom(CurrentRoom).roomLogic);
            world.GetRoom(CurrentRoom).OnRoomEnter(world, this);
            triggeredRoomEnter = true;
        }
    }

    public override void Tick(World world)
    {
        foreach (Item item in items.Values)
        {
            item.OnEvent(EventType.ON_TICK, world, this, CurrentRoom, PositionInRoom);
        }
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

    public void PickUpItem(World world)
    {
        ItemPickup itemPickup = (ItemPickup)world.BoxCastAll(CurrentRoom, PositionInRoom, Size).Find(x => x is ItemPickup);

        if (itemPickup != null)
        {
            Item itemToPickUp = itemPickup.PickupItem(world);
            UnityEngine.Debug.Log(itemToPickUp);
            Item itemToDrop = ChangeEquipment(itemToPickUp);
            if (itemToDrop != null)
                world.AddEntity(new ItemPickup(itemToDrop), itemPickup.CurrentRoom, itemPickup.PositionInRoom);
        }

    }

    protected override void OnDamage(World world)
    { base.OnDamage(world); TriggerItemEvents(EventType.ON_DAMAGE, world); }
    protected override void OnHeal(World world)
    { base.OnHeal(world); TriggerItemEvents(EventType.ON_HEAL, world); }
    protected override void OnDeath(World world)
    {
        base.OnDeath(world);
        UnityEngine.Debug.Log("Dead");
        TriggerItemEvents(EventType.ON_DEATH, world);
    }
}
