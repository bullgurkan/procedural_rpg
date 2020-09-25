


public class ItemPickup : Entity
{
    Item item;
    public ItemPickup(Item item) : base(Position.one * 500, spriteId: item.spriteId, renderPriority: WorldRenderer.RenderPriority.PICKUP, tag:TagType.PICKUP, isTrigger:true)
    {
        this.item = item;
    }

    public Item PickupItem(World world)
    {
        world.QueueEntityRemoval(Id);
        return item;
    }
}