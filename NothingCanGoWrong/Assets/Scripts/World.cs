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
    private float travelResourcesGrowthRate = 4f;
    private float techLevelGrowthRate = 2f;

    private void Start()
    {
        GrowthRate();
    }

    // Increase population, resources and technology

    private void GrowthRate()
    {
        CancelInvoke("IncreasePopulation");
        InvokeRepeating("IncreasePopulation", 0f, growthInterval);
        CancelInvoke("IncreasePlayerBuildingResources");
        InvokeRepeating("IncreasePlayerBuildingResources", 0f, growthInterval);
        CancelInvoke("IncreasePlayerTravelResources");
        InvokeRepeating("IncreasePlayerTravelResources", 0f, growthInterval);
        CancelInvoke("IncreaseTechLevel");
        InvokeRepeating("IncreaseTechLevel", 0f, growthInterval);
    }

    private void IncreasePopulation()
    {
        if (currentPopulation >= maxPopulation)
        {
            currentPopulation = Mathf.RoundToInt(Mathf.Lerp(currentPopulation, maxPopulation, Time.deltaTime)) - 1;
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

    private void IncreasePlayerTravelResources()
    {
        if (PlayerStats.instance.technologyLevel < 1)
        {
            return;
        }
        else
        {
            float growth = 1;
            growth = factories * travelResourcesGrowthRate * Mathf.Round(PlayerStats.instance.technologyLevel);
            PlayerStats.instance.travelResources += growth;
        }
    }

    private void IncreaseTechLevel()
    {
        // Increase if labs > 0
        float growth = 1;
        growth = labs * techLevelGrowthRate;
        PlayerStats.instance.technologyLevel += (growth / 500);
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
                    if (PlayerStats.instance.buildingResources < 1000)
                    {
                        Debug.Log("Not enough building resources.");
                        return;
                    }
                    break;
                case "factory":
                    if (PlayerStats.instance.buildingResources < 2000)
                    {
                        Debug.Log("Not enough building resources.");
                        return;
                    }
                    break;
                case "lab":
                    if (PlayerStats.instance.buildingResources < 7000)
                    {
                        Debug.Log("Not enough building resources.");
                        return;
                    }
                    break;
                default: break;
            }
            StartCoroutine(BuildStructureCoroutine(structureType));
        }
    }

    private IEnumerator BuildStructureCoroutine(string structureType)
    {
        switch (structureType)
        {
            case "city":
                PlayerStats.instance.buildingResources -= 1000;
                break;
            case "factory":
                PlayerStats.instance.buildingResources -= 2000;
                break;
            case "lab":
                PlayerStats.instance.buildingResources -= 7000;
                break;
        }

        float baseTime = 10f;
        float minTime = 2f;
        float constructionTime = Mathf.Clamp(baseTime * Mathf.Pow(0.5f, currentPopulation / 30000f), minTime, baseTime);

        Debug.Log($"Construcción de {structureType} en proceso... Tiempo estimado: {constructionTime:F1} segundos.");

        WorldStatsUI.instance.constructionText.gameObject.SetActive(true);
        WorldStatsUI.instance.constructionText.text = structureType;
        WorldStatsUI.instance.constructionProgressBar.gameObject.SetActive(true);
        WorldStatsUI.instance.constructionProgressBar.value = 0;

        float elapsedTime = 0;

        while (elapsedTime < constructionTime)
        {
            elapsedTime += Time.deltaTime;
            WorldStatsUI.instance.constructionProgressBar.value = elapsedTime / constructionTime;
            yield return null;
        }

        WorldStatsUI.instance.constructionText.gameObject.SetActive(false);
        WorldStatsUI.instance.constructionProgressBar.gameObject.SetActive(false);

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

    public void DeleteStructure(string structureType)
    {
        switch (structureType)
        {
            case "city":
                if (cities > 0)
                {
                    cities--;
                    maxPopulation = Mathf.RoundToInt(maxPopulation * 0.8f);
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
        if (PlayerStats.instance.buildingResources >= 10000)
        {
            PlayerStats.instance.buildingResources -= 10000;
            maxStructuresSize += 3;
        }
        else
        {
            Debug.Log("Not enough building resources.");
        }
    }
}