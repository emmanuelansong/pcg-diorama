using sc.terrain.vegetationspawner;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIButtons : MonoBehaviour
{
   
    // Start is called before the first frame update
    void Start()
    {
         
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //switch betwen orthographic and perspective
    public void ChangePerspective()
    {
        if (Camera.main.orthographic == true)
            Camera.main.orthographic = false;
        else
            Camera.main.orthographic = true;
    }

    public void Regenerate()
    {
        
        GameObject treeGen = GameObject.Find("TreeGen");
        GameObject rockGen = GameObject.Find("RockGen");
        //List<GameObject> rockGenChildren = rockGen.transform.chil

        Terrain.activeTerrain.GetComponent<TerrainGeneration>().generate();
        VegetationSpawner spawner = GameObject.Find("VegetationGen").GetComponent<VegetationSpawner>();

        spawner.RandomizeSeed();
        spawner.RespawnTerrain();

        for (int i = 0; i < rockGen.transform.childCount; i++)
        {
            Destroy(rockGen.transform.GetChild(i).gameObject);
        }

       rockGen.GetComponent<RockGen>().ChooseAndPlaceRandomRock();
        
    }

}
