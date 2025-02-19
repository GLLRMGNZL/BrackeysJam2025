using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

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
    public bool isSettled;
    public List<GameObject> activeProgressBars = new List<GameObject>();

    public Animator worldUIAnim;
    public Animator camAnim;

    private int populationGrowthRate = 3;
    private float growthInterval = 1f;
    private float buildingResourcesGrowthRate = 7f;
    private float travelResourcesGrowthRate = 4f;
    private float techLevelGrowthRate = 2f;
    private int maxSimultaneousConstructions => Mathf.RoundToInt(Mathf.Min(PlayerStats.instance.technologyLevel + 1f, 4f));

    // Material Slope
    [Header("Material Slope")]
    public Renderer planetRenderer;
    public Material mat1;
    public Material mat2;
    public int steps = 15;
    public float baseStepDuration = 10000f;
    public float stepDuration = 10000f;

    private float minStepDuration = 5f;
    private Material transitionMaterial;
    private int currentStep = 0;
    private bool isTransitioning = false;
    private bool isTransitionComplete = false;
    

    private void Start()
    {
        if (this.worldName == "Earth")
        {
            isSettled = true;
        }
        else
        {
            isSettled = false;
        }

        GrowthRate();

        // Material Slope
        transitionMaterial = new Material(mat1);
        planetRenderer.material = transitionMaterial;
        StartTransition();
    }

    private void Update()
    {
        // Material Slope: Depending on CurrentPopulation and currentStructuresSize, stepDuration diminishes
        stepDuration = Mathf.Max(minStepDuration, baseStepDuration - (currentPopulation * currentStructuresSize) * 0.005f);

        if (!isTransitioning)
        {
            StartTransition();
        }

        if (isTransitionComplete)
        {
            if (Player.instance.selectedWorld == this)
            {
                Player.instance.selectedWorld = null;
                worldUIAnim.SetBool("isOpen", false);
                camAnim.SetBool("isOpen", false);
            }
            this.gameObject.SetActive(false);
        }
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
        if (activeProgressBars.Count >= maxSimultaneousConstructions)
        {
            Debug.Log("Límite de construcciones simultáneas alcanzado.");
            return;
        }

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
                currentStructuresSize++;
                break;
            case "factory":
                PlayerStats.instance.buildingResources -= 2000;
                currentStructuresSize++;
                break;
            case "lab":
                PlayerStats.instance.buildingResources -= 7000;
                currentStructuresSize++;
                break;
        }

        float baseTime = 10f;
        float minTime = 2f;
        float constructionTime = Mathf.Clamp(baseTime * Mathf.Pow(0.5f, currentPopulation / 30000f), minTime, baseTime);

        Debug.Log($"Construcción de {structureType} en proceso... Tiempo estimado: {constructionTime:F1} segundos.");

        GameObject progressBar = Instantiate(WorldStatsUI.instance.progressBarPrefab, WorldStatsUI.instance.progressBarContainer);
        Slider slider = progressBar.GetComponent<Slider>();
        slider.value = 0;
        activeProgressBars.Add(progressBar);

        float elapsedTime = 0;

        while (elapsedTime < constructionTime)
        {
            elapsedTime += Time.deltaTime;
            slider.value = elapsedTime / constructionTime;
            yield return null;
        }

        activeProgressBars.Remove(progressBar);
        Destroy(progressBar);

        switch (structureType)
        {
            case "city":
                cities++;
                maxPopulation = Mathf.RoundToInt(maxPopulation * 1.15f);
                break;

            case "factory":
                factories++;
                break;

            case "lab":
                labs++;
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

    // Material Slope

    public void StartTransition()
    {
        if (!isTransitioning)
        {
            isTransitioning = true;
            //StopCoroutine(TransitionCoroutine());
            StartCoroutine(TransitionCoroutine());
        }
    }

    private IEnumerator TransitionCoroutine()
    {
        for (currentStep = 1; currentStep < steps; currentStep++)
        {
            float t = (float)currentStep / steps;

            // Transition between material colors
            transitionMaterial.color = Color.Lerp(mat1.color, mat2.color, t);

            // Transition between textures
            if (mat1.mainTexture != null && mat2.mainTexture != null)
            {
                transitionMaterial.mainTexture = (t < 0.5f) ? mat1.mainTexture : mat2.mainTexture;
            }
            yield return new WaitForSeconds(stepDuration);
        }

        isTransitioning = false;
        isTransitionComplete = true;
    }
}