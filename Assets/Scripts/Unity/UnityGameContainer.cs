using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class UnityGameContainer : MonoBehaviour
{
    public SpriteRenderer entityObjectPrefab;
    public List<Sprite> sprites;

    Vector2 input;
    Entity e;
    World world;
    private void Start()
    {
        Dictionary<string, Sprite> spriteReg = new Dictionary<string, Sprite>();
        spriteReg.Add("default", sprites[0]);
        UnityWorldRenerer worldRenerer = new UnityWorldRenerer(entityObjectPrefab, transform, spriteReg, 60);

        world = new World(10,10,500,worldRenerer);

        e = new Entity(Position.one * 10, name: "Player");
        world.AddEntity(e, Position.zero, Position.zero, true);
        world.AddEntity(new Entity(Position.one * 10, name: "Rock"), Position.zero, Position.down * 10, true);

        e.MoveInLine(Position.down, 2, world, false);
        //e.MoveInLine(Position.up, 2, world);

        world.Tick();
    }

    void Update()
    {
        input += new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")) * Time.deltaTime * 60;
    }

    private void FixedUpdate()
    {
        //input = input.normalized * 4;
        e.MoveInLine(new Position((int)input.x, (int)input.y), 2, world, true);
        input = Vector2.zero;
    }
}

