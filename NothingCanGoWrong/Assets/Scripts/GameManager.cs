using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (StarSystem.instance.planetsDestroyed == 3)
        {
            Debug.Log("Transition to game over");
        }
    }
}