using System;
using System.Collections.Generic;
using UnityEngine;

public class World
{
    Dictionary<int, Entity> entities;
    List<ITickable> tickables;
    Room[][] rooms;
    public int RoomSize { get; private set; }
    public WorldRenderer WorldRenderer { get; private set; }

    public World(int worldSizeX, int worldSizeY, int roomSize, WorldRenderer worldRenderer)
    {
        RoomSize = roomSize;
        WorldRenderer = worldRenderer;
        entities = new Dictionary<int, Entity>();
        tickables = new List<ITickable>();
        rooms = new Room[worldSizeX][];
        for (int x = 0; x < worldSizeX; x++)
        {
            rooms[x] = new Room[worldSizeY];
            for (int y = 0; y < worldSizeY; y++)
            {
                rooms[x][y] = new Room();
                AddEntity(new Entity(Position.zero, Position.one * RoomSize, name: $"Room {x}, {y}"), new Position(x, y), Position.zero);
            }
        }
    }

    public void Tick()
    {
        foreach (var tickable in tickables)
        {
            tickable.Tick(this);
        }
    }

    public void AddEntity(Entity entity, Position roomPos, Position posInRoom, bool shouldCameraFollow = false)
    {
        entity.Id = GenerateId();
        entities.Add(entity.Id, entity);
        if (entity is ITickable)
            tickables.Add((ITickable)entity);

        //scales the size with accuracy
        entity.SetSize(entity.Size, this, true);

        //if(entity.Size.x > 0 || entity.Size.y > 0)
        entity.SetCurrentRoom(roomPos, this);
        entity.SetPositionInRoom(posInRoom, this, true);

        WorldRenderer.AddEntity(entity, this, shouldCameraFollow);
    }


    //This will be horrible with a lot of entities and should be replaced later
    private int GenerateId()
    {
        int i = 0;
        while (entities.ContainsKey(i))
            i++;
        return i;
    }

    public Entity[] BoxCastinLine(int entityToIgnoreId, Position roomPosition, Position pos, Position direction, int distance, Position size)
    {
        if (distance % 2 == 1)
            throw new Exception("distance has to be dividable by 2");

        List<Entity> colldingEntities = new List<Entity>();

        Position destination = pos + direction * distance;
        Position normal = Position.RightNormal(destination);


        int shortestDistancesFromLine = int.MaxValue;
        int longestDistancesFromLine = int.MinValue;
        for (int x = -1; x < 2; x += 2)
        {
            for (int y = -1; y < 2; y += 2)
            {
                int distanceFormLine = Position.Dot(pos + new Position(x * size.x, y * size.y), normal);
                if (distanceFormLine < shortestDistancesFromLine)
                {
                    shortestDistancesFromLine = distanceFormLine;
                }

                if (distanceFormLine > longestDistancesFromLine)
                {
                    longestDistancesFromLine = distanceFormLine;
                }

            }
        }

 

        Position posBetweenOriginAndDest = pos + (direction * distance) / 2;
        Position sizeOfOriginToDestination = new Position(Math.Abs(direction.x), Math.Abs(direction.y)) * distance + size;

        foreach (Entity entity in GetRoom(roomPosition).entities)
        {
            if (entity.Id != entityToIgnoreId && IsColliding(posBetweenOriginAndDest, sizeOfOriginToDestination, entity.PositionInRoom, entity.Size))
            {
                if (IsDistanceToLineLongerThan(entity.PositionInRoom, entity.Size, normal, shortestDistancesFromLine, longestDistancesFromLine))
                    colldingEntities.Add(entity);
            }
                



        }

        foreach (Position direciton in Position.directions)
        {
            Position room2 = roomPosition + direciton;
            if (GetRoom(room2) != null)
            {
                foreach (Entity entity in GetRoom(room2).entities)
                {
                    Position convertedEntityPos = ConvertPositionBetweenRooms(entity.PositionInRoom, roomPosition, room2);
                    if (IsColliding(posBetweenOriginAndDest, sizeOfOriginToDestination, convertedEntityPos, entity.Size))
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


    private bool IsDistanceToLineLongerThan(Position boxPos, Position boxSize, Position normal, int shortestDistancesFromLine1, int longestDistancesFromLine1)
    {


        int shortestDistancesFromLine2 = int.MaxValue, longestDistancesFromLine2 = int.MinValue;
        for (int x = -1; x < 2; x += 2)
        {
            for (int y = -1; y < 2; y += 2)
            {
                int distanceFormLine = Position.Dot(boxPos + new Position(x * boxSize.x, y * boxSize.y), normal);
                if (distanceFormLine > shortestDistancesFromLine1 && distanceFormLine < longestDistancesFromLine1)
                {
                    return true;
                }
                else
                {
                    if (distanceFormLine < shortestDistancesFromLine2)
                        shortestDistancesFromLine2 = distanceFormLine;
                    if(distanceFormLine > longestDistancesFromLine2)
                        longestDistancesFromLine2 = distanceFormLine;
                }
            }
        }

        return shortestDistancesFromLine2 > 0 != longestDistancesFromLine2 > 0;
    }

    protected bool IsColliding(Position pos1, Position size1, Position pos2, Position size2)
    {
        if (size1 == Position.zero || size2 == Position.zero)
            return false;
        return IsBoxCornersColliding(pos1 - size1 / 2, pos1 + size1 / 2, pos2 - size2 / 2, pos2 + size2 / 2);
    }
    protected bool IsBoxCornersColliding(Position box1LowerLeft, Position box1UpperRightCorner, Position box2LowerLeft, Position box2UpperRightCorner)
    {
        return (box2LowerLeft.x < box1UpperRightCorner.x && box2UpperRightCorner.x > box1LowerLeft.x) && (box2LowerLeft.y < box1UpperRightCorner.y && box2UpperRightCorner.y > box1LowerLeft.y);
    }

    public Room GetRoom(Position roomPosition)
    {
        if (roomPosition.x >= 0 && roomPosition.x < rooms.Length && roomPosition.y >= 0 && roomPosition.y < rooms[roomPosition.x].Length)
            return rooms[roomPosition.x][roomPosition.y];
        return null;
    }

    public Position ConvertPositionBetweenRooms(Position pos, Position room1, Position room2)
    {
        return pos - (room2 - room1) * RoomSize;
    }

    public Position RoomDeltaIfOutsideOfRoom(Position pos)
    {
        return new Position(pos.x / (RoomSize / 2), pos.y / (RoomSize / 2));
    }
}

