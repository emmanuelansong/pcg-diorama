using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnMisc : MonoBehaviour
{
    public List<GameObject> misc;

    float minRadius;
    float maxRadius;
    public int count;
    // Start is called before the first frame update
    void Start()
    {
        count = Random.Range(0, misc.Count);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDrawGizmos()
    {
        minRadius = GetComponent<SphereCollider>().radius;
        maxRadius = minRadius + 10;
        //bounds = transform
        Gizmos.DrawWireSphere(transform.position, minRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, maxRadius);
    }
}
