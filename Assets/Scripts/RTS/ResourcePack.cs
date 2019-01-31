using UnityEngine;
using System.Collections;


public class ResourcePack : MonoBehaviour {
    public string name;
    public int techLevel; //Convert this to a getter
    public int amount;
    public float pickupRadius = 5f;
    public bool dropped;
    public ResourceType type;


	// Use this for initialization
	public void GetInfo (ResourceInfo containedResourceInfo) {
    
        name = containedResourceInfo.name;
        techLevel = containedResourceInfo.techLevel;
	}
    //TO DO - Add ability 
    public void Update()
    {

    }
    public void GetPickedUp(WorldObject worldObject)
    {
        //Add resources to inventory
        gameObject.SetActive(false);
        //Destroy(this.gameObject);
    }


}

public class PackagedWorldObject: ResourcePack
{
    public WorldObject packagedObject;


    public void InitializePackage()
    {
        name = packagedObject.gameObject.name;
        techLevel = packagedObject.techLevel;
        amount = 1;
        dropped = false;
    }
}

//Used for serializing resource packs and building things with blueprints
[System.Serializable]
public class ResourcePackData
{
    public ResourceType type;
    public int amount;
    //Automatically set dropped to false or remove it entirely
}