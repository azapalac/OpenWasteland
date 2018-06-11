using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;




//A world object is a unit, structure, or resource. Any object in the world that can be selected and interacted with
//Sets up the generic WorldObject architecture
//Handle "default" actions here
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
	public Image icon;
	public Image ordersBar;
    public int techLevel;
	public Vector3 center;
    public List<GameObject> commandButtons;
	protected GameObject selectionBoxTemplate, selectionBox;
	protected SpriteRenderer selectionBoxRenderer;
	protected Vector3 scale;
	public Player owner;
	private List<Action> currentActions; //These are all the actions that the object is CURRENTLY performing
    private List<Action> queuedActions; //These are all the actions that it WILL perform but currently can't
    private Dictionary<ActionType, Action> validActions; //These are all the actions it CAN perform
    
    protected bool currentlySelected = false;
    public bool Selected { get { return currentlySelected;  } }

    
    
	protected virtual void Start(){
        validActions = new Dictionary<ActionType, Action>();
        owner = GameObject.Find("Player").GetComponent<Player>();
        Action[] allActions = GetComponents<Action>();
        for(int i = 0; i < allActions.Length; i++)
        {
            validActions.Add(allActions[i].Type, allActions[i]);
        }
  
        if(currentActions == null)
        {
            currentActions = new List<Action>();
        }

        if(queuedActions == null)
        {
            queuedActions = new List<Action>();
        }
        gameObject.layer = ResourceManager.WorldObjectLayer;
       
		center = this.transform.position;
		selectionBoxTemplate = ResourceManager.SelectionBox;
		selectionBox = GameObject.Instantiate(selectionBoxTemplate, center, Quaternion.identity) as GameObject;
		selectionBox.transform.Rotate(new Vector3(90, 0, 0));
		scale = GetFootprint(this.gameObject.GetComponent<Collider>().bounds.size)*2;
		selectionBox.transform.localScale = scale;
		selectionBoxRenderer = selectionBox.GetComponent<SpriteRenderer>();
		selectionBox.transform.parent = this.transform;

   	}

	// Update is called once per frame
	protected virtual void Update () {
        
        DrawSelection();

        for(int i = 0; i < currentActions.Count; i++)
        {
            currentActions[i].Execute(this);
        }
        
    }

    protected float scalingFactor
    {
        get
        {
            return selectionBoxRenderer.bounds.extents.magnitude;
        }
    }

    //What the object draws when it is selected
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
        if (selected)
        {
            owner.SelectedObjects.Add(this);
        }
        else
        {
            owner.SelectedObjects.Remove(this);
        }
     
	}
	
	public virtual void LeftMouseClick(GameObject hitObject, Vector3 hitPoint, Player controller){
        //Only handle input if currently selected
        if (currentlySelected && hitObject.name != "Ground")
        {
            WorldObject worldObject = hitObject.transform.root.GetComponent<WorldObject>();

            if (worldObject) ChangeSelection(worldObject, controller);
        }
      
	}
    
    public void QueueAction(Action action)
    {
        //Add an action to the queue
        queuedActions.Insert(0, action);
    }

    public void DequeueAction()
    {
        if (queuedActions.Count > 0) {
            //Remove an action from the queue and add it to the pool of current actions
            Action action = queuedActions[0];
            queuedActions.Remove(action);
            currentActions.Add(action);
        }
        else
        {
            Debug.LogError("ERROR: Action Queue is empty!");
        }
    }

    public void RightMouseClick(GameObject hitObject, Vector3 hitPoint, Player controller )
    {
            //For every action, set up its right click if it's valid
            for(int i = 0; i < ResourceManager.GetEnumLength(typeof(ActionType)); i++)
            {
                if (CanDo((ActionType)i)){
                    validActions[(ActionType)i].SetUpRightClick(hitPoint, hitObject);
                }
            }
        
    }
    

    public void StopDoing(ActionType actionType)
    {
        if (IsDoing(validActions[actionType]))
        {
            StopDoing(validActions[actionType]);
        }
    }

    public void StartDoing(ActionType actionType)
    {
        if (CanDo(actionType))
        {
            StartDoing(validActions[actionType]);
        }
    }

    public bool IsDoing(Action action)
    {
        return currentActions.Contains(action);
    }

    public bool CanDo(ActionType type)
    {
        return validActions.ContainsKey(type);
    }

    public void StartDoing(Action action)
    {
        currentActions.Add(action);
    }

    public void StopDoing(Action action)
    {
        currentActions.Remove(action);
    }
   
	private void ChangeSelection(WorldObject worldObject, Player controller){
		SetSelection(false);
        if (controller.ObjectSelected()) {
            controller.SelectedObjects.Clear();
        }
		controller.SelectedObjects.Add(worldObject);
		worldObject.SetSelection(true);
	}

    Vector3 GetFootprint(Vector3 scale)
    {
        return new Vector3(scale.x, scale.z, 1);
    }

   
}
