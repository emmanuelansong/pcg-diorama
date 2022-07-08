using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockGen : MonoBehaviour
{
    public List<GameObject> rocks;
    [SerializeField]
    private int count = 100;
    // Start is called before the first frame update
    void Start()
    {
         ChooseAndPlaceRandomRock();
    }
    public void ChooseAndPlaceRandomRock()
    {
        List<GameObject> List = rocks;
        GenerationAlgorithms ga = new GenerationAlgorithms();
        ObjectShuffler shuffler = new ObjectShuffler();

        for (int i = 0; i < count; i++)
        {
            shuffler.ShuffleList(List);

            GameObject selectedObject = List[0];
            Vector3 temp = ga.RandomTerrainPosition(Terrain.activeTerrain);
            Debug.DrawRay(temp, Vector3.down, Color.red);
            if (Physics.Raycast(temp, Vector3.down, out RaycastHit hit, 1000f))
            {
                if (hit.collider.name == "Terrain")
                {
                    // Quaternion.LookRotation(hit.normal)
                    GameObject create = Instantiate(selectedObject, hit.point, transform.rotation);
                    create.transform.rotation = Quaternion.FromToRotation(transform.up, hit.normal) * create.transform.rotation;
                    create.transform.rotation = Quaternion.Euler(create.transform.rotation.x, UnityEngine.Random.rotation.y, create.transform.rotation.z);

                    create.transform.parent = transform;
                }

            }
        }
        
        

    }


}
