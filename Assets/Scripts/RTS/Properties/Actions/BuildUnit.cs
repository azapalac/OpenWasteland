using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildUnit : Action
{
    public List<Blueprint> BuildableUnits{ get; set; }
    Blueprint unitToBuild;
    private float buildTimer;
    public override ActionType Type { get { return ActionType.BuildUnit; } }
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetUpBuild(UnitBlueprint unit)
    {
        if (BuildableUnits.Contains(unit))
        {
            unitToBuild = unit;
            buildTimer = 0;
        }

    }
}
