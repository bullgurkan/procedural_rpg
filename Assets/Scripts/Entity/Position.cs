using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public struct Position
{
    public int x, y;

    public static Position zero = new Position(0, 0);
    public static Position one = new Position(1, 1);
    public static Position right = new Position(1, 0);
    public static Position left = new Position(-1, 0);
    public static Position up = new Position(0, 1);
    public static Position down = new Position(0, -1);
    public static Position[] directions = { right, left, up, down };



    public Position(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public static Position operator *(Position a, int b) => new Position(a.x * b, a.y * b);
    public static Position operator /(Position a, int b) => new Position(a.x / b, a.y / b);
    public static Position operator -(Position a) => a * -1;
    public static Position operator +(Position a) => a;
    public static Position operator +(Position a, Position b) => new Position(a.x + b.x, a.y + b.y);
    public static Position operator -(Position a, Position b) => a + (-b);

    public static bool operator ==(Position a, Position b) => a.x == b.x && a.y == b.y;
    public static bool operator !=(Position a, Position b) => !(a == b);
    public static float Dot(Position a, Position b) => a.x * b.x + a.y * b.y;
    public static Position RightNormal(Position a) => new Position(-a.y, a.x);

    public override string ToString() => $"({x}, {y})";
    public override bool Equals(object obj) => base.Equals(obj);
    public override int GetHashCode() => base.GetHashCode();

}

