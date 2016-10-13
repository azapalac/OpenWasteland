using UnityEngine;
using System.Collections.Generic;

namespace RTS
{
    public static class ActionManager
    {

        private static List<string> allActions
        {
            get
            {
                return new List<string> {
                    //Basic Action categories
                    "Move",
                    "Attack",
                    "Take Damage",
                    "Pick Up Resources",
                    "Load Blueprint",
                    "Construct Unit",
                    //Harvest actions
                    "Harvest Junk",
                    "Harvest Stone",
                    "Open Gate",
                    "Close Gate"
                };
            }

        }

        //Adds the string only if the list contains it. Use instead of the standard add function
        public static void AddAction(List<string> actionList, string s)
        {
            if (allActions.Contains(s))
            {
                actionList.Add(s);
            }
            else {
                Debug.Log("Error! Requested action not found!");
            }

        }

        public static void LoadBlueprint(this WorldObject obj, Blueprint blueprint)
        {
            obj.loadedBlueprints.Add(blueprint);
            GameObject button = GameObject.Instantiate(GameManager.ConstructionButton, Vector3.zero, Quaternion.identity) as GameObject;
            ConstructionButton cb = button.GetComponent<ConstructionButton>();
            button.transform.Translate(new Vector3(7 * Screen.width / 8, Screen.height / 2));
            cb.SetUp(blueprint, obj);
        }

        //Add overrides for this
        public static  void TakeDamage(this WorldObject obj, int damage)
        {
            //Override this once shields and armor are figured out (for units)
            if (obj.CanDo("Take Damage"))
            {
                obj.hitPoints -= damage;
                if (obj.hitPoints <= 0)
                {
                    //Change this later
                    obj.Destroy();
                }
            }
        }

        public static void StartUnitConstruction(this WorldObject obj, Blueprint constructionBlueprint)
        {
            if(obj.CanDo("Construct Unit"))
            {
                if (obj.activeBlueprints.Count < obj.activeBlueprintLimit)
                {
                    obj.currentActionTimers.Add(constructionBlueprint.ConstructionTime);
                    obj.activeBlueprints.Add(constructionBlueprint);
                    obj.activeActionList.Add("Construct Unit");
                }else if (obj.queuedConstructionProjects.Count < obj.constructionQueueLimit)
                {
                    ConstructionProject p = new ConstructionProject();
                    p.timer = constructionBlueprint.ConstructionTime;
                    p.blueprint = constructionBlueprint;
                    p.currentAction = "Construct Unit";
                    obj.queuedConstructionProjects.Add(p);
                }
            }
        }


        public static void FinishUnitConstruction(this WorldObject obj, int index)
        {
            ConstructionBlueprint b = obj.activeBlueprints[index] as ConstructionBlueprint;

            //Create a spawn vector - this should be a constant value.
            Vector3 spawnVect = new Vector3(5, 0, 0);
            GameObject G = GameObject.Instantiate(b.product, obj.gameObject.transform.position + spawnVect, Quaternion.identity) as GameObject;
            //Remove from dictionary
            obj.activeBlueprints.Remove(b);
            obj.currentActionTimers.Remove(obj.currentActionTimers[index]);
            obj.activeActionList.Remove("Construct Unit");
            
            //Start next project, if there's already one in the queue
            while(obj.queuedConstructionProjects.Count > 0 && obj.activeBlueprints.Count < obj.activeBlueprintLimit)
            {
                int nextProjectIndex = obj.queuedConstructionProjects.Count - 1;
                ConstructionProject nextProject = obj.queuedConstructionProjects[nextProjectIndex];
                obj.StartUnitConstruction(nextProject.blueprint);
                obj.queuedConstructionProjects.Remove(nextProject);
            }
        }
  

        public static void ContinueAction(this WorldObject obj, int index)
        {
            //This needs MAJOR refactoring
            string action = obj.activeActionList[index];
            obj.currentActionTimers[index] -= Time.deltaTime;
             if(obj.currentActionTimers[index] <= 0)
              {
                  switch (action)
                  {
                      case "Construct Unit":
                          obj.FinishUnitConstruction(index);
                          break;

                        

                      default:
                          break;
                  }
              }
        }

    }
}