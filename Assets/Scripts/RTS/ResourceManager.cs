using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;


   


    
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

        static Texture2D whiteTexture;
        public static Texture2D WhiteTexture
        {
            get
            {
                if (whiteTexture == null)
                {
                    whiteTexture = new Texture2D(1, 1);
                    whiteTexture.SetPixel(0, 0, Color.white);
                    whiteTexture.Apply();
                }
                return whiteTexture;
            }
        }
        public static void DrawScreenRect(Rect rect, Color color)
        {
            GUI.color = color;
            GUI.DrawTexture(rect, WhiteTexture);
            GUI.color = Color.white;
        }

        public static Rect GetScreenRect(Vector3 screenPosition1, Vector3 screenPosition2)
        {
            // Move origin from bottom left to top left
            screenPosition1.y = Screen.height - screenPosition1.y;
            screenPosition2.y = Screen.height - screenPosition2.y;
            // Calculate corners
            var topLeft = Vector3.Min(screenPosition1, screenPosition2);
            var bottomRight = Vector3.Max(screenPosition1, screenPosition2);
            // Create Rect
            return Rect.MinMaxRect(topLeft.x, topLeft.y, bottomRight.x, bottomRight.y);
        }

        public static Color selectionColor
        {
            get
            {
                return new Color(0, 1, 0, 0.5f);
            }
        }
    }

  
    public enum Rarity
    {
        Junk,
        Common,
        Uncommon,
        Rare,
        Treasured,
        Legendary,
        Unobtanium
    }

    public enum ResourceType
    {
      Iron, 
      Stone,
      Charcoal,
      Scrap,
      Steel
    }

    public enum DropAmount //Multipliers. This is multiplied by the size factor of the object
    {
    Single,
    Small,
    Medium, 
    Large
}

    public class Resource
    {
        public string name { get; set; }
        public int dropAmount { get; set; }
        public Rarity rarity { get; set; }
        public int TechLevel { get; set; }

        public override string ToString()
        {
            return "  " + dropAmount + " " + name;
        }

    }

    public class Inventory
    {
        private List<Resource> resources;
        private int _capacity;
        public int Capacity { get { return _capacity; } }

        public Inventory(int capacity)
        {
            _capacity = capacity;
        }
        public void AddResource(Resource resource)
        {
            //Calculate capacity, then if there is room for the resource, add it in
        }

    }
    public class ObjectManager
    {

        //Manages stats for in-game objects
        private static Dictionary<ResourceType, Resource> allResources
        {
            
            get
            {
                return new Dictionary<ResourceType, Resource>
                {
                    {ResourceType.Iron, new Resource { name = "Iron", rarity = Rarity.Uncommon, TechLevel = 1} },
                    {ResourceType.Stone, new Resource { name = "Stone", rarity = Rarity.Common, TechLevel = 0} },
                    {ResourceType.Charcoal, new Resource {name = "Charcoal", rarity = Rarity.Common, TechLevel = 0 } },
                    {ResourceType.Scrap, new Resource { name = "Scrap", rarity = Rarity.Junk, TechLevel = 0} },
                    {ResourceType.Steel, new Resource { name = "Steel", rarity = Rarity.Rare, TechLevel = 2} },
                };
            }
        }
        public static Resource GetResource(int amount, ResourceType type)
        {
            Resource r = GetResource(type);
            r.dropAmount = amount;
            return r;
        }
        public static Resource GetResource(ResourceType type)
        {
            if (allResources.ContainsKey(type))
            {
                return allResources[type];
            }
            else
            {
                return new Resource {name = "ERROR", rarity = Rarity.Unobtanium, TechLevel = -1 };
            }
        }


       //Move this to a prefab level instead of a database.
        public static Dictionary<string, List<Resource>> ResourceDrops
        {
            get
            {
                return new Dictionary<string, List<Resource>>
                {
                    { "Junk", new List<Resource> {
                        GetResourceDrop(ResourceType.Scrap, Random.Range(20, 40)),
                        GetResourceDrop(ResourceType.Steel, Random.Range(5, 30)),
                        GetResourceDrop(ResourceType.Iron, Random.Range(1, 20)),
                    } },
                };
            }
        }


        private static Resource GetResourceDrop(ResourceType resource, int dropAmount)
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

        private static void GetDrop(Resource drop, ResourceType type, int dropAmount, float chance)
        {
            if (Random.Range(0f, 1f) < chance)
            {
                drop.dropAmount = dropAmount;
            }
        }

         
    }

[System.Serializable]
public class ResourceDrop
{

    public ResourceType type;
    public Rarity rarity;
    public DropAmount dropAmount;

}



