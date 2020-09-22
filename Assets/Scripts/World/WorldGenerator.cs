using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static WorldRenderer;
using static Room;

public class WorldGenerator
{
    System.Random random;

    RenderPriority wallRenderPrio = RenderPriority.WALL;
    RenderPriority floorRenderPrio = RenderPriority.FLOOR;


    int amountOfRoomsToGenerate;
    int difficulty;
    List<RoomLogic> roomsWithLogicToGenerate;
    List<RoomLogic> roomLogicRegistry;

    string wallSprite, floorSprite;

    private readonly int wallWidth, doorWidth;
    private readonly Position tileSize;
    private EnemyGenerator enemyGen;
    private int precentOfRoomWithLogic;

    public WorldGenerator(int seed, int wallWidth, int doorWithInTiles, string wallSprite, string floorSprite, int precentOfRoomWithLogic, int startingDifficulty)
    {
        random = new System.Random(seed);
        this.wallSprite = wallSprite;
        this.wallWidth = wallWidth;
        tileSize = Position.one * wallWidth;
        this.floorSprite = floorSprite;
        doorWidth = wallWidth * doorWithInTiles;
        difficulty = startingDifficulty;
        roomLogicRegistry = new List<RoomLogic>();
        roomsWithLogicToGenerate = new List<RoomLogic>();
        enemyGen = new EnemyGenerator();
        this.precentOfRoomWithLogic = precentOfRoomWithLogic;




        roomLogicRegistry.Add(new RoomLogicTrapAmbush());


    }


    public Room[][] GenerateRoomMap(World world)
    {
        difficulty += difficulty / 4;
        roomsWithLogicToGenerate.Clear();
        roomsWithLogicToGenerate.Add(new StartRoomLogic());
        roomsWithLogicToGenerate.Add(new ExitRoomLogic());

        amountOfRoomsToGenerate = random.Next(10, 20 + difficulty);
        if (roomLogicRegistry.Count > 0)
        {
            for (int i = 0; i < (amountOfRoomsToGenerate * precentOfRoomWithLogic) / 100; i++)
            {
                roomsWithLogicToGenerate.Add(roomLogicRegistry[random.Next(roomLogicRegistry.Count)].CreateNew());

            }
        }

        Room[][] roomMap = new Room[world.worldSize.x][];
        for (int x = 0; x < world.worldSize.x; x++)
            roomMap[x] = new Room[world.worldSize.y];



        Position pos = new Position(world.worldSize.x / 2, world.worldSize.y / 2);
        Position dir = new Position();

        roomMap[pos.x][pos.y] = new Room(pos);


        List<Position> roomsWithEmptyNeighbour = new List<Position>();
        List<Position> validMoves = new List<Position>();

        for (int i = 1; i < amountOfRoomsToGenerate; i++)
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

                if (roomsWithEmptyNeighbour.Count > 0 && random.Next(0, 10) == 0)
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
                    roomMap[pos.x][pos.y].roomSides[f] = rs;
                }

                if (-dir == Position.directions[f])
                {

                    roomMap[pos.x + dir.x][pos.y + dir.y] = new Room(pos + dir);
                    roomMap[pos.x + dir.x][pos.y + dir.y].roomSides[f] = rs;
                    roomMap[pos.x + dir.x][pos.y + dir.y].distanceFromStart = roomMap[pos.x][pos.y].distanceFromStart + 1;
                }

            }

            pos += dir;


            dir = Position.zero;

        }
        return roomMap;
    }

    public void GenerateRoomWalls(World world)
    {

        for (int x = 0; x < world.worldSize.x; x++)
            for (int y = 0; y < world.worldSize.y; y++)
            {
                Room room = world.GetRoom(new Position(x, y));
                if (room != null)
                {
                    //floor
                    world.AddEntity(new Entity(Position.zero, Position.one * world.RoomSize, floorSprite, $"RoomFloor {room.RoomPosition}", tileSize, floorRenderPrio), room.RoomPosition, Position.zero);
                    for (int i = 0; i < Position.directions.Length; i++)
                    {


                        Position cornerDir = Position.directions[i] + Position.RightNormal(Position.directions[i]);
                        Room neighbourRoom = world.GetRoom(room.RoomPosition + cornerDir);


                        if (Position.directions[i].x > 0 || Position.directions[i].y > 0 || neighbourRoom == null)
                        {
                            AddWallEntity(world, tileSize, room, cornerDir * (world.RoomSize / 2), $"RoomConer{cornerDir}");
                        }

                        //walls
                        if (room.roomSides[i] != RoomSide.OPEN)
                        {
                            neighbourRoom = world.GetRoom(room.RoomPosition + Position.directions[i]);
                            Position normal = Position.RightNormal(Position.directions[i]);
                            Position absNormal = new Position(Math.Abs(normal.x), Math.Abs(normal.y));

                            if (Position.directions[i].x > 0 || Position.directions[i].y > 0 || neighbourRoom == null)
                            {

                                if (room.roomSides[i] == RoomSide.CLOSED)
                                {
                                    Position size = absNormal * (world.RoomSize - wallWidth) + Position.directions[i - i % 2] * wallWidth;
                                    AddWallEntity(world, size, room, Position.directions[i] * (world.RoomSize / 2), $"RightDoorWall{Position.directions[i]}");
                                }
                                else
                                {
                                    Position size = absNormal * (world.RoomSize / 2 - wallWidth / 2 - doorWidth / 2) + Position.directions[i - i % 2] * wallWidth;
                                    int doorOffsetFromMiddle = world.RoomSize / 4 - wallWidth / 4 - doorWidth / 4 + doorWidth / 2;
                                    AddWallEntity(world, size, room, Position.directions[i] * (world.RoomSize / 2) + normal * doorOffsetFromMiddle, $"RightDoorWall{Position.directions[i]}");
                                    AddWallEntity(world, size, room, Position.directions[i] * (world.RoomSize / 2) + normal * -doorOffsetFromMiddle, $"LeftDoorWall{Position.directions[i]}");
                                }
                            }

                            if (room.roomSides[i] == RoomSide.DOOR)
                            {
                                Position size = absNormal * doorWidth + Position.directions[i - i % 2] * wallWidth;
                                room.AddDoor(new Entity(size, null, wallSprite, $"Door in room {room.RoomPosition}", tileSize, wallRenderPrio), Position.directions[i] * (world.RoomSize / 2));
                            }

                        }



                    }
                }

            }

    }

    private void AddWallEntity(World world, Position size, Room room, Position pos, string name)
    {
        world.AddEntity(new Entity(size, null, wallSprite, $"{name} in room {room.RoomPosition}", tileSize, wallRenderPrio), room.RoomPosition, pos);
    }


    public void GenerateRoomContents(World world)
    {
        List<Room> emptyRooms = new List<Room>();

        for (int x = 0; x < world.worldSize.x; x++)
            for (int y = 0; y < world.worldSize.y; y++)
            {
                Room room = world.GetRoom(new Position(x, y));
                if (room != null)
                    emptyRooms.Add(room);
            }

        foreach (RoomLogic room in roomsWithLogicToGenerate)
        {
            if (emptyRooms.Count > 0)
                room.Generate(world, random, emptyRooms, enemyGen, difficulty);
        }

    }



}

