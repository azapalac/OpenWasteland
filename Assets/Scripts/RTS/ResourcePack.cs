using UnityEngine;
using System.Collections;


public class ResourcePack : MonoBehaviour {
    public Resource containedResource;
    public string containedResourceName;
    public int containedResourceAmount;
    public float pickupRadius = 5f;
    public bool dropped;
	// Use this for initialization
	public void Initialize () {
     //   containedResource = ObjectManager.GetResource(containedResourceName);
        
	}
	

    public void GetPickedUp(WorldObject worldObject)
    {
        //Add resources to inventory

        Destroy(this.gameObject);
    }


}
