using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YearCollider : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        PlayerStats.instance.year++;
    }
}