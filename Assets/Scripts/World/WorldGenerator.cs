using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static WorldRenderer;

public class WorldGenerator
{
    System.Random random;
    int wallWidth, doorWidth;
    RenderPriority wallRenderPrio = RenderPriority.WALL;
    RenderPriority floorRenderPrio = RenderPriority.FLOOR;



    string wallSprite, floorSprite;

    public WorldGenerator(int seed, int wallWidth, int doorWithInTiles, string wallSprite, string floorSprite)
    {
        random = new System.Random(seed);
        this.wallSprite = wallSprite;
        this.wallWidth = wallWidth;
        this.floorSprite = floorSprite;
        doorWidth = wallWidth * doorWithInTiles;
    }

    private enum RoomSide
    {
        CLOSED, DOOR, OPEN
    }
    public void PopulateWorld(int worldSizeX, int worldSizeY, World world)
    {
        RoomSide[][][] roomMap = GenerateRoomMap(worldSizeX, worldSizeY, world);

        GenerateRoomEntitiesFromMap(roomMap, worldSizeX, worldSizeY, world);
    }

    private RoomSide[][][] GenerateRoomMap(int worldSizeX, int worldSizeY, World world)
    {
        //right, left, up, down (same order as Position.directions), distanceFromStart
        RoomSide[][][] roomMap = new RoomSide[worldSizeX][][];
        for (int x = 0; x < worldSizeX; x++)
            roomMap[x] = new RoomSide[worldSizeY][];



        Position pos = new Position(worldSizeX/2, worldSizeY/2);
        Position dir = new Position();

        roomMap[pos.x][pos.y] = new RoomSide[5];

        int roomAmountToGenerate = 40;

        List<Position> roomsWithEmptyNeighbour = new List<Position>();
        List<Position> validMoves = new List<Position>();

        for (int i = 1; i < roomAmountToGenerate; i++)
        {
            while (dir == Position.zero)
            {
                validMoves.Clear();
                foreach (Position direction in Position.directions)
                {

                    if (world.RoomInBounds(pos + direction) && roomMap[pos.x + direction.x][pos.y + direction.y] == null)
                    {
                        validMoves.Add(direction);
                    }

                }

                if(roomsWithEmptyNeighbour.Count > 0 && random.Next(0, 10) == 0)
                {
                    pos = roomsWithEmptyNeighbour[0];
                    roomsWithEmptyNeighbour.RemoveAt(0);
                    continue;
                }

                //Debug.Log(validMoves.Count);
                if (validMoves.Count == 0)
                {
                    if (roomsWithEmptyNeighbour.Count > 0)
                    {
                        pos = roomsWithEmptyNeighbour[0];
                        roomsWithEmptyNeighbour.RemoveAt(0);
                        continue;
                    }
                    else
                        break;
                   
                }
                    

                if (validMoves.Count > 1)
                    roomsWithEmptyNeighbour.Add(pos);

                dir = validMoves[random.Next(validMoves.Count)];

            }


            RoomSide rs = (RoomSide)random.Next(1, 3);
            for (int f = 0; f < Position.directions.Length; f++)
            {
                if (dir == Position.directions[f])
                {
                    roomMap[pos.x][pos.y][f] = rs;
                }

                if (-dir == Position.directions[f])
                {

                    roomMap[pos.x + dir.x][pos.y + dir.y] = new RoomSide[5];
                    roomMap[pos.x + dir.x][pos.y + dir.y][f] = rs;
                    roomMap[pos.x + dir.x][pos.y + dir.y][4] = roomMap[pos.x][pos.y][4] + 1;
                }

            }

            pos += dir;


            dir = Position.zero;

        }
        return roomMap;
    }
    private void GenerateRoomEntitiesFromMap(RoomSide[][][] roomMap, int worldSizeX, int worldSizeY, World world)
    {
        Position tileWidth = Position.one * wallWidth;

        for (int x = 0; x < worldSizeX; x++)
        {
            for (int y = 0; y < worldSizeY; y++)
            {
                if (roomMap[x][y] != null)
                    for (int i = 0; i < Position.directions.Length; i++)
                    {
                        //floor
                        world.AddEntity(new Entity(Position.zero, renderPriority: floorRenderPrio, renderSize: Position.one * world.RoomSize, tileSize: tileWidth, spriteId: floorSprite, name: $"RoomFloor {x},{y}"), new Position(x, y), Position.zero);

                        Position cornerDir = Position.directions[i] + Position.RightNormal(Position.directions[i]);
                        Position neighbourRoom = new Position(x, y) + cornerDir;


                        if (Position.directions[i].x > 0 || Position.directions[i].y > 0 || !world.RoomInBounds(neighbourRoom) || roomMap[neighbourRoom.x][neighbourRoom.y] == null)
                        {
                            world.AddEntity(new Entity(cornerDir * wallWidth, renderPriority: wallRenderPrio, tileSize: tileWidth, spriteId: wallSprite, name: $"RoomConer ({x}, {y}) at dir {cornerDir}"), new Position(x, y), cornerDir * (world.RoomSize / 2));
                        }

                        //walls
                        if (roomMap[x][y][i] != RoomSide.OPEN)
                        {
                            neighbourRoom = new Position(x, y) + Position.directions[i];
                            if (Position.directions[i].x > 0 || Position.directions[i].y > 0 || !world.RoomInBounds(neighbourRoom) || roomMap[neighbourRoom.x][neighbourRoom.y] == null)
                            {
                                Position normal = Position.RightNormal(Position.directions[i]);
                                if (roomMap[x][y][i] == RoomSide.CLOSED)
                                {
                                    Position size = new Position(Math.Abs(normal.x), Math.Abs(normal.y)) * (world.RoomSize - wallWidth) + Position.directions[i - i % 2] * wallWidth;
                                    world.AddEntity(new Entity(size, renderPriority: wallRenderPrio, tileSize: tileWidth, spriteId: wallSprite, name: $"Wall in room ({x}, {y}) at dir {Position.directions[i]}"), new Position(x, y), Position.directions[i] * (world.RoomSize / 2));
                                }
                                else
                                {
                                    Position size = new Position(Math.Abs(normal.x), Math.Abs(normal.y)) * (world.RoomSize / 2 - wallWidth / 2 - doorWidth / 2) + Position.directions[i - i % 2] * wallWidth;
                                    int doorOffsetFromMiddle = world.RoomSize / 4 - wallWidth / 4 - doorWidth / 4 + doorWidth / 2;
                                    world.AddEntity(new Entity(size, renderPriority: wallRenderPrio, tileSize: tileWidth, spriteId: wallSprite, name: $"DoorWall in room ({x}, {y}) at dir {Position.directions[i]}"), new Position(x, y), Position.directions[i] * (world.RoomSize / 2) + normal * doorOffsetFromMiddle);
                                    world.AddEntity(new Entity(size, renderPriority: wallRenderPrio, tileSize: tileWidth, spriteId: wallSprite, name: $"DoorWall in room ({x}, {y}) at dir {Position.directions[i]}"), new Position(x, y), Position.directions[i] * (world.RoomSize / 2) + normal * -doorOffsetFromMiddle);
                                }
                            }
                        }



                    }
            }
        }
    }

}

