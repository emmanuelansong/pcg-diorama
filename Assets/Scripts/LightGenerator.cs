using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
public class LightGenerator : MonoBehaviour
{
    [System.Serializable]
    public class TimeOfDay
    {
        public string name;
        public Vector3 rotation;
        public bool enabled;
    }

    public List<TimeOfDay> times;

    private void Start()
    {
        SelectTimeOfDay(times);
    }
    void SelectTimeOfDay(List<TimeOfDay> times)
    {
        int random = Random.Range(0, times.Count);

        times[random].enabled = true;

        Light light = gameObject.GetComponent<Light>();
        RandomiseLightComponent(light, times[random]);
   
    }

    void RandomiseLightComponent(Light light, TimeOfDay time) 
    {
        PostProcessVolume ppv = Camera.main.GetComponent<PostProcessVolume>();
        Bloom bloom;
        ppv.profile.TryGetSettings(out bloom);

        ColorGrading colorGrading; 
        ppv.profile.TryGetSettings(out colorGrading);
        light.intensity = Random.value;

        if (time.enabled)
        {
            
            //RenderSettings.fogColor = Color.gray;
            
            if (time.name == "Dawn")
            {
                //RenderSettings.fogColor = new Color(132, 114, 85, 1);
                
                light.transform.rotation = Quaternion.Euler(time.rotation);
                colorGrading.temperature.value = (Random.Range(0f, 100f));
            }
            if (time.name == "Sunrise")
            {
                light.transform.rotation = Quaternion.Euler(time.rotation);
                colorGrading.temperature.value = (Random.Range(-25f, 100f));
            }
            if (time.name == "Midday")
            {
                light.transform.rotation = Quaternion.Euler(time.rotation);
                colorGrading.temperature.value = (Random.Range(0f, 25f));
            }
            if (time.name == "Sunset")
            {
                light.transform.rotation = Quaternion.Euler(time.rotation);
                colorGrading.temperature.value = (Random.Range(-25f, 100f));
                
                //RenderSettings.fogColor = new Color(77, 77, 77, 1);
            }
            if (time.name == "Dusk")
            {
                light.transform.rotation = Quaternion.Euler(time.rotation);
                colorGrading.temperature.value = (Random.Range(-25f, 0));
            }
            

            
        }
    }

    private void OnDrawGizmos()
    {
    }


}
