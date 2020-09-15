using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : EntityLiving
{
    Dictionary<Slot, Item> items;
    List<Effect> effects;

    Dictionary<Stat, float> baseStats;

    private bool triggeredRoomEnter;
    private int wallWidth;


    public enum Slot
    {
        CHEST, LEG, FEET, HEAD, RING, AMULET, WEAPON, CAPE
    }


    public Character(Position size, int wallWidth, string spriteId = null, string name = null) : base(size, spriteId, name)
    {
        stats = new Dictionary<Stat, float>();
        effects = new List<Effect>();
        items = new Dictionary<Slot, Item>();
        this.wallWidth = wallWidth;

        GenerateBaseStats();
        foreach (var stat in baseStats)
        {
            stats.Add(stat.Key, stat.Value);
        }
    }

    private void GenerateBaseStats()
    {
        baseStats = new Dictionary<Stat, float>();

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

        foreach (var item in stats)
        {
            stats[item.Key] = baseStats[item.Key];
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
        if (!triggeredRoomEnter && PositionInRoom.x < world.RoomSize/2 - (Size.x + wallWidth) && PositionInRoom.x > (Size.x + wallWidth) - world.RoomSize/2 && PositionInRoom.y < world.RoomSize/2 - (Size.y + wallWidth) && PositionInRoom.y > (Size.y + wallWidth) - world.RoomSize/2)
        { 
            world.GetRoom(CurrentRoom).OnRoomEnter(world, this);
            triggeredRoomEnter = true;
        }
    }

    public override void Tick(World world)
    {

    }
}
