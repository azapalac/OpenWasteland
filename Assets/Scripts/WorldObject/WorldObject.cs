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

    public WorldObject UnPack()
    {
        return new WorldObject();
    }
}

public class ConstructionProject{
    public float timer;
    public string currentAction;
    public Blueprint blueprint;
}

//A world object is a unit, structure, or resource
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

    //Stats
    public int attackDamage;
    public int armor { get; set; }
    public float attackSpeed { get; set; }
    public int healthRegenRate;
    public int maxHitPoints;
    public float moveSpeed;
    public float maxRange;

    public Size size;
	public string objectName;
	public Image buildImage;
	public Image ordersBar;
    public int techLevel;
	public Vector3 center;
    public List<GameObject> commandButtons;
	protected GameObject selectionBoxTemplate, selectionBox;
	protected SpriteRenderer selectionBoxRenderer;
	protected Vector3 scale;
    public int hitPoints;

    public Vector3 source, destination;
    public List<Blueprint> loadedBlueprints;
    public int blueprintLimit;

    //Limit to the number of active construction projects
    public int activeBlueprintLimit = 3;

    //Limit to the number of queued actions
    public int constructionQueueLimit = 5;

    public List<Resource> rebuildValue, sellValue, resourceYield, resourceInventory;

    public List<Blueprint> activeBlueprints;
    public List <float> currentActionTimers;
    public List<string> activeActionList;
    public List<ConstructionProject> queuedConstructionProjects;

	public Player owner;
	public List<string> actions;

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
        loadedBlueprints = new List<Blueprint>();

        activeBlueprints = new List <Blueprint>();
        currentActionTimers = new List <float>();
        activeActionList = new List<string>();
        queuedConstructionProjects = new List<ConstructionProject>();
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

        for(int i = 0; i < activeActionList.Count; i++)
        {
            this.ContinueAction(i);
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

    public Resource Pack()
    {
        return new Resource();
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
	public virtual void LeftMouseClick(GameObject hitObject, Vector3 hitPoint, Player controller){
        //Only handle input if currently selected
        if (currentlySelected && hitObject.name != "Ground")
        {
            WorldObject worldObject = hitObject.transform.root.GetComponent<WorldObject>();

            if (worldObject) ChangeSelection(worldObject, controller);
        }
      
	}

    public virtual void RightMouseClick(GameObject hitObject, Vector3 hitPoint, Player controller )
    {


        if (currentlySelected && hitObject.name == "Ground")
        {
            //Move the object, if possible
            StartMoving(hitPoint);

        }else if(currentlySelected && hitObject.layer == ResourceManager.WorldObjectLayer)
        {
            WorldObject worldObject = hitObject.GetComponent<WorldObject>();
            //if the object is owned by someone
            if(worldObject.owner != null)
            {
                //Determine action based on allegiance
                if(worldObject.owner == controller)
                {
                    //Move to the friendly unit or building
                    //If carrying resources, drop them off
                }
            }
            if (hitObject.tag == "Resource")
            {
                if (CanDo("Harvest " + hitObject.GetComponent<HarvestableObject>().type))
                {
                    DealDamage(hitObject.GetComponent<HarvestableObject>());
                }
            }
        }
    }
    protected void StartAttacking(Vector3 enemyPoint)
    {

    }
    protected void StartMoving(Vector3 hitPoint)
    {
        if (CanDo("Move"))
        {
            source = transform.position;
            destination = hitPoint;
        }
    }
	private void ChangeSelection(WorldObject worldObject, Player controller){
		SetSelection(false);
		if(controller.SelectedObject) controller.SelectedObject.SetSelection(false);
		controller.SelectedObject = worldObject;
		worldObject.SetSelection(true);
	}
}
