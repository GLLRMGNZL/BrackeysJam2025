using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WorldStatsUI : MonoBehaviour
{
    public TextMeshProUGUI population;

    public void showCurrentWorldPopulation(int currentPopulation, int maxPopulation)
    {
        population.text = "" + currentPopulation + "/" + maxPopulation;
    }
}
