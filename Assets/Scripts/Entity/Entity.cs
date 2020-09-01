using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Entity
{
    public Position PositionOfCurrentRoom { get; private set; }

    //size and pos in room will be scaled by world accuracy
    public  Position PositionInRoom { get; private set; }
    public Position Size { get; private set; }
    public int Id { get; set; }
    public int SpriteId;

    public void SetPositionInRoom(Position pos, World world, bool force)
    {
        Position oldPos = PositionInRoom;
        //world.IsColliding(size, );
        //positionInRoom = world
    }



}

