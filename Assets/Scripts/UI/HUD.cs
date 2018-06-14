using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class HUD : MonoBehaviour {
    public Image ordersBar;
    private Image multiSelectImage;
    private const float renderWidth = 20.0f;
    //Change color based on allegiance to player
	public GameObject selectionBox;
    private MouseRect selectionRect;
    public GameObject radius;
	private SpriteRenderer selectionBoxRenderer;
	private Vector3 size;
	private const int ORDERS_BAR_WIDTH = 150, RESOURCE_BAR_HEIGHT = 40;
	private Player player;
	public Text nameField;
    private bool paused;
	public Image mouseCursor;
	public Sprite defaultCursor, moveCursor, attackCursor, selectCursor, harvestCursor;
    public GameObject imagePrefab;
    


	// Use this for initialization
	void Awake () {
		Cursor.visible = false;
		mouseCursor.color = Color.black;
		//mouseCursor.rectTransform.position = Vector3.zero;
		player = transform.root.GetComponent<Player>();
		selectionBoxRenderer = selectionBox.GetComponent<SpriteRenderer>();
		selectionBoxRenderer.color = Color.clear;

		ResourceManager.StoreSelectionBoxItems(selectionBox);
        ResourceManager.StoreRadiusItems(radius);
        selectionRect = new MouseRect();
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

  //  void 

	void HandleGUI(){
        //Work on this later!!!
		//string selectionName = "";
		if(player.ObjectSelected()){
			nameField.text  = player.SelectedObjects[0].objectName;
        }
        else
        {
            nameField.text = "";
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
            if (hit.collider.gameObject.GetComponent<WorldObject>() != null)
            {
                //Should be red for enemies (At War), green for allies, and yellow for neutral
                mouseCursor.sprite = selectCursor;
                mouseCursor.color = Color.green;

                //If an object is selected, override cursor
                if (player.ObjectSelected())
                {

                  
                 //Change the cursor if  ANY selected object can do the thing
                 
                    for (int i = 0; i < player.SelectedObjects.Count; i++)
                    {
                       if (player.SelectedObjects[i].CanDo(ActionType.Harvest))
                        {

                        }         
                     }
                    
                }

            } else {
                mouseCursor.sprite = defaultCursor;
                mouseCursor.color = Color.black;
            }

        }
        else {
            mouseCursor.sprite = defaultCursor;
            mouseCursor.color = Color.black;
        }

       
	}


    public MouseRect DrawMultiSelect(Vector3 origMousePos, Vector3 currMousePos)
    {

        Vector3 diff = currMousePos - origMousePos;
        Vector3 rectOrigin = Vector3.zero;

        //Make the rect work for all quadrants

        Vector3 posBA = new Vector3(origMousePos.x + diff.x, origMousePos.y, 0);
        Vector3 posAB = new Vector3(origMousePos.x, origMousePos.y + diff.y, 0);


        if (diff.x > 0 && diff.y > 0)
        {
            //First Quadrant
            rectOrigin = origMousePos;

        }
        else if (diff.x < 0 && diff.y > 0)
        {
            //Second Quadrant
            rectOrigin = posBA;
        }
        else if (diff.x > 0 && diff.y < 0)
        {
            //Third Quadrant
            rectOrigin = posAB;
        }
        else if (diff.x < 0 && diff.y < 0)
        {
            //Fourth Quadrant
            rectOrigin = currMousePos;
        }
        Rect screenRect = ResourceManager.GetScreenRect(origMousePos, currMousePos);
        Rect raycastRect = new Rect(rectOrigin.x, rectOrigin.y, Mathf.Abs(diff.x), Mathf.Abs(diff.y));
        selectionRect = new MouseRect(screenRect, raycastRect, origMousePos, currMousePos);
     
        return selectionRect;


    }

    public void StopMultiSelect()
    {
        selectionRect = new MouseRect();
    }

    private void OnGUI()
    {
        ResourceManager.DrawScreenRect(selectionRect.screenRect, ResourceManager.selectionColor);

    }


    public bool MouseInBounds(){

		Vector3 mousePos = Input.mousePosition;
		bool insideWidth = mousePos.x >= 0 && mousePos.x <= Screen.width - ORDERS_BAR_WIDTH;
		

		return insideWidth;
	}
}
public class MouseRect
{
    public Rect screenRect;
    public Rect raycastRect;
    public Vector3 origPos;
    public Vector3 currPos;
    public MouseRect(Rect rect1, Rect rect2,  Vector3 origPos, Vector3 currPos)
    {
        this.origPos = origPos;
        this.currPos = currPos;
        screenRect = rect1;
        raycastRect = rect2;

    }

    public MouseRect()
    {

    }

}