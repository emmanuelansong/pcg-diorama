using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeGenerator : MonoBehaviour
{
    public GameObject bridge;
    
    GenerationAlgorithms ga;

    public List<Vector3> points;
    public int count;

    List<GameObject> existing;
    // Start is called before the first frame update
    void Start()
    {
        existing = new List<GameObject>();
        ga = new GenerationAlgorithms();
        
        for (int i = 0; i < count; i++)
        {
            points.Add(ga.RandomTerrainPosition(Terrain.activeTerrain));
            
            if (points[i].y >= 100) 
            {
                if(points[i].x < (Terrain.activeTerrain.terrainData.size.x - 100) && points[i].x < (Terrain.activeTerrain.terrainData.size.z - 100))
                {
                    RaycastBridge();
                }
                
            }

            

        }
    }

    void RaycastBridge()
    {
        float maxVertical = 100f;
        float maxHorizontal = 250f;
        RaycastHit hit;

        bool rightCheck = false;
        bool leftCheck = false;

        //Vector3 size = new Vector3(1, 1, 1);
        foreach (Vector3 point in points)
        {
            if (Physics.Raycast(point, Vector3.down, out hit, maxVertical))
            {
                //Gizmos.color = Color.red;
                //Gizmos.DrawRay(transform.position, Vector3.down * hit.distance);

                Gizmos.color = Color.blue;
                Vector3 leftPos = Vector3.zero;
                Vector3 rightPos = Vector3.zero;

                //right side
                if (Physics.Raycast(point, Vector3.right, out hit, maxHorizontal))
                {
                    //Gizmos.DrawRay(transform.position, Vector3.right * hit.distance);
                    //Gizmos.DrawWireSphere(hit.point, size.y / 2);
                    rightPos = hit.point;
                    rightCheck = true;
                }

                //left side
                if (Physics.Raycast(point, Vector3.left, out hit, maxHorizontal))
                {
                    //Gizmos.DrawRay(transform.position, Vector3.left * hit.distance);
                    //Gizmos.DrawWireSphere(hit.point, size.y / 2);
                    leftPos = hit.point;
                    leftCheck = true;
                }

                else
                {
                    //Gizmos.color = Color.green;
                    //Gizmos.DrawRay(transform.position, Vector3.down * maxVertical);
                }
                if (rightCheck && leftCheck)
                {
                    CreateBridge(rightPos, leftPos, point);
                    
                    
                }
            }
        }

    }
    public void CreateBridge(Vector3 rightSpherePos, Vector3 leftSpherePos, Vector3 spawnPoint)
    {
        float distance = Vector3.Distance(rightSpherePos, leftSpherePos);
        Vector3 midpoint = (leftSpherePos + rightSpherePos) / 2;
        Vector3 dir = (leftSpherePos - rightSpherePos);
        if (distance / 5f <= 60 && distance / 5f >= 30)
        {
            GameObject go = Instantiate(bridge, spawnPoint, transform.rotation);
            go.transform.position = midpoint;

            //float localScaleX = bridge.transform.localScale.x;
            float localScaleZ = Mathf.Lerp(1, distance / 5f, .5f);
            float localScaleY = Mathf.Lerp(1, distance / 5f, .5f);
            go.transform.localScale = new Vector3(distance / 5f, localScaleY, localScaleZ);
            go.transform.right = dir;
            go.transform.parent = transform;

            existing.Add(go);

            if (existing.Count > 0)
            {
                for (int i = 1; i < existing.Count; i++)
                {
                    if (Vector3.Distance(go.transform.position, existing[i].transform.position) < 1)
                    {
                        Destroy(go);
                    }
                }
            }
        }
        
        

    }
}
