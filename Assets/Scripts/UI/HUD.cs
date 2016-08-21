using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using RTS;

public class HUD : MonoBehaviour {
	public Image ordersBar, resourceBar;

	//Change color based on allegiance to player
	public GameObject selectionBox;
    public GameObject radius;
	private SpriteRenderer selectionBoxRenderer;
	private Vector3 size;
	private const int ORDERS_BAR_WIDTH = 150, RESOURCE_BAR_HEIGHT = 40;
	private Player player;
	public Text nameField;
    private bool paused;
	public Image mouseCursor;
	public Sprite defaultCursor, moveCursor, attackCursor, selectCursor, harvestCursor;

	// Use this for initialization
	void Awake () {
		Cursor.visible = false;
		mouseCursor.color = Color.black;
		//mouseCursor.rectTransform.position = Vector3.zero;
		player = transform.root.GetComponent<Player>();
		selectionBoxRenderer = selectionBox.GetComponent<SpriteRenderer>();
		selectionBoxRenderer.color = Color.clear;
		//size = this.gameObject.GetComponent<Collider>().bounds.size;
		//Debug.Log ("Size: " + size);
		ResourceManager.StoreSelectionBoxItems(selectionBox);
        ResourceManager.StoreRadiusItems(radius);
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown(KeyCode.Escape) && !paused)
        {
            Time.timeScale = 0;
            Cursor.visible = true;
            paused = true;
        }else if(Input.GetKeyDown(KeyCode.Escape) && paused)
        {
            Time.timeScale = 1;
            Cursor.visible = false;
            paused = false;
        }
        
		HandleGUI();
	}

	void HandleGUI(){
		string selectionName = "";
		if(player.SelectedObject){
			selectionName  = player.SelectedObject.objectName;
		}
		
		if(!selectionName.Equals("")){
			nameField.text = selectionName;
		}

		DrawMouseCursor();
	}

    //WELCOME TO RAYCAST HELL
	private void DrawMouseCursor(){
		mouseCursor.rectTransform.position  = Input.mousePosition;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        //standard behavior
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.gameObject.layer == ResourceManager.WorldObjectLayer)
            {
                //Should be red for enemies (At War), green for allies, and yellow for neutral
                mouseCursor.sprite = selectCursor;
                mouseCursor.color = Color.green;
            } else {
                mouseCursor.sprite = defaultCursor;
                mouseCursor.color = Color.black;
            }

        }
        else {
            mouseCursor.sprite = defaultCursor;
            mouseCursor.color = Color.black;
        }

        //If an object is selected, override cursor
        if (player.SelectedObject){

            if(Physics.Raycast(ray, out hit)){
                
                if (player.SelectedObject.CanDo("Move") && hit.collider.gameObject.name == "Ground")
                {
                    mouseCursor.sprite = moveCursor;
                    mouseCursor.color = Color.black;
                }

                //check if the object is harvestable
                if (hit.collider.gameObject.GetComponent<HarvestableObject>() != null)
                {
                    HarvestableObject h = hit.collider.gameObject.GetComponent<HarvestableObject>();
                    if (player.SelectedObject.CanDo("Harvest " + h.type))
                    {
                        mouseCursor.sprite = harvestCursor;
                        mouseCursor.color = Color.yellow;
                    }
                }
            }
		}
	}



	public bool MouseInBounds(){

		Vector3 mousePos = Input.mousePosition;
		bool insideWidth = mousePos.x >= 0 && mousePos.x <= Screen.width - ORDERS_BAR_WIDTH;
		bool insideHeight = mousePos.y >= 0 && mousePos.y <= Screen.height - RESOURCE_BAR_HEIGHT;

		return insideWidth && insideHeight;
	}
}
