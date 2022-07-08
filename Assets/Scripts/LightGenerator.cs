using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// /
/// </summary>
public class LightGenerator : MonoBehaviour
{
    public int count = 100;
    public int secondCount;

    public int age;
    public Vector3 treePos;
    public List<GameObject> tree;
    public List<GameObject> treePlaceholder;
    public float radius;

    private List<Vector3> points = new List<Vector3>();
    public Terrain terrain;
    
    
    
    
    public Vector3 RandomTerrainPosition(Terrain terrain)
    {
        //Get the terrain size in all 3 dimensions
        Vector3 terrainSize = terrain.terrainData.size;

        //Choose a uniformly random x and z to sample y
        float rX = Random.Range(0, terrainSize.x);
        float rZ = Random.Range(0, terrainSize.z);

        //Sample y at this point and put into an offset vec3
        Vector3 sample = new Vector3(rX, 0, rZ);
        sample.y = terrain.SampleHeight(sample);


        //Just return terrain pos + sample offset
        return terrain.GetPosition() + sample;
    }
    public Vector3 RandomPointInRadius(Vector3 refPoint, float radius)
    {

        //Sample reference point + random offset * rad
        var sample = refPoint + Random.insideUnitSphere * radius;
        //Then set the y to sampled terrain height
        sample.y = terrain.SampleHeight(sample);
        //And return the sample!
        return sample;

    }

    private void Start()
    {
        terrain = FindObjectOfType<Terrain>();

        for (int i = 0; i < count; i++)
        {
            points.Add(RandomTerrainPosition(terrain));

        }
        for (int j = 0; j < points.Count; j++)
        {

            //Debug.Log(v3);
            for (int i = 0; i < secondCount; i++)
            {
                //v3.Add(RandomPointInRadius(points[i], radius));
                //Vector3 offset = new Vector3(v3.x, 0, v3.z);

                Vector3 rot = new Vector3(0, Random.Range(0, 720), 0);
                Vector3 scaleChange = new Vector3(Random.Range(0.5f, 2), Random.Range(0.5f, 2), Random.Range(0.5f, 2));

                //prefabs[picker].transform.localScale = scaleChange;

                for (int x = 0; x < tree.Count; x++)
                {
                    tree[x] = Instantiate(tree[x], RandomPointInRadius(points[i], radius), Quaternion.Euler(rot));

                    tree[x].transform.parent = gameObject.transform;
                    //tree[x].transform.localScale = scaleChange;
                    cullTrees(tree[x]);

                    
                    treePlaceholder.Add(tree[x]);
                }

            }

        }

    }

    public void cullTrees(GameObject prefab)
    {
        if (!terrain.terrainData.bounds.Contains(prefab.transform.position))
        {
            //Debug.Log("Tree destroyed");
            Destroy(prefab);

        }


        for (int i = 0; i < points.Count; i++)
        {
            //Debug.Log($"Position: ({terrain.terrainData.GetSteepness(points[i].x, points[i].y)})");

            if (prefab.transform.position.y < 20)
            {
                Destroy(prefab);
            }


        }

    }
}
