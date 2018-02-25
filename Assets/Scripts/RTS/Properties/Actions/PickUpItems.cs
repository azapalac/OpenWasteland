using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpItems : Action
{
    public Inventory inventory;
    public override ActionType Type { get { return ActionType.PickUpItems; } }
    private Resource resource;

    public void SetUpPickup(Resource resourceToPickUp)
    {
        resource = resourceToPickUp;
        active = true;
    }

    public override void Execute(WorldObject worldObj)
    {
        if (active)
        {
            inventory.AddResource(resource);

            Stop();
        }
    }

    public PickUpItems(int capacity)
    {
        inventory = new Inventory(capacity);
    }


}