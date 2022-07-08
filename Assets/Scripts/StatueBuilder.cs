using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/********* Statue Building Algorithm **********/

public class StatueBuilder : MonoBehaviour
{
    
    public GameObject basePrefab;
    public List<GameObject> statues;


    void Start()
    {
        //get height and position of base prefab
        var baseHeight = basePrefab.transform.GetChild(0).GetComponent<Renderer>().bounds.size.y;
        Vector3 pos = transform.position + new Vector3(0, basePrefab.transform.position.y + baseHeight, 0);

        //give base random rotation
        Vector3 rot = new Vector3(0, Random.Range(0, 720), 0);
        basePrefab = Instantiate(basePrefab, transform.position, Quaternion.Euler(rot));
        basePrefab.transform.parent = transform;

        //select random statue
        GameObject obj = Instantiate(statues[Random.Range(0, statues.Count)], pos, basePrefab.transform.rotation);
        obj.transform.parent = transform;

    }
    // Update is called once per frame

    
}
