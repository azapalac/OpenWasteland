using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropLoot : Action
{
    public float dropRadius;

    public bool TestLootDrop;
    public static int ObjectDestroyed { get { return -1; } }
    public int lootDropThreshold;
    public List<ResourceDrop> lootToDrop;
    private int harvestDamageToTake;


    public override ActionType Type { get { return ActionType.DropLoot; } }

    void Update()
    {
        if (TestLootDrop)
        {
            DropAllResources(WorldObject.Size.Medium);
            TestLootDrop = false;
        }
    }

    private void MultiplyAmounts(ResourcePack drop, float factor)
    {
       
    }

    //Doesn't matter if the loot drop is calculated when the object is created or when it is destroyed.
    //Every object can only drop once. Stretch goal - Minor loot drops when object is losing 
    //This doesn't overload SetUpRightClick
    public void SetUpDropLoot(int harvestDamage)
    {
        if (!worldObject.IsDoing(this))
        {
            harvestDamageToTake = harvestDamage;
            active = true;

        }
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
            worldObject.UnloadAction(this);
        }

    }

    public void DropAllResources(WorldObject.Size worldObjectSize)
    {
        //Drop all contained resources
        for (int i = 0; i < lootToDrop.Count; i++)
        {
            //Note prefabs are loaded by GetResourcePack

            ResourcePack pack = lootToDrop[i].GetResourcePack(worldObjectSize);
            //Don't instantiate the pack if it didn't drop
            if (pack.dropped)
            {
              

                Vector3 positionToSpawn = transform.position + RandomPointInCicle(dropRadius);

                GameObject g = Instantiate(pack.gameObject, positionToSpawn , Quaternion.identity);
            }
        }

    }


    private Vector3 RandomPointInCicle(float radius)
    {
        var angle = Random.Range(0f, 1f) * Mathf.PI * 2;
        radius = Mathf.Sqrt(Random.Range(0f, 1f)) * radius;
        var x =  radius * Mathf.Cos(angle);
        var y =  radius * Mathf.Sin(angle);
        return new Vector3(x, 0, y);
    }
}
