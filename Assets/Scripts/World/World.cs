﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class World
{
    Dictionary<int, Entity> entities;
    List<ITickable> tickables;
    Room[][] rooms;
    int roomSize;
    public int Accuracy { get; private set; }

    public World(int worldSizeX, int worldSizeY, int roomSize, int accuracy)
    {
        this.roomSize = roomSize;
        this.Accuracy = accuracy;
        entities = new Dictionary<int, Entity>();
        tickables = new List<ITickable>();
        rooms = new Room[worldSizeX][];
        for (int x = 0; x < worldSizeX; x++)
        {
            rooms[x] = new Room[worldSizeY];
            for (int y = 0; y < worldSizeY; y++)
            {
                rooms[x][y] = new Room();
            }
        }
    }

    public void Tick()
    {
        foreach (var item in entities)
        {
            Console.WriteLine(item.Value.PositionInRoom);
        }
        foreach (var tickable in tickables)
        {
            tickable.Tick(this);
        }
    }

    public void AddEntity(Entity entity, Position roomPos, Position posInRoom)
    {
        entity.Id = GenerateId();
        entities.Add(entity.Id, entity);
        if (entity is ITickable)
            tickables.Add((ITickable)entity);

        //scales the size with accuracy
        entity.SetSize(entity.Size, this, true);

        entity.SetCurrentRoom(roomPos, this);
        entity.SetPositionInRoom(posInRoom, this, true);
    }


    //This will be horrible with a lot of entities and should be replaced later
    private int GenerateId()
    {
        int i = 0;
        while (entities.ContainsKey(i))
            i++;
        return i;
    }

    public Entity[] BoxCastinLine(Position roomPosition, Position pos, Position direction, int distance, Position size)
    {
        List<Entity> colldingEntities = new List<Entity>();

        Position destination = pos + direction * distance * Accuracy;
        Position normal = Position.RightNormal(destination);

        float shortestDistancesFromLine = int.MaxValue;
        float longestDistancesFromLine = int.MinValue;
        for (int x = -1; x < 2; x += 2)
        {
            for (int y = -1; y < 2; y += 2)
            {
                float distanceFormLine = Position.Dot(pos + new Position(x * size.x, y * size.y), normal);
                if (distanceFormLine < shortestDistancesFromLine)
                {
                    shortestDistancesFromLine = distanceFormLine;
                }
                else if (distanceFormLine > longestDistancesFromLine)
                {
                    longestDistancesFromLine = distanceFormLine;
                }

            }
        }

        foreach (Entity entity in GetRoom(roomPosition).entities)
        {
            if (IsColliding(pos, size, entity.PositionInRoom, entity.Size))
                if (IsDistanceToLineLongerThan(entity.PositionInRoom, entity.Size, normal, shortestDistancesFromLine, longestDistancesFromLine))
                    colldingEntities.Add(entity);
                    
        }

        foreach (Position direciton in Position.directions)
        {
            Position room2 = roomPosition + direciton;
            if (GetRoom(room2) != null)
            {
                foreach (Entity entity in GetRoom(room2).entities)
                {
                    Position convertedEntityPos = ConvertPositionBetweenRooms(entity.PositionInRoom, roomPosition, room2);
                    if (IsColliding(pos, size, convertedEntityPos, entity.Size))
                        if (IsDistanceToLineLongerThan(convertedEntityPos, entity.Size, normal, shortestDistancesFromLine, longestDistancesFromLine))
                            colldingEntities.Add(entity);
                }
            }

        }

        return colldingEntities.ToArray();
    }

    public Entity BoxCast(Position roomPosition, Position pos, Position size)
    {
        foreach (Entity entity in GetRoom(roomPosition).entities)
        {
            if (IsColliding(pos, size, entity.PositionInRoom, entity.Size))
                return entity;
        }

        foreach (Position direciton in Position.directions)
        {
            Position room2 = roomPosition + direciton;
            if (GetRoom(room2) != null)
            {
                foreach (Entity entity in GetRoom(room2).entities)
                {
                    if (IsColliding(pos, size, ConvertPositionBetweenRooms(entity.PositionInRoom, roomPosition, room2), entity.Size))
                        return entity;
                }
            }
        }

        return null;
    }


    private bool IsDistanceToLineLongerThan(Position boxPos, Position boxSize, Position normal, float shortestDistancesFromLine, float longestDistancesFromLine)
    {
        
        int oldDistanceFormLineNeg = 0;
        for (int x = -1; x < 2; x += 2)
        {
            for (int y = -1; y < 2; y += 2)
            {
                float distanceFormLine = Position.Dot(boxPos + new Position(x * boxSize.x, y * boxSize.y), normal);
                if (distanceFormLine >= shortestDistancesFromLine && distanceFormLine <= longestDistancesFromLine)
                {
                    return true;
                }
                else
                {
                    int distanceFormLineNeg = distanceFormLine > 0 ? 1 : -1;
                    if (oldDistanceFormLineNeg != distanceFormLineNeg && oldDistanceFormLineNeg != 0)
                        return true;
                    oldDistanceFormLineNeg = distanceFormLineNeg;
                }


            }
        }
        return false;
    }

    protected bool IsColliding(Position pos1, Position size1, Position pos2, Position size2)
    {
        return IsBoxCornersColliding(pos1 - size1 / 2, pos1 + size1 / 2, pos2 - size2 / 2, pos2 + size2 / 2);
    }
    protected bool IsBoxCornersColliding(Position box1LowerLeft, Position box1UpperRightCorner, Position box2LowerLeft, Position box2UpperRightCorner)
    {
        return (box2LowerLeft.x <= box1UpperRightCorner.x && box2UpperRightCorner.x >= box1LowerLeft.x) && (box2LowerLeft.y <= box1UpperRightCorner.y && box2UpperRightCorner.y >= box1LowerLeft.y);
    }

    public Room GetRoom(Position roomPosition)
    {
        if (roomPosition.x >= 0 && roomPosition.x < rooms.Length && roomPosition.y >= 0 && roomPosition.y < rooms[roomPosition.x].Length)
            return rooms[roomPosition.x][roomPosition.y];
        return null;
    }

    public Position ConvertPositionBetweenRooms(Position pos, Position room1, Position room2)
    {
        return pos + (room2 - room1) * roomSize * Accuracy;
    }

    public Position RoomDeltaIfOutsideOfRoom(Position pos)
    {
        return new Position(pos.x / (roomSize / 2), pos.y / (roomSize / 2));
    }
}
