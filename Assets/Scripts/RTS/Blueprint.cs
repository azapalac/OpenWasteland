using UnityEngine;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System;

namespace RTS
{
    public abstract class Blueprint
    {

        public string Name{ get; set; }
        public float ConstructionTime { get; set; }
        public int TechLevel { get; set; }
        public List<Resource> Ingredients {get; set;}
        public int KnowledgePoints { get; set;}
       
    }

    public class ConstructionBlueprint: Blueprint
    {
        //For buildings, the population cost will just be zero
        public int populationCost { get; set; }
        public GameObject product { get; set; }
    }

    public class CraftingBlueprint: Blueprint
    {
        public Resource Product { get; set; }
    }




    public class BlueprintManager
    {
        private static KeyValuePair<string, ConstructionBlueprint> MakeBlueprint(
            string name, int populationCost, int techLevel, List<Resource> ingredients, float constructionTime)
        {
            return new KeyValuePair<string, ConstructionBlueprint>(name, new ConstructionBlueprint
            {
                Name = name,
                TechLevel = techLevel,
                populationCost = populationCost,
                Ingredients = ingredients,
                product = GameManager.WorldObjectDictionary[name],
                ConstructionTime = constructionTime

            });
        }

        private static KeyValuePair<string, CraftingBlueprint> MakeBlueprint(
           string name,  int techLevel, List<Resource> ingredients, float constructionTime, Resource product)
        {
            return new KeyValuePair<string, CraftingBlueprint>(name, new CraftingBlueprint
            {
                Name = name,
                TechLevel = techLevel,
                Ingredients = ingredients,
                Product = product,
                ConstructionTime = constructionTime
            });
        }

        public static Dictionary<string, Blueprint> BaseBlueprintDictionary;
        public static void SetUpBlueprintDictionary()
        {
            BaseBlueprintDictionary = new Dictionary<string, Blueprint>()
            {
                //Example of a unit construction blueprint
                {"Harvester", new ConstructionBlueprint {
                    Name = "Harvester",
                    populationCost = 1,
                    TechLevel = 0,
                    Ingredients = new List<Resource> {
                        ObjectManager.GetResource(10,"Scrap"),
                        ObjectManager.GetResource(5, "Stone")
                    },
                    ConstructionTime = 5f,
                    product = GameManager.WorldObjectDictionary["Harvester"],

                } },

                //Example of a crafting blueprint
                {"Iron", new CraftingBlueprint {
                    Name = "Iron",
                    TechLevel = 0,
                    Ingredients = new List<Resource>
                    {
                        ObjectManager.GetResource(20, "Scrap")
                    },
                    ConstructionTime = 3f,
                    Product = ObjectManager.GetResource(5, "Iron")

                } }
            };
        }

    }
}
