using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Runtime.CompilerServices;
using System;

    public class Blueprint: MonoBehaviour
    {
    public float constructionTime;
    public Button constructionButton;
    public int techLevel;

    [SerializeField]
    public List<ResourcePackData> ingredients;
    
    [Space]

    public int knowledgePoints;


    
    public GameObject product;

    }




  

