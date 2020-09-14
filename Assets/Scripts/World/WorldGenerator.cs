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
    public void PopulateWorld(int worldSizeX, int worldSizeY)
    {

        //right, left, up, down (same order as Position.directions)
        //Dictionary<Room>
        RoomSide[][][] roomMap = new RoomSide[worldSizeX][][];
        for (int x = 0; x < worldSizeX; x++)
        {
            roomMap[x] = new RoomSide[worldSizeY][];
            for (int y = 0; y < worldSizeY; y++)
            {
                //roomMap[x][y] = new RoomSide[4];
            }
        }



        Position pos = new Position(0, 0);
        Position dir = new Position();

        roomMap[pos.x][pos.y] = new RoomSide[4];

        int steps = 200;

        List<Position> validMoves = new List<Position>();

        for (int i = 0; i < steps; i++)
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
                //Debug.Log(validMoves.Count);
                if (validMoves.Count == 0)
                    break;

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

                    roomMap[pos.x + dir.x][pos.y + dir.y] = new RoomSide[4];
                    roomMap[pos.x + dir.x][pos.y + dir.y][f] = rs;
                }

            }

            pos += dir;


            dir = Position.zero;

        }

        Position tileWidth = Position.one * wallWidth;



        world.AddEntity(new Entity(Position.one * wallWidth, renderPriority: wallRenderPrio, tileSize: tileWidth, spriteId: wallSprite, name: $"RoomConer (0, 0) at dir (-1, -1)"), Position.zero, -Position.one * (world.RoomSize / 2));

        for (int x = 0; x < worldSizeX; x++)
        {
            for (int y = 0; y < worldSizeY; y++)
            {
                if (roomMap[x][y] != null)
                    for (int i = 0; i < roomMap[x][y].Length; i++)
                    {
                        //floor
                        world.AddEntity(new Entity(Position.zero, renderPriority: floorRenderPrio, renderSize: Position.one * world.RoomSize, tileSize: tileWidth, spriteId: floorSprite, name: $"RoomFloor {x},{y}"), new Position(x, y), Position.zero);

                        //corners
                        if (x == 0)
                            world.AddEntity(new Entity(Position.one * wallWidth, renderPriority: wallRenderPrio, tileSize: tileWidth, spriteId: wallSprite, name: $"RoomConer ({x}, {y}) at dir (-1, 1)"), new Position(x, y), (Position.left + Position.up) * (world.RoomSize / 2));
                        if (y == 0)
                            world.AddEntity(new Entity(Position.one * wallWidth, renderPriority: wallRenderPrio, tileSize: tileWidth, spriteId: wallSprite, name: $"RoomConer ({x}, {y}) at dir (1, -1)"), new Position(x, y), (Position.right + Position.down) * (world.RoomSize / 2));

                        world.AddEntity(new Entity(Position.one * wallWidth, renderPriority: wallRenderPrio, tileSize: tileWidth, spriteId: wallSprite, name: $"RoomConer ({x}, {y}) at dir (1, 1)"), new Position(x, y), Position.one * (world.RoomSize / 2));

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
                        //walls
                        if (roomMap[x][y][i] != RoomSide.OPEN)
                        {
                            Position neighbourRoom = new Position(x, y) + Position.directions[i];
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



        //world.AddEntity(new Entity(Position.zero, Position.one * world.RoomSize, name: $"Room {x}, {y}"), new Position(x, y), Position.zero);
    }

    private void PopulateRoom()
    {

    }

}

