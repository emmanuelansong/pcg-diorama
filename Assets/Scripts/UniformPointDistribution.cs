using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UniformPointDistribution: MonoBehaviour
{
    //public List<GameObject> prefabs;
    [SerializeField]
    private int count = 100;
    private Terrain terrain;

    private List<GameObject> pooledObjects;

    [SerializeField]
    private List<GameObject> objectToPull;

    public List<Vector3> points;
    // Start is called before the first frame update

    private void Awake()
    {
        TerrainGeneration tg = Terrain.activeTerrain.GetComponent<TerrainGeneration>();
        terrain = Terrain.activeTerrain;

        SetCount(tg.frequency, tg.lacu);

    }
    void Start()
    {
        pooledObjects = new List<GameObject>();

        GameObject tmp;
        for (int i = 0; i < count; i++)
        {
            for (int x = 0; x < objectToPull.Count; x++)
            {
                tmp = Instantiate(objectToPull[Random.Range(0, objectToPull.Count)]);
                tmp.SetActive(false);
                pooledObjects.Add(tmp);
                tmp.transform.parent = GameObject.Find("Pooler").transform;

            }
        }

        points = new List<Vector3>();

        for (int i = 0; i < count; i++)
        {

            points.Add(RandomTerrainPosition(terrain));

            if (!terrain.terrainData.bounds.Contains(points[i]))
            {
                //outside terrain bounds, delete point
                points.RemoveAt(i);

            }

            //for raycast
            points[i] += new Vector3(0, 50, 0);

        }
        terrain = FindObjectOfType<Terrain>();
        
        PlaceRocks();
        PlacePillar();

    }
    public int SetCount(float frequency, float lacu)
    {
        if (gameObject.tag == ("Rock"))
        {
            if (frequency == 2)
            {
                if (lacu == 2)
                {
                    return count =  100;
                }


                if (lacu > 2)
                {
                    return count = 250;
                }
            }

            if (frequency == 3)
            {
                if (lacu < 2)
                {
                    return count = 300;
                }
            }

            if (frequency == 1)
            {
                return count = 350;
            }
        }


        if (gameObject.tag == ("Pillar"))
        {
            if (frequency == 2)
            {
                if (lacu == 2)
                {
                    return count = 100;
                }


                if (lacu > 2)
                {
                    return count = 50;
                }
            }

            if (frequency == 3)
            {
                if (lacu < 2)
                {
                    return count = 75;
                }
            }

            if (frequency == 1)
            {
                return count = 150;
            }
        }
        return 0;
    }    
    public Vector3 RandomTerrainPosition(Terrain terrain)
    {
        terrain = Terrain.activeTerrain;
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

    public void cullObject(GameObject prefab)
    {
        if (!terrain.terrainData.bounds.Contains(prefab.transform.position))
        {
            prefab.SetActive(false);
        }

        if (prefab.transform.tag == ("Rock"))
        {
            if (prefab.transform.position.y > 75)
            {

                prefab.SetActive(false);
            }
        }

       

    }

    public GameObject GetPooledObject(Vector3 position, Quaternion rotation)
    {
        for (int i = 0; i < count; i++)
        {
            if (!pooledObjects[i].activeInHierarchy)
            {
                pooledObjects[i].transform.position = position;
                pooledObjects[i].transform.rotation = rotation;

                return pooledObjects[i];
            }

            pooledObjects[i].transform.parent = gameObject.transform;
        }
        return null;
    }

    void PlacePillar()
    {
        if (transform.name == ("PillarGenerator"))
        {
            for (int j = 0; j < count; j++)
            {
                Vector3 rot = new Vector3(Random.Range(-90, 90), Random.Range(0, 720), gameObject.transform.rotation.z);

                Ray ray = new Ray(points[j], Vector3.down);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, 1000))
                {
                    float angle = Vector3.Angle(hit.normal, Vector3.up);

                    //if flat terrain
                    if (angle == 0)
                    {
                        
                        var spawnObject = GetPooledObject(hit.point, Quaternion.Euler(rot));

                        
                        spawnObject.SetActive(true);
                        spawnObject.transform.parent = transform;

                        if (spawnObject.name == "Ruined Tower_01(Clone)")
                        {
                            spawnObject.transform.rotation = Quaternion.Euler(new Vector3(-90, spawnObject.transform.rotation.y, 0));
                            if (spawnObject.transform.position.y > 10 && spawnObject.transform.position.y < 100)
                            {
                                spawnObject.SetActive(false);
                            }
                        }

                    }




                }
            }
        }
    }

    void PlaceRocks()
    {
        if (gameObject.tag == ("Rock"))
        {
            RaycastHit hit;
            for (int j = 0; j < count; j++)
            {
                Ray ray = new Ray(points[j], Vector3.down);

                if (Physics.Raycast(ray, out hit, 1000))
                {
                    float angle = Vector3.Angle(hit.normal, Vector3.up);

                    if (angle < 45)
                    {
                        Vector3 scaleChange = new Vector3(Random.Range(0.5f, 2), Random.Range(0.5f, 2), Random.Range(0.5f, 5));
                        Vector3 rot = new Vector3(0, Random.Range(0, 720), 0);
                        var spawnObject = GetPooledObject(RandomTerrainPosition(terrain), Quaternion.Euler(rot));

                        spawnObject.transform.position = hit.point;
                        spawnObject.transform.rotation = Quaternion.FromToRotation(spawnObject.transform.up, hit.normal) * spawnObject.transform.rotation;

                        spawnObject.transform.localScale = scaleChange;
                        spawnObject.transform.parent = gameObject.transform;
                        spawnObject.SetActive(true);
                        cullObject(spawnObject);
                    }


                }
            }
        }
        
        
    }
}
