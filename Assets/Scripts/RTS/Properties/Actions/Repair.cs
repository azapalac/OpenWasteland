using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Repair : Action
{
    public override ActionType Type { get { return ActionType.Repair; } }
    public float repairTime;
    public int repairAmount;
    private float repairTimer;
    WorldObject repairTarget;

    public void SetUpRepair(WorldObject target)
    {
        repairTimer = 0;
        repairTarget = target;

    }

    public override void Execute(WorldObject worldObj)
    {
        if (repairTarget.CanDo(ActionType.TakeDamage))
        {
            repairTimer += Time.deltaTime;
            if (repairTimer > repairTime)
            {
                //Post healing event to target
            }
        }
    }

    public Repair(float repairTime, int repairAmount)
    {
        this.repairTime = repairTime;
        this.repairAmount = repairAmount;

    }
}


