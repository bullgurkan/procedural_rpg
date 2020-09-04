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
    int positionUnitsPerUnityUnit;
    int cameraEntityToFollowId = -1;

    public UnityWorldRenerer(SpriteRenderer entityObjectPrefab, Transform worldTransform, Dictionary<string, Sprite> spriteRegistry, int positionUnitsPerUnityUnit)
    {
        this.entityObjectPrefab = entityObjectPrefab;
        this.worldTransform = worldTransform;
        this.spriteRegistry = spriteRegistry;
        this.positionUnitsPerUnityUnit = positionUnitsPerUnityUnit;
        currentlyUsedObjects = new Dictionary<int, SpriteRenderer>();

        unusedObjects = new Stack<SpriteRenderer>();
    }
    public override void AddEntity(Entity entity, World world, bool shouldCameraFollow = false)
    {
        if(unusedObjects.Count > 0)
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
        currentlyUsedObjects[entity.Id].gameObject.SetActive(true);
        currentlyUsedObjects[entity.Id].transform.localScale = ToVector2(entity.RenderSize);

        if (shouldCameraFollow)
        {
            cameraEntityToFollowId = entity.Id;
        }
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
        return new Vector2(pos.x, pos.y)/ positionUnitsPerUnityUnit;
    }
}

