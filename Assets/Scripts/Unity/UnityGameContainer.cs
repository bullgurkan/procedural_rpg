﻿using System;
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
    bool activated;
    Vector2 mousePos;
    Character e;
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

        e = new Character(Position.one * 400, 760, name: "Player");

        List<Character> players = new List<Character>();
        players.Add(e);

        world = new World(10, 10, 7600, worldRenerer, players);

        Item item = new Item();
        item.actions.Add(Effect.EventType.ON_ACTIVATION, new SpawnProjectileAction(new Position(100, 100), 10));


        e.ChangeEquipment(item);

    }

    void Update()
    {
        input += new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")) * Time.deltaTime * 60;
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            mousePos = Input.mousePosition;
            activated = true;
        }

    }

    private void FixedUpdate()
    {
        //input = input.normalized * 4;
        if (input != Vector2.zero)
            e.MoveInLine(new Position((int)input.x, (int)input.y), 100, world, true);
        input = Vector2.zero;

        if (activated)
        {
            e.Activate(world, new Position((int)mousePos.x, (int)mousePos.y));
            activated = false;
        }
           

        world.Tick();
    }
}

