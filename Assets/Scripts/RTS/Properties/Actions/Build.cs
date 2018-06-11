using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Build : Action {

    private List<Blueprint> knownBlueprints;

    public float totalBuildTime;
    private float buildTimer;

   

    public override ActionType Type
    {
        get
        {
            return ActionType.Build;
        }
    }
   
    // Use this for initialization
    void Start () {
        knownBlueprints = new List<Blueprint>();
	}
    private void AddBlueprint(Blueprint blueprint)
    {
        knownBlueprints.Add(blueprint);
    }

    public void SetUp(Blueprint blueprintToBuild)
    {

    }

    public override void Execute(WorldObject worldObj)
    {
        
    }
    // Update is called once per frame
    void Update () {
		
	}
}
