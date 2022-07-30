using sc.terrain.vegetationspawner;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/********* Tree Generator **********/

public class TreeGen : MonoBehaviour
{
    private void Start()
    {
        RandomiseTree();
    }
    
    void RandomiseTree()
    {
        VegetationSpawner spawner = GameObject.Find("VegetationGen").GetComponent<VegetationSpawner>();
        
        spawner.RandomizeSeed();
        spawner.RespawnTerrain();
    }

}