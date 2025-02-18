using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerStatsUI : MonoBehaviour
{
    #region Singleton
    public static PlayerStatsUI instance;

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

    public TextMeshProUGUI yearText;
    public TextMeshProUGUI buildingResources;
    public TextMeshProUGUI travelResources;
    public TextMeshProUGUI techLevel;

    private void Update()
    {
        yearText.text = PlayerStats.instance.year.ToString();
        buildingResources.text = PlayerStats.instance.buildingResources.ToString();
        travelResources.text = PlayerStats.instance.travelResources.ToString();
        techLevel.text = PlayerStats.instance.technologyLevel.ToString();
    }
}