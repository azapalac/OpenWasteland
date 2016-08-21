using UnityEngine;
using System.Collections.Generic;
using RTS;
public class HarvestableObject : WorldObject {
    //Get drop amount from database, then multiply by size factor (also in database)
    public string type;

    protected override void Start()
    {
        base.Start();
        //Get type from database dictionary, based on name
        //i.e. "Large Scrap pile" -> "Scrap"

        //
        ActionManager.AddAction(actions, "Take Damage");

    }




    //Get list and multiply it by size factor
    public override List<Resource> getDestructionYield()
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
    }
    public override void Destroy()
    {
        SpawnLoot();
        base.Destroy();
    }

    public override void SpawnLoot()
    {
        List<Resource> loot = getDestructionYield();
        foreach(Resource r in loot)
        {
            //Multiply this by some scaling factor
            Vector3 delta = Random.insideUnitCircle * scalingFactor;
            if (r.dropAmount > 0)
            {

                GameObject G = Instantiate(SpawnLootPack(r), this.transform.position + delta, Quaternion.identity) as GameObject;
                G.GetComponent<ResourcePack>().CloneResource(r);
            }
        }

    }

    public GameObject SpawnLootPack(Resource resource)
    {
        
        GameObject LootPack = GameManager.PrefabDictionary[resource.name];

        
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
