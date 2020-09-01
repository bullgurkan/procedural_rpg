using System;
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
    int accuracy;

    public World(int worldSizeX, int worldSizeY, int roomSize, int accuracy)
    {
        this.roomSize = roomSize;
        this.accuracy = accuracy;
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

        entity.PositionOfCurrentRoom = roomPos;
        entity.PositionOfCurrentRoom = roomPos;
    }


    //This will be horrible with a lot of entities and should be replaced later
    private int GenerateId()
    {
        int i = 0;
        while (entities.ContainsKey(i))
            i++;
        return i;
    }

    public Entity[] BoxCastinLine(Position roomPosition, Position pos, Position direction, int distance,  Position size)
    {
        List<Entity> colldingEntities = new List<Entity>();

        Position destination = pos + direction * distance * accuracy;
        Position normal = Position.RightNormal(destination);

        foreach (Entity entity in GetRoom(roomPosition).entities)
        {
            if (IsColliding(pos, size, entity.PositionInRoom, entity.Size))
                if()
                colldingEntities.Add(entity);
        }

        foreach (Position direciton in Position.directions)
        {
            Position room2 = roomPosition + direciton;
            foreach (Entity entity in GetRoom(room2).entities)
            {
                if (IsColliding(pos, size, ConvertPositionBetweenRooms(entity.PositionInRoom, roomPosition, room2), entity.Size))
                    colldingEntities.Add(entity);
            }
        }

        return null;
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
            foreach (Entity entity in GetRoom(room2).entities)
            {
                if (IsColliding(pos, size, ConvertPositionBetweenRooms(entity.PositionInRoom, roomPosition, room2), entity.Size))
                    return entity;
            }
        }

        return null;
    }


    private bool IsCol(Position boxPos, Position boxSize, Position movedBoxNormal, Position movedBoxNormal2)
    {
        
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
        return rooms[roomPosition.x][roomPosition.y];
    }

    public Position ConvertPositionBetweenRooms(Position pos, Position room1, Position room2)
    {
        return pos + (room2 - room1) * roomSize * accuracy;
    }

    public Position RoomDeltaIfOutsideOfRoom(Position pos)
    {
        return new Position(pos.x % (roomSize / 2), pos.y % (roomSize / 2));
    }
}

