using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
namespace RTS {

    public static class ResourceManager {
        
        public static int WorldObjectLayer { get { return 8; } }
        public static float ScrollSpeed { get { return 25; } }
        public static float RotateSpeed { get { return 100; } }
        public static int ScrollWidth { get { return 25; } }
        //Make this a list later on
        private static GameObject selectionBox;
        private static GameObject radius;
        public static GameObject GetRadius { get { return radius; } }
        public static GameObject GetSelectionBox { get { return selectionBox; } }
        public static void StoreSelectionBoxItems(GameObject s) {
            selectionBox = s;
        }
        public static void StoreRadiusItems(GameObject r)
        {
            radius = r;
        }
        //Remember to change this once the game is not played on a flat surface - Use raycasting!!
        public static float MinCameraHeight { get { return 10; } }

        public static float MaxCameraHeight { get { return 40; } }
        public static float RotateAmount { get { return 10; } }
        private static Vector3 invalidPosition = new Vector3(-99999, -99999, -99999);
        public static Vector3 InvalidPosition { get { return invalidPosition; } }


    }



    public class ObjectManager
    {

        //Manages stats for in-game objects
        private static Dictionary<string, Resource> allResources
        {
            
            get
            {
                return new Dictionary<string, Resource>
                {
                    {"Iron", new Resource { name = "Iron", rarity = Rarity.Uncommon, TechLevel = 1} },
                    {"Stone", new Resource { name = "Stone", rarity = Rarity.Common, TechLevel = 0} },
                    {"Scrap", new Resource { name = "Scrap", rarity = Rarity.Junk, TechLevel = 0} },
                };
            }
        }

        public static Resource GetResource(string name)
        {
            if (allResources.ContainsKey(name))
            {
                return allResources[name];
            }
            else
            {
                return new Resource {name = "ERROR", rarity = Rarity.Unobtanium, TechLevel = -1 };
            }
        }

        public static Dictionary<string, List<Resource>> ResourceDrops
        {
            get
            {
                return new Dictionary<string, List<Resource>>
                {
                    { "Junk", new List<Resource> {
                        GetResourceDrop("Scrap", Random.Range(20, 40)),
                        GetResourceDrop("Stone", Random.Range(5, 30)),
                        GetResourceDrop("Iron", Random.Range(1, 20)),
                    } },
                };
            }
        }

        private static Resource GetResourceDrop(string resource, int dropAmount)
        {
            Resource drop = new Resource();
            drop.dropAmount = 0;
            
            if (allResources.ContainsKey(resource))
            {
                drop = allResources[resource];
                Rarity rarity = drop.rarity;
                switch (rarity)
                {
                    case Rarity.Junk:
                        GetDrop(drop, resource, dropAmount, 1f);
                        break;

                    case Rarity.Common:
                        GetDrop(drop, resource, dropAmount, .8f);
                        break;

                    case Rarity.Uncommon:
                        GetDrop(drop, resource, dropAmount, .4f);
                        break; 

                    case Rarity.Rare:
                        GetDrop(drop, resource, dropAmount, .1f);
                        break;

                    case Rarity.Treasured:
                        GetDrop(drop, resource, dropAmount, .02f);
                        break;

                    case Rarity.Legendary:
                        GetDrop(drop, resource, dropAmount, .001f);
                        break;

                    case Rarity.Unobtanium:
                        Debug.Log("Error! Trying to obtain a resource not currently in the database");
                        break;

                }
            }

            return drop;

        }

        private static void GetDrop(Resource drop, string name, int dropAmount, float chance)
        {
            if (Random.Range(0f, 1f) < chance)
            {
                drop.dropAmount = dropAmount;
            }
        }

        
        private static Dictionary<string, GameObject> PackPrefabs
            {

            get { return new Dictionary<string, GameObject>(); }
            }
    }
    public class ActionManager {

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
                    //Harvest actions
                    "Harvest Junk",
                    "Harvest Stone"

                };
            }

        }

            //Adds the string only if the list contains it. Use instead of the standard add function
            public static void AddAction(List<string> actionList, string s)
            {
                if (allActions.Contains(s)) {
                    actionList.Add(s);
                }else{
                    Debug.Log("Error! Requested action not found!");
                }

            }
        


        



    }
}
