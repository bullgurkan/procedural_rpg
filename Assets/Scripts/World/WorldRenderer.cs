using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public abstract class WorldRenderer
{
    public abstract void AddEntity(Entity entity, World world, bool shouldCameraFollow = false);
    public abstract void RemoveEntity(int id);
    public abstract void UpdateEntityPosition(Entity entity, World world);
    public abstract void UpdateEntitySprite(Entity entity);
    public abstract void ClearWorld();


    //Gui
    public abstract void UpdateHealthBar(Character player);
    public abstract void UpdatePlayerStatDisplay(Character player);
    public abstract void AddItemComparisonOverlay(Item item1, Item item2, World world);

    public enum RenderPriority
    {
        FLOOR, EXIT, WALL, PICKUP, DEFAULT, PROJECTILE, FOREGROUND
    }

   
}

