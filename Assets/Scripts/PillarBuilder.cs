using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//unused script
public class PillarBuilder : MonoBehaviour
{
    public RaycastHit hit;
    public List<GameObject> basePrefabs;
    public List<GameObject> columns;
    
    int basePicker;
  
    public int noOfSpawns = 4;
    public float angle = 0;
    public float radius = 5f;

    public List<GameObject> centreObject;
    // Start is called before the first frame update
    void Start()
    {
        //transform.parent = GameObject.Find("Raycast").transform;
        basePicker = Random.Range(0, basePrefabs.Count);

        var baseHeight = basePrefabs[basePicker].transform.GetChild(0).GetComponent<Renderer>().bounds.size.y;

        radius = Random.Range(4, 8);
        noOfSpawns = Random.Range(4, 9);

        Vector3 pos = transform.position + new Vector3(0, basePrefabs[basePicker].transform.position.y + baseHeight, 0);
        Vector3 statuePos = transform.position + new Vector3(0, 2.3f, 0);
        

        //statue
        var centreObjectGo = Instantiate(centreObject[Random.Range(0, centreObject.Count)], statuePos, Quaternion.Euler(new Vector3(0, Random.rotation.y, 0)));
        centreObjectGo.transform.parent = transform;
  
        //shuffle algorithm: list to array
        //var objectArray = columns.ToArray();

        //base prefab
        var baseGo = Instantiate(basePrefabs[basePicker], transform.position, basePrefabs[basePicker].transform.rotation);
        baseGo.transform.parent = transform;

        for (int i = 0; i < noOfSpawns; i++)
        {
            ShuffleList(columns);

            //create circle formation
            angle = i * Mathf.PI * 2 / noOfSpawns;
            float x = Mathf.Cos(angle) * radius;
            float z = Mathf.Sin(angle) * radius;
            pos = transform.position + new Vector3(x, basePrefabs[basePicker].transform.position.y + baseHeight, z);
            float angleDeg = angle * Mathf.Rad2Deg;
            Quaternion rot = Quaternion.Euler(0, angleDeg, 0);


            GameObject column = Instantiate(columns[0], pos, rot);
            column.transform.parent = transform;

            //column.transform.localScale *= 2f;
        }
        
    }

    //Fisher-Yates shuffle
    public void ShuffleList(List<GameObject> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int r = i + Random.Range(0, list.Count - i);
            //  Debug.Log(r);
            var K = list[r];
            //Debug.Log(columns[r]);
            list[r] = list[i];
            list[i] = K;
            
        }
        

        
    }
   

    
}
