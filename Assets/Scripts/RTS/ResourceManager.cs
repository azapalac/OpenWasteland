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

    public enum DropDensity //Multipliers. This is multiplied by the size factor of the object
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
      
    
        public static bool GetDropChance(Rarity rarity)
    {
           bool b = false;
                switch (rarity)
                {
                    case Rarity.Junk:
                        b = GetDrop(1f);
                        break;

                    case Rarity.Common:
                        b= GetDrop(.8f);
                        break;

                    case Rarity.Uncommon:
                        b = GetDrop(.4f);
                        break; 

                    case Rarity.Rare:
                        b = GetDrop(.1f);
                        break;

                    case Rarity.Treasured:
                        b = GetDrop(.02f);
                        break;

                    case Rarity.Legendary:
                        b = GetDrop(.01f);
                        break;

                    case Rarity.Unobtanium:
                        Debug.Log("Error! Trying to obtain an unavailable resource ");
                       
                        break;

                }
        return b;
    }

        public static Resource GetResourceDrop(ResourceType type)
        {
        return allResources[type];
        }

        private static bool GetDrop(float chance)
        {
        return Random.Range(0f, 1f) < chance;
          
        }

         
    }

[System.Serializable]
public class ResourceDrop
{

    public ResourceType type;
    public Rarity rarity;
    public DropDensity dropDensity;
  

    public ResourcePack GetResourcePack(WorldObject.Size objectSize)
    {
        
        Resource resource = ObjectManager.GetResourceDrop(type);
        //Load and instantiate pack
        string name = resource.name + "Pack";
        GameObject g = Resources.Load("ResourcePacks/" + name) as GameObject;
        ResourcePack  pack = g.GetComponent<ResourcePack>();
        pack.containedResource = resource;

        if (ObjectManager.GetDropChance(rarity))
        {
            pack.dropped = true;
            //Change drip amount based on size and amount dropped;
            switch (objectSize)
            {
                //Put this in a database eventually
                case WorldObject.Size.Tiny:
                    pack.containedResource.dropAmount = Random.Range(1, 5);
                    break;

                case WorldObject.Size.Small:
                    pack.containedResource.dropAmount = Random.Range(1, 10);
                    break;

                case WorldObject.Size.Medium:
                    pack.containedResource.dropAmount = Random.Range(3, 13);
                    break;

                case WorldObject.Size.Large:
                    pack.containedResource.dropAmount = Random.Range(5, 15);
                    break;


                case WorldObject.Size.Massive:
                    pack.containedResource.dropAmount = Random.Range(8, 25); 
                    break;

                case WorldObject.Size.Gigantic:
                    pack.containedResource.dropAmount = Random.Range(15, 35);
                    break;
            }
            

            switch (dropDensity)
            {
                case DropDensity.Single:
                    pack.containedResource.dropAmount = 1;
                    break;

                case DropDensity.Small:
                    pack.containedResource.dropAmount = Mathf.CeilToInt(pack.containedResource.dropAmount * 0.5f);
                    break;

                case DropDensity.Medium:
                    //Multiply by 1, so do nothing
                    break;

                case DropDensity.Large:
                    pack.containedResource.dropAmount *= 2;
                    break;
            }
            pack.containedResourceAmount = pack.containedResource.dropAmount;

        }
        else
        {
            pack.dropped = false;
        }




        return pack;
    }

}



