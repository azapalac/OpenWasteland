using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Holding off on build actions until blueprints are implemented. Actions and blueprints have a weird relationship with each other.
//Each BuildStructure/BuildUnit is a seperate command - BuildStructure indicates that this CAN build structures.
public class BuildStructure : Action
{
    public List<Blueprint> BuildableStructures { get; set; }
    Blueprint structureToBuild;
    private float buildTimer;
    public override ActionType Type { get { return ActionType.BuildStructure; } }

    public void SetUpBuild(ConstructionBlueprint structure)
    {
        //Check if build is possible
        if (BuildableStructures.Contains(structure))
        {
            structureToBuild = structure;
            buildTimer = 0;
            active = true;
        }
    }

    public override void Execute(WorldObject worldObj)
    {
        if (active)
        {
            buildTimer += Time.deltaTime;

            if (buildTimer >= structureToBuild.ConstructionTime)
            {
                //Finish building the structure!
                active = false;
            }
        }
    }

}