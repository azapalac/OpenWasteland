﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


    public enum ActionType
    {
        Default,
        Move,
        Attack,
        TakeDamage,
        Harvest,
        DropLoot,
        Build,
        Repair

    }
    public enum Armor
    {
        Unarmored
    }

public class Action : Property
{ 


   protected bool active;

   public bool Active { get { return active; } }
   public virtual ActionType Type { get { return ActionType.Default; } }
            

   public virtual void SetUpRightClick(Vector3 hitPoint, GameObject clickedObject)
   {
        //Some actions have default right click triggers, others don't
   }
   

   public virtual void Execute(WorldObject worldObj)
   {
      if (!active)
       {
           return;
       }

   }
            
            

    public void Stop()
    {
      active = false;
    }

}
