using UnityEngine;
using System.Collections.Generic;
using RTS;
public class HarvestableObject : WorldObject {
    //Get drop amount from database, then multiply by size factor (also in database)
    public string type;

    protected override void Start()
    {
        base.Start();
       
    }




    //Get list and multiply it by size factor. Do this in Actions instead
  /*  public override List<Resource> getDestructionYield()
    {

        List<Resource> yield = ObjectManager.ResourceDrops[type];

        switch (this.size)
        {
            case Size.Tiny:
                MultiplyAmounts(yield, 0.2f);
                break;

            case Size.Small:
                MultiplyAmounts(yield, 0.6f);
                break;

            case Size.Medium:
                MultiplyAmounts(yield, 1f);
                break;

            case Size.Large:
                MultiplyAmounts(yield, 2.5f);
                break;


            case Size.Massive:
                MultiplyAmounts(yield, 4f);
                break;

            case Size.Gigantic:
                MultiplyAmounts(yield, 10f);
                break;

        }
        return yield;
    }*/
  

    public GameObject SpawnLootPack(Resource resource)
    {
        GameObject LootPack = new GameObject();

        if (GameManager.DropDictionary.ContainsKey(resource.name))
        {
            LootPack = GameManager.DropDictionary[resource.name];
        }
        else
        {
            Debug.LogError("Resource not contained in drop dictionary");
        }

        
        return LootPack;
    }


    private void MultiplyAmounts(List<Resource> yield, float factor)
    {
        foreach(Resource resource in yield)
        {
            resource.dropAmount = Mathf.RoundToInt(resource.dropAmount * factor);
        }
    }
}
