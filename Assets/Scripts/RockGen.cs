using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockGen : MonoBehaviour
{
    public List<GameObject> rocks;
    [SerializeField]
    private int count = 100;
    List<Vector3> points = new List<Vector3>();
    // Start is called before the first frame update
    void Start()
    {
        count = Random.Range(0, 501);
         ChooseAndPlaceRandomRock();
    }
    public void ChooseAndPlaceRandomRock()
    {
        List<GameObject> List = rocks;

        //initialise
        GenerationAlgorithms ga = new GenerationAlgorithms();
        ObjectShuffler shuffler = new ObjectShuffler();

        for (int i = 0; i < count; i++)
        {
            //shuffle between rocks
            shuffler.ShuffleList(List);

            //select random from shuffled list
            GameObject selectedObject = List[0];


            Vector3 temp = ga.RandomTerrainPosition(Terrain.activeTerrain) + new Vector3(0, 100, 0);
            //Gizmos.color = Color.red;
            //Gizmos.DrawLine(temp, Vector3.down);

            //raycast from temporary pos
            if (Physics.Raycast(temp, Vector3.down, out RaycastHit hit, 1000f))
            {
                if (hit.collider.name == "Terrain")
                {
                    // Quaternion.LookRotation(hit.normal)
                    GameObject create = Instantiate(selectedObject, hit.point, transform.rotation);
                    create.transform.rotation *= Quaternion.FromToRotation(transform.up, hit.normal);
                    
                    Vector3 randomY = new Vector3(0, Random.rotation.y, 0);

                    create.transform.rotation *= Quaternion.Euler(randomY);
                    //Gizmos.DrawRay(hit.normal);

                    
                    
                    //create.transform.rotation =  Quaternion.Euler(create.transform.rotation.x, randomY, create.transform.rotation.z);

                    create.transform.parent = transform;
                }

            }

        }
        
        

    }

    private void OnDrawGizmosSelected()
    {
        GenerationAlgorithms ga = new GenerationAlgorithms();
        
        //for (int i = 0; i < count; i++)
        {
            //points.Add(ga.RandomTerrainPosition(Terrain.activeTerrain) + new Vector3(0, 100, 0));
            //Vector3 temp = ga.RandomTerrainPosition(Terrain.activeTerrain) + new Vector3(0, 100, 0);
            


            //foreach (Vector3 temp in points)
            {
                

                //raycast from temporary pos
                if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 500))
                {
                    if (hit.collider.name == "Terrain")
                    {
                        Gizmos.color = Color.red;
                        Gizmos.DrawRay(transform.position, Vector3.down * hit.distance);

                        //float angle = Vector3.Angle(hit.normal, yourRay)
                        Gizmos.color = Color.white;
                        Gizmos.DrawRay(hit.point, hit.normal * hit.distance/2);
                        Gizmos.DrawWireSphere(hit.point, 10);



                    }

                }
            }

        }
    }

}
