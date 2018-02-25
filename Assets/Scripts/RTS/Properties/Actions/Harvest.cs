using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Harvest : Action
{
    private List<string> HarvestableObjects;

    private float harvestTime; //Add getters to these if we need to, currently unsure if necessary
    private float harvestTimer;
    private int harvestAmount;
    private WorldObject target;
    public override ActionType Type { get { return ActionType.Harvest; } }
    //There's only one harvest range, melee.

    public void SetUpHarvest(WorldObject harvestTarget)
    {
        harvestTimer = 0;
        target = harvestTarget;
    }

    public override void Execute(WorldObject worldObj)
    {
        if (target.CanDo(ActionType.DropLoot) && HarvestableObjects.Contains(target.objectName))
        {

            harvestTimer += Time.deltaTime;
            if (harvestTimer >= harvestTime)
            {
                //Post loot drop event for target
                harvestTimer = 0;

            }
        }
        else
        {
            //Post error message
        }

    }

    public Harvest(int harvestAmount, float harvestTime)
    {
        this.harvestAmount = harvestAmount;
        this.harvestTime = harvestTime;
    }
}
