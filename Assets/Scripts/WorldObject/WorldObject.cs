using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using RTS;

public class Resource{
	string name;
	int amount;

	public string ToString(){
		return "  " + amount + " " + name;
	}
}

public class WorldObject : MonoBehaviour {
	public string objectName;
	public Image buildImage;
	public Image ordersBar;
	public Vector3 center;
	public Button[] commands;
	protected GameObject selectionBoxTemplate, selectionBox;
	protected SpriteRenderer selectionBoxRenderer;
	protected Vector3 size;
	public int  hitPoints, maxHitPoints;

	public List<Resource> cost, rebuildValue, sellValue;
	public List<List<Resource>> upgradeValues;


	protected Player player;
	protected string[] actions = {};
	protected bool currentlySelected = false;
	//public List<
	// Use this for initialization
	protected virtual void Awake(){

	}
	protected virtual void Start () {
		center = new Vector3(this.transform.position.x, this.transform.position.y + 1, this.transform.position.z);
		selectionBoxTemplate = ResourceManager.GetSelectionBox;
		selectionBox = GameObject.Instantiate(selectionBoxTemplate, center, Quaternion.identity) as GameObject;
		selectionBox.transform.Rotate(new Vector3(90, 0, 0));
		size = this.gameObject.GetComponent<Collider>().bounds.size;
		selectionBox.transform.localScale = size;
		selectionBoxRenderer = selectionBox.GetComponent<SpriteRenderer>();
		selectionBox.transform.parent = this.transform.root;
	}
	
	// Update is called once per frame
	protected virtual void Update () {

			DrawSelection();

	}

	private void DrawSelection(){
		if(currentlySelected){
			//TODO: Change this to be red for enemies and green for allies
			selectionBoxRenderer.color = Color.black;
		}else{
			selectionBoxRenderer.color = Color.clear;
		}
	}

	protected virtual void OnGUI(){

	}

	public void SetSelection(bool selected){
		currentlySelected = selected;

	}

	public string[] GetActions(){
		return actions;
	}

	public virtual void PerformAction(string actiontoPerform){

	}

	public virtual void MouseClick(GameObject hitObject, Vector3 hitPoint, Player controller){
		//Only handle input if currently selected
		if(currentlySelected && hitObject.name !="Ground"){
			WorldObject worldObject = hitObject.transform.root.GetComponent<WorldObject>();

			if(worldObject) ChangeSelection(worldObject, controller);
		}
	}


	private void ChangeSelection(WorldObject worldObject, Player controller){
		SetSelection(false);
		if(controller.SelectedObject) controller.SelectedObject.SetSelection(false);
		controller.SelectedObject = worldObject;
		worldObject.SetSelection(true);
	}
}
