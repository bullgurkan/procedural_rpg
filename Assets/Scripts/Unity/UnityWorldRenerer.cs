using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class UnityWorldRenerer : WorldRenderer
{

    Dictionary<int, SpriteRenderer> currentlyUsedObjects;
    Stack<SpriteRenderer> unusedObjects;
    SpriteRenderer entityObjectPrefab;
    Transform worldTransform;
    Dictionary<string, Sprite> spriteRegistry;
    HealthbarManager hpMan;
    ItemComparer itemComp;
    int positionUnitsPerUnityUnit;
    int cameraEntityToFollowId = -1;

    public UnityWorldRenerer(SpriteRenderer entityObjectPrefab, Transform worldTransform, Dictionary<string, Sprite> spriteRegistry, int positionUnitsPerUnityUnit, HealthbarManager hpMan, ItemComparer itemComp)
    {
        this.entityObjectPrefab = entityObjectPrefab;
        this.worldTransform = worldTransform;
        this.spriteRegistry = spriteRegistry;
        this.positionUnitsPerUnityUnit = positionUnitsPerUnityUnit;
        this.hpMan = hpMan;
        this.itemComp = itemComp;
        currentlyUsedObjects = new Dictionary<int, SpriteRenderer>();

        unusedObjects = new Stack<SpriteRenderer>();
    }
    public override void AddEntity(Entity entity, World world, bool shouldCameraFollow = false)
    {
        if (unusedObjects.Count > 0)
        {
            currentlyUsedObjects.Add(entity.Id, unusedObjects.Pop());
        }
        else
        {
            currentlyUsedObjects.Add(entity.Id, GameObject.Instantiate(entityObjectPrefab, worldTransform));
        }

        currentlyUsedObjects[entity.Id].name = entity.Name ?? entity.Id.ToString();
        currentlyUsedObjects[entity.Id].sprite = entity.SpriteId != null ? (spriteRegistry.ContainsKey(entity.SpriteId) ? spriteRegistry[entity.SpriteId] : spriteRegistry["default"]) : spriteRegistry["default"];
        currentlyUsedObjects[entity.Id].transform.localPosition = EntityPosToUnityPos(entity, world);
        currentlyUsedObjects[entity.Id].sortingOrder = (int)entity.RenderPrio;


        if (entity.TileSize != Position.zero)
        {
            currentlyUsedObjects[entity.Id].drawMode = SpriteDrawMode.Tiled;
            currentlyUsedObjects[entity.Id].transform.localScale = ToVector2(entity.TileSize);
            currentlyUsedObjects[entity.Id].size = new Vector2(entity.RenderSize.x / entity.TileSize.x, entity.RenderSize.y / entity.TileSize.y);
        }
        else
        {
            currentlyUsedObjects[entity.Id].size = Vector2.one;
            currentlyUsedObjects[entity.Id].transform.localScale = ToVector2(entity.RenderSize);
        }


        if (shouldCameraFollow)
        {
            cameraEntityToFollowId = entity.Id;
            UpdateEntityPosition(entity, world);
        }


        currentlyUsedObjects[entity.Id].gameObject.SetActive(true);
    }

    public override void UpdateEntityPosition(Entity entity, World world)
    {

        if (currentlyUsedObjects.ContainsKey(entity.Id))
        {
            currentlyUsedObjects[entity.Id].transform.localPosition = EntityPosToUnityPos(entity, world);
            if (entity.Id == cameraEntityToFollowId)
                Camera.main.transform.position = new Vector3(currentlyUsedObjects[entity.Id].transform.position.x, currentlyUsedObjects[entity.Id].transform.position.y, Camera.main.transform.position.z);

        }

    }

    public override void RemoveEntity(int id)
    {
        currentlyUsedObjects[id].gameObject.SetActive(false);
        unusedObjects.Push(currentlyUsedObjects[id]);
        currentlyUsedObjects.Remove(id);
    }


    private Vector2 EntityPosToUnityPos(Entity entity, World world)
    {
        return ToVector2(entity.PositionInRoom) + ToVector2(entity.CurrentRoom) * world.RoomSize;
    }
    private Vector2 ToVector2(Position pos)
    {
        return new Vector2(pos.x, pos.y) / positionUnitsPerUnityUnit;
    }

    public Position FromVector2ToPlayerPosition(Vector2 pos, World world)
    {

        return new Position((int)(pos.x * positionUnitsPerUnityUnit), (int)(pos.y * positionUnitsPerUnityUnit)) - world.GetMainPlayer.CurrentRoom * world.RoomSize;
    }

    public override void ClearWorld()
    {
        List<int> allObjects = new List<int>(currentlyUsedObjects.Keys);
        foreach (var i in allObjects)
        {
            RemoveEntity(i);
        }
    }

    public override void UpdateEntitySprite(Entity entity)
    {

        if (currentlyUsedObjects.ContainsKey(entity.Id))
            currentlyUsedObjects[entity.Id].sprite = entity.SpriteId != null ? (spriteRegistry.ContainsKey(entity.SpriteId) ? spriteRegistry[entity.SpriteId] : spriteRegistry["default"]) : spriteRegistry["default"];
        else
            Debug.Log("Entity does not exist");

    }

    public override void UpdateHealthBar(Character player)
    {
        hpMan.SetHealth(player.GetStat(EntityLiving.Stat.MAX_HEALTH), player.Health);
    }

    public override void UpdatePlayerStatDisplay(Character player)
    {
        UpdateHealthBar(player);
    }

    public override void AddItemComparisonOverlay(Item item1, Item item2, World world)
    {
        itemComp.SetitemsToCompare(item1, item2);
    }
}

