using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckFlatTerrain : MonoBehaviour
{
    private Terrain terrain;
    
    
    Ray ray;
    RaycastHit hit;
    
    public int count;
    public GameObject[] trees;
    public GameObject gameObjectsToAvoid;
    public List<Transform> avoid = new List<Transform>();

    //buildings
    public List<GameObject> structures;
    
    
    // Start is called before the first frame update
    void Start()
    {
        /* PillarBuilder pb = new PillarBuilder();

         terrain = FindObjectOfType<Terrain>();

        //get gameobjects you want to not intersect
         gameObjectsToAvoid = GameObject.Find("RockGenerator");

         if (gameObjectsToAvoid.activeInHierarchy)
         {
             for (int i = 0; i < gameObjectsToAvoid.transform.childCount; i++)
                 avoid.Add(gameObjectsToAvoid.transform.GetChild(i));
         }

         trees = GameObject.FindGameObjectsWithTag("Tree");

         foreach(var x in trees)
             avoid.Add(x.transform);

         count = Random.Range(50, 50);

         List<Vector3> points = InitialSpawnPoints();
         for (int j = 0; j < count; j++)
         {
             pb.ShuffleList(structures);

             ray = new Ray(points[j], Vector3.down);

             //raycast towards terrain
             if (Physics.Raycast(ray, out hit, 1000))
             {

                 float angle = Vector3.Angle(hit.normal, Vector3.up);

                 //random rotation
                 Vector3 rot = new Vector3(0, Random.Range(0, 720), 0);

                 //if flat terrain..
                 if (angle == 0)
                 {

                     //instantiate
                     var obj = Instantiate(structures[Random.Range(0, structures.Count)], hit.point, Quaternion.Euler(rot));
                     obj.transform.parent = transform;


                     if (avoid.Count > 0)
                     {
                         foreach (var x in avoid)
                         {
                             if (Vector3.Distance(obj.transform.position, x.position) < 5)
                             {
                                 obj.SetActive(false);
                             }
                         }
                     }

                     for (int i = 0; i < points.Count; i++)
                     {
                         if (!terrain.terrainData.bounds.Contains(obj.transform.position))
                         {
                             obj.SetActive(false);

                         }
                     }

                 }
             }
         }*/
        
        idk(count);
    }

   
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

    List<Vector3> InitialSpawnPoints()
    {
        List<Vector3> points = new List<Vector3>();
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

        return points;
    }

    public void idk(int count)
    {

        for (int j = 0; j < count; j++)
        {
            ray = new Ray(InitialSpawnPoints()[j], Vector3.down);

            //raycast towards terrain
            if (Physics.Raycast(ray, out hit, 1000))
            {
                Debug.Log(hit.point);
            }
        }
    }
}
