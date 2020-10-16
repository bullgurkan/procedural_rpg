using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class UnityGameContainer : MonoBehaviour
{
    public SpriteRenderer entityObjectPrefab;
    public HealthbarManager hpMan;
    public ItemComparer itemComp;
    public List<Sprite> sprites;
    public List<string> spriteIds;
    public float tps = 1;


    Vector2 input;
    bool activated;
    bool pickupItem;
    Vector2 mousePos;
    Character e;
    World world;
    UnityWorldRenerer worldRenerer;

    float timer = 0;
    private void Start()
    {
        Dictionary<string, Sprite> spriteReg = new Dictionary<string, Sprite>();
        for (int i = 0; i < sprites.Count; i++)
        {
            spriteReg.Add(spriteIds[i], sprites[i]);
        }
        //spriteReg.Add("default", sprites[0]);
        worldRenerer = new UnityWorldRenerer(entityObjectPrefab, transform, spriteReg, 800, hpMan, itemComp);

        e = new Character(Position.one * 400, 760, name: "Player");

        List<Character> players = new List<Character>();
        players.Add(e);

        world = new World(10, 10, 7600, worldRenerer, players, new WorldGenerator(1, 760, 3, "wall", "floor", 40, 4));

        Item item = new Item(Character.Slot.WEAPON);
        item.actions.Add(Effect.EventType.ON_ACTIVATION, new CooldownAction(item, new SpawnProjectileAction(item, new Position(100, 100), 20, new DamageAction(item, EntityLiving.Stat.ATTACK_POWER, EntityLiving.Stat.ARMOR)), item));
        item.stats.Add(EntityLiving.Stat.ATTACK_POWER, 10);
        item.stats.Add(EntityLiving.Stat.LIFESTEAL, 3);
        item.stats.Add(EntityLiving.Stat.MAX_HEALTH, 50);

        e.ChangeEquipment(item, worldRenerer);

        worldRenerer.AddItemComparisonOverlay(item, item, world);

    }

    void Update()
    {
        input += new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        if (Input.GetKey(KeyCode.Mouse0))
        {
            mousePos = Input.mousePosition;
            activated = true;
        }

        if (Input.GetKeyDown(KeyCode.E))
            pickupItem = true;

        timer += Time.deltaTime;

        if (timer > 1 / tps)
        {
            timer -= 1 / tps;

            if (input != Vector2.zero)
            {
                Vector2 normInput = input.normalized * 2;

                e.MoveInLine(new Position((int)normInput.x, (int)normInput.y), (int)(e.GetStat(EntityLiving.Stat.MOVEMENT_SPEED) * ((normInput.x != 0 && normInput.y != 0) ? 1.5f : 1)), world, true);
            }

            input = Vector2.zero;

            if (activated)
            {
                e.Activate(world, worldRenerer.FromVector2ToPlayerPosition(Camera.main.ScreenToWorldPoint(mousePos), world));
                activated = false;
            }

            if (pickupItem)
            {
                e.OpenItemCompareOverlay(world);
                pickupItem = false;
            }


            world.Tick();
        }
    }

    private void FixedUpdate()
    {
        //input = input.normalized * 4;

    }
}

