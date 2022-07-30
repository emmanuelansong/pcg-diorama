using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/********* Camp Building Algorithm **********/
public class CampBuilder : MonoBehaviour
{
    

    public RaycastHit hit;
    public List<GameObject> campfires;
    public List<GameObject> campsites;
    public List<GameObject> misc;
    int noOfTents = 0;
    public float angle = 0;
    public float radius = 5f;
    public int noOfMisc = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        //random index
        radius = Random.Range(5, 10);
        int randomCamp = Random.Range(2, campsites.Count);
        int randomMisc = Random.Range(0, misc.Count);
        noOfTents = Random.Range(1, 4);
        noOfMisc = Random.Range(0, 10);

        //every camp has a camfire in its centre
        var campfire = Instantiate(campfires[Random.Range(0, campfires.Count)], transform.position, Quaternion.Euler(0, Random.rotation.y, 0));
        campfire.transform.parent = transform;

        GameObject camps;
        for (int i = 0; i < noOfTents; i++)
        {
            //create a number of tents in a circular motion w/ random angle and rotation
            angle = i * Mathf.PI * 2 / noOfTents;
            float x = Mathf.Cos(angle) * radius;
            float z = Mathf.Sin(angle) * radius;
            Vector3 pos = transform.position + new Vector3(x, 0, z);
            float angleDeg = angle * Mathf.Rad2Deg;
            Quaternion rot = Quaternion.Euler(0, angleDeg * Random.rotation.y, 0);

            //var tentHeight = campsites[randomCamp].GetComponent<Renderer>().bounds.size.x;
            //var tentPos = campsites[randomCamp].transform.position + Vector3.down;

            //instantiate tent
            camps = Instantiate(campsites[randomCamp], pos + Vector3.up, rot);

            camps.transform.parent = transform;


            //add miscellaneous items
            for (int j = 0; j < noOfMisc; j++)
            {
                
                GameObject extras = Instantiate(misc[Random.Range(0, j)], RandomPointInRadius(camps.transform.position, 5) + (Vector3.up / 2), misc[randomMisc].transform.rotation);

                //make sure they dont intersect with one another
                if (Vector3.Distance(extras.transform.position, camps.transform.position) > camps.GetComponent<Renderer>().bounds.size.x)
                {
                    extras.transform.position += new Vector3(2.5f, 0, 0);
                }
                extras.transform.parent = transform;

                if (Vector3.Distance(extras.transform.position, extras.transform.position) < extras.GetComponent<Renderer>().bounds.size.x)
                {
                    extras.transform.position += new Vector3(extras.GetComponent<Renderer>().bounds.size.x, 0, 0);
                }
            }



        }

        


    }

    //random pos in radius
    public Vector3 RandomPointInRadius(Vector3 refPoint, float radius)
    {
        
        //Sample reference point + random offset * rad
        var sample = refPoint + Random.insideUnitSphere * radius;

        //Then set the y to sampled terrain height
        sample.y = 0;
        //And return the sample!
        return sample;

    }

}
