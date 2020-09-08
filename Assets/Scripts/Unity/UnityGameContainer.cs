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
    public List<string> spriteIds;

    Vector2 input;
    Entity e;
    World world;
    private void Start()
    {
        Dictionary<string, Sprite> spriteReg = new Dictionary<string, Sprite>();
        for (int i = 0; i < sprites.Count; i++)
        {
            spriteReg.Add(spriteIds[i], sprites[i]);
        }
        //spriteReg.Add("default", sprites[0]);
        UnityWorldRenerer worldRenerer = new UnityWorldRenerer(entityObjectPrefab, transform, spriteReg, 1000);

        world = new World(10,10,7600,worldRenerer);

        e = new Entity(Position.one * 400, name: "Player");
        world.AddEntity(e, Position.zero, Position.zero, true);
        world.AddEntity(new Entity(Position.one * 400, name: "Rock"), Position.zero, Position.down * 400);

        e.MoveInLine(Position.down, 2, world, false);

        Projectile p = new Projectile(Position.one * 300, new Position(1, 0), name:"Proj");
        world.AddEntity(p, Position.zero, Position.up * 800);
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
        if(input != Vector2.zero)
            e.MoveInLine(new Position((int)input.x, (int)input.y), 1000, world, true);
        input = Vector2.zero;

        world.Tick();
    }
}

