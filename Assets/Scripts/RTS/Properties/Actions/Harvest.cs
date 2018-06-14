using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Harvest : Action
{

    public float harvestTime;
    private float harvestTimer;
    public int harvestAmount;
    private DropLoot targetDropLoot;
    public override ActionType Type { get { return ActionType.Harvest; } }
    //There's only one harvest range, melee.

    public override void SetUpRightClick(Vector3 hitPoint, GameObject clickedObject)
    {
        WorldObject harvestTarget = clickedObject.GetComponent<WorldObject>();
        if (harvestTarget != null)
        {

            //TODO: Check range and queue movement if out of range

            if (!worldObject.IsDoing(this) && harvestTarget.CanDo(ActionType.DropLoot))
            {
                harvestTimer = 0;
                targetDropLoot = harvestTarget.GetComponent<DropLoot>();
                worldObject.StartDoing(this);
                active = true;
            }
            else
            {

            }
        }
    }

    public override void Execute(WorldObject worldObj)
    {
       //Loot is only dropped when things die
      //If target still exists, keep going. Otherwise stop.
      if(targetDropLoot == null)
        {
            active = false;
            worldObject.StopDoing(this);
            return;
        }

      harvestTimer += Time.deltaTime;
      if (harvestTimer >= harvestTime)
      {
            targetDropLoot.SetUpDropLoot(harvestAmount);
            harvestTimer = 0;

           
      }
        
    }

   
}
