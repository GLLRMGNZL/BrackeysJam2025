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

    //TODO: Methods

    /* Build structure
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
                    cities++;
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
    }

    // Terraforming (increase StructureSize)
    public void Terraforming()
    {
        maxStructuresSize+=5;
    }
}