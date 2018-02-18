using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using RTS;




//A world object is a unit, structure, or resource. Any object in the world that can be selected and interacted with
//Sets up the generic WorldObject architecture
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
	public Image icon;
	public Image ordersBar;
    public int techLevel;
	public Vector3 center;
    public List<GameObject> commandButtons;
	protected GameObject selectionBoxTemplate, selectionBox;
	protected SpriteRenderer selectionBoxRenderer;
	protected Vector3 scale;
    public int hitPoints;

	public Player owner;
	private List<Action> actionQueue; //These are all the actions that the object is CURRENTLY performing
    private Dictionary<ActionType, Action> validActions; //These are all the actions it CAN perform
	protected bool currentlySelected = false;
	//public List<
	// Use this for initialization
	protected virtual void Awake(){
        //CHANGE THIS LATER
        owner = GameObject.Find("Player").GetComponent<Player>();
    }
	protected virtual void Start () {

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
        for(int i = 0; i < actionQueue.Count; i++)
        {
            actionQueue[i].Execute(this);
        }
        //Handle actions in action queue
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
   
	public void SetSelection(bool selected){
		currentlySelected = selected;

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
        //Switch Default actions based on default actions

        if (currentlySelected && hitObject.name == "Ground")
        {
            TriggerMove(transform.position, hitPoint);


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
                
            }
        }
    }
    
    public bool CanDo(ActionType type)
    {
        return validActions.ContainsKey(type);
    }


   
	private void ChangeSelection(WorldObject worldObject, Player controller){
		SetSelection(false);
        if (controller.ObjectSelected()) {
            controller.SelectedObjects.Remove(this);
        }
		controller.SelectedObjects.Add(worldObject);
		worldObject.SetSelection(true);
	}

    #region event posting mini-functions
    public void TriggerMove(Vector3 source, Vector3 destination)
    {
        if (CanDo(ActionType.Move))
        {
            Move move = validActions[ActionType.Move] as Move;
            move.SetUpMove(source, destination);
            actionQueue.Add(move);
        }
    }

    public void TriggerAttack(WorldObject target)
    {
        if (CanDo(ActionType.Attack)){
            Attack attack = validActions[ActionType.Attack] as Attack;
            attack.SetUpAttack(target);
            actionQueue.Add(attack);
        }
    }


    public void TriggerTakeDamage(int damage, Attack.AttackEffect effect)
    {
        if (CanDo(ActionType.TakeDamage))
        {
            TakeDamage takeDamage = validActions[ActionType.TakeDamage] as TakeDamage;
            takeDamage.SetUpTakeDamage(damage, effect);
            actionQueue.Add(takeDamage);
        }
    }


#endregion
}
