using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using RTS;

public class Resource{
	public string name { get; set; }
	public int dropAmount { get; set; }
    public Rarity rarity { get; set; }
    public int TechLevel { get; set; }


	public override string ToString(){
		return "  " + dropAmount + " " + name;
	}
}

public enum Rarity
{
    Junk,
    Common,
    Uncommon,
    Rare, 
    Treasured,
    Legendary,
    Unobtanium
}
public class WorldObject : MonoBehaviour {
    public enum Size
    {
        Tiny,
        Small,
        Medium,
        Large, 
        Massive,
        Gigantic
    }
    public Size size;
	public string objectName;
	public Image buildImage;
	public Image ordersBar;
    int techLevel;
	public Vector3 center;
	public Button[] commands;
	protected GameObject selectionBoxTemplate, selectionBox;
	protected SpriteRenderer selectionBoxRenderer;
	protected Vector3 scale;
	public int  hitPoints, maxHitPoints;
    public int attackDamage;
    public float moveSpeed;
    public Vector3 source, destination;

	public List<Resource> cost, rebuildValue, sellValue, resourceYield, resourceInventory;
	public List<List<Resource>> upgradeValues;
    public float maxRange;
	public Player owner;
	protected List<string> actions;
	protected bool currentlySelected = false;
	//public List<
	// Use this for initialization
	protected virtual void Awake(){

	}
	protected virtual void Start () {
        actions = new List<string>();
        resourceInventory = new List<Resource>();
        gameObject.layer = ResourceManager.WorldObjectLayer;
        //Retrieve tech level here
		center = new Vector3(this.transform.position.x, this.transform.position.y + 1, this.transform.position.z);
		selectionBoxTemplate = ResourceManager.GetSelectionBox;
		selectionBox = GameObject.Instantiate(selectionBoxTemplate, center, Quaternion.identity) as GameObject;
		selectionBox.transform.Rotate(new Vector3(90, 0, 0));
		scale = this.gameObject.GetComponent<Collider>().bounds.size;
		selectionBox.transform.localScale = scale;
		selectionBoxRenderer = selectionBox.GetComponent<SpriteRenderer>();
		selectionBox.transform.parent = this.transform.root;

        
	}

	// Update is called once per frame
	protected virtual void Update () {

		DrawSelection();
        
        if (CanDo("Move") && Vector3.Distance(transform.position, destination) > moveSpeed*Time.deltaTime*2)
        {
            Vector3 dir = destination - source;
            dir.Normalize();
            transform.Translate(dir * moveSpeed * Time.deltaTime);
        }

    }

    protected float scalingFactor
    {
        get
        {
            return selectionBoxRenderer.bounds.extents.magnitude;
        }
    }

    public virtual void DrawSelection(){
		if(currentlySelected){
			//TODO: Change this to be red for enemies and green for allies
			selectionBoxRenderer.color = Color.black;
		}else{
			selectionBoxRenderer.color = Color.clear;
		}
	}

    public virtual List<Resource> getDestructionYield()
    {
        //The parent function just returns an empty list
        return new List<Resource>();
    }
    
    public virtual void DealDamage(WorldObject otherObject)
    {
        //For now this is very simple. All units will have infinite range. Will add range and DPS mechanics later
 
        Debug.Log("Attacking");
        otherObject.TakeDamage(attackDamage);
    }
    public virtual void TakeDamage(int damage)
    {
        //Override this once shields and armor are figured out (for units)
        if(CanDo("Take Damage"))
        {
            hitPoints -= damage;
            if(hitPoints <= 0)
            {
                //Change this later
                Destroy();
            }
        }
    }
	public void SetSelection(bool selected){
		currentlySelected = selected;

	}
    public virtual void SpawnLoot()
    {

    }

    public void PickUpLoot(ResourcePack loot)
    {
        resourceInventory.Add(loot.containedResource);
        Destroy(loot.gameObject);
    }

    public virtual void Destroy()
    {
        Destroy(this.gameObject);
    }
    //Determine whether the object can perform a specific action
    public bool CanDo(string s)
    {
        return actions.Contains(s);
    }

	public List<string> GetActions(){
		return actions;
	}

    ///TODO: Refactor this to work with context sensitive clicking
    ///For now, basic is just fine
	public virtual void MouseClick(GameObject hitObject, Vector3 hitPoint, Player controller){
        //Only handle input if currently selected
        if (currentlySelected && hitObject.name != "Ground")
        {
            WorldObject worldObject = hitObject.transform.root.GetComponent<WorldObject>();

            //if (worldObject) ChangeSelection(worldObject, controller);
        }

        if (currentlySelected && hitObject.name == "Ground")
        {
            //Move the object, if possible
            if (CanDo("Move"))
            {
                source = transform.position;
                destination = hitPoint;
            }
        }

         if (currentlySelected && hitObject.tag == "Resource")
        {
            if (CanDo("Harvest " + hitObject.GetComponent<HarvestableObject>().type))
            {
                DealDamage(hitObject.GetComponent<HarvestableObject>());
            }
        }
	}


	private void ChangeSelection(WorldObject worldObject, Player controller){
		SetSelection(false);
		if(controller.SelectedObject) controller.SelectedObject.SetSelection(false);
		controller.SelectedObject = worldObject;
		worldObject.SetSelection(true);
	}
}
