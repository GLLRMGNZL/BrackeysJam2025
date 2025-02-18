using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour
{
    public string worldName;
    public int maxPopulation;
    public int currentPopulation;
    public int currentStructuresSize;
    public int maxStructuresSize;
    public int cities;
    public int factories;
    public int labs;

    private int populationGrowthRate = 3;
    private float growthInterval = 1f;
    private float buildingResourcesGrowthRate = 7f;

    private void Start()
    {
        GrowthRate();
    }

    //TODO: Methods

    // Increase population, resources and technology

    private void GrowthRate()
    {
        CancelInvoke("IncreasePopulation");
        InvokeRepeating("IncreasePopulation", 0f, growthInterval);
        CancelInvoke("IncreasePlayerBuildingResources");
        InvokeRepeating("IncreasePlayerBuildingResources", 0f, growthInterval);
        CancelInvoke("IncreasePlayerTravelResources");
        InvokeRepeating("IncreasePlayerTravelResources", 0f, growthInterval);
    }

    private void IncreasePopulation()
    {
        if (currentPopulation >= maxPopulation)
        {
            currentPopulation = maxPopulation;
        }
        else
        {
            int growth = 1;

            growth = cities * populationGrowthRate;

            currentPopulation += growth;
        }
    }

    private void IncreasePlayerBuildingResources()
    {
        if (PlayerStats.instance.technologyLevel < 1)
        {
            float growth = 1;
            growth = factories * buildingResourcesGrowthRate;
            PlayerStats.instance.buildingResources += growth;
        }
        else
        {
            float growth = 1;
            growth = factories * buildingResourcesGrowthRate * PlayerStats.instance.technologyLevel;
            PlayerStats.instance.buildingResources += growth;
        }
        
    }

    private void IncreaseTechLevel()
    {
        // Increase if labs > 0
    }

    private void IncreasePlayerTravelResources()
    {
        float growth = 1;
        growth = factories * buildingResourcesGrowthRate * PlayerStats.instance.technologyLevel;
        PlayerStats.instance.buildingResources += growth;
    }

    /* Build and delete structure
     *  City
     *  Factory
     *  Lab
     */

    public void BuildStructure(string structureType)
    {
        if (currentStructuresSize >= maxStructuresSize)
        {
            Debug.Log("¡No hay espacio en este planeta para más construcciones!");
        }
        else
        {
            switch (structureType)
            {
                case "city":
                    cities++;
                    maxPopulation = Mathf.RoundToInt(maxPopulation * 1.15f);
                    currentStructuresSize++;
                    break;
                case "factory":
                    factories++;
                    currentStructuresSize++;
                    break;
                case "lab":
                    labs++;
                    currentStructuresSize++;
                    break;
                default: break;
            }
        }
    }

    public void DeleteStructure(string structureType)
    {
        switch (structureType)
        {
            case "city":
                if (cities > 0)
                {
                    cities--;
                    maxPopulation = Mathf.RoundToInt(maxPopulation * -1.15f);
                    currentStructuresSize--;
                }
                else
                {
                    Debug.Log("No hay ciudades que derribar");
                }
                break;
            case "factory":
                if (factories > 0)
                {
                    factories--;
                    currentStructuresSize--;
                }
                else
                {
                    Debug.Log("No hay fábricas que derribar");
                }
                break;
            case "lab":
                if (labs > 0)
                {
                    labs--;
                    currentStructuresSize--;
                }
                else
                {
                    Debug.Log("No hay laboratorios que derribar");
                }
                break;
            default: break;
        }

    }

    // Terraforming (increase StructureSize)
    public void Terraforming()
    {
        maxStructuresSize += 3;
    }
}