using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class SpawnProjectileAction : Action
{
    Position size;
    int speed;
    public SpawnProjectileAction(Position size, int speed)
    {
        this.size = size;
        this.speed = speed;
    }
    public override void OnActivation(World world, Character character, Position room, Position positionInRoom)
    {
        Position delta = world.ConvertPositionBetweenRooms(positionInRoom, room, character.CurrentRoom) - character.PositionInRoom;
        
        world.AddEntity(new Projectile(size, delta * speed / delta.Magnitude), character.CurrentRoom, character.PositionInRoom + delta * character.Size.x / delta.Magnitude);
    }
}

