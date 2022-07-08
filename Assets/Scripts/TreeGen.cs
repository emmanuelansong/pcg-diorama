using sc.terrain.vegetationspawner;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/********* Tree Generator **********/

public class TreeGen : MonoBehaviour
{
    
    private List<Vector3> points = new List<Vector3>();
    [SerializeField]
    private int count = 100;

    public float radius;

    private Terrain terrain;

    TerrainGeneration tg;
    [SerializeField]
    private List<GameObject> coniferTrees;

    private void Start()
    {

        RandomiseTree();
        //CreatePointsAndCheckTerrain("Grass");
    }
    
    void RandomiseTree()
    {
        VegetationSpawner spawner = GameObject.Find("VegetationGen").GetComponent<VegetationSpawner>();
        
        spawner.RandomizeSeed();
        spawner.RespawnTerrain();
    }

    private void ChooseRandomTree(RaycastHit hit)
    {
        ObjectShuffler shuffler = new ObjectShuffler();
        shuffler.ShuffleList(coniferTrees);

        GameObject selectedTree = coniferTrees[0];

        GameObject create = Instantiate(selectedTree, new Vector3(hit.point.x, hit.point.y, hit.point.z), transform.rotation);
        create.transform.rotation = Quaternion.Euler(create.transform.rotation.x, UnityEngine.Random.rotation.y, create.transform.rotation.z);
        create.transform.parent = transform;
   
    }

    //check if terrain is = string passed by raycasting poisson points onto surface --> then convert the hitpoint into world space 
    public void CreatePointsAndCheckTerrain(string surface)
    {
        Terrain terrain = Terrain.activeTerrain;
        GenerationAlgorithms ga = new GenerationAlgorithms();

        float radius = 4f;
        Vector3 regionSize = new Vector3(1000, 1000, 1000);
        int rejectionSamples = 30;

        List<Vector3> points = ga.GeneratePoints(radius, regionSize, rejectionSamples);
        foreach (Vector3 point in points)
        {
            if (Physics.Raycast(point, Vector3.down, out RaycastHit hit, 1000f))
            {
                Vector3 terrainPosition = hit.point - terrain.transform.position;

                Vector3 splatMapPosition = new Vector3(terrainPosition.x / terrain.terrainData.size.x, 0, terrainPosition.z / terrain.terrainData.size.z);

                int x = Mathf.FloorToInt(splatMapPosition.x * terrain.terrainData.alphamapWidth);

                int z = Mathf.FloorToInt(splatMapPosition.z * terrain.terrainData.alphamapHeight);

                float[,,] alphaMap = terrain.terrainData.GetAlphamaps(x, z, 1, 1);

                for (int i = 1; i < alphaMap.Length; i++)
                {
                    //for non-blending
                    //if (alphaMap[0, 0, i] > alphaMap[0, 0, primaryIndex])

                    //terrain blending
                    if (alphaMap[0, 0, i] > 0)
                    {
                        //if on grass
                        if (terrain.terrainData.terrainLayers[i].name.Contains(surface))
                        {
                            if (alphaMap[0, 0, i] < 0.3)
                            {
                                ChooseRandomTree(hit);
                                break;
                            }
                        }
                    }
                }
            }
        }
    }


}