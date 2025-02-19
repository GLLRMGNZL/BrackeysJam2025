using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TravelManager : MonoBehaviour
{
    #region Singleton
    public static TravelManager instance;

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

    public Animator animator;
    public TextMeshProUGUI PlanetName;

    public void TravelAndSettle()
    {
        if (PlayerStats.instance.travelResources >= 1000)
        {
            Player.instance.selectedWorld.isSettled = true;
            Player.instance.selectedWorld.currentPopulation += 25;
            animator.SetBool("isOpen", false);
        }
        else
        {
            Debug.Log("Recursos insuficientes!");
        }
    }
}
