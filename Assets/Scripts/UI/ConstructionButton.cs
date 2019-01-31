using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class ConstructionButton : Property {
    public Button button;
    public Blueprint parentBlueprint;
    public Build buildAction;
    // Use this for initialization
    private bool SetUpCalled;
    public void SetUp()
    {
        if (worldObject.CanDo(ActionType.Build))
        {

        }
        SetUpCalled = true;
    }
	
	// Update is called once per frame
	void Update () {
	
	}
    public void StartBuild()
    {
      
    }
}
