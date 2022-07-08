using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Pooler : MonoBehaviour
{
    // Start is called before the first frame update

    public static Pooler sharedInstance;
    private List<GameObject> pooledObjects;
    public List<GameObject> objectToPull;
    public int amountToPull;

    private void Awake()
    {
        sharedInstance = this;
    }
    private void Start()
    {
        pooledObjects = new List<GameObject>();

        GameObject tmp;
        for(int i = 0; i < amountToPull; i++)
        {
            for (int x = 0; x < objectToPull.Count; x++)
            {
                tmp = Instantiate(objectToPull[Random.Range(0, objectToPull.Count)]);
                tmp.SetActive(false);
                pooledObjects.Add(tmp);
                
            }
            
        }
        

    }
    
    public GameObject GetPooledObject(Vector3 position, Quaternion rotation)
    {
        for (int i = 0; i < amountToPull; i++)
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

}
