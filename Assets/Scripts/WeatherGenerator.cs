using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityStandardAssets.ImageEffects;

public class WeatherGenerator : MonoBehaviour
{
    PostProcessVolume ppv;
    Bloom bloom;
    GameObject randomWeather;
    // Start is called before the first frame update
    void Start()
    {
        ppv = Camera.main.GetComponent<PostProcessVolume>();
        ppv.profile.TryGetSettings(out bloom);
        int randomizer = Random.Range(0, transform.childCount);
        randomWeather = transform.GetChild(randomizer).gameObject;

        randomWeather.SetActive(true);
        

    }

    // Update is called once per frame
    void Update()
    {
        if (randomWeather.activeInHierarchy)
        {
            if (randomWeather.name == "Rain")
            {
                bloom.intensity.value = 2;
            }
        }
        else
            bloom.intensity.value = 0;
    }

    void FogGenerator()
    {
        GlobalFog gf = Camera.main.GetComponent<GlobalFog>();

        gf.height = Random.Range(0, 100);

        gf.heightDensity = Random.Range(0.001f, 5);

        gf.startDistance = Random.Range(0,100);

        int random = Random.Range(0, 2);

        if (random == 0)
        {
            gf.heightFog = true;
        }
        else
            gf.heightFog = false;

            
    }
}
