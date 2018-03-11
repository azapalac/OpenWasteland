using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropLoot : Action
{
    public static int ObjectDestroyed { get { return -1; } }
    public int lootDropThreshold;
    public List<ResourceDrop> lootToDrop;
    private int harvestDamageToTake;

    public override ActionType Type { get { return ActionType.DropLoot; } }

    

    private void MultiplyAmounts(ResourcePack drop, float factor)
    {
       
    }

    //Doesn't matter if the loot drop is calculated when the object is created or when it is destroyed.
    //Every object can only drop once. Stretch goal - Minor loot drops when object is losing 
    public void SetUpDropLoot(int harvestDamage)
    {
        harvestDamageToTake = harvestDamage;
        active = true;
    }

    public override void Execute(WorldObject worldObj)
    {
        if (active)
        {
            lootDropThreshold -= harvestDamageToTake;
            if (lootDropThreshold <= 0 || harvestDamageToTake == ObjectDestroyed)
            {
                active = false;
                DropAllResources(worldObj.size);
            }
        }

    }

    public void DropAllResources(WorldObject.Size worldObjectSize)
    {
        //Drop all contained resources


        List<ResourcePack> yield = new List<ResourcePack>();
        for(int i = 0; i < lootToDrop.Count; i++) {


            switch (worldObjectSize)
            {
                /*
                case WorldObject.Size.Tiny:
                    MultiplyAmounts(lootToDrop[i], 0.2f);
                    break;

                case WorldObject.Size.Small:
                    MultiplyAmounts(yield, 0.6f);
                    break;

                case WorldObject.Size.Medium:
                    MultiplyAmounts(yield, 1f);
                    break;

                case WorldObject.Size.Large:
                    MultiplyAmounts(yield, 2.5f);
                    break;


                case WorldObject.Size.Massive:
                    MultiplyAmounts(yield, 4f);
                    break;

                case WorldObject.Size.Gigantic:
                    MultiplyAmounts(yield, 10f);
                    break;*/
            }

        }

        //Get Yield amounts from and assign them

    }

}
