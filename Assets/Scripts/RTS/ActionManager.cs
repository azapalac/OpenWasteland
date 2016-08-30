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

        public static void LoadBlueprint(this WorldObject obj, Blueprint b)
        {
            obj.loadedBlueprints.Add(b);
            GameObject button = GameObject.Instantiate(GameManager.ConstructionButton) as GameObject;
            ConstructionButton cb = button.GetComponent<ConstructionButton>();
            cb.SetUp(b, obj);
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
                obj.currentActionTimers.Add("Construct Unit", constructionBlueprint.ConstructionTime);
                obj.activeBlueprints.Add("Construct Unit", constructionBlueprint);
                obj.activeActionList.Add(constructionBlueprint.Name);
            }
        }

        public static Blueprint GetActiveBlueprint(this WorldObject obj, string s)
        {
            return obj.activeBlueprints[s];
        }

        public static void FinishUnitConstruction(this WorldObject obj)
        {
            ConstructionBlueprint b = obj.GetActiveBlueprint("Construct Unit") as ConstructionBlueprint;
            GameObject G = GameObject.Instantiate(b.product, obj.gameObject.transform.position, Quaternion.identity) as GameObject;
            //Remove from dictionary
            obj.activeBlueprints.Remove("Construct Unit");
            obj.currentActionTimers.Remove("Construct Unit");
            obj.activeActionList.Remove(b.Name);
        }
  

        public static void ContinueAction(this WorldObject obj, string action)
        {
            //This needs MAJOR refactoring
            // obj.currentActionTimers[action] -= Time.deltaTime;
            /*  if(obj.currentActionTimers[action] <= 0)
              {
                  switch (action)
                  {
                      case "Construct Unit":
                          obj.FinishUnitConstruction();
                          break;

                      default:
                          break;
                  }
              }*/
            obj.FinishUnitConstruction();
        }

    }
}