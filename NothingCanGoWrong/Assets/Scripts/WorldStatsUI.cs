using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WorldStatsUI : MonoBehaviour
{
    #region Singleton
    public static WorldStatsUI instance;

    private void Awake()
    {

        if (instance == null)
        {
            instance = this;
        }
        else
        {
            return;
        }

    }
    #endregion

    public TextMeshProUGUI worldName;
    public TextMeshProUGUI population;
    public TextMeshProUGUI infrastructure;
    public TextMeshProUGUI cities;
    public TextMeshProUGUI factories;
    public TextMeshProUGUI labs;

    private void Update()
    {
        if (Player.instance.selectedWorld)
        {
            ShowWorldStats(Player.instance.selectedWorld);
        }
    }

    public void ShowWorldStats(World world)
    {
        ShowWorldName(world.worldName);
        ShowCurrentWorldPopulation(world.currentPopulation, world.maxPopulation);
        ShowCurrentWorldInfrastructure(world.currentStructuresSize, world.maxStructuresSize);
        ShowNumberOfCities(world.cities);
        ShowNumberOfFactories(world.factories);
        ShowNumberOfLabs(world.labs);
    }

    private void ShowWorldName(string name)
    {
        worldName.text = name;
    }

    private void ShowCurrentWorldPopulation(int currentPopulation, int maxPopulation)
    {
        population.text = currentPopulation + "/" + maxPopulation;
    }

    private void ShowCurrentWorldInfrastructure(int currentInfrastructure, int maxInfrastructure)
    {
        infrastructure.text = currentInfrastructure + "/" + maxInfrastructure;
    }

    private void ShowNumberOfCities(int nCities)
    {
        cities.text = nCities.ToString();
    }

    private void ShowNumberOfFactories(int nFactories)
    {
        factories.text = nFactories.ToString();
    }

    private void ShowNumberOfLabs(int nLabs)
    {
        labs.text = nLabs.ToString();
    }
}