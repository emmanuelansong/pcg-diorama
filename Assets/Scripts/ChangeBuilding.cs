using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeBuilding : MonoBehaviour
{
    //Renderer renderer;
    public Material[] materials;
    public List<Texture> newMaterial;

    public List<GameObject> misc;
    public float minRadius = 10f;
    public 
        float maxRadius = 10f;
    public int count;

    // Start is called before the first frame update
    void Start()
    {
        
        int picker = Random.Range(0, newMaterial.Count);

        if (gameObject.tag == "Desert_House")
        {
            materials = GetComponent<MeshRenderer>().materials;
            materials[7].color = Random.ColorHSV();
            materials[7].mainTexture = newMaterial[picker];
        }

        if(gameObject.tag == "Tent")
        {
            materials = GetComponent<MeshRenderer>().materials;
            materials[0].color = Random.ColorHSV();
           
        }

        if (gameObject.tag == "Light")
        {
            //newMaterial[picker] = materials[0].mainTexture;
            //materials[0].mainTexture = newMaterial[picker];
            //GetComponentInChildren<Light>().color = Random.ColorHSV();

        }

        if(gameObject.name == "Directional Light")
        {

            List<Color> colours = new List<Color> {Color.white, new Vector4(1,0.3f,0) };

            GetComponent<Light>().color = colours[Random.Range(0, colours.Count)];
        }

        if(gameObject.name ==  "PillarStructure" || gameObject.name == "BuildingGen" || gameObject.name == "SquarePyramid" || gameObject.name == "Pyramid")
        {
            count = Random.Range(0, 100);

            for(int i = 0; i < count; i++)
            {
                var miscObj = Instantiate(misc[Random.Range(0, misc.Count)],RandomPoint(gameObject.transform.position, maxRadius),transform.rotation);
                
                if(Vector3.Distance(miscObj.transform.position, gameObject.transform.position) < minRadius)
                {
                    miscObj.SetActive(false);
                }
            }

            if(gameObject.name == "SquarePyramid")
            {
                float intensity = GameObject.Find("Directional Light").GetComponent<Light>().intensity;
                if (intensity > 0.9)
                {
                    gameObject.transform.GetChild(5).gameObject.SetActive(false);
                }

                
            }

            gameObject.transform.localScale *= Random.Range(1, 5);
        }
    }

    public Vector3 RandomPoint(Vector3 origin, float radius)
    {

        //Sample reference point + random offset * rad
        var sample = origin + Random.insideUnitSphere * radius;

        //Then set the y to sampled terrain height
        sample.y = 0;//terrain.SampleHeight(sample);
        //And return the sample!
        return sample;
    }


}
