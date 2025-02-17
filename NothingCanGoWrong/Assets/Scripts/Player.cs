using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    #region Singleton
    public static Player instance;

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

    public World selectedWorld;

    public void BuildStructure(string structureType)
    {
        selectedWorld.BuildStructure(structureType);
        WorldStatsUI.instance.ShowWorldStats(selectedWorld);
    }
}