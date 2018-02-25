using UnityEngine;
using System.Collections.Generic;

using UnityEngine.UI;
public class GameManager : MonoBehaviour {
    public GameObject constructionButton;
    public static GameObject ConstructionButton;

    public GameObject[] dropPrefabList;
    public static GameObject[] ResourcePackPrefabs;
    //Use Resources instead of base worldObjects. Much less arbitrary that way.
    //public GameObject[] baseWorldObjectPrefabList;
    public static GameObject[] baseWorldObjects;

   

    void Awake()
    {
        ConstructionButton = constructionButton;
        ResourcePackPrefabs = dropPrefabList;
       // baseWorldObjects = baseWorldObjectPrefabList;

       // BlueprintManager.SetUpBlueprintDictionary();
    }



}

