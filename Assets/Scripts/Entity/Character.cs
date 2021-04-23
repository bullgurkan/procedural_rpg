using System;
using System.Collections.Generic;
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
        baseStats[Stat.ATTACK_POWER] = 1;
        baseStats[Stat.MAX_HEALTH] = 10;
        baseStats[Stat.ATTACK_SPEED] = 5;
        baseStats[Stat.MOVEMENT_SPEED] = 20;
        return baseStats;
    }

    public Item ChangeEquipment(Item item, WorldRenderer worldRenderer)
    {
        UnityEngine.Debug.Log(item);
        Item prevItem = null;
        if (items.ContainsKey(item.Slot))
            prevItem = items[item.Slot];
        items[item.Slot] = item;
        RecalculateStats(worldRenderer);
        return prevItem;
    }

    public override void RecalculateStats(WorldRenderer worldRenderer)
    {

        base.RecalculateStats(worldRenderer);

        int prevMaxHealth = GetStat(Stat.MAX_HEALTH);

        foreach (var item in items)
        {

            item.Value.ModifyStats(this);
        }

        health += GetStat(Stat.MAX_HEALTH) - prevMaxHealth;

        worldRenderer.UpdatePlayerStatDisplay(this);
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
            Enemy enemy = world.FindEnemy(CurrentRoom, PositionInRoom, world.RoomSize);
            item.OnEvent(EventType.ON_TICK, world, this, enemy, CurrentRoom, enemy?.PositionInRoom ?? Position.zero, new List<EventType>());
        }
    }

    private void TriggerItemEvents(EventType e, EntityLiving causer, World world, List<EventType> usedEventTypes)
    {

        foreach (Item item in items.Values)
        {
            item.OnEvent(e, world, this, causer, causer.CurrentRoom, causer.PositionInRoom, usedEventTypes);
        }
    }

    public void Activate(World world, Position activationPos)
    {
        foreach (Item item in items.Values)
        {
            Enemy enemy = world.FindEnemy(CurrentRoom, PositionInRoom, world.RoomSize);
            item.OnEvent(EventType.ON_ACTIVATION, world, this, enemy, CurrentRoom, activationPos, new List<EventType>());
        }
    }

    public void OpenItemCompareOverlay(World world)
    {
        ItemPickup itemPickup = (ItemPickup)world.BoxCastAll(CurrentRoom, PositionInRoom, Size).Find(x => x is ItemPickup);
    
        if (itemPickup != null)
        {
            Item itemToLookAt = itemPickup.PeekAtItem();
            items.TryGetValue(itemToLookAt.Slot, out Item currentItemInSlot);

            world.WorldRenderer.AddItemComparisonOverlay(currentItemInSlot, itemToLookAt, world);
        }

    }
    public void PickUpItem(World world)
    {
        ItemPickup itemPickup = (ItemPickup)world.BoxCastAll(CurrentRoom, PositionInRoom, Size).Find(x => x is ItemPickup);

        if (itemPickup != null)
        {
            Item itemToPickUp = itemPickup.PickupItem(world);
            Item itemToDrop = ChangeEquipment(itemToPickUp, world.WorldRenderer);
            if (itemToDrop != null)
                world.AddEntity(new ItemPickup(itemToDrop), itemPickup.CurrentRoom, itemPickup.PositionInRoom);
        }

    }

    protected override void OnDamage(World world, EntityLiving causer, List<EventType> usedEventTypes)
    { base.OnDamage(world, causer, usedEventTypes); TriggerItemEvents(EventType.ON_DAMAGE, causer, world, usedEventTypes); world.WorldRenderer.UpdateHealthBar(this); }
    protected override void OnHeal(World world, EntityLiving causer, List<EventType> usedEventTypes)
    { base.OnHeal(world, causer, usedEventTypes); TriggerItemEvents(EventType.ON_HEAL, causer, world, usedEventTypes); world.WorldRenderer.UpdateHealthBar(this); }
    protected override void OnDeath(World world, EntityLiving causer, List<EventType> usedEventTypes)
    {
        base.OnDeath(world, causer, usedEventTypes);


        TriggerItemEvents(EventType.ON_DEATH, causer, world, usedEventTypes);
        if (health < 0)
        {
            UnityEngine.Debug.Log("killed by " + causer);
            world.QueueEntityRemoval(Id);
        }
            
    }
    public override void OnEnemyKill(World world, EntityLiving causer, List<EventType> usedEventTypes)
    { base.OnDamage(world, causer, usedEventTypes);  TriggerItemEvents(EventType.ON_ENEMY_KILL, causer, world, usedEventTypes); }
    public override void OnEnemyHit(World world, EntityLiving causer, List<EventType> usedEventTypes)
    { base.OnDamage(world, causer, usedEventTypes);  TriggerItemEvents(EventType.ON_ENEMY_HIT, causer, world, usedEventTypes); }
}
