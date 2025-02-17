using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public TextMeshProUGUI yearText;
    public static int year = 0;

    public int technologyLevel;
    public float buildingResources;
    public float travelResources;

    private void Update()
    {
        yearText.text = year.ToString();
    }
}
