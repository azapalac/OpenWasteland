using UnityEngine;
using System.Collections.Generic;
using RTS;
using UnityEngine.UI;
public class GameManager : MonoBehaviour {
    public GameObject constructionButton;
    public static GameObject ConstructionButton;

    public GameObject[] dropPrefabList;
    public static GameObject[] ResourcePackPrefabs;

    public GameObject[] baseWorldObjectPrefabList;
    public static GameObject[] baseWorldObjects;


    void Awake()
    {
        ConstructionButton = constructionButton;
        ResourcePackPrefabs = dropPrefabList;
        baseWorldObjects = baseWorldObjectPrefabList;
        //Initialize all prefabs
        for(int i = 0; i < ResourcePackPrefabs.Length; i++)
        {
            ResourcePackPrefabs[i].GetComponent<ResourcePack>().Initialize();
        }
        BlueprintManager.SetUpBlueprintDictionary();
    }

    private static  Dictionary<string, GameObject> GetDropMap()
    {
        Dictionary<string, GameObject> map = new Dictionary<string, GameObject>();
        
        for(int i = 0; i < ResourcePackPrefabs.Length; i++)
        {
            map.Add(ResourcePackPrefabs[i].GetComponent<ResourcePack>().containedResource.name, ResourcePackPrefabs[i]);
        }
        return map;
    }

    public static Dictionary<string, GameObject> DropDictionary
    {
        get
        {
            return GetDropMap();
        }
    }


    //Optimize this so it only has to run once
    private static Dictionary<string, GameObject> GetWorldObjectMap()
    {
        Dictionary<string, GameObject> map = new Dictionary<string, GameObject>();

        for (int i = 0; i < baseWorldObjects.Length; i++)
        {
            GameObject r = baseWorldObjects[i];
            map.Add(baseWorldObjects[i].name, baseWorldObjects[i]);
        }
        return map;
    }


    
    public static Dictionary<string, GameObject> WorldObjectDictionary
    {
        get
        {
            return GetWorldObjectMap();
        }
    }
}

