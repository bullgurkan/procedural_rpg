using System;

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

        //right, left, up, down (same order as Position.directions)
        RoomSide[][][] roomMap = new RoomSide[worldSizeX][][];
        for (int x = 0; x < worldSizeX; x++)
        {
            roomMap[x] = new RoomSide[worldSizeY][];
            for (int y = 0; y < worldSizeY; y++)
            {
                roomMap[x][y] = new RoomSide[4];
            }
        }

        Position pos = new Position(0, 0);
        Position dir = new Position();

        int steps = 20;

        for (int i = 0; i < steps; i++)
        {
            while (dir == Position.zero)
            {
                dir.x = random.Next(-1, 2);
                dir.y = random.Next(-1, 2);


                if (pos.x + dir.x > worldSizeX || pos.x + dir.x < 0)
                    dir.x = 0;

                if (pos.y + dir.y > worldSizeY || pos.y + dir.y < 0)
                    dir.y = 0;


                if (dir.x != 0 && dir.y != 0)
                    if (random.Next(0, 2) == 0)
                        dir.x = 0;
                    else dir.y = 0;


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
                for (int i = 0; i < roomMap[x][y].Length; i++)
                {
                    //floor
                    world.AddEntity(new Entity(Position.zero, renderPriority: floorRenderPrio, renderSize: Position.one * world.RoomSize, tileSize: tileWidth, spriteId: floorSprite, name: $"RoomFloor {x},{y}"), new Position(x, y), Position.zero);

                    //corners
                    if (x == 0)
                        world.AddEntity(new Entity(Position.one * wallWidth, renderPriority: wallRenderPrio, tileSize: tileWidth, spriteId: wallSprite, name: $"RoomConer ({x}, {y}) at dir (-1, 1)"), new Position(x, y), (Position.left + Position.up) * (world.RoomSize / 2));
                    if(y == 0)
                        world.AddEntity(new Entity(Position.one * wallWidth, renderPriority: wallRenderPrio, tileSize: tileWidth, spriteId: wallSprite, name: $"RoomConer ({x}, {y}) at dir (1, -1)"), new Position(x, y), (Position.right + Position.down) * (world.RoomSize / 2));

                    world.AddEntity(new Entity(Position.one * wallWidth, renderPriority: wallRenderPrio, tileSize: tileWidth, spriteId: wallSprite, name: $"RoomConer ({x}, {y}) at dir (1, 1)"), new Position(x, y), Position.one * (world.RoomSize / 2));

                    //walls
                    if (roomMap[x][y][i] != RoomSide.OPEN)
                    {
                        if (Position.directions[i].x > 0 || Position.directions[i].y > 0 || x == 0 && Position.directions[i].x < 0 || y == 0 && Position.directions[i].y < 0)
                        {
                            Position normal = Position.RightNormal(Position.directions[i]);
                            if (roomMap[x][y][i] == RoomSide.CLOSED)
                            {
                                Position size = new Position(Math.Abs(normal.x), Math.Abs(normal.y)) * (world.RoomSize - wallWidth) + Position.directions[i - i % 2] * wallWidth;
                                world.AddEntity(new Entity(size, renderPriority: wallRenderPrio, tileSize: tileWidth, spriteId: wallSprite, name: $"Wall in room ({x}, {y}) at dir {Position.directions[i]}"), new Position(x, y), Position.directions[i] * (world.RoomSize / 2));
                            }
                            else
                            {
                                Position size = new Position(Math.Abs(normal.x), Math.Abs(normal.y)) * (world.RoomSize / 2 - wallWidth/2 - doorWidth/2) + Position.directions[i - i % 2] * wallWidth;
                                int doorOffsetFromMiddle = world.RoomSize / 4 - wallWidth/4 - doorWidth/4 + doorWidth / 2;
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

