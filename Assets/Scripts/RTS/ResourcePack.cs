using UnityEngine;
using System.Collections;
using RTS;
public class ResourcePack : MonoBehaviour {
    public Resource containedResource;
    public string containedResourceName;
    public int containedResourceAmount;
    public float pickupRadius = 5f;
	// Use this for initialization
	public void Initialize () {
        containedResource = ObjectManager.GetResource(containedResourceName);
        
	}


    public void CloneResource(Resource r) {
        containedResource = r;
        containedResourceAmount = r.dropAmount;
    }
	// Update is called once per frame
	void Update () {
        Collider[] colliders = Physics.OverlapSphere(transform.position, pickupRadius);

        for(int i = 0; i < colliders.Length; i++)
        {
            
            if(colliders[i].gameObject.layer == ResourceManager.WorldObjectLayer)
            {
                WorldObject worldObject = colliders[i].gameObject.GetComponent<WorldObject>();
                if (worldObject.CanDo("Pick Up Resources"))
                {
                    worldObject.PickUpLoot(this);
                }
            }
        }
	}

    public void PickUp(WorldObject worldObject)
    {
        //Add resources to inventory

        Destroy(this.gameObject);
    }


}
