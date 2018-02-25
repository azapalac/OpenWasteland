using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class ConstructionButton : MonoBehaviour {
    public Button button;
    public Blueprint loadedBlueprint;
    public WorldObject parentObject;
	// Use this for initialization

    public void SetUp(Blueprint b, WorldObject obj)
    {
        loadedBlueprint = b;
        parentObject = obj;
        button = gameObject.GetComponent<Button>();
        gameObject.GetComponentInChildren<Text>().text = "Build " + loadedBlueprint.Name;
        gameObject.GetComponent<RectTransform>().position = new Vector3(Screen.width / 2, Screen.height / 2);
        transform.parent = GameObject.Find("HUD").transform;
    }
	
	// Update is called once per frame
	void Update () {
	
	}
    public void StartBuild()
    {
       // parentObject.StartUnitConstruction(loadedBlueprint);
    }
}
